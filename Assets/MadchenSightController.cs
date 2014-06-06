using UnityEngine;
using System.Collections;

public class MadchenSightController : MonoBehaviour {

    public float rotationSpeed = 10f;
    public int maxRotationAngle = 60;

    public MadchenController madchenController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float move = Input.GetAxis("Vertical");
        // Quaternion oldRotation = transform.rotation.Clone();
        transform.Rotate(new Vector3(0,0, rotationSpeed * move * Time.deltaTime));

        madchenController.Flip();
        madchenController.Flip();

        float angle = Quaternion.Angle(Quaternion.Euler(0,0,0), transform.rotation);
       
        if (angle > maxRotationAngle/2) {
            transform.Rotate(new Vector3(0,0, -rotationSpeed * move * Time.deltaTime));
        }

    }


/*    
        float move = Input.GetAxis("Vertical");
        Quaternion targetRotation = Quaternion.Euler(0,0,transform.rotation.z + rotationSpeed * move * Time.deltaTime);
        Debug.Log("targetrotation z: " + targetRotation.z);
        Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);
        if (targetRotation.z < maxRotationAngle/2) {
            transform.Rotate(new Vector3(0,0,targetRotation.z));
            madchenController.Flip();
            madchenController.Flip();
        }
*/
}