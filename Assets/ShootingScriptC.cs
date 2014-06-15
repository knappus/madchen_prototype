using UnityEngine;
using System.Collections;



public class ShootingScriptC : MonoBehaviour {

    public GameObject madchen;
    public Rigidbody2D projectile;
    public int speed = 10;

    private MadchenController madchenController;

	// Use this for initialization
	void Start () {
        madchen = GameObject.FindWithTag("Player");
        madchenController = madchen.GetComponent("MadchenController") as MadchenController;
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetButtonDown("Fire1")) {
            GameObject[] teddies = GameObject.FindGameObjectsWithTag("Teddy");
            
            GameObject sightMask = GameObject.FindWithTag("PlayerSightMask");

            Debug.Log("right: " + sightMask.transform.right);
            Debug.Log("up: " + sightMask.transform.up);


            if (teddies.Length == 0) {
                Rigidbody2D clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody2D;
                Vector2 throwAngle = new Vector2(sightMask.transform.right.x, sightMask.transform.right.y);
                if (madchenController.IsFacingRight()) {
                    //clone.AddForce(new Vector2(speed, 0));
                    // clone.velocity = new Vector2(speed, clone.velocity.y);
                    clone.velocity = throwAngle * speed;
                } else {
                    //clone.AddForce(new Vector2(-speed, 0));
                    clone.velocity = new Vector2(throwAngle.x * -speed, throwAngle.y * speed);
                }
                Destroy(clone.gameObject, 5);
            }
        }

	}
}
