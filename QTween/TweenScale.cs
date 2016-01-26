using UnityEngine;
using System.Collections;

public class TweenScale : Tweener {

	protected override void Enable () {
		if(rewindOnEnable) {
			transform.localScale = from;
		}
	}

	void Update () {
		transform.localScale = Tween(transform.localScale);
	}
}
