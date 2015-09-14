#define _CD_Use_Singlton_
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CDManager
{
#if _CD_Use_Singlton_
	public static CDManager Instance { get; private set; }
	static CDManager()
	{
		Instance = new CDManager();
	}
#endif
	public float CurrentTime { get { return Time.fixedTime; } }
	public bool RegisterCD(int id, float totalSecond, Action<int> onFinished, Action<int, float> onRefresh)
	{
		var cd = m_cdList.Find(item => item.ID == id);
		if (null == cd)
		{
			cd = new CDData(this, id, totalSecond);
			cd.OnFinished += onFinished;
			cd.OnRefresh += onRefresh;
			m_cdList.Add(cd);
			return true;
		}
		else
		{
			if (cd.TotalSecond == totalSecond)
			{
				cd.OnFinished += onFinished;
				cd.OnRefresh += onRefresh;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	public void UnregisterCD(int id, Action<int> onFinished, Action<int, float> onRefresh)
	{
		var cd = m_cdList.Find(item => item.ID == id);
		if (null != cd)
		{
			cd.OnFinished -= onFinished;
			cd.OnRefresh -= onRefresh;
		}
	}
	public bool StartCD(int id)
	{
		var cd = m_cdList.Find(item => item.ID == id);
		if (null != cd)
		{
			cd.Start();
			return true;
		}
		else
		{
			return false;
		}
	}
	public void ClearCD(int id)
	{
		var cd = m_cdList.Find(item => item.ID == id);
		if (null != cd)
		{
			cd.Clear();
		}
	}
	public void OnFixedUpdate()
	{
		for (int index = 0; index < m_cdList.Count; ++index)
		{
			m_cdList[index].OnFixedUpdate();
		}
	}
	public List<CDData> CDList { get { return m_cdList; } }
	private List<CDData> m_cdList = new List<CDData>();
}

public class CDData
{
	public int ID { get; private set; }
	public Action<int> OnFinished;
	public Action<int, float> OnRefresh;
	public float RemainSecond
	{
		get
		{
			float currentTime = m_manager.CurrentTime;
			float remain = TotalSecond - (currentTime - m_startSecond);
			return remain > 0 ? remain : 0;
		}
	}
	private float m_startSecond;
	public float TotalSecond { get; private set; }
	private CDManager m_manager;
	public bool IsCDRunning { get; private set; }
	public CDData(CDManager manager, int id, float totalSecond)
	{
		m_manager = manager;
		ID = id;
		TotalSecond = totalSecond;
		IsCDRunning = false;
	}
	public void Start()
	{
		m_startSecond = Time.fixedTime;
		IsCDRunning = true;
	}
	public void Clear()
	{
		m_startSecond = 0.0f;
		IsCDRunning = false;
	}
	public void Restart()
	{
		IsCDRunning = true;
		m_startSecond = m_manager.CurrentTime;
	}
	public void OnFixedUpdate()
	{
		if (IsCDRunning)
		{
			float remain = RemainSecond;
			OnCDRefresh(remain);
			if (remain <= 0)
			{
				OnCDFinished();
				IsCDRunning = false;
			}
		}
	}
	public void OnCDFinished()
	{
		if (null != OnFinished)
		{
			OnFinished(ID);
		}
	}
	public void OnCDRefresh(float remain)
	{
		if (null != OnRefresh)
		{
			OnRefresh(ID, remain);
		}
	}
}