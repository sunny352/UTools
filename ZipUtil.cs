using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using UniRx;
using UnityEngine;

public static class ZipUtil
{
	public static IEnumerator Create(string name)
	{
		string pakPath = string.Format("{0}/{1}.pak", Application.temporaryCachePath, name);
		string srcPath = string.Format("{0}/{1}", Application.persistentDataPath, name);
		return Observable.Start(() =>
		{
			FastZip fastZip = new FastZip();
			fastZip.CreateZip(pakPath, srcPath, true, null);
			return Unit.Default;
		}).ObserveOnMainThread().ToYieldInstruction();
	}
	public static IEnumerator Extract(string name)
	{
		string pakPath = string.Format("{0}/{1}.pak", Application.temporaryCachePath, name);
		string srcPath = string.Format("{0}/{1}", Application.persistentDataPath, name);
		return Observable.Start(() =>
		{
			FastZip fastZip = new FastZip();
			fastZip.ExtractZip(pakPath, srcPath, string.Empty);
			return Unit.Default;
		}).ObserveOnMainThread().ToYieldInstruction();
	}
}
