using UnityEngine;
using System.Collections;

public class TriggerSound : MonoBehaviour {
	public int group = -1;
	public AudioClip audioClip;
	public AudioSource audioSource;
	public int triggerGroup = -1;
	public float triggerTime = -1f;
	public bool playOnAwake = false;

	bool previousIsPlaying = false;
	bool triggered = false; 

	void OnEnable () {
		TweenTrigger.OnTrigger += FireTrigger;
		TweenTrigger.OnStop += HandleOnStop;
		TweenTrigger.OnStopAll += HandleOnStopAll;
		triggered = false;
		if(playOnAwake) {
			PlaySound();
		}
	}

	void OnDisable () {
		TweenTrigger.OnTrigger -= FireTrigger;
		TweenTrigger.OnStop -= HandleOnStop;
		TweenTrigger.OnStopAll -= HandleOnStopAll;
	}

	void Update() {

		if(!audioSource) {
			return;
		}

		if(triggerTime >= 0 && audioSource.time >= triggerTime && triggerGroup >= 0 && !triggered) {
			triggered = true;
			TweenTrigger.PlayTween(triggerGroup);
		}

		if(previousIsPlaying && !audioSource.isPlaying && triggerGroup >= 0 && triggerTime < 0f) {
			TweenTrigger.PlayTween(triggerGroup);
		}

		previousIsPlaying = audioSource.isPlaying;
	}

	void FireTrigger(int _group) {
		if(_group >= 0 && _group == group) {
			PlaySound();
		}
	}

	protected void HandleOnStopAll() {
		StopAllCoroutines();
		audioSource.Stop ();
	}

	protected void HandleOnStop (int _group) {
		if(_group >= 0 && group == _group) {
			StopAllCoroutines();
			audioSource.Stop ();
		}
	}

	void PlaySound() {
		if(audioSource) {
			if(audioClip != null) {
				audioSource.PlayOneShot(audioClip);
			} else {
				audioSource.Play();
			}
		}
	}
}
