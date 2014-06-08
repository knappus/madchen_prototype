using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    // monsters sight area
    public GameObject sightMask;
    public int sightRadius = 60;
    public float sightLength = 100f;

    private bool chasingPlayer = false;

    private GameObject player;

    // enemies start and end position
    float startPos;
    float endPos;

    // units enemy moves right
    public float unitsToMove = 5f;
    // enemy movement speed
    public float moveSpeed = 2f;

    bool moveRight = true;



	// Use this for initialization
	void Start () {
        CreateSightMask();
        player = GameObject.Find("Player");
	}
	
    void FixedUpdate() {
        // move, depending on if player is being chased
        if (chasingPlayer) {
            MoveChasing();
        } else {
            MovePatrolling();
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
        transform.Translate(direction.x/Mathf.Abs(direction.x) * moveSpeed * Time.deltaTime, 0,0);
    }


    void Awake() {
        startPos = transform.position.x;
        endPos = startPos + unitsToMove;

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

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
            Debug.Log("Kill player");
        }
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
