using UnityEngine;
using System.Collections;

public class TweenRotation : Tweener {

	protected override void Enable (){
		if(rewindOnEnable) {
			if (useLocal) {
				transform.localRotation = Quaternion.Euler(from);
			} else {
				transform.rotation = Quaternion.Euler(from);
			}
		}
	}

	void Update () {
		if(useLocal) {
			transform.localRotation = Quaternion.Euler(Tween(transform.localRotation.eulerAngles));
		} else {
			transform.rotation = Quaternion.Euler(Tween(transform.rotation.eulerAngles));
		}
	}
}
