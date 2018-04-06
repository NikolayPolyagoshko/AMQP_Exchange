/*
 * Создано в SharpDevelop.
 * Пользователь: N.Polyagoshko
 * Дата: 11.08.2015
 * Время: 14:55
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AMQP_Db;
using EasyNetQ;
using EasyNetQ.Topology;

namespace AMQP_Exchange
{
	/// <summary>
	/// Description of Sender.
	/// </summary>
	class Sender : Worker
	{
		private readonly int PollInterval;
		private TextWriter dbLog = null;
		
		public Sender(string connStr, IBus bus, RabbitQueue queue)
			: base(connStr, bus, queue, String.Format("sender_{0}", queue.Queue_Id))
		{
			PollInterval = (Queue.SenderPollInterval > 0) ? (int)Queue.SenderPollInterval : 10000;
		}
		
		
		public override void Start()
		{
			var aBus = Bus.Advanced;
			
			if (DebugFlag.Enabled) {
				try {
					dbLog = new StreamWriter(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName, String.Format("{0}.dblog", _Name))) {AutoFlush = true};
				} catch (Exception ex) {
					Trace.TraceWarning("{2}\t{0}: не удалось создать журнал dblog: {1}", _Name, ex.Message, DateTime.Now);
				}
			}
			
			new LogRecord() {
				Source = this._Name,
				HostId = this.HostId,
				QueueId = this.QueueId,
				Message = "Обработчик отправки сообщений запущен",
				Details = QueueFullName }
			.TryWrite(dbConnStr, dbLog);
			
			while (!ShouldStop) {
				using (var exdb = new exDb(dbConnStr)) {
					exdb.Log = dbLog;
					Outbound message;
					
					while (aBus.IsConnected &&
					       null != (message = exdb.Outbound.FirstOrDefault(o => o.QueueId == this.QueueId && o.DateSent == null)))
					{
						new LogRecord() {
							Source = this._Name,
							HostId = this.HostId,
							QueueId = this.QueueId,
							Outbound_Id = message.Message_Id,
							Message = "Начинаем отправку сообщения...",
							Details = String.Format("{0} символов", message.Message.Length) }
						.TryWrite(exdb);
						
						byte[] data;
						try {
							data = Queue.Base64Data ? Convert.FromBase64String(message.Message)
								: Encoding.UTF8.GetBytes(message.Message);
							
						} catch (Exception ex) {
							new LogRecord() {
								Source = this._Name,
								HostId = this.HostId,
								QueueId = this.QueueId,
								Outbound_Id = message.Message_Id,
								IsError = true,
								Message = "Ошибка декодирования base64",
								Details = ex.Message }
							.TryWrite(exdb);
							
							message.ErrorFlag = true;
							message.DateSent = new DateTime(1900, 1, 1);
							exdb.TrySubmitChanges();
							continue;
						}
						
						new LogRecord() {
							Source = this._Name,
							HostId = this.HostId,
							QueueId = this.QueueId,
							Outbound_Id = message.Message_Id,
							Message = Queue.Base64Data ? "Декодировали base64" : "Сформировали массив байт",
							Details = String.Format("{0} байт", data.Length) }
						.TryWrite(exdb);
						
						try {
							var mp = new MessageProperties();
							if (!String.IsNullOrWhiteSpace(message.AppMsgId)) {
								mp.MessageId = message.AppMsgId;
							}
							
							IExchange ex = String.IsNullOrWhiteSpace(this.ExchangeName) ? Exchange.GetDefault() :
								new Exchange(this.ExchangeName);
							
							aBus.Publish(ex, this.QueueName,  true, false, mp, data);
						
						} catch (Exception ex) {
							new LogRecord() {
								Source = this._Name,
								HostId = this.HostId,
								QueueId = this.QueueId,
								Outbound_Id = message.Message_Id,
								IsError = true,
								Message = "Ошибка при отправке сообщения",
								Details = ex.Message }
							.TryWrite(exdb);
							Thread.Sleep(PollInterval);
							
							continue;
						}
						
						new LogRecord() {
							Source = this._Name,
							HostId = this.HostId,
							QueueId = this.QueueId,
							Outbound_Id = message.Message_Id,
							Message = "Сообщение успешно отправлено!",
							Details = this.QueueFullName }
						.TryWrite(exdb);
						
						message.DateSent = DateTime.Now;
						exdb.TrySubmitChanges();
					}
				}
				
				Thread.Sleep(PollInterval);
			}
			
			// Вышли из главного цикла (ShouldStop = true)
			new LogRecord() {
				Source = this._Name,
				HostId = this.HostId,
				QueueId = this.QueueId,
				Message = "Получена команда остановить обработчик. Нормальное завершение работы",
				Details = QueueFullName }
			.TryWrite(dbConnStr, dbLog);
		}
	}
}
