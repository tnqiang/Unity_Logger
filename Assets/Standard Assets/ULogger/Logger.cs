using UnityEngine;
using System.Diagnostics;

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
			public string		content;
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

		private static void BroardCastLogDetail(LogLevel level, string content)
		{
			if (null != s_onLogOccur) 
			{
				LogDetail logDetail = new LogDetail();
				logDetail.level = level;
				logDetail.content = content;
				s_onLogOccur(logDetail);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Info (object message)
		{
			if (logLevel >= LogLevel.INFO)
			{
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.Log (message);
				BroardCastLogDetail (LogLevel.INFO, message + stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Warning (object message, Object context)
		{
			if (logLevel > LogLevel.INFO)
			{
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.LogWarning (message);
				BroardCastLogDetail (LogLevel.WARNING, message + stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Error (object message, Object context)
		{
			if (logLevel > LogLevel.WARNING)
			{
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.LogError (message);
				BroardCastLogDetail (LogLevel.ERROR, message + stackInfo);
			}
		}
	}
}
