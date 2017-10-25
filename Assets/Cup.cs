using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class Cup : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	void gestureStateChangedHandler(object sender, GestureStateChangeEventArgs e) {
		Debug.Log ("Gesture changed");
	}
	public void gest() {
		Debug.Log ("tap detected");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
