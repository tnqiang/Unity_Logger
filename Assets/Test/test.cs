using UnityEngine;
using System.Collections;
using Log;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LogCollector collerctor = LogCollector.pInstance;
		StartCoroutine (GenerateLog ());
	}

	IEnumerator GenerateLog()
	{
		for (int i=0; i<100; ++i) {
			yield return new WaitForSeconds(1);
			Logger.Info("log: " + i);
		}
		yield return null;
	}
}
