using UnityEngine;
using System.Collections;
using System;

public class SlerpCurve : ScriptableObject
{
	public Vector3[] Points;
	void OnEnable()
	{
		m_controlPoint = PathControlPointGenerator(Points);
	}
	// from 0 to 1
	public Vector3 GetLinedPoints(float current)
	{
		if (current < 0.0f)
		{
			current = 0.0f;
		}
		if (current > 1.0f)
		{
			current = 1.0f;
		}
		return Interp(m_controlPoint, current);
	}
	private Vector3[] m_controlPoint;
	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		Vector3[] suppliedPath;
		Vector3[] vector3s;

		//create and store path points:
		suppliedPath = path;

		//populate calculate path;
		int offset = 2;
		vector3s = new Vector3[suppliedPath.Length + offset];
		Array.Copy(suppliedPath, 0, vector3s, 1, suppliedPath.Length);

		//populate start and end control points:
		//vector3s[0] = vector3s[1] - vector3s[2];
		vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
		vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);

		//is this a closed, continuous loop? yes? well then so let's make a continuous Catmull-Rom spline!
		if (vector3s[1] == vector3s[vector3s.Length - 2])
		{
			Vector3[] tmpLoopSpline = new Vector3[vector3s.Length];
			Array.Copy(vector3s, tmpLoopSpline, vector3s.Length);
			tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
			tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
			vector3s = new Vector3[tmpLoopSpline.Length];
			Array.Copy(tmpLoopSpline, vector3s, tmpLoopSpline.Length);
		}

		return (vector3s);
	}
	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int numSections = pts.Length - 3;
		int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
		float u = t * (float)numSections - (float)currPt;

		Vector3 a = pts[currPt];
		Vector3 b = pts[currPt + 1];
		Vector3 c = pts[currPt + 2];
		Vector3 d = pts[currPt + 3];

		return .5f * (
			(-a + 3f * b - 3f * c + d) * (u * u * u)
			+ (2f * a - 5f * b + 4f * c - d) * (u * u)
			+ (-a + c) * u
			+ 2f * b
		);
	}
}
