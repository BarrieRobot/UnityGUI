using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingProduct : MonoBehaviour {

	private GameObject currentActive;
	private int currentIndex=0;
	bool dragging = false;

	public float cameraBoundaryLeft;
	public float cameraBoundaryRight;

	GameObject[] items = new GameObject[3];

	public GameObject soda;
	public GameObject snack;
	public GameObject coffee;
	public float gravitySpeed;
	public float centreBounds;
	public float itemZCoord = -1;

	public TouchInterpreter touchInput;

	float mouseresetTimer = 0;
	float mouseResetAfter = 1;

	// Use this for initialization
	void Start () {
		items[0]=soda;
		items[1]=snack;
		items[2]=coffee;
		currentActive = Instantiate(items[currentIndex], new Vector3 (0, 0, itemZCoord), Quaternion.identity);

	}

	void processTouchInput(Vector2 point)
	{
		dragging = true;
		Debug.Log ("onmousedrag");
		if (lastMousePos != 99) {
			Debug.Log ("moving");
			float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
			//Vector3 mouseloc = Camera.main.ScreenToWorldPoint (new Vector3 (point.x, point.y, distance_to_screen));
			currentActive.transform.position = new Vector3 (currentActive.transform.position.x + ((point.x + lastMousePos) * Time.deltaTime), 0, itemZCoord);
			Debug.Log (point.x);
			mouseresetTimer = 0;
		} else {
			float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
			//Vector3 mouseloc = Camera.main.ScreenToWorldPoint (new Vector3 (point.x, point.y, distance_to_screen));
			lastMousePos = point.x;
			Debug.Log ("not moving " + lastMousePos);
		}
	}
	float lastMousePos;
	// Update is called once per frame
	void Update () {
		mouseresetTimer += Time.deltaTime;
		if (mouseresetTimer > mouseResetAfter) {
			dragging = false;
			Debug.Log ("resetting");
			mouseresetTimer = 0;
			lastMousePos = 99;
		}


		processTouchInput (touchInput.getScaledAverageTouchPoint ());

		if (currentActive.transform.localPosition.x > cameraBoundaryRight) {
			//currentIndex = (currentIndex-1) % items.Length;
			currentIndex = (currentIndex - 1);
			if (currentIndex < 0)
				currentIndex = items.Length - 1;
			Destroy (currentActive);
			currentActive = Instantiate (items [currentIndex], new Vector3 (cameraBoundaryLeft + 1, 0, itemZCoord), Quaternion.identity);
		} else if (currentActive.transform.localPosition.x < cameraBoundaryLeft) {
			//currentIndex = (currentIndex+1) % items.Length;
			currentIndex = (currentIndex + 1);
			if (currentIndex > items.Length - 1)
				currentIndex = 0;
			Destroy (currentActive);
			currentActive = Instantiate (items [currentIndex], new Vector3 (cameraBoundaryRight - 1, 0, itemZCoord), Quaternion.identity);
		} else if (!dragging) {
			if (currentActive.transform.position.x < -centreBounds)
				currentActive.transform.position = new Vector3 (currentActive.transform.position.x + gravitySpeed * Time.deltaTime, 0, itemZCoord);
			else if (currentActive.transform.position.x > centreBounds)
				currentActive.transform.position = new Vector3 (currentActive.transform.position.x - gravitySpeed * Time.deltaTime, 0, itemZCoord);
		}
	}
}
