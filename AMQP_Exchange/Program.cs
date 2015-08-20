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
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace AMQP_Exchange
{
	static class Program
	{
		/// <summary>
		/// This method starts the service.
		/// </summary>
		static void Main()
		{
			try {
				Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(Path.GetTempPath(), Exchange_Svc.MyServiceName, "trace.log")));
			} catch (Exception ex) {
				Trace.TraceWarning("Main: не удалось создать журнал trace: {0}", ex.Message);
			}
			Trace.AutoFlush = true;
			
			#if !DEBUG
			// To run more than one service you have to add them here
			ServiceBase.Run(new ServiceBase[] { new Exchange_Svc() });
			#else
			DebugFlag.Enabled = true;
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			new Exchange_Svc().Start();
			#endif
		}
	}
}
