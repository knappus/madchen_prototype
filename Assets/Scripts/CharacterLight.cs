using UnityEngine;
using System.Collections;

public class CharacterLight : MonoBehaviour {

    public float rotation = 45f;
    public GameObject player;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
       offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate () {
        transform.position = player.transform.position + offset;

        Vector3 playerScale = player.transform.localScale;
        
        Quaternion lightRotation = transform.rotation;
        lightRotation.x = rotation * playerScale.x;

        float move = Input.GetAxis("Vertical");
        transform.rotation = Quaternion.Euler(transform.rotation.x + move, rotation * playerScale.x, 0);

    }
}
