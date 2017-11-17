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
		ReceiveStateChange ();
		HandleState ();
	}

	void HandleState () {
		switch (CurrentState.currentState) {
		case State.WAIT_FOR_NFC:
			NFCScanLockedOverlay.SetActive (true);
			break;
		case State.SELECTING:
			NFCScanLockedOverlay.SetActive (false);
			break;
		}
	}

	void ReceiveStateChange() {
		if (udpreceiver != null) {
			string packet = udpreceiver.getLatestUDPPacket ();
			if (packet != null) {
				var N = JSON.Parse(packet);

				if (N != null && N ["state"] != null) { 
					//TODO better check for message
					int state = N ["state"].AsInt;
					switch(state) {
					case (int)State.WAIT_FOR_NFC:
						CurrentState.currentState = State.WAIT_FOR_NFC;
						break;
					case (int)State.SELECTING: 
						CurrentState.currentState = State.SELECTING;
						break;
						//	Debug.Log (point.AsArray[0].AsFloat + " " + point.AsArray[1].AsFloat);
					}
				}
			}
		}
	}
}
