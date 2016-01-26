using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TweenFill : MonoBehaviour {
	public int group = -1;
	public Image image;
	public float from, to, time, delay = -1f;
	public bool playOnAwake = true;
	public bool rewindOnEnable = false;
	public bool loop;
	public int triggerGroup = -1;
	public float triggerTime = -1f;
	public AnimationCurve curve;
	bool waiting;
	bool playTween;
	bool triggered = false;
	float currentTime = 0f;
	
	void Awake() {
		if(delay >= 0) {
			StartCoroutine(WaitAndPlay());
		} else {
			playTween = playOnAwake;
		}
	}
	
	void OnEnable() {
		TweenTrigger.OnTrigger += FireTrigger;
		TweenTrigger.OnStop += HandleOnStop;
		TweenTrigger.OnStopAll += HandleOnStopAll;
		currentTime = 0f;
		triggered = false;
		if(rewindOnEnable) {
			Rewind();
		}

		if(delay >= 0) {
			StartCoroutine(WaitAndPlay());
		} else {
			playTween = playOnAwake;
		}
	}
	
	void OnDisable() {
		TweenTrigger.OnTrigger -= FireTrigger;
		TweenTrigger.OnStop -= HandleOnStop;
		TweenTrigger.OnStopAll -= HandleOnStopAll;
		Rewind ();
	}

	IEnumerator WaitAndPlay() {
		if(waiting) {
			yield break;
		}
		waiting = true;
		yield return new WaitForSeconds(delay);
		waiting = false;
		playTween = true;
	}
	
	void Rewind() {
		currentTime = 0f;

		if(image != null ) {
			image.fillAmount = from;
		}
	}
	
	void Update() {
		if(playTween) {
			Tween();
		}
	}
	
	void Tween() {
		if( image == null || curve == null || curve.length == 0) {
			return;
		}
		
		if(triggerTime >= 0 && currentTime >= triggerTime && triggerGroup >= 0 && !triggered) {
			triggered = false;
			TweenTrigger.PlayTween(triggerGroup);
		}
		
		if(currentTime >= time) {
			if(triggerGroup >= 0 && triggerTime < 0f) {
				TweenTrigger.PlayTween(triggerGroup);
			}
			
			if (loop) {
				currentTime = 0f;
			} else {
				playTween = false;
				return;
			}
		}

		currentTime += Time.deltaTime;
		
		float curvePos = 1f - curve.Evaluate(currentTime / Mathf.Max (time, 0.01f));
			
		if(image) {
			image.fillAmount = curvePos;
		}		
	}
	
	void FireTrigger(int _group) {
		if(group < 0) {
			return;
		}
		
		if(group == _group) {
			currentTime = 0f;
			if(delay >= 0) {
				StartCoroutine(WaitAndPlay());
			} else {
				playTween = true;
			}
		}
	}

	protected void HandleOnStop(int _group) {
		if(_group >= 0 && group == _group) {
			StopAllCoroutines();
			playTween = false;
		}
	}

	
	void HandleOnStopAll () {
		StopAllCoroutines();
		playTween = false;
	}
}
