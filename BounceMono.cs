using UnityEngine;

public class BounceMono : MonoBehaviour
{
	public Transform Target;
	public float Value = 5.0f;
	public float MaxSpeed = 10.0f;
	public float MinSpeed = 2.0f;
	void LateUpdate()
	{
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
