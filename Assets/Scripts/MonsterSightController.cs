using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSightController : MonoBehaviour {

    public MonsterController monsterController;

    private List<GameObject> collidingPlatforms = new List<GameObject>();


    List<GameObject> FindGameObjectsWithLayer(int layer) {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++) {
           if (goArray[i].layer == layer) {
             goList.Add(goArray[i]);
           }
        }
        return goList;
    }


	// Use this for initialization
	void Start () {
	    collidingPlatforms = FindGameObjectsWithLayer(LayerMask.NameToLayer("LevelGeometry"));
        Debug.Log("collidingcount: " + collidingPlatforms.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<GameObject> GetCollidingPlatforms() {
        return this.collidingPlatforms;
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            monsterController.CheckPlayerSpotted();
        }
        
        if (other.tag == "Teddy") {
            Debug.Log("enter teddy");
            monsterController.CheckTeddySpotted();
        } 

        /*
        if (LayerMask.LayerToName(other.gameObject.layer) == "LevelGeometry") {
            collidingPlatforms.Add(other.gameObject);
            Debug.Log("collidingcount: " + collidingPlatforms.Count);
        }
        */
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

        /*
        if (LayerMask.LayerToName(other.gameObject.layer) == "LevelGeometry") {
            // collidingPlatforms.Remove(other.gameObject);
            // Debug.Log("collidingcount: " + collidingPlatforms.Count);
        }
        */
    }


}
