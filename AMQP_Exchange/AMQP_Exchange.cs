/*
 * Создано в SharpDevelop.
 * Пользователь: N.Polyagoshko
 * Дата: 10.08.2015
 * Время: 16:53
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using AMQP_Db;
using EasyNetQ;

namespace AMQP_Exchange
{	
	public class Exchange_Svc : ServiceBase
	{
		private readonly ConnectionStringSettings db;
		private List<RabbitHost> RabbitHosts;
		private List<RabbitQueue> RabbitQueues;
		private List<IBus> Buses = new List<IBus>();
		private bool ShouldStop;
		private List<Worker> workers;
		
		private TextWriter dbLog = null;
		private System.Timers.Timer timer;
		private object TimerLock = new object();
		
		public string dbConnStr { get{ return db.ConnectionString;}}
		
		const string _Name = "служба";
		public const string MyServiceName = "AMQP Exchange";
		public const string MyServiceDescription = "Служба обмена AMQP Exchange";
		
		public Exchange_Svc()
		{
			InitializeComponent();
			
			db = ConfigurationManager.ConnectionStrings["Default"];
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			foreach (var bus in Buses) {
				bus.Dispose();
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// Start this service.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			string[] svcArgs = Environment.GetCommandLineArgs();
			if (svcArgs.Length > 0 && svcArgs.Contains("-debug")) {
				Trace.TraceInformation("{0}\tDebug enabled", DateTime.Now);
				DebugFlag.Enabled = true;
			}
			Start();
		}
		
		public void Start()
		{
			try {
				Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName));
			} catch (Exception ex) {
				Trace.TraceWarning("{2}\t{0}: не удалось создать папку логов: {1}", _Name, ex.Message, DateTime.Now);
			}
			
			if (DebugFlag.Enabled) {
				try {
					dbLog = new StreamWriter(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName, "service.dblog")) {AutoFlush = true};
				} catch (Exception ex) {
					Trace.TraceWarning("{2}\t{0}: не удалось создать журнал dblog: {1}", _Name, ex.Message, DateTime.Now);
				}
			}
			
			Trace.TraceInformation("{1}\t{0}: запускается...", _Name, DateTime.Now);
			
			if (dbConnStr == null || String.IsNullOrWhiteSpace(dbConnStr)) {
				FailStart("Запуск невозможен: строка подключения к БД не может быть пустой");
			}
			
			using (var SQLconn = new SqlConnection(dbConnStr)) {
				// Пытаемся подключиться к БД
				int errorCounter = 0;
				while (SQLconn.State == ConnectionState.Closed) {
					try {
						SQLconn.Open();
					} catch (Exception ex) {
						if (ex is InvalidOperationException) {
							FailStart("Запуск невозможен: ошибка в строке подключения к БД \"{0}\"", dbConnStr);
						}
						else if (errorCounter >= 3) {
							FailStart("Превышено допустимое количество неудачных попыток подключения к БД. Последняя ошибка: {0}", ex.Message);
						}
						else {
							errorCounter++;
							Trace.TraceWarning("{1}\tНеудачная попытка подключения к БД: {0}", ex.Message, DateTime.Now);
							Thread.Sleep(1000);
						}
					}
				}
				
				// На этом этапе соединение с БД установлено
				try {
					new LogRecord() {
						Source = _Name,
						Message = "Успешный запуск, установлено соединение с БД",
						Details = dbConnStr }
					.TryWrite(SQLconn, dbLog);
				} catch (Exception ex) {
					FailStart("Запуск невозможен: ошибка записи в БД, {0}", ex.Message);
				}
				// Получаем список хостов RabbitMQ
				using (var exdb = new exDb(SQLconn)) {
					exdb.Log = dbLog;
					RabbitHosts = exdb.Hosts.Where(h => exdb.Queues.Any(q => q.Host_Id == h.Host_Id)).ToList();
					RabbitQueues = exdb.Queues.ToList();
					if (RabbitHosts.Count == 0) {
						var msg = "В таблице Hosts нет активных записей. Остановка службы";
						new LogRecord() {
							Source = _Name,
							IsError = true,
							Message = msg }
						.TryWrite(exdb);
						FailStart(msg);
					}
				}
				// Подключаемся к хостам RabbitMQ
				foreach (var host in RabbitHosts) {

						var rabbit_conn = new ConnectionConfiguration();
						rabbit_conn.PrefetchCount = Convert.ToUInt16((host.PrefetchCount > 0) ? host.PrefetchCount : 1);
						rabbit_conn.PublisherConfirms = true;
						//r_conn.Port = Convert.ToUInt16((host.Port > 0) ? host.Port : 5671);
						if (!String.IsNullOrWhiteSpace(host.VirtualHost)) {
							rabbit_conn.VirtualHost = host.VirtualHost;
						}
						if (!String.IsNullOrWhiteSpace(host.Username)) {
							rabbit_conn.UserName = host.Username;
							if (!String.IsNullOrWhiteSpace(host.Password)) {
								rabbit_conn.Password = host.Password;
							}
						}
						
						var r_host = new HostConfiguration();
						r_host.Host = host.Host;
						r_host.Port = Convert.ToUInt16((host.Port > 0) ? host.Port : host.SslEnabled ? 5671 : 5672);
						if (host.SslEnabled) {
							r_host.Ssl.Enabled = true;
							//r_host.Ssl.ServerName = host.Host;
							r_host.Ssl.AcceptablePolicyErrors = System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors | System.Net.Security.SslPolicyErrors.RemoteCertificateNotAvailable | System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;
						}
						rabbit_conn.Hosts = new[] { r_host };
						
						new LogRecord() {
							Source = _Name,
							HostId = host.Host_Id,
							Message = String.Format("Подключаемся к {0}серверу RabbitMQ...", host.SslEnabled ? "SSL " : ""),
							Details = host.ConnectionString }
						.TryWrite(SQLconn, dbLog);
						
						IEasyNetQLogger r_Log = new EasyNetQ.Loggers.NullLogger();
						if (DebugFlag.Enabled) {
							try {
								r_Log = new RabbitTextLogger(new StreamWriter(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName, String.Format("rabbit_{0}.log", host.Host_Id))) {AutoFlush = true});
								//r_Log = new EasyNetQ.Loggers.ConsoleLogger();
							} catch (Exception ex) {
								Trace.TraceWarning("{4}\t{0}: не удалось создать журнал rabbit_{1} ({2}): {3}", _Name, host.Host_Id, host.ConnectionString, ex.Message, DateTime.Now);
							}
						}
						
						try {
							rabbit_conn.Validate();
							var bus = RabbitHutch.CreateBus(rabbit_conn, services => services.Register<IEasyNetQLogger>(logger => r_Log));
							Buses.Add(bus);
							foreach (var q in RabbitQueues.Where(q => q.Host_Id == host.Host_Id)) {
								q.Bus = bus;
							}
							
							for (int i = 0; i < 10; i++) {
								if (bus.IsConnected) {
									new LogRecord() {
										Source = _Name,
										HostId = host.Host_Id,
										Message = String.Format("Установлено {0}подключение к RabbitMQ", host.SslEnabled ? "SSL " : ""),
										Details = host.ConnectionString }
									.TryWrite(SQLconn, dbLog);
									
									break;
								}
								Thread.Sleep(10);
							}
							
						} catch (Exception ex) {
							new LogRecord() {
								Source = _Name,
								IsError = true,
								Message = String.Format("Ошибка при {0}подключении к серверу RabbitMQ: {1}", host.SslEnabled ? "SSL " : "", ex.Message),
								Details = rabbit_conn.ToString() }
							.TryWrite(SQLconn, dbLog);
						}
//					}
				}
				
				if (Buses.Count == 0) {
					var msg = "Не удалось подключиться ни к одному из хостов RabbitMQ. Остановка службы";
					new LogRecord() {
						Source = _Name,
						IsError = true,
						Message = msg }
					.TryWrite(SQLconn, dbLog);
					FailStart(msg);
				}
				
				// Запускаем обработчики очередей
				workers = new List<Worker>();
				foreach (var queue in RabbitQueues.Where(q => q.Bus != null)) {
					Worker w;
					
					if (queue.Direction.Trim() == "In") {
						w = new Receiver(dbConnStr, (IBus)queue.Bus, queue);	
					} else if (queue.Direction.Trim() == "Out") {
						w = new Sender(dbConnStr, (IBus)queue.Bus, queue);
					} else {
						new LogRecord() {
							Source = _Name,
							HostId = queue.Host.Host_Id,
							QueueId = queue.Queue_Id,
							IsError = true,
							Message = String.Format("Неизвестный тип очереди: '{0}'", queue.Direction),
							Details = queue.Name }
						.TryWrite(SQLconn, dbLog);
						
						continue;
					}
					
					new LogRecord() {
						Source = _Name,
						HostId = queue.Host.Host_Id,
						QueueId = queue.Queue_Id,
						Message = "Запускаем обработчик очереди сообщений...",
						Details = w.QueueFullName }
					.TryWrite(SQLconn, dbLog);
					
					w.Run();
					workers.Add(w);
				}
				
				timer = new System.Timers.Timer(30000D);
				timer.AutoReset = true;
				timer.Elapsed += timer_Elapsed;
				timer.Start();
				
				#if DEBUG
				while (!workers.All(w => w.Thread.Join(3000))) {
					continue;
				}
				#endif
			}
		}
		
		private void FailStart(string format, params object[] args)
		{
			FailStart(String.Format(format, args));
		}
		private void FailStart(string message)
		{
			this.ExitCode = 1;
			this.EventLog.WriteEntry(message, EventLogEntryType.Error);
			Trace.TraceError("{0}\t{1}", message, DateTime.Now);
			this.Stop();
		}

		void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (!Monitor.TryEnter(TimerLock) || ShouldStop) {
				return;
			}
			
			using (var exdb = new exDb(dbConnStr)) {
				foreach (var bus in Buses.Where(b => !b.IsConnected)) {
					new LogRecord() {
						Source = _Name,
						HostId = RabbitQueues.FirstOrDefault(q => bus == q.Bus).Host_Id,
						IsError = true,
						Message = "Утеряно соединение с сервером RabbitMQ. Выполняются попытки переподключения...",
						Details = RabbitQueues.FirstOrDefault(q => bus == q.Bus).Host.ConnectionString }
					.TryWrite(dbConnStr, dbLog);
				}
				
				foreach (var w in workers) {
					if (!w.IsAlive) {
						new LogRecord() {
							Source = _Name,
							HostId = w.HostId,
							QueueId = w.QueueId,
							Message = "Обнаружено непредвиденное завершение обработчика очереди сообщений. Перезапускаем обработчик...",
							Details = w.QueueFullName }
						.TryWrite(dbConnStr, dbLog);
						w.Respawn();
					}
				}
			}
			
			Monitor.Exit(TimerLock);
		}
		
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			this.ShouldStop = true;
			timer.Enabled = false;
			
			new LogRecord() {
				Source = _Name,
				Message = "Получена команда остановиться. Останавливаем обработчики..." }
			.TryWrite(dbConnStr, dbLog);
			
			foreach (var w in workers) {
				w.ShouldStop = true;
			}
			
			for (int i = 0; i < 50; i++) {
				if (workers.All(w => !w.IsAlive)) {
					break;
				}
				Thread.Sleep(250);
			}
			
			foreach (var w in workers.Where(w => w.IsAlive)) {
				new LogRecord() {
					Source = _Name,
					HostId = w.HostId,
					QueueId = w.QueueId,
					IsError = true,
					Message = "Обработчик не остановился за отведенное время. Принудительное завершение работы...",
					Details = w.QueueFullName }
				.TryWrite(dbConnStr, dbLog);
				
				w.Kill();
			}
			
			new LogRecord() {
					Source = _Name,
					Message = "Все обработчики остановлены. Завершение работы службы" }
			.TryWrite(dbConnStr, dbLog);
			
			Trace.TraceInformation("{1}\t{0}: нормальное завершение работы", _Name, DateTime.Now);
		}
	}
}
