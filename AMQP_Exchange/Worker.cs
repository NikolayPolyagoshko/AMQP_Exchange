/*
 * Создано в SharpDevelop.
 * Пользователь: N.Polyagoshko
 * Дата: 12.08.2015
 * Время: 12:04
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AMQP_Db;
using EasyNetQ;

namespace AMQP_Exchange
{
	/// <summary>
	/// Description of Worker.
	/// </summary>
	abstract class Worker
	{
		protected readonly string dbConnStr;
		// Была мысль, что Worker должен работать с Connection, а не ConnectionString, но:
		// DataContext сам управляет пулами соединений, если при создании указать ConnectionString, но не в случае Connection 
		
		// protected readonly IDbConnection _dbConn;
		protected readonly TextWriter dbTextLog;
		protected readonly IBus Bus;
		protected readonly RabbitQueue Queue;
		protected readonly string _Name;
		
		public int HostId {get {return Queue.Host.Host_Id;}}
		public int QueueId {get {return Queue.Queue_Id;}}
		public string QueueName {get {return Queue.Name;}}
		public string QueueFullName {get {return String.Format("{0} : {1}", Queue.Name, Queue.Direction);}}
		public string ExchangeName {get {return Queue.Exchange;}}
		//public IDbConnection dbConn {get {return _dbConn;}}
		
		public bool ShouldStop {get; set;}
		public Thread Thread {get; private set;}
		public bool IsAlive {get {return this.Thread != null && this.Thread.IsAlive;}}
		
		protected Worker(string connStr, IBus bus, RabbitQueue queue, string name)
		{
			if (String.IsNullOrEmpty(connStr)) {
				throw new ArgumentNullException("connStr");
			}
			if (bus == null) {
				throw new ArgumentNullException("bus");
			}
			if (queue == null) {
				throw new ArgumentNullException("queue");
			}
			dbConnStr = connStr;
			
			if (DebugFlag.Enabled) {
				try {
					dbTextLog = new StreamWriter(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName, String.Format("{0}.dbTextLog", _Name))) {AutoFlush = true};
				} catch (Exception ex) {
					Trace.TraceWarning("{2}\t{0}: не удалось создать журнал dbTextLog: {1}", _Name, ex.Message, DateTime.Now);
				}
			}
			
			Bus = bus;
			Queue = queue;
			_Name = name;
		}
		
		
		public abstract void Start();
		
		public void SafeStart()
		{
			try {
				this.Start();
				
			} catch (Exception ex) {
				
				new LogRecord() {
					Source = this._Name,
					HostId = this.HostId,
					QueueId = this.QueueId,
					Message = "Сбой обработчика!",
					IsError = true,
					Details = ex.Message }
				.TryWrite(dbConnStr, dbTextLog);
				
				if (!ShouldStop) {
					Respawn();
				}
				
				//throw;
			}
		}
		
		public void Run()
		{
			if (Thread != null && Thread.IsAlive) {
				throw new ThreadStateException();
			}
			this.Thread = new Thread(new ThreadStart( this.SafeStart ));
			this.Thread.IsBackground = true;
			this.Thread.Start();
		}
		
		public void Kill(int Delay = 100)
		{
			if (!IsAlive) {
				return;
			}
			ShouldStop = true;
			if (!this.Thread.Join(Delay)) {
				this.Thread.Abort();
			}
		}
		
		public void Respawn()
		{
			if (ShouldStop) {
				return;
			}
			if (IsAlive) {
				Kill();
			}
			Run();
		}
	}
}
