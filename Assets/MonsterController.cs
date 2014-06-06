using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public GameObject sightMask;
    public int sightRadius = 60;
    public float sightLength = 100f;

    private Vector3 startPosition;
    private float roamRadius = 0.5f;
    public float roamDistance = 0.1f;

    private bool chasingPlayer = false;

    private GameObject player;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        CreateSightMask();
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	   FreeRoam();
	}
     
    void FreeRoam() {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector3 finalPosition = hit.position;
        
        //_nav.destination = finalPosition;

        if (Vector3.Distance(startPosition, transform.position+randomDirection) < roamDistance) 
            rigidbody2D.AddForce(randomDirection);
        else
            rigidbody2D.AddForce(-randomDirection);
    }


    public void CheckPlayerSpotted() {
        // string[] layers = new string[] {"LevelGeometry", "Monster"};
        int layerMask = LayerMask.GetMask(new string[] {"LevelGeometry", "Player"});

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Infinity, layerMask, -Mathf.Infinity, Mathf.Infinity); 
        Debug.Log("hit: " + hit.transform.gameObject);
        if (hit.transform.tag == "Player") {
            SpottedPlayer();
        } else {
            LostPlayer();
        }
    }

    public void SpottedPlayer() {
        chasingPlayer = true;
        Debug.Log("Spotted Player");
    }
    public void LostPlayer() {
        chasingPlayer = false;
        Debug.Log("Lost Player");
    }

    void CreateSightMask() {
        // calculate point 2
        float origX = transform.position.x;
        float origY = transform.position.y;
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


        sightMask.GetComponent<MeshFilter>().mesh = msh;
        //PolygonCollider2D collider = sightMask.collider as PolygonCollider2D;
        sightMask.GetComponent<PolygonCollider2D>().SetPath(0, vertices2D);

    }

}
