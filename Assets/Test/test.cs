using UnityEngine;
using System.Collections;
using Log;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LogCollector.Show ();
		StartCoroutine (GenerateLog ());
	}

	IEnumerator GenerateLog()
	{
		for (int i=0; i<20; ++i) 
		{
			yield return new WaitForSeconds(1);
			Logger.Info("info: " + i);
			Logger.Warning("warning: " + i);
			Logger.Error("Error: " + i);
		}
		yield return null;
	}
}
