using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NSUListView;

namespace Log
{
	public class LoggerItemView : IUListItemView
	{
		public GameObject infoImg;
		public GameObject waringImg;
		public GameObject errorImg;
		public Text loggerText;

		public override void SetData (object data)
		{
			Logger.LogDetail logDetail = data as Logger.LogDetail;
			loggerText.text = logDetail.content;
			switch (logDetail.level) 
			{
			case Logger.LogLevel.INFO:
				infoImg.SetActive(true);
				waringImg.SetActive(false);
				errorImg.SetActive(false);
				break;
			case Logger.LogLevel.WARNING:
				infoImg.SetActive(false);
				waringImg.SetActive(true);
				errorImg.SetActive(false);
				break;
			case Logger.LogLevel.ERROR:
				infoImg.SetActive(false);
				waringImg.SetActive(false);
				errorImg.SetActive(true);
				break;
			}
		}
	}
}