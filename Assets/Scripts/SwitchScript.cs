using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	//public GameObject s;
	public GameObject platform;
	public Sprite off;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.name == "Player") {
			Debug.Log ("Entering Switch Collider");
			spriteRenderer.sprite = off;
			Destroy (platform);
		}
	}
}
