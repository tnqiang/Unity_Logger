using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NSUListView;
using System.Text.RegularExpressions;
using System.IO;
using System;

namespace Log
{
	public class LogCollector : MonoBehaviour
	{
		#region Singleton
		public static LogCollector pInstance 
		{
			get 
			{
				if (instance == null)
				{
					instance = (LogCollector)UnityEngine.Object.FindObjectOfType (typeof(LogCollector));
				}
				
				if (instance == null) 
				{
					GameObject prefab = Resources.Load ("LogCollector") as GameObject;
					GameObject GO = GameObject.Instantiate (prefab) as GameObject;
					//GO.hideFlags = GO.hideFlags | HideFlags.HideAndDontSave;	// Only hide it if this manager was autocreated
					instance = GO.GetComponent<LogCollector> ();
				}
				
				DontDestroyOnLoad (instance.gameObject);
				return instance;
			}
		}

		static LogCollector instance;
		#endregion

		public LoggerListView 			listView;
		public Text						txtDetail;
		private List<Logger.LogDetail> 	m_lstLogDetail;
		private List<Logger.LogDetail> 	m_lstFilteredLog;
		private	bool					m_logInfoEnable;
		private bool					m_logWaringEnable;
		private bool					m_logErrorEnable;
		private string					m_regEx;
		private string					m_logFileName;
		private StreamWriter			m_fileWriter;

		public static void Init()
		{
			LogCollector instance = LogCollector.pInstance;
			instance.DoInit ();
			instance.HideUI ();
		}

		public void ShowInUI ()
		{
			LogCollector collector = LogCollector.pInstance;
			if (false == collector.gameObject.activeSelf) 
			{
				collector.gameObject.SetActive (true);
			}
		}
		
		public void HideUI ()
		{
			LogCollector collector = LogCollector.pInstance;
			if (collector.gameObject.activeSelf) 
			{
				collector.gameObject.SetActive (false);
			}
		}

		public void ShowInBrowser()
		{
			if (null != m_fileWriter) 
			{
				m_fileWriter.Flush();
			}
			if (!string.IsNullOrEmpty (m_logFileName)) 
			{
				Application.OpenURL (m_logFileName);
			}
		}

		public void Clear ()
		{
			m_lstLogDetail.Clear ();
			RefreshView ();
		}
		 
		public void OnFileterChangeInfo(bool state)
		{
			if (state) 
			{
				AddLogFilter (Logger.LogLevel.INFO);
			} 
			else 
			{
				RemoveLogFilter(Logger.LogLevel.INFO);
			}
		}

		public void OnFileterChangeWarning(bool state)
		{
			if (state) 
			{
				AddLogFilter (Logger.LogLevel.WARNING);
			} 
			else 
			{
				RemoveLogFilter(Logger.LogLevel.WARNING);
			}
		}

		public void OnFileterChangeError(bool state)
		{
			if (state) 
			{
				AddLogFilter (Logger.LogLevel.ERROR);
			} 
			else 
			{
				RemoveLogFilter(Logger.LogLevel.ERROR);
			}
			ShowInBrowser ();
		}

		public void OnFilterChange (string regEx)
		{
			m_regEx = regEx;
			RefreshView ();
		}

		private void AddLogFilter (Logger.LogLevel logLevel)
		{
			switch (logLevel) 
			{
			case Logger.LogLevel.INFO:
				m_logInfoEnable = true;
				break;
			case Logger.LogLevel.WARNING:
				m_logWaringEnable = true;
				break;
			case Logger.LogLevel.ERROR:
				m_logErrorEnable = true;
				break;
			}
			Debug.Log ("logLevel: " + logLevel);
			RefreshView ();
		}

		private void RemoveLogFilter (Logger.LogLevel logLevel)
		{
			switch (logLevel) 
			{
			case Logger.LogLevel.INFO:
				m_logInfoEnable = false;
				break;
			case Logger.LogLevel.WARNING:
				m_logWaringEnable = false;
				break;
			case Logger.LogLevel.ERROR:
				m_logErrorEnable = false;
				break;
			}
			RefreshView ();
		}

		private void DoInit ()
		{
			Logger.OnLogOccur += OnLogOccur;
			m_lstLogDetail = new List<Logger.LogDetail> ();
			m_lstFilteredLog = new List<Logger.LogDetail> ();
			LoggerListView listView = GetComponentInChildren<LoggerListView> ();
			listView.OnClicked += OnClick;
			m_logInfoEnable = true;
			m_logWaringEnable = true;
			m_logErrorEnable = true;
			m_regEx = string.Empty;
			CreateLogFile ();
		}

		void OnDestroy ()
		{
			Logger.OnLogOccur -= OnLogOccur;
			listView.OnClicked -= OnClick;
			m_lstLogDetail.Clear ();
			CloseLogFile ();
		}

		private void OnLogOccur (Logger.LogDetail logDetail)
		{
			if (null != logDetail) 
			{
				m_lstLogDetail.Add (logDetail);
				RefreshView ();
				AppendToFile(logDetail);
			}
		}

		private void RefreshView ()
		{
			List<object> lstData = new List<object> ();
			m_lstFilteredLog.Clear ();
			for (int i=0; i<m_lstLogDetail.Count; ++i) 
			{
				if (IsLogFiltered (m_lstLogDetail [i])) 
				{
					m_lstFilteredLog.Add (m_lstLogDetail [i]);
					lstData.Add (m_lstLogDetail [i]);
				}
			}
			listView.SetData (lstData);
		}

		private bool IsLogFiltered (Logger.LogDetail logDetail)
		{
			bool ret = false;
			switch (logDetail.level) 
			{
			case Logger.LogLevel.INFO:
				if (m_logInfoEnable) ret = true;
				break;
			case Logger.LogLevel.WARNING:
				if (m_logWaringEnable) ret = true;
				break;
			case Logger.LogLevel.ERROR:
				if (m_logErrorEnable) ret = true;
				break;
			}

			// reg compare
			if (!string.IsNullOrEmpty (m_regEx) && ret) 
			{
				Regex regex = new Regex (m_regEx);
				ret = regex.IsMatch (logDetail.userLog);
			}

			return ret;
		}

		private void OnClick (int index)
		{
			if (index != -1 && m_lstFilteredLog.Count > index) 
			{
				txtDetail.text = m_lstFilteredLog [index].detail 
					+ m_lstFilteredLog [index].detail;
			}
		}

		private void CreateLogFile()
		{
			DateTime now = DateTime.Now;
			m_logFileName = Application.persistentDataPath + "/log_" + now.ToString ("yyyy_mm_dd__hh_mm_ss") + ".html";
			m_fileWriter = new StreamWriter (m_logFileName);
		}

		private void AppendToFile(Logger.LogDetail logDetail)
		{
			string logLine = "<p><font color=\"log_color\">log_content</font></p>\n";
			string color = "black";
			if (null != m_fileWriter)
			{
				switch(logDetail.level)
				{
				case Logger.LogLevel.INFO:
					color = "black";
					break;
				case Logger.LogLevel.WARNING:
					color = "maroon";
					break;
				case Logger.LogLevel.ERROR:
					color = "red";
					break;
				}
				logLine = logLine.Replace ("log_color", color);
				logLine = logLine.Replace ("log_content", logDetail.detail);
				m_fileWriter.WriteLine(logLine);
			}
		}

		private void CloseLogFile()
		{
			if (null != m_fileWriter) 
			{
				m_fileWriter.Close();
				m_fileWriter = null;
			}
		}
	}
}