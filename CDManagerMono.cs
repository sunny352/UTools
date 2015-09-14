using System.Collections.Generic;
using UnityEngine;

public class CDManagerMono : MonoBehaviour
{
	void Start()
	{
		List<CDDataMono> cdList = new List<CDDataMono>();
		for (int index = 0; index < CDList.Length; ++index)
		{
			if (cdList.Exists(item => item.ID == CDList[index].ID))
			{
				Debug.LogWarningFormat("ID {0} reused", CDList[index].ID);
				continue;
			}
			cdList.Add(CDList[index]);
		}
		CDList = cdList.ToArray();
	}
	public void StartCD(int id)
	{
		for (int index = 0; index < CDList.Length; ++index )
		{
			if (CDList[index].ID == id)
			{
				CDList[index].Start();
			}
		}
	}
	public void ClearCD(int id)
	{
		for (int index = 0; index < CDList.Length; ++index)
		{
			if (CDList[index].ID == id)
			{
				CDList[index].Clear();
			}
		}
	}

	void FixedUpdate()
	{
		for (int index = 0; index < CDList.Length; ++index)
		{
			CDList[index].OnFixedUpdate();
		}
	}
	public CDDataMono[] CDList;
}

[System.Serializable]
public class CDDataMono
{
	public int ID;
	public float TotalSecond;
	public GameObject gameObject;
	public float RemainSecond
	{
		get
		{
			float currentTime = Time.fixedTime;
			float remain = TotalSecond - (currentTime - m_startSecond);
			return remain > 0 ? remain : 0;
		}
	}
	private float m_startSecond;
	public bool IsCDRunning { get; private set; }
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
		if (null != gameObject)
		{
			gameObject.SendMessage("OnCDFinished", ID, SendMessageOptions.DontRequireReceiver);
		}
	}
	public void OnCDRefresh(float remain)
	{
		if (null != gameObject)
		{
			gameObject.SendMessage("OnCDRefresh", string.Format("{0}|{1}", ID, remain), SendMessageOptions.DontRequireReceiver);
		}
	}
}