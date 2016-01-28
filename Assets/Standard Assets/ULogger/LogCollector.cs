using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NSUListView;

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
					instance = (LogCollector)Object.FindObjectOfType(typeof(LogCollector));
				
				if (instance == null)
				{
					GameObject prefab = Resources.Load("LogCollector") as GameObject;
					GameObject GO = GameObject.Instantiate(prefab) as GameObject;
					//GO.hideFlags = GO.hideFlags | HideFlags.HideAndDontSave;	// Only hide it if this manager was autocreated
					instance = GO.GetComponent<LogCollector>();
					instance.Initialize();
				}
				
				DontDestroyOnLoad(instance.gameObject);
				return instance;
			}
		}
		static LogCollector instance;

		public static void Show()
		{
			LogCollector collector = LogCollector.pInstance;
			if (false == collector.gameObject.activeSelf) 
			{
				collector.gameObject.SetActive(true);
			}
		}

		public static void Hide()
		{
			LogCollector collector = LogCollector.pInstance;
			if (collector.gameObject.activeSelf) 
			{
				collector.gameObject.SetActive(false);
			}
		}
		#endregion

		public LoggerListView 			listView;
		public Text						txtDetail;
		private List<Logger.LogDetail> 	m_lstLogDetail;

		private void Initialize()
		{
			Logger.OnLogOccur += OnLogOccur;
			m_lstLogDetail = new List<Logger.LogDetail> ();
			LoggerListView listView = GetComponentInChildren<LoggerListView> ();
			listView.OnClicked += OnClick;
		}

		void OnDestroy()
		{
			Logger.OnLogOccur -= OnLogOccur;
			listView.OnClicked -= OnClick;
			m_lstLogDetail.Clear ();
		}

		private void OnLogOccur(Logger.LogDetail logDetail)
		{
			if (null != logDetail) 
			{
				m_lstLogDetail.Add(logDetail);
				RefreshView();
			}
		}

		private void RefreshView()
		{
			List<object> lstData = new List<object> ();
			for (int i=0; i<m_lstLogDetail.Count; ++i) 
			{
				lstData.Add(m_lstLogDetail[i]);
			}
			listView.SetData (lstData);
		}

		private void OnClick(int index)
		{
			if (index != -1 && m_lstLogDetail.Count > index) 
			{
				txtDetail.text = m_lstLogDetail[index].content + m_lstLogDetail[index].content;
			}
		}
	}
}