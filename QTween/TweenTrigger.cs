using UnityEngine;
using System.Collections;

public class TweenTrigger : MonoBehaviour {
	public int group = -1;
	public float delay = -1f;

	float currentTime = 0f;

	public delegate void Trigger(int _group);
	public static event Trigger OnTrigger;

	public delegate void Stop(int _group);
	public static event Stop OnStop;

	public delegate void StopAll();
	public static event StopAll OnStopAll;

	void OnEnable() {
		if(delay >= 0f) {
			StartCoroutine(WaitAndTrigger());
		}
	}

	public void PlayTween() {
		PlayTween (group);
	}

	public static void PlayTween(int _group) {
		if(OnTrigger != null && _group >= 0) {
			OnTrigger(_group);
		}
	}

	public void StopTween() {
		StopTween (group);
	}

	public static void StopTween(int _group) {
		if(OnStop != null && _group >= 0) {
			OnStop(_group);
		}
	}

	public void StopAllTweens() {
		if(OnStopAll != null) {
			OnStopAll();
		}
	}

	IEnumerator WaitAndTrigger() {
		yield return new WaitForSeconds(delay);
		PlayTween ();
	}
}
