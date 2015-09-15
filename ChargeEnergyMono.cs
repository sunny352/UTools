using UnityEngine;
using System.Collections;

public class ChargeEnergyMono : MonoBehaviour
{
	public float PreTime;
	public float Duration;
	public int MaxLevel;
	public GameObject Target;
	public bool IsFireOnFinished = false;
	public bool IsFireOnEnd = true;
	void OnBegin()
	{
		Debug.Log("OnBegin");
		m_isCharging = true;
		m_beginTime = Time.time;
	}
	void OnEnd()
	{
		Debug.Log("OnEnd");
		if (m_isCharging)
		{
			if (IsFireOnEnd)
			{
				OnFire(Level);
			}
		}
		Reset();
	}
	void OnInterupt()
	{
		m_isCharging = false;
		m_beginTime = 0.0f;
		if (null != Target)
		{
			Target.SendMessage("OnChargeInterupt", SendMessageOptions.DontRequireReceiver);
		}
		Reset();
	}
	public void Reset()
	{
		m_isCharging = false;
		m_beginTime = 0.0f;
		m_preLevel = 0;
	}
	private float m_beginTime;
	private bool m_isCharging;
	public int Level
	{
		get
		{
			float currentTime = Time.time;
			float chargeDuration = currentTime - m_beginTime - PreTime;
			if (chargeDuration < 0)
			{
				return 0;
			}
			int level = (int)(chargeDuration / Duration) + 1;
			return Mathf.Min(level, MaxLevel);
		}
	}
	public bool IsCharging { get { return m_isCharging; } }
	private int m_preLevel = 0;
	void FixedUpdate()
	{
		if (m_isCharging)
		{
			int currentLevel = Level;
			if (currentLevel != m_preLevel)
			{
				m_preLevel = currentLevel;
				if (1 == currentLevel)
				{
					OnStart();
				}
				OnLevelUp(currentLevel);
				if (currentLevel == MaxLevel)
				{
					OnFinished();
				}
			}
		}
	}
	private void OnStart()
	{
		if (null != Target)
		{
			Target.SendMessage("OnChargeStart", SendMessageOptions.DontRequireReceiver);
		}
	}
	private void OnFire(int currentLevel)
	{
		if (null != Target)
		{
			Target.SendMessage("OnChargeFire", currentLevel, SendMessageOptions.DontRequireReceiver);
		}
	}
	private void OnLevelUp(int currentLevel)
	{
		if (null != Target)
		{
			Target.SendMessage("OnChargeLevelUp", currentLevel, SendMessageOptions.DontRequireReceiver);
		}
	}
	private void OnFinished()
	{
		if (null != Target)
		{
			Target.SendMessage("OnChargeFinished", SendMessageOptions.DontRequireReceiver);
		}
		if (IsFireOnFinished)
		{
			OnFire(Level);
			Reset();
		}
	}
}
