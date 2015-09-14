using UnityEngine;

public class Bounce
{
	public Transform Target;
	public float Value;
	public float MaxSpeed;
	public float MinSpeed;

	private Transform transform;
	public void Enable()
	{
		m_isEnable = true;
	}
	public void Disable()
	{
		m_isEnable = false;
	}
	private bool m_isEnable = true;
	public void Init(Transform selfTrans)
	{
		transform = selfTrans;
	}
	public void LateUpdate()
	{
		if (!m_isEnable)
		{
			return;
		}
		if (null == Target)
		{
			return;
		}
		var distance = Target.position - transform.position;
		float speed = Mathf.Clamp(distance.sqrMagnitude * Value, MinSpeed, MaxSpeed);
		var transPos = Time.deltaTime * speed * distance.normalized;
		if (transPos.sqrMagnitude < distance.sqrMagnitude)
		{
			transform.Translate(transPos, Space.World);
		}
		else
		{
			transform.position = Target.position;
		}
	}
}
