using UnityEngine;
using System.Diagnostics;
using System.Text;

namespace Log
{
	public class Logger
	{	
		public enum LogLevel
		{
			INFO,
			WARNING,
			ERROR
		}

		public class LogDetail
		{
			public LogLevel 	level;
			public string		userLog;
			public string		detail;
		}

		public static LogLevel logLevel = LogLevel.INFO;

		public delegate void LogOccurHandler(LogDetail detail);
		private static event LogOccurHandler s_onLogOccur;
		public static event LogOccurHandler OnLogOccur
		{
			add
			{
				s_onLogOccur -= value;
				s_onLogOccur += value;
			}
			remove
			{
				s_onLogOccur -= value;
			}
		}

		private static void BroardCastLogDetail(LogLevel level, string log)
		{
			if (null != s_onLogOccur) 
			{
				LogDetail logDetail = new LogDetail();
				logDetail.level = level;
				logDetail.userLog = log;
				string detail = log;
				StackTrace stackTrace = new StackTrace(true);
				StackFrame[] frames = stackTrace.GetFrames();
				StringBuilder builder = new StringBuilder();
				for(int i=2; i<frames.Length; ++i)
				{
					StackFrame f = frames[i-2];
					builder.AppendFormat("\n{0} {1}:{2} Line:{3}", 
					                     i-2, System.IO.Path.GetFileName(f.GetFileName()), 
					                     f.GetMethod().Name,
					                     f.GetFileLineNumber());
				}
				detail += builder.ToString();
				logDetail.detail = detail;
				s_onLogOccur(logDetail);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Info (string format, params object[] args)
		{
			if (logLevel < LogLevel.WARNING)
			{
				string message = string.Format(format, args);
				UnityEngine.Debug.Log (message);
				BroardCastLogDetail (LogLevel.INFO, message.ToString());
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Warning (string format, params object[] args)
		{
			if (logLevel < LogLevel.ERROR)
			{
				string message = string.Format(format, args);
				UnityEngine.Debug.LogWarning (message);
				BroardCastLogDetail (LogLevel.WARNING, message.ToString());
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Error (string format, params object[] args)
		{
			if (logLevel <= LogLevel.ERROR)
			{
				string message = string.Format(format, args);
				UnityEngine.Debug.LogError (message);
				BroardCastLogDetail (LogLevel.ERROR, message.ToString());
			}
		}
	}
}
