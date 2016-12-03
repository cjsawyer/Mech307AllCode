using UnityEngine;
using System.Collections;

public class SetCameraToScreenSpace : MonoBehaviour {

	// For use with an overlay canvas. Had to move to ScaleCanvasByCamera and a screenspace canvas to put sprites over UIButtons
	//*
	int sw, sh;

	void OnEnable() {
		Move();
	}

	void Move() {
		Camera.main.orthographicSize = Screen.height / 2;

		sh = (int)(2 * Camera.main.orthographicSize);
		sw = (int)(sh * Camera.main.aspect);
		Camera.main.transform.position = new Vector3 (sw/2f, sh/2f, -10);
	}
	//*/
}
