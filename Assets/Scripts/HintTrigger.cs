using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour {

	public GameObject hint;

	// Use this for initialization
	void Start () {
		hint.active = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other){
		
		if (other.gameObject.name == "Player") {
			Debug.Log ("Entering Hint Collider");
			hint.active = true;
			Destroy (hint, 5);
			Destroy (this, 5);
		}
	}

}
