using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

    public float tileScaling = 0.05f;


	// Use this for initialization
	void Start () {
	    this.gameObject.renderer.sharedMaterial.SetTextureScale("_MainTex",tileScaling * new Vector2(this.gameObject.transform.lossyScale.x,this.gameObject.transform.lossyScale.y)) ;
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos() {
        this.gameObject.renderer.sharedMaterial.SetTextureScale("_MainTex",tileScaling * new Vector2(this.gameObject.transform.lossyScale.x,this.gameObject.transform.lossyScale.y)) ;
    }
}
