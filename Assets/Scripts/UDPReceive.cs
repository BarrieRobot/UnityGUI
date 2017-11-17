using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.VR;

using SimpleJSON;

public class UDPReceive : MonoBehaviour {

	private Thread receiveThread;
	private UdpClient client;
	private bool running = true;

	// public string IP = "127.0.0.1"; default local
	public int port; // define > init

	// infos
	public string lastReceivedUDPPacket="";
	public int lastReceivedRFIDID = 0;
	private JSONArray lastReceivedCursors;

	// start from shell
	private static void Main() {
		UDPReceive receiveObj = new UDPReceive();
		receiveObj.init();

		string text="";
		do {
			text = Console.ReadLine();
		}
		while(!text.Equals("exit"));
	}

	// start from unity3d
	public void Start() {
		init();
	}

	// init
	private void init() {
		//print("UDPSend.init()");

		// define port
		port = 5005;

		// status
		//print(" \t to 127.0.0.1 : "+port);
		//print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");

		receiveThread = new Thread(
			new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();

	}

	// receive thread
	private  void ReceiveData() {

		client = new UdpClient(port);
		while (running) {

			try {
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);

				if (data == null || data.Length == 0){
					Debug.Log("Invalid data received, exiting..");
					return;
				}
				string text = Encoding.UTF8.GetString(data);
				lastReceivedUDPPacket=text;
				//Debug.Log("Received: " + lastReceivedUDPPacket);
			}
			catch (Exception err) {
				Debug.LogException (err);
			}
			ParseData ();
			//Thread.Sleep (100);
		}
	}

	void ParseData() {
		JSONNode data = JSON.Parse(lastReceivedUDPPacket);
		if (data != null) {
			if (data ["cursors"] != null) {
				lastReceivedCursors = data ["cursors"].AsArray;
			} else if (data ["rfid"] != null) {
				lastReceivedRFIDID = data ["rfid"].AsInt;
			} else {
				Debug.LogError ("Invalid data received: " + data);
			}
		}
	}

	void OnDisable() 
	{ 
		//running = false;
		if ( receiveThread!= null) 
			receiveThread.Abort(); 

		client.Close(); 
	} 

	public JSONArray getLastCursors() {
		return lastReceivedCursors;
	}

	// getLatestUDPPacket
	// cleans up the rest
	public string getLatestUDPPacket() {
		return lastReceivedUDPPacket;
	}
}