using UnityEngine;
using System.Collections;

public class TriggerActive : MonoBehaviour {
	public int group = -1;
	public bool setActive = false;
	public bool startOnEnable = false;
	public bool rewindOnEnable = false;
	public float waitTime = -1f;
	public GameObject target;
	
	
	void OnEnable() {
		TweenTrigger.OnTrigger += FireTrigger;
		
		if(rewindOnEnable) {
			if(target) {
				target.SetActive(!setActive);
			}
		}

		if(waitTime >= 0f && startOnEnable) {
			StartCoroutine(WaitAndFire());
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
			StartCoroutine(WaitAndFire());
		}
	}

	IEnumerator WaitAndFire() {
		yield return new WaitForSeconds (Mathf.Max (0f,waitTime));
		if(target) {
			target.SetActive(setActive);
		}
	}
}
