using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CategorySelector : MonoBehaviour
{

	private bool itemChosen = false;

	public TouchInterpreter touchInput;

	public Vector2 coffeeMinSelection;
	public Vector2 coffeeMaxSelection;
	public Vector2 sodaMinSelection;
	public Vector2 sodaMaxSelection;

	void Start() {
	}

	void Update () {
		/*timer += Time.deltaTime;
		if (timer > time) {
			timer = 0;
			Destroy (spawnedCoffee);
			Instantiate (explosion, coffeeLocation.transform.position, Quaternion.identity);
		}*/

		List<Vector2> touches = touchInput.getTouchPoints ();
		if (!itemChosen) { 
			foreach (Vector2 touch in touches) {

				if (touch.x > coffeeMinSelection.x && touch.x < coffeeMaxSelection.x
				    && touch.y > coffeeMinSelection.y && touch.y < coffeeMaxSelection.y) {
					itemChosen = true;
					explodeItem (ECategory.COFFEE);
					return;
				} else if (touch.x > sodaMinSelection.x && touch.x < sodaMaxSelection.x
				           && touch.y > sodaMinSelection.y && touch.y < sodaMaxSelection.y) {
					itemChosen = true;
					explodeItem (ECategory.SODA);
					//SceneManager.LoadScene("main", LoadSceneMode.Single);	
					return;
				} 
			}
		}
	}

	void explodeItem(ECategory cat) {
		if (CurrentCategory.category.Equals(ECategory.NONE)) {
			CurrentCategory.category = cat;
			GetComponent<CartegoriesChoice> ().explodeItem (cat);
		} else {
			CurrentCategory.category = cat;
			SceneManager.LoadScene ("main", LoadSceneMode.Single);	
		}
	}


	bool isTouchInBounds(Vector2 touch, Vector2 minBound, Vector2 maxBound) {
		if (touch.x > minBound.x && touch.x < maxBound.x
			&& touch.y > minBound.y && touch.y < maxBound.y) {
			return true;
		}
		return false;
	}
}

