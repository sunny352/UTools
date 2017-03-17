using UnityEngine;
using UnityEngine.Profiling;

//用于显示游戏运行帧率和内存占用
public static class ShowProfile
{
	public static float SampleTime = 0.5f;
	private static float m_lastUpdateTime = 0.0f;
	private static float m_fps = 0.0f;
	private static int m_frame = 0;
	public static void Update()
	{
		++m_frame;
	}
	public static void FixedUpdate()
	{
		float currentTime = Time.time;
		float flameTime = currentTime - m_lastUpdateTime;
		if (flameTime >= SampleTime)
		{
			m_fps = m_frame / flameTime;
			m_lastUpdateTime = currentTime;
			m_frame = 0;
			long monoMemory = System.GC.GetTotalMemory(false);
			uint totalMemory = Profiler.GetTotalAllocatedMemory();
			m_info = string.Format("Total:{0:0.0}MB\nMono:{1:0.0}MB\nFPS:{2:0.0}", totalMemory / 1024.0f / 1024.0f, monoMemory / 1024.0f / 1024.0f, m_fps);
		}
	}

	private static Rect m_place = new Rect(0.0f, 0.0f, 300, 100);
	private static string m_info = string.Empty;
	public static void OnGUI()
	{
		GUI.Label(m_place, m_info);
	}
	public static string GetInfo()
	{
		return m_info;
	}
}
