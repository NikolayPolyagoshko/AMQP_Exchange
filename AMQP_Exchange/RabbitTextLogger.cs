/*
 * Создано в SharpDevelop.
 * Пользователь: N.Polyagoshko
 * Дата: 14.08.2015
 * Время: 14:55
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.IO;
using EasyNetQ;

namespace AMQP_Exchange
{
	/// <summary>
	/// Description of RabbitTextLogger.
	/// </summary>
	
	public class RabbitTextLogger : IEasyNetQLogger
    {
		private readonly TextWriter Output;
        public bool Debug { get; set; }
        public bool Info { get; set; }
        public bool Error { get; set; }

        public RabbitTextLogger(TextWriter output)
        {
        	Output = output;
            Debug = true;
            Info = true;
            Error = true;
        }

        public void DebugWrite(string format, params object[] args)
        {
            if (!Debug) return;
            SafeWrite("DEBUG: " + format, args);
        }

        public void InfoWrite(string format, params object[] args)
        {
            if (!Info) return;
            SafeWrite("INFO: " + format, args);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            if (!Error) return;
            SafeWrite("ERROR: " + format, args);
        }

        public void SafeWrite(string format, params object[] args)
        {
            // even a zero length args paramter causes WriteLine to interpret 'format' as
            // a format string. Rather than escape JSON, better to check the intention of 
            // the caller.
            if (args.Length == 0)
            {
                Output.WriteLine(format);
            }
            else
            {
                Output.WriteLine(format, args);
            }
        }

        public void ErrorWrite(Exception exception)
        {
            Output.WriteLine(exception.ToString());
        }
	}
}
