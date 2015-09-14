using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class EditorSlerpCurve
{
	[MenuItem("UTools/SlerpCurve")]
	public static void CreateSlerpCurve()
	{
		string filePath = EditorUtility.SaveFilePanelInProject("Slerp Curve", string.Empty, "asset", string.Empty);
		if (string.IsNullOrEmpty(filePath))
		{
			return;
		}
		SlerpCurve obj = ScriptableObject.CreateInstance<SlerpCurve>();
		List<Vector3> posList = new List<Vector3>();
		foreach (var gameObject in Selection.gameObjects)
		{
			posList.Add(gameObject.transform.position);
		}
		if (posList.Count > 1)
		{
			obj.Points = posList.ToArray();
		}
		AssetDatabase.CreateAsset(obj, filePath);
	}
}
