using UnityEngine;
using System.Collections;



public class ShootingScriptC : MonoBehaviour {

    public GameObject madchen;
    public Rigidbody2D projectile;
    public int speed = 10;

    private MadchenController madchenController;
    private GameObject teddyProjectile;

    public bool pickUp = false;

	// Use this for initialization
	void Start () {
        madchen = GameObject.FindWithTag("Player");
        madchenController = madchen.GetComponent("MadchenController") as MadchenController;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire1")) {
            if (this.teddyProjectile == null) {
                ThrowTeddy();
            } else {
                if (!pickUp) {
                    DestroyProjectile();
                }
            }
            
        }

	}

    void ThrowTeddy() {
        GameObject[] teddies = GameObject.FindGameObjectsWithTag("Teddy");
            
        GameObject sightMask = GameObject.FindWithTag("PlayerSightMask");

        Debug.Log("right: " + sightMask.transform.right);
        Debug.Log("up: " + sightMask.transform.up);


        if (teddies.Length == 0) {
            GameObject teddy = GameObject.Find("Teddy");
            teddy.renderer.enabled = false;
            Rigidbody2D clone = Instantiate(this.projectile, this.transform.position, this.transform.rotation) as Rigidbody2D;
            this.teddyProjectile = clone.gameObject;
            Vector2 throwAngle = new Vector2(sightMask.transform.right.x, sightMask.transform.right.y);
            if (madchenController.IsFacingRight()) {
                //clone.AddForce(new Vector2(speed, 0));
                // clone.velocity = new Vector2(speed, clone.velocity.y);
                clone.velocity = throwAngle * speed;
            } else {
                //clone.AddForce(new Vector2(-speed, 0));
                clone.velocity = new Vector2(throwAngle.x * -speed, throwAngle.y * speed);
            }
            if (!pickUp) {
                Invoke("DestroyProjectile", 5);
            }
        }
    }

    public void DestroyProjectile() {
        CancelInvoke("DestroyProjectile");
        Destroy(this.teddyProjectile);
        this.teddyProjectile = null;
        GameObject teddy = GameObject.Find("Teddy");
        teddy.renderer.enabled = true;
    }
}
