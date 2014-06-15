﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MonsterController : MonoBehaviour {

    // monsters sight area
    public GameObject sightMask;
    public int sightRadius = 60;
    public float sightLength = 100f;

    private bool chasingPlayer = false;
    private bool chasingTeddy = false;
    private GameObject chasedTeddy;

    private GameObject player;

    // enemies start and end position
    float startPos;
    float endPos;

    // units enemy moves right
    public float unitsToMove = 5f;
    // enemy movement speed
    public float moveSpeed = 1f;
    public float chaseSpeed = 5f;

    bool moveRight = true;

    public MadchenController madchenController;
    public Color defaultColor = new Color(158,95,95,0);
    public Color spottedColor = new Color(150,28,28,0);

    public float distanceToTeddy = 0.5f;

	// Use this for initialization
	void Start () {
        CreateSightMask();
        player = GameObject.Find("Player");
	}
	
    void FixedUpdate() {
        if (this.chasingTeddy) {
            CheckTeddySpotted();
        }
        // move, depending on if player is being chased
        if (chasingPlayer && !madchenController.IsHidden()) {
            MoveChasing();
        } else {
            if (chasingTeddy) {
                MoveChasingTeddy();
            } else {
                MovePatrolling();
            }
        }

    }
    
     
    void MovePatrolling() {
       transform.Translate(transform.localScale.x/Mathf.Abs(transform.localScale.x) * moveSpeed * Time.deltaTime, 0,0);

       if ((rigidbody2D.position.x >= endPos) && moveRight) {     
            moveRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
       }
       if ((rigidbody2D.position.x <= startPos) && !moveRight) {
            moveRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
       }
    }

    void MoveChasing() {
        Vector2 direction = player.transform.position - transform.position;
        transform.Translate(direction.x/Mathf.Abs(direction.x) * chaseSpeed * Time.deltaTime, 0,0);
    }

    void MoveChasingTeddy() {
        Vector2 direction = chasedTeddy.transform.position - transform.position;
        if (Mathf.Abs(direction.x) > distanceToTeddy)
            transform.Translate(direction.x/Mathf.Abs(direction.x) * chaseSpeed * Time.deltaTime, 0,0);
    }

    void Awake() {
        startPos = transform.position.x;
        endPos = startPos + unitsToMove;

    }

    public void CheckPlayerSpotted() {
        // string[] layers = new string[] {"LevelGeometry", "Monster"};
        int layerMask = LayerMask.GetMask(new string[] {"LevelGeometry", "Player"});

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Infinity, layerMask, -Mathf.Infinity, Mathf.Infinity); 
        // Debug.Log("hit: " + hit.transform.gameObject);
        if ((hit.transform.tag == "Player") && (!madchenController.IsHidden())) {
            SpottedPlayer();
        } else {
            LostPlayer();
        }
    }

    public void SpottedPlayer() {
        chasingPlayer = true;
        // Debug.Log("Spotted Player");
        // LostTeddy();
        sightMask.GetComponent<MeshRenderer>().materials[0].color = spottedColor;
    }
    public void LostPlayer() {
        chasingPlayer = false;
        Debug.Log("Lost Player");

        sightMask.GetComponent<MeshRenderer>().materials[0].color = defaultColor;
    }

    public void CheckTeddySpotted() {
        GameObject[] teddies = GameObject.FindGameObjectsWithTag("Teddy");
        if (teddies.Length > 0) {
            if (!chasingPlayer) {
                GameObject teddy = GameObject.FindGameObjectsWithTag("Teddy")[0];

                // string[] layers = new string[] {"LevelGeometry", "Monster"};
                int layerMask = LayerMask.GetMask(new string[] {"LevelGeometry", "Teddy"});

                RaycastHit2D hit = Physics2D.Raycast(transform.position, teddy.transform.position - transform.position, Mathf.Infinity, layerMask, -Mathf.Infinity, Mathf.Infinity); 

                // Debug.Log("hit: " + hit.transform.gameObject);
                if (hit.transform.tag == "Teddy") {
                    SpottedTeddy(teddy);
                } else {
                    LostTeddy();
                }
            }
        } else {
            LostTeddy();
        }
    }

    public void SpottedTeddy(GameObject teddy) {
        this.chasingTeddy = true;
        this.chasedTeddy = teddy;
        // Debug.Log("Spotted Teddy");
        FocusOnTeddy(teddy);
        // Debug.Log("Spotted Player");
    }

    public void ExitTeddy() {
        // LostTeddy();
        FocusOnTeddy(chasedTeddy);
    }
    public void LostTeddy() {
        chasingTeddy = false;
        // Debug.Log("Lost Teddy");
        CreateSightMask();
    }


    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
            Debug.Log("Kill player");
        }
    }



    List<Vector2> GetTeddyCorners(GameObject teddy) {
        Vector3 center = teddy.GetComponent<SpriteRenderer>().bounds.center;
        Vector3 extents = teddy.GetComponent<SpriteRenderer>().bounds.extents;
        List<Vector2> corners = new List<Vector2>();
        float extentX = teddy.transform.localScale.x/2;
        float extentY = teddy.transform.localScale.y/2;
        corners.Add(new Vector2(center.x - extentX, center.y - extentY));
        corners.Add(new Vector2(center.x - extentX, center.y + extentY));
        corners.Add(new Vector2(center.x + extentX, center.y - extentY));
        corners.Add(new Vector2(center.x + extentX, center.y + extentY));

        // Debug.Log("Corners: " + corners[0].x + ", " + corners[0].y + ";" + corners[1].x + ", " + corners[1].y + ";" + corners[2].x + ", " + corners[2].y + ";" + corners[3].x + ", " + corners[3].y + ";");

        return corners;
    }



    void FocusOnTeddy(GameObject teddy) {
        // Debug.Log("Create Shadow for Platform");

        List<Vector2> corners = GetTeddyCorners(teddy);

        List<float> angles = new List<float>();
        float minAngle = 400f;
        float maxAngle = -400f;
        int minAngleIndex = 0;
        int maxAngleIndex = 0;
        for (int i=0; i<corners.Count; i++)
        {
            float dy = corners[i].y - transform.position.y;
            float dx = corners[i].x - transform.position.x;
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

        float origMaxX = corners[maxAngleIndex].x;
        float origMaxY = corners[maxAngleIndex].y;
        float origMinX = corners[minAngleIndex].x;
        float origMinY = corners[minAngleIndex].y;
        //float deltaY = 30 * Mathf.Sin(Mathf.Deg2Rad * sightRadius/2);
        //float deltaX = 30 * Mathf.Cos(Mathf.Deg2Rad * sightRadius/2);

        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(origMaxX - transform.position.x, origMaxY - transform.position.y),
            new Vector2(origMinX - transform.position.x, origMinY - transform.position.y)
        };
         
        Vector2[] uvs = vertices2D;
         

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
 
        // float shadowDepth = shadowInstance.transform.position.z;

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
        
        if (!moveRight) {
            Vector3 newScale = sightMask.transform.localScale;
            newScale.x = -1;
            sightMask.transform.localScale = newScale;
        }
        
        sightMask.GetComponent<MeshFilter>().mesh = msh;
        sightMask.GetComponent<PolygonCollider2D>().SetPath(0, vertices2D);

    }



    void CreateSightMask() {
        // calculate point 2
        float origX = 0;
        float origY = 0;
        float deltaY = sightLength * Mathf.Sin(Mathf.Deg2Rad * sightRadius/2);
        float deltaX = sightLength * Mathf.Cos(Mathf.Deg2Rad * sightRadius/2);

        Debug.Log("blub");
        Debug.Log("deltaX: " + deltaX);
        Debug.Log("<deltaY></deltaY>: " + deltaY);

        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(origX, origY),
            new Vector2(deltaX-origX, deltaY-origY),
            new Vector2(deltaX-origX, origY-deltaY)
        };
        Vector2[] colliderVertices = new Vector2[] {
            new Vector2(origX, origY),
            new Vector2(deltaX-origX, deltaY-origY),
            new Vector2(deltaX-origX, origY-deltaY),
            new Vector2(origX, origY)
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

        Vector3 origScale = sightMask.transform.localScale;
        origScale.x = 1;
        sightMask.transform.localScale = origScale;

        sightMask.GetComponent<MeshFilter>().mesh = msh;
        //PolygonCollider2D collider = sightMask.collider as PolygonCollider2D;
        sightMask.GetComponent<PolygonCollider2D>().SetPath(0, vertices2D);

        sightMask.GetComponent<MeshRenderer>().materials[0].color = defaultColor;
    }


}
