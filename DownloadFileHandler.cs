using System.IO;
using UnityEngine.Networking;

public class DownloadFileHandler : DownloadHandlerScript
{
	private FileStream mFileStream;
	private HttpUtil.DownloadCallback mDownloadCallback;
	private HttpUtil.FinishedCallback mFinishedCallback;
	public DownloadFileHandler(FileStream fileStream, HttpUtil.DownloadCallback downloadCallback, HttpUtil.FinishedCallback finishedCallback)
	{
		mFileStream = fileStream;
		mDownloadCallback = downloadCallback;
		mFinishedCallback = finishedCallback;
	}
	private int totalLength;
	private int downloadedLength;
	protected override void CompleteContent()
	{
		if (null != mFinishedCallback)
		{
			mFinishedCallback(totalLength);
		}
	}

	protected override float GetProgress()
	{
		return (float)downloadedLength / (float)totalLength;
	}

	protected override void ReceiveContentLength(int contentLength)
	{
		totalLength = contentLength;
	}

	protected override bool ReceiveData(byte[] data, int dataLength)
	{
		downloadedLength += dataLength;
		mFileStream.Write(data, 0, dataLength);
		if (null != mDownloadCallback)
		{
			mDownloadCallback(downloadedLength, totalLength, GetProgress());
		}
		return true;
	}
}
