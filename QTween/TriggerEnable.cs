using UnityEngine;
using System.Collections;

public class TriggerEnable : MonoBehaviour {
	public int group = -1;
	public bool setActive = false;
	public bool playOnAwake = false;
	public bool rewindOnEnable = false;
	public MonoBehaviour component;


	void OnEnable() {
		TweenTrigger.OnTrigger += FireTrigger;

		if(rewindOnEnable) {
			if(component) {
				component.enabled = !setActive;
			}
		}

		if(playOnAwake) {
			component.enabled = setActive;
		}
	}

	void OnDisable() {
		TweenTrigger.OnTrigger -= FireTrigger;
	}

	void FireTrigger(int _group) {
		if(group < 0) {
			return;
		}
		
		if(group == _group) {
			if(component) {
				component.enabled = setActive;
			}
		}
	}
}
