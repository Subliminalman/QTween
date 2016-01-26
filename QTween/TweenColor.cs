using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TweenColor : MonoBehaviour {
	public int group = -1;
	public SpriteRenderer sprite;
	public Image image;
	public Text text;
	public Color from = new Color(1f,1f,1f,0f), to = Color.white;
	public float time;
	public float delay = -1f;
	public bool playOnAwake = true;
	public bool rewindOnEnable = false;
	public bool loop;
	public int triggerGroup = -1;
	public float triggerTime = -1f;
	public AnimationCurve curve;
	bool playTween;
	bool waiting = false;
	bool triggered = false;
	float currentTime = 0f;

	void Awake() {
		if(sprite == null) {
			sprite = gameObject.GetComponent<SpriteRenderer>();
		}

		if(image == null) {
			image = gameObject.GetComponent<Image>();
		}

		if(text == null) {
			text = gameObject.GetComponent<Text>();
		}

		if(delay >= 0) {
			if(playOnAwake){
				StartCoroutine(WaitAndPlay());
			}
		} else {
			playTween = playOnAwake;
		}
	}

	void OnEnable() {
		TweenTrigger.OnTrigger += FireTrigger;
		TweenTrigger.OnStop += HandleOnStop;
		TweenTrigger.OnStopAll += HandleOnStopAll;
		StopAllCoroutines();
		waiting = false;
		triggered = false;
		currentTime = 0f;
		if(rewindOnEnable) {
			ResetColor();
		}

		if(delay >= 0) {
			if(playOnAwake) {
				StartCoroutine(WaitAndPlay());
			}
		} else {
			playTween = playOnAwake;
		}
	}



	void OnDisable() {
		TweenTrigger.OnTrigger -= FireTrigger;
		TweenTrigger.OnStop -= HandleOnStop;
		TweenTrigger.OnStopAll -= HandleOnStopAll;
		Rewind ();
		ResetColor();
		waiting = false;
		StopAllCoroutines();
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
		playTween = false;
		currentTime = 0f;
		if(sprite!= null) {
			sprite.color = from;
		}

		if(text != null) {
			text.color = from;
		}

		if(image != null ) {
			image.color = from;
		}
	}

	void Update() {
		if(playTween) {
			Tween();
		}
	}

	void Tween() {
		if((sprite == null && image == null && text == null) || curve == null || curve.length == 0) {
			return;
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
				return;
			}
		}
		
		float totalCurve = curve[curve.length - 1].time;
		
		currentTime += Time.deltaTime;
		
		float curvePos = curve.Evaluate(currentTime / Mathf.Max (time, 0.01f));

		Color lerpColor = Color.Lerp(from, to, curvePos);

		if(sprite) {
			sprite.color = lerpColor;
		}

		if(image) {
			image.color = lerpColor;
		}

		if(text) {
			text.color = lerpColor;
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

	protected void HandleOnStop (int _group) {
		if(_group >= 0 && group == _group) {
			StopAllCoroutines();
			playTween = false;
		}
	}

	void HandleOnStopAll () {
		StopAllCoroutines();
		playTween = false;
	}

	void ResetColor() {
		if(sprite) {
			sprite.color = from;
		}
		
		if(image) {
			image.color = from;
		}
		
		if(text) {
			text.color = from;
		}
	}
}
