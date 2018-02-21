/*
 * Создано в SharpDevelop.
 * Пользователь: N.Polyagoshko
 * Дата: 10.08.2015
 * Время: 16:54
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AMQP_Db
{
	public partial class exDb
	{
	  	public bool TrySubmitChanges(bool Rethrow = false)
	  	{
	  		using (var sw = new StringWriter()) {
	  			TextWriter stored_Log = this.Log;
	  			this.Log = sw;
	  			
	  			try {
	  				base.SubmitChanges();
	  				return true;
	  			} catch (Exception ex) {
	  				Trace.TraceError("Ошибка при работе с БД: {0}\nТекст запроса: {1}", ex.Message, sw.ToString());
	  				if (Rethrow) {
	  					throw;
	  				}
	  				return false;
	  			} finally {
	  				if (stored_Log != null) {
	  					stored_Log.Write(sw.ToString());
	  				}
	  				this.Log = stored_Log;
	  			}
	  		}	  		
	  	}
	}
	
	public partial class LogRecord
	{
		public bool Write(exDb db)
		{
			db.LogTable.InsertOnSubmit(this);
			return db.TrySubmitChanges();
		}
		public bool Write(string connStr, TextWriter dbLog = null)
		{
			using (var exdb = new exDb(connStr)) {
				exdb.Log = dbLog;
				exdb.LogTable.InsertOnSubmit(this);
				return exdb.TrySubmitChanges();
			}
		}
		public bool Write(IDbConnection conn, TextWriter dbLog = null)
		{
			using (var exdb = new exDb(conn)) {
				exdb.Log = dbLog;
				exdb.LogTable.InsertOnSubmit(this);
				return exdb.TrySubmitChanges();
			}
		}
	}
	
	public partial class RabbitHost
	{
		private string _ConnectionString;
		public string ConnectionString { 
			get {
				if (!String.IsNullOrEmpty(_ConnectionString)) {
					return _ConnectionString;
				} else {
					var sb = new StringBuilder();
					sb.AppendFormat("host={0}", Host);
					if (Port > 0) {
						sb.AppendFormat(":{0}", Port);
					}
					if (!String.IsNullOrWhiteSpace(VirtualHost)) {
						sb.AppendFormat(";virtualHost={0}", VirtualHost);
					}
					if (!String.IsNullOrWhiteSpace(Username)) {
						sb.AppendFormat(";username={0}", Username);
						if (!String.IsNullOrWhiteSpace(Password)) {
							sb.AppendFormat(";password={0}", Password);
						}
					}
					sb.AppendFormat(";prefetchcount={0}", (PrefetchCount > 0) ? PrefetchCount : 1);
					sb.Append(";publisherConfirms=true");
					
					return (_ConnectionString = sb.ToString());
				}
			}
		}
	}
	
	public partial class RabbitQueue
	{
		public object Bus {get; set;}
	}
}