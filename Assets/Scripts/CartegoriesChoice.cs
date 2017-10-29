using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;

public class CartegoriesChoice : MonoBehaviour {

	public float cameraBoundaryLeft;
	public float cameraBoundaryRight;

	public GameObject sodaLocation;
	public GameObject coffeeLocation;
	public GameObject snackLocation;

	public GameObject explosion;

	public GameObject soda;
	public GameObject snack;
	public GameObject coffee;

	private GameObject spawnedSoda;
	private GameObject spawnedSnack;
	private GameObject spawnedCoffee;

	public TouchInterpreter touchInput;

	public float itemZCoord = -1;

	[SerializeField]
	public Vector2 coffeeMinSelection;
	public Vector2 coffeeMaxSelection;
	public Vector2 sodaMinSelection;
	public Vector2 sodaMaxSelection;

	// Use this for initialization
	void Start () {
		spawnedSoda = Instantiate (soda, sodaLocation.transform.position, Quaternion.identity);
		spawnedCoffee = Instantiate (coffee, coffeeLocation.transform.position, Quaternion.identity);
		spawnedSnack = Instantiate (snack, snackLocation.transform.position, Quaternion.identity);
	}


	float timer = 0;
	float time = 5;
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > time) {
			timer = 0;
			Destroy (spawnedCoffee);
			Instantiate (explosion, coffeeLocation.transform.position, Quaternion.identity);
		}

		List<Vector2> touches = touchInput.getTouchPoints ();
		foreach (Vector2 touch in touches) {
			if (touch.x > coffeeMinSelection.x && touch.x < coffeeMaxSelection.x
			    && touch.y > coffeeMinSelection.y && touch.y < coffeeMaxSelection.y) {

				Destroy (spawnedCoffee);
				Instantiate (explosion, coffeeLocation.transform.position, Quaternion.identity);

			} else if (touch.x > sodaMinSelection.x && touch.x < sodaMaxSelection.x
				&& touch.y > sodaMinSelection.y && touch.y < sodaMaxSelection.y){

				Destroy (spawnedSoda);
				Instantiate (explosion, sodaLocation.transform.position, Quaternion.identity);

			}
		}
	}

	/*
	void OnMouseDrag()
	{
		dragging = true;
		Debug.Log ("onmousedrag");
		if (lastMousePos != 99) {
			Debug.Log ("moving");
			float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
			Vector3 mouseloc = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
			currentActive.transform.position = new Vector3 (currentActive.transform.position.x + ((mouseloc.x + lastMousePos) * Time.deltaTime), 0, itemZCoord);
			Debug.Log (mouseloc.x);
			mouseresetTimer = 0;
		} else {
			float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
			Vector3 mouseloc = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
			lastMousePos = mouseloc.x;
			Debug.Log ("not moving " + lastMousePos);
		}
	}*/
}
