using System.Collections;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
	public float Delay;
	IEnumerator Start()
	{
		yield return new WaitForSeconds(Delay);
		Object.Destroy(gameObject);
	}
}
