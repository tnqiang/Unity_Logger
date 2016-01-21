using UnityEngine;
using System.Diagnostics;

namespace Log
{
	public class Logger
	{	
		public static bool showLog = true;
		[Conditional("LOG_DETAIL")]	
		public static void Log(object obj = null)
		{
			if (showLog) {
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.Log (obj + stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void Log(object message, Object context)
		{
			if (showLog) {
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.Log (message, context);
				UnityEngine.Debug.Log (stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void LogWarning(object obj = null)
		{
			if (showLog) {
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.LogWarning (obj + stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void LogWarning(object message, Object context)
		{
			if (showLog) {
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.LogWarning (message, context);
				UnityEngine.Debug.LogWarning (stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void LogError(object obj = null)
		{
			if (showLog) {
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.LogError (obj + stackInfo);
			}
		}

		[Conditional("LOG_DETAIL")]	
		public static void LogError(object message, Object context)
		{
			if (showLog) {
				string stackInfo = new StackTrace (false).ToString ();
				UnityEngine.Debug.LogError (message, context);
				UnityEngine.Debug.LogError (stackInfo);
			}
		}
	}
}
