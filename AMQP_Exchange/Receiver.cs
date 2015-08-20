/*
 * Создано в SharpDevelop.
 * Пользователь: N.Polyagoshko
 * Дата: 11.08.2015
 * Время: 14:56
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMQP_Db;
using EasyNetQ;

namespace AMQP_Exchange
{
	/// <summary>
	/// Description of Receiver.
	/// </summary>
	class Receiver : Worker
	{
		private readonly Encoding Encoding;
		private TextWriter dbLog = null;
		
		public Receiver(string connStr, IBus bus, RabbitQueue queue) 
			: base(connStr, bus, queue, String.Format("receiver_{0}", queue.Queue_Id))
		{
			this.Encoding = (Queue.Codepage > 0) ? Encoding.GetEncoding((int)Queue.Codepage) : Encoding.UTF8;
		}
		
		public override void Start()
		{
			if (DebugFlag.Enabled) {
				try {
					dbLog = new StreamWriter(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName, String.Format("{0}.dblog", _Name))) {AutoFlush = true};
				} catch (Exception ex) {
					Trace.TraceWarning("{0}: не удалось создать журнал dblog: {1}", _Name, ex.Message);
				}
			}
			
			new LogRecord() {
				Source = this._Name,
				HostId = this.HostId,
				QueueId = this.QueueId,
				Message = "Обработчик получения сообщений запущен",
				Details = QueueFullName }
			.Write(dbStr, dbLog);
			
			while (!Bus.IsConnected) {
				Thread.Sleep(1000);
			}
			
			var aBus = Bus.Advanced;
			var queue = aBus.QueueDeclare(QueueName);
			var handle = aBus.Consume(queue, (body, messprops, messinfo) 
			             => Task.Factory.StartNew(()
			             => MessageHandler(body, messprops, messinfo)));
			
			while (!ShouldStop) {
				// FIXME: Если сервер игнорирует durable=true для очереди она не переживет его рестарт
				// при потере связи с сервером и последующем её восстановлении нужно проверять существование 
				// очереди и пересоздавать в случае необходимости
				// события aBus.Disconnected aBus.Connected
				Thread.Sleep(1000);
			}
			
			// Вышли из главного цикла (ShouldStop = true)
			handle.Dispose();
			new LogRecord() {
				Source = this._Name,
				HostId = this.HostId,
				QueueId = this.QueueId,
				Message = "Получена команда остановить обработчик. Нормальное завершение работы",
				Details = QueueFullName }
			.Write(dbStr, dbLog);
		}
		
		private void MessageHandler(Byte[] body, MessageProperties mp, MessageReceivedInfo mi)
		{
			using (var exdb = new exDb(dbStr)) {
				exdb.Log = dbLog;
				
				new LogRecord() {
					Source = this._Name,
					HostId = this.HostId,
					QueueId = this.QueueId,
					Message = String.Format("Принято сообщение {0} байт", body.Length),
					Details =  this.QueueFullName }
				.Write(exdb);
			
				var msg = Queue.Base64Data ? Convert.ToBase64String(body) 
					: this.Encoding.GetString(body);
				
				var record = new Inbound() {
				    QueueId = this.QueueId,
				    DateReceived = DateTime.Now,
				    Message = msg };
				exdb.Inbound.InsertOnSubmit(record);
				exdb.TrySubmitChanges();
			
				new LogRecord() {
					Source = this._Name,
					HostId = this.HostId,
					QueueId = this.QueueId,
					Inbound_Id = record.Message_Id,
					Message = String.Format("Записали {0}сообщение в БД", Queue.Base64Data ? "base64 " : ""),
					Details = String.Format("{0} символов", msg.Length) }
				.Write(exdb);
			}
		}
	}
}
