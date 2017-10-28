using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;
using SimpleJSON;

public class TouchInterpreter : MonoBehaviour {

	public UDPReceive receiver;
	List<Point> myList = new List<Point>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (receiver != null) {
			string packet = receiver.getLatestUDPPacket ();
			if (packet != null) {

				myList.Clear ();
				var N = JSON.Parse(packet);
				//Debug.Log (N ["cursors"].AsArray [0].AsArray);
				JSONArray points = N ["cursors"].AsArray;
				foreach (object p in points) {
					JSONNode point = JSON.Parse (p.ToString());
				//	Debug.Log (point.AsArray[0].AsFloat + " " + point.AsArray[1].AsFloat);
					myList.Add (new Point (point.AsArray [0].AsFloat, point.AsArray [1].AsFloat));
				}
			//	foreach (var point in N["cursors"].AsArray[0].AsArray) {
			//		Debug.Log (point[0] + "," + point[1]);
			//	}
				//Debug.Log ("parsed " + N["cursors"].AsArray[0].AsArray[0] + "" + N["cursors"].AsArray[0].AsArray[0]);
			}
		}
	}


	public Point getAverageTouchPoint() {
		float sumX = 0;
		float sumY = 0;
		foreach (Point p in myList) {
			sumX += p.x;
			sumY += p.y;
		}
		Point result = new Point (sumX / myList.Count, sumY / myList.Count);
		Debug.Log ("Average hit is: [" + result.x + ", " + result.y + "]");
		return result;
	}

	public Point getScaledAverageTouchPoint () {
		return scalePoint (getAverageTouchPoint ());
	}

	// Scales to [-5 ... 5]
	public Point scalePoint(Point p) {
		return new Point ((p.x - 0.5f) * 10, p.y);
	}
}
