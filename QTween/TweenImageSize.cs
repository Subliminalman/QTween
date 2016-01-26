using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TweenImageSize : Tweener {
	public Image image;


	protected override void Enable () {
		if(rewindOnEnable) {
			image.rectTransform.rect.Set (image.rectTransform.rect.x, image.rectTransform.rect.y, from.x, from.y);
		}
	}
	
	void Update () {
		//transform.localScale = Tween(transform.localScale);
		Vector3 size = Tween(image.rectTransform.rect.size);
		image.rectTransform.sizeDelta = size;
		//image.rectTransform.rect.Set(image.rectTransform.rect.x, image.rectTransform.rect.y, size.x, size.y);
	}
}
