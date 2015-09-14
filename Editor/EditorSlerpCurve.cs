using UnityEngine;
using System.Collections;
using UnityEditor;

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
		AssetDatabase.CreateAsset(obj, filePath);
	}
}
