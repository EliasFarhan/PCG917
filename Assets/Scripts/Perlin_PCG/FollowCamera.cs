using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
	Camera mainCamera;
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = mainCamera.transform.position.to2();
	}
}
