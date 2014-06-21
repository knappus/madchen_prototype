using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	//public GameObject s;
	public GameObject platform;
	public Sprite off;
	public Sprite on;
	private SpriteRenderer spriteRenderer;

	private Vector3 moveFrom;
	public Transform moveTo;
	public float moveSpeed = 1f;
	public float moveBackSpeed = 1f;

	public bool timer;
	public int seconds;

	// Use this for initialization
	void Start () {
		moveFrom = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z);
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.name == "Player") {
			Debug.Log ("Entering Switch Collider");
			spriteRenderer.sprite = off;
			// Destroy (platform);
			StartCoroutine(MovePlatform());
			if (timer) {
				StartCoroutine(MovePlatformBack());
			}
		}
	}

	IEnumerator MovePlatform() {
		Debug.Log ("Move platform");
		while(Vector3.Distance(platform.transform.position, moveTo.position) > 1f)
        {
			platform.transform.position = Vector3.Lerp(platform.transform.position, moveTo.position, Time.deltaTime * moveSpeed);

        	ShadowScript shadowScript = (ShadowScript) platform.GetComponent(typeof(ShadowScript));
        	shadowScript.CalculateCorners();
			yield return null;
        }
		Debug.Log ("Moving to done");
		yield return null;
	}

	IEnumerator MovePlatformBack() {
        yield return new WaitForSeconds(seconds);
		Debug.Log ("Move platform back");
		spriteRenderer.sprite = on;
		while(Vector3.Distance(platform.transform.position, moveFrom) > 0.05f)
        {
			platform.transform.position = Vector3.Lerp(platform.transform.position, moveFrom, Time.deltaTime * moveBackSpeed);

        	ShadowScript shadowScript = (ShadowScript) platform.GetComponent(typeof(ShadowScript));
        	shadowScript.CalculateCorners();
			yield return null;
        }
	}
}
