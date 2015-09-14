using System.Collections;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
	public float Delay;
	IEnumerator Start()
	{
		Debug.LogFormat("{0}", Time.time);
		yield return new WaitForSeconds(Delay);
		Debug.LogFormat("{0}", Time.time);
		Object.Destroy(gameObject);
	}
}
