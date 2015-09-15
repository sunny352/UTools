using UnityEngine;
using System.Collections;
using System;

public class ChargeEnergy
{
	public float PreTime;
	public float Duration;
	public int MaxLevel;
	public bool IsFireOnFinished = false;
	public bool IsFireOnEnd = true;

	public Action OnChargeStart;
	public Action<int> OnChargeFire;
	public Action<int> OnChargeLevelUp;
	public Action OnChargeFinished;
	public Action OnChargeInterupt;
	public void OnBegin()
	{
		Debug.Log("OnBegin");
		m_isCharging = true;
		m_beginTime = Time.time;
	}
	public void OnEnd()
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
	public void OnInterupt()
	{
		m_isCharging = false;
		m_beginTime = 0.0f;
		if (null != OnChargeInterupt)
		{
			OnChargeInterupt();
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
	public void FixedUpdate()
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
		if (null != OnChargeStart)
		{
			OnChargeStart();
		}
	}
	private void OnFire(int currentLevel)
	{
		if (null != OnChargeFire)
		{
			OnChargeFire(currentLevel);
		}
	}
	private void OnLevelUp(int currentLevel)
	{
		if (null != OnChargeLevelUp)
		{
			OnChargeLevelUp(currentLevel);
		}
	}
	private void OnFinished()
	{
		if (null != OnChargeFinished)
		{
			OnChargeFinished();
		}
		if (IsFireOnFinished)
		{
			OnFire(Level);
			Reset();
		}
	}
}
