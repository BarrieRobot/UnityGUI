using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class StateManager : MonoBehaviour {

	public UDPReceive udpreceiver;
	public GameObject NFCScanLockedOverlay;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("update");
		ReceiveStateChange ();
		HandleState ();
	}

	void HandleState () {
		switch (CurrentState.currentState) {
		case State.WAIT_FOR_NFC:
			NFCScanLockedOverlay.SetActive (true);
			break;
		case State.SELECTING:
			Debug.Log ("disable panel");
			NFCScanLockedOverlay.SetActive (false);
			break;
		}
	}

	// Checks for RFID message
	void ReceiveStateChange() {
		if (udpreceiver != null) {
			Debug.Log ("checking id");
			int rfidid = udpreceiver.getLastReceivedRFID ();
			Debug.Log ("checking id: " + rfidid);

			if (rfidid != null && rfidid != -1) {
				Debug.Log ("set selecting");
				CurrentState.currentState = State.SELECTING;
			} else {
				CurrentState.currentState = State.WAIT_FOR_NFC;
			}
		}
	}
}
