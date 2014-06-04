using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShadowScript : MonoBehaviour {

    public GameObject shadowPrefab;

    private GameObject player;
    private List<Vector2> corners;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        corners = GetCorners();
	}
	
	// Update is called once per frame
	void Update () {
        
        GenerateShadow(); 
    }

    List<Vector2> GetCorners() {
        Vector3 center = transform.TransformPoint(GetComponent<MeshFilter>().mesh.bounds.center);
        Vector3 extents = GetComponent<MeshFilter>().mesh.bounds.extents;
        List<Vector2> corners = new List<Vector2>();
        float extentX = transform.localScale.x/2;
        corners.Add(new Vector2(center.x - extentX, center.y - extents.y));
        corners.Add(new Vector2(center.x - extentX, center.y + extents.y));
        corners.Add(new Vector2(center.x + extentX, center.y - extents.y));
        corners.Add(new Vector2(center.x + extentX, center.y + extents.y));

        // Debug.Log("Corners: " + corners[0].x + ", " + corners[0].y + ";" + corners[1].x + ", " + corners[1].y + ";" + corners[2].x + ", " + corners[2].y + ";" + corners[3].x + ", " + corners[3].y + ";");

        return corners;
    }

    // get shadowmask mesh or create object if no mesh present
    GameObject GetShadow() {
        foreach (Transform child in transform) {
            if (child.tag == "PlayerSightMask") {
                // return existing shadowmask
                return child.gameObject;
            }
        }

        // return new object
        GameObject shadowInstance = GameObject.Instantiate(shadowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        shadowInstance.transform.parent = transform;
        return shadowInstance;
    }

    void GenerateShadow() {
        // Debug.Log("Create Shadow for Platform");

        Vector3 center = transform.TransformPoint(GetComponent<MeshFilter>().mesh.bounds.center);
        Vector3 extents = GetComponent<MeshFilter>().mesh.bounds.extents;


        List<float> angles = new List<float>();
        float minAngle = 400f;
        float maxAngle = -400f;
        int minAngleIndex = 0;
        int maxAngleIndex = 0;
        for (int i=0; i<corners.Count; i++)
        {
            float dy = corners[i].y - player.transform.position.y;
            float dx = corners[i].x - player.transform.position.x;
            angles.Add(Mathf.Atan2(dy,dx) * Mathf.Rad2Deg);
            // Debug.Log("angle: " + angles[i]);

            if (angles[i] < minAngle) {
                minAngle = angles[i];
                minAngleIndex = i;
            }
            if (angles[i] > maxAngle) {
                maxAngle = angles[i];
                maxAngleIndex = i;
            }
        }


        // Debug.Log("max angle: " + maxAngle);
        // Debug.Log("min angle: " + minAngle);
     
        GameObject shadowInstance;
        shadowInstance = GetShadow();
        // position 0,0?
        
        float origMaxX = corners[maxAngleIndex].x;
        float origMaxY = corners[maxAngleIndex].y;
        float origMinX = corners[minAngleIndex].x;
        float origMinY = corners[minAngleIndex].y;
        //float deltaY = 30 * Mathf.Sin(Mathf.Deg2Rad * sightRadius/2);
        //float deltaX = 30 * Mathf.Cos(Mathf.Deg2Rad * sightRadius/2);

        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(origMaxX, origMaxY),
            new Vector2(origMaxX + 30*Mathf.Cos(Mathf.Deg2Rad * maxAngle), origMaxY + 30*Mathf.Sin(Mathf.Deg2Rad * maxAngle)),
            new Vector2(origMinX + 30*Mathf.Cos(Mathf.Deg2Rad * minAngle), origMinY + 30*Mathf.Sin(Mathf.Deg2Rad * minAngle)),
            new Vector2(origMinX, origMinY)
        };
         
        Vector2[] uvs = vertices2D;
         

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
 
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.uv = uvs;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
 
        // Set up game object with mesh;
        // gameObject.AddComponent(typeof(MeshRenderer));
        // MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        // filter.mesh = msh;

        shadowInstance.GetComponent<MeshFilter>().mesh = msh;

        // Debug.Log("Created Shadow");
    }
}

