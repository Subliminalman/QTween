using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tweener : MonoBehaviour {
	public int group = -1;
	public Vector3 from, to;
	public float time;
	public float delay = -1f;
	public bool playOnAwake = true;
	public bool rewindOnEnable = false;
	public bool loop;
	public bool useLocal;
	public int triggerGroup = -1;
	public float triggerTime = -1f;
	public AnimationCurve curve;
	[HideInInspector] protected bool playTween = false;
	bool triggered = false;
	bool waiting = false;
	float currentTime = 0f;

	void OnEnable() {
		TweenTrigger.OnTrigger += FireTrigger;
		TweenTrigger.OnStop += HandleOnStop;
		TweenTrigger.OnStopAll += HandleOnStopAll;
		Enable();
		triggered = false;
		if(playOnAwake) {
			if(delay >= 0) {
				StartCoroutine(WaitAndPlay());
			} else {
				PlayTween();
			}
		}
	}
	
	void OnDisable() {
		TweenTrigger.OnTrigger -= FireTrigger;
		TweenTrigger.OnStop -= HandleOnStop;
		TweenTrigger.OnStopAll -= HandleOnStopAll;
		waiting = false;
		StopAllCoroutines();
	}

	void Awake() {
		if(playOnAwake) {
			if(delay >= 0) {
				StartCoroutine(WaitAndPlay());
			} else {
				playTween = playOnAwake;
			}
		}
	}

	protected void FireTrigger(int _group) {
		if(group < 0) {
			return;
		}

		if(group == _group) {
			if(delay >= 0) {
				StartCoroutine(WaitAndPlay());
			} else {
				PlayTween ();
			}
		}
	}

	protected void HandleOnStopAll () {
		StopAllCoroutines();
		playTween = false;
	}

	protected void HandleOnStop (int _group) {
		if(_group >= 0 && group == _group) {
			StopAllCoroutines();
			playTween = false;
		}
	}

	IEnumerator WaitAndPlay() {
		if(waiting) {
			yield break;
		}

		waiting = true;

		yield return new WaitForSeconds (delay);
		waiting = false;
		PlayTween();
	}

	public void PlayTween() {
		currentTime = 0f;
		playTween = true;
	}

	protected Vector3 Tween(Vector3 vector) {
		if(curve == null || curve.length == 0 || !playTween) {
			return vector;
		}

		if(triggerTime >= 0 && currentTime >= triggerTime && triggerGroup >= 0 && !triggered) {
			triggered = true;
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
				return vector;
			}
		}

		float totalCurve = curve[curve.length - 1].time;
		
		currentTime += Time.deltaTime;
			
		float curvePos = curve.Evaluate(currentTime / Mathf.Max (time, 0.01f));
		
		Vector3 tween = from + ((to - from) * curvePos);

		return tween;
	}

	virtual protected void Enable(){}
}
