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
            
    }

    void OnTriggerStay2D (Collider2D other) {
        if (other.tag == "Player") {
            monsterController.CheckPlayerSpotted();
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            monsterController.LostPlayer();
        }
    }


}
