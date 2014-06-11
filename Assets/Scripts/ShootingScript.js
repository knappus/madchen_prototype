var projectile : Rigidbody2D;
var speed = 10;

function Update () {

if( Input.GetButtonDown("Fire1")) {

	clone = Instantiate(projectile, transform.position, transform.rotation);
	clone.velocity = transform.TransformDirection(Vector2(speed,0));
	
	Destroy (clone.gameObject, 5);

}}