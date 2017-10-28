﻿using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.VR;

public class UDPReceive : MonoBehaviour {

	private Thread receiveThread;
	private UdpClient client;
	private bool running = true;

	// public string IP = "127.0.0.1"; default local
	public int port; // define > init

	// infos
	public string lastReceivedUDPPacket="";

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

				//print(">> " + text);
				lastReceivedUDPPacket=text;
				//Debug.Log("Received: " + lastReceivedUDPPacket);
			}
			catch (Exception err) {
				Debug.LogException (err);
			}
			//Thread.Sleep (100);
		}
	}

	void OnDisable() 
	{ 
		//running = false;
		if ( receiveThread!= null) 
			receiveThread.Abort(); 

		client.Close(); 
	} 


	// getLatestUDPPacket
	// cleans up the rest
	public string getLatestUDPPacket() {
		return lastReceivedUDPPacket;
	}
}