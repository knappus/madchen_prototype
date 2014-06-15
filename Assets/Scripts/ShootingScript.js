
var madchen : GameObject;
var projectile : Rigidbody2D;
var speed = 10;

function Update () {

madchen = GameObject.FindWithTag("Player");
var script = madchen.GetComponent("MadchenController");

if( Input.GetButtonDown("Fire1")) {
        
    teddies = GameObject.FindGameObjectsWithTag ("Teddy");
	
    if (teddies.length == 0) {

        clone = Instantiate(projectile, transform.position, transform.rotation);
        
        if (script.facingRight == false) { 
            clone.velocity.x = speed*-1;
     
            }else {
            clone.velocity.x = speed;
            }

        Destroy (clone.gameObject, 5);
    }

}}