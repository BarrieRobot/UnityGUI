using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChoosingProduct : MonoBehaviour {

	private GameObject currentActive;
	private int currentIndex=0;
	bool dragging = false;

	public float cameraBoundaryLeft;
	public float cameraBoundaryRight;

	public List<GameObject> sodas;
	public List<GameObject> coffees;
	public List<GameObject> items;

	public float gravitySpeed;
	public float centreBounds;
	public float itemZCoord = -1;

	public TouchInterpreter touchInput;

	float mouseresetTimer = 0;
	float mouseResetAfter = 1;

	public float moveSpeed; 

	private float resetToCatsTimer = 0;
	public float resetToCategoriesTime = 10;

	public Vector2 confirmAreaMin;
	public Vector2 confirmAreaMax;

	private bool switching = false;

	public Vector2 leftArrowMin;
	public Vector2 leftArrowMax;
	public Vector2 rightarrowMin;
	public Vector2 rightArrowMax;

	bool confirmed = false;

	private bool moving;
	// Use this for initialization
	void Start () {
		//if (CurrentCategory.category.Equals(ECategory.COFFEE)) {
			currentActive = Instantiate (coffees [currentIndex], new Vector3 (0, 0, itemZCoord), Quaternion.identity);
			items = coffees;
		confirmed = false;
		//} else if (CurrentCategory.category.Equals(ECategory.SODA)) {
		//	currentActive = Instantiate (sodas [currentIndex], new Vector3 (0, 0, itemZCoord), Quaternion.identity);
		//	items = sodas;
		//}
	}

	void processTouchInput(Vector2 point)
	{
		dragging = true;
		Debug.Log ("onmousedrag");
		if (lastMousePos != 99) {
			Debug.Log ("moving");
			float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
			//Vector3 mouseloc = Camera.main.ScreenToWorldPoint (new Vector3 (point.x, point.y, distance_to_screen));
			currentActive.transform.position = new Vector3 (currentActive.transform.position.x + ((point.x * moveSpeed/*+ lastMousePos*/) * Time.deltaTime), 0, itemZCoord);
			Debug.Log (point.x);
			mouseresetTimer = 0;
		} else {
			float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
			//Vector3 mouseloc = Camera.main.ScreenToWorldPoint (new Vector3 (point.x, point.y, distance_to_screen));
			lastMousePos = point.x;
			Debug.Log ("not moving " + lastMousePos);
		}
	}

	void moveLeftOrRight(Vector2 point) {
		if (isTouchInBounds (point, leftArrowMin, leftArrowMax)) {
			StartCoroutine ("MoveRight");
		} else if (isTouchInBounds (point, rightarrowMin, rightArrowMax)) {
			StartCoroutine ("MoveLeft");
		}
	}

	IEnumerator MoveRight () {
		moving = true;
		while (currentActive.transform.position.x < cameraBoundaryRight) {
			yield return new WaitForSeconds (0.01f);
			currentActive.transform.position = new Vector3 (currentActive.transform.position.x + (moveSpeed * Time.deltaTime), 0, itemZCoord);	
		}
	/*	currentIndex = (currentIndex - 1);
		if (currentIndex < 0)
			currentIndex = items.Count - 1;
		//Destroy (currentActive);*/
		moving = false;
	}

	IEnumerator MoveLeft () {
		moving = true;
		while (currentActive.transform.position.x > cameraBoundaryLeft) {
			yield return new WaitForSeconds (0.01f);
			currentActive.transform.position = new Vector3 (currentActive.transform.position.x - (moveSpeed * Time.deltaTime), 0, itemZCoord);
		}
	/*	currentIndex = (currentIndex + 1);
		if (currentIndex > items.Count - 1)
			currentIndex = 0;
		//Destroy (currentActive);*/
		moving = false;
	}

	IEnumerator ResetAfter2Sec () {
		yield return new WaitForSeconds (2);
		goToCategoryChoice ();
	}

	/*
	void processCategoryChange() {
		List<Vector2> touches = touchInput.getTouchPoints ();
		if (!switching) {
			switching = true;
			foreach (Vector2 touch in touches) {
				if (isTouchInBounds(touch, coffeeMinSelection, coffeeMaxSelection)) {

					StartCoroutine ("ChooseItem");
					return;
				} else if (isTouchInBounds(touch, sodaMinSelection, sodaMaxSelection)) {

					StartCoroutine ("ChooseItem");
					return;
				}
			}
		}

	}*/
	float lastMousePos;
	// Update is called once per frame
	void Update () {
		mouseresetTimer += Time.deltaTime;
		resetToCatsTimer += Time.deltaTime;
		if (mouseresetTimer > mouseResetAfter) {
			dragging = false;
			Debug.Log ("resetting");
			mouseresetTimer = 0;
			lastMousePos = 99;
		}
		if (resetToCatsTimer > resetToCategoriesTime) {
			resetToCatsTimer = 0;
			goToCategoryChoice ();
		}

		if (!confirmed) {
			if (isTouchInBounds (touchInput.getScaledAverageTouchPoint (), confirmAreaMin, confirmAreaMax)) {
				confirmSelection ();
			}

			if (!moving)
				moveLeftOrRight (touchInput.getAverageTouchPoint ());
			//processTouchInput (touchInput.getScaledAverageTouchPoint ());
			//processCategoryChange ();
			processItems ();
		}
	}

	bool isTouchInBounds(Vector2 touch, Vector2 minBound, Vector2 maxBound) {
		if (touch.x > minBound.x && touch.x < maxBound.x
			&& touch.y > minBound.y && touch.y < maxBound.y) {
			return true;
		}
		return false;
	}

	void confirmSelection() {
		confirmed = true;
		Rigidbody rig = currentActive.GetComponent<Rigidbody> ();
		rig.isKinematic = false;
		StartCoroutine ("ResetAfter2Sec");
	}

	void processItems() {
		if (currentActive.transform.localPosition.x > cameraBoundaryRight) {
			//currentIndex = (currentIndex-1) % items.Length;
			currentIndex = (currentIndex - 1);
			if (currentIndex < 0)
				currentIndex = items.Count - 1;
			Destroy (currentActive);
			currentActive = Instantiate (items [currentIndex], new Vector3 (cameraBoundaryLeft + 1, 0, itemZCoord), Quaternion.identity);
		} else if (currentActive.transform.localPosition.x < cameraBoundaryLeft) {
			//currentIndex = (currentIndex+1) % items.Length;
			currentIndex = (currentIndex + 1);
			if (currentIndex > items.Count - 1)
				currentIndex = 0;
			Destroy (currentActive);
			currentActive = Instantiate (items [currentIndex], new Vector3 (cameraBoundaryRight - 1, 0, itemZCoord), Quaternion.identity);
		} else if (!moving) { 
			// Gravitate
			if (currentActive.transform.position.x < -centreBounds)
				currentActive.transform.position = new Vector3 (currentActive.transform.position.x + gravitySpeed * Time.deltaTime, 0, itemZCoord);
			else if (currentActive.transform.position.x > centreBounds)
				currentActive.transform.position = new Vector3 (currentActive.transform.position.x - gravitySpeed * Time.deltaTime, 0, itemZCoord);
		}
	}

	void goToCategoryChoice() {
		SceneManager.LoadScene("main", LoadSceneMode.Single);	
	}
}
