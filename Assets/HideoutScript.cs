using UnityEngine;
using System.Collections;

public class HideoutScript : MonoBehaviour {

    private GameObject player;
    public MadchenController madchenController;


	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            madchenController.Hide();
        }
            
    }

    void OnTriggerStay2D (Collider2D other) {
        /*
        if (other.tag == "Player") {
            madchenController.Hide();
        }
        */
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            madchenController.Unhide();
        }
    }


}
