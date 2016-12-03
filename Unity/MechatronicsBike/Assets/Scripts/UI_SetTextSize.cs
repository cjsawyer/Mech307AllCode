using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_SetTextSize : MonoBehaviour {

	public float precentOfParrentHeight = 0.8f;
	private Text text;
	private RectTransform parentTransform;

	void Update () {


		if (text==null) {
			parentTransform = transform.parent.gameObject.GetComponent<RectTransform>();
			text = GetComponent<Text>();
		}


		float parrentHeight = parentTransform.rect.height;
		// Unity font sizes are in pixels
		text.fontSize = (int)(precentOfParrentHeight * parrentHeight);

	}

}