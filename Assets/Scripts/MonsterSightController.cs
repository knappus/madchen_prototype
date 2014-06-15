using UnityEngine;
using System.Collections;

public class MonsterSightController : MonoBehaviour {

    public MonsterController monsterController;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            monsterController.CheckPlayerSpotted();
        }
        
        if (other.tag == "Teddy") {
            Debug.Log("enter teddy");
            monsterController.CheckTeddySpotted();
        } 
    }

    void OnTriggerStay2D (Collider2D other) {
        if (other.tag == "Player") {
            monsterController.CheckPlayerSpotted();
        }
        /*
        if (other.tag == "Teddy") {
            Debug.Log("stay teddy");
            monsterController.CheckTeddySpotted();
        } 
        */
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            monsterController.LostPlayer();
        }
        /*
        if (other.tag == "Teddy") {
            Debug.Log("exit teddy");
            monsterController.ExitTeddy();
            // monsterController.LostTeddy();
        } 
        */
    }


}
