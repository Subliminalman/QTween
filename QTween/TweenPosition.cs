using UnityEngine;
using System.Collections;

public class TweenPosition : Tweener {

	protected override void Enable () {
		if(rewindOnEnable) {
			if(useLocal) {
				transform.localPosition = from;
			} else {
				transform.position = from;
			}
		}
	}

	void Update () {
		if(useLocal) {
			transform.localPosition = Tween (transform.localPosition);
		} else {
			transform.position = Tween (transform.position);
		}
	}
}
