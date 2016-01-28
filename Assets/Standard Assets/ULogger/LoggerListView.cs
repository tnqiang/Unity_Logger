using UnityEngine;
using System.Collections;
using NSUListView;

namespace Log
{
	public delegate void OnClickHandler(int index);
	public class LoggerListView : USimpleListView 
	{
		private event OnClickHandler m_onClick;
		public event OnClickHandler OnClicked
		{
			add{
				m_onClick -= value;
				m_onClick += value;
			}
			remove
			{
				m_onClick -= value;
			}
		}

		public override void OnClick(int index)
		{
			if (m_onClick != null) 
			{
				m_onClick(index);
			}
		}
	}
}