﻿using UnityEngine;
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
		
		#endregion

		public IUListView				listView;
		private List<Logger.LogDetail> 	m_lstLogDetail;

		private void Initialize()
		{
			Logger.OnLogOccur += OnLogOccur;
			m_lstLogDetail = new List<Logger.LogDetail> ();
		}

		void OnDestroy()
		{
			Logger.OnLogOccur -= OnLogOccur;
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
	}
}