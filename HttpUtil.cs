using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpUtil
{
	public enum ENFileExist
	{
		enReplace,	//替换
		enCancel,	//取消下载
	}
	public delegate void DownloadCallback(int current, int total, float progress);
	public delegate void FinishedCallback(int downloaded);
	public static IEnumerator Download(string url, string fileName, ENFileExist existOpt, DownloadCallback downloadCallback, FinishedCallback finishedCallback)
	{
		string finalFilePath = Application.persistentDataPath + "/" + fileName;
		FileInfo finalFileInfo = new FileInfo(finalFilePath);
		if (finalFileInfo.Exists)
		{
			switch (existOpt)
			{
				case ENFileExist.enReplace:
					break;
				case ENFileExist.enCancel:
					yield break;
				default:
					break;
			}
		}
		string tempFilePath = Application.temporaryCachePath + "/" + fileName + ".tmp";
		FileInfo tempFileInfo = new FileInfo(tempFilePath);
		using (FileStream fileStream = File.Open(tempFilePath, tempFileInfo.Exists ? FileMode.Append : FileMode.CreateNew))
		{
			using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET, new DownloadFileHandler(fileStream, downloadCallback, finishedCallback), null))
			{
				if (tempFileInfo.Exists)
				{
					request.SetRequestHeader("RANGE", string.Format("bytes={0}-", tempFileInfo.Length));
				}
				yield return request.Send();
			}
		}
		if (finalFileInfo.Exists)
		{
			File.Delete(finalFilePath);
		}
		File.Move(tempFilePath, finalFilePath);
		File.Delete(tempFilePath);
	}
}
