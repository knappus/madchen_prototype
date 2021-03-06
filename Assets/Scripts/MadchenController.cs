﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MadchenController : MonoBehaviour {

    public float maxSpeed = 10f;
    private bool facingRight = true;
    public GameObject light;

    public GameObject sightMask;
    public int sightRadius = 60;

    private bool hidden = false;

    private BoxCollider2D collider;

    private int monsterLayer; 
    private int playerLayer;

    public Color spottedColor = new Color(142, 90, 90, 1f);

	// Use this for initialization
	void Start () {
        Init();
        CreateSightMask();
	}
	
	void FixedUpdate () {
	   	float move = Input.GetAxis("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

        if (move > 0 && !facingRight) {
			Flip ();
		}
		else if (move < 0 && facingRight) {
			Flip ();
		}



        if (Input.GetButtonDown("LoadLevel1"))
            Application.LoadLevel("proto_level1");
        if (Input.GetButtonDown("LoadLevel2"))
            Application.LoadLevel("proto_level2");
        if (Input.GetButtonDown("LoadLevel3"))
            Application.LoadLevel("proto_level3");
        if (Input.GetButtonDown("LoadLevel4"))
            Application.LoadLevel("proto_level4");
        if (Input.GetButtonDown("LoadLevel5"))
            Application.LoadLevel("proto_level5");
        if (Input.GetButtonDown("LoadLevel6"))
            Application.LoadLevel("proto_level6_jump");
        if (Input.GetButtonDown("LoadLevel7"))
            Application.LoadLevel("proto_level7_lift");
        if (Input.GetButtonDown("LoadLevel8"))
            Application.LoadLevel("proto_level1");

	}

    void Init() {
        this.monsterLayer = LayerMask.NameToLayer("Monster");
        this.playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(monsterLayer, playerLayer, false);

    }

    public void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        GameObject hint = GameObject.Find("Hint");
        if (hint != null) {
            Vector3 hintScale = hint.transform.localScale;
            hintScale.x *= -1;
            hint.transform.localScale = hintScale;
        }
    }

    public bool IsFacingRight() {
        return facingRight;
    }


    public void Spotted() {
        SpriteRenderer renderer = (SpriteRenderer) GetComponent(typeof(SpriteRenderer));
        renderer.color = spottedColor; // Set to opaque black
    }

    public void Unspotted() {
        SpriteRenderer renderer = (SpriteRenderer) GetComponent(typeof(SpriteRenderer));
        renderer.color = new Color(1f, 1f, 1f, 1f); // Set to opaque black
    }

    public void Hide() {
        this.hidden = true;
        // BoxCollider2D b = collider as BoxCollider2D;
        // b.isTrigger = true;
        Physics2D.IgnoreLayerCollision(monsterLayer, playerLayer, true);
        
        //Debug.Log("hide");
    }
    public void Unhide() {
        this.hidden = false;
        // this.collider.isTrigger = false;

        Physics2D.IgnoreLayerCollision(monsterLayer, playerLayer, false);

        //Debug.Log("unhide");
    }
    public bool IsHidden() {
        return this.hidden;
    }


    public void Die() {
        Debug.Log("dead");
        // yield WaitForSeconds(0.1); // or however long you want it to wait
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Monster") {
            this.Die();
        }
        if (coll.gameObject.tag == "Teddy") {
            GameObject go = GameObject.Find("Barrel");
            ShootingScriptC shootingScript = (ShootingScriptC) go.GetComponent(typeof(ShootingScriptC));
            shootingScript.DestroyProjectile();
        }
    }

    void CreateSightMask() {
        // calculate point 2
        // float origX = transform.position.x;
        // float origY = transform.position.y;
        float origX = 0;
        float origY = 0;
        
        float deltaY = 100 * Mathf.Sin(Mathf.Deg2Rad * sightRadius/2);
        float deltaX = 100 * Mathf.Cos(Mathf.Deg2Rad * sightRadius/2);

        Debug.Log("blub");
        Debug.Log("deltaX: " + deltaX);
        Debug.Log("<deltaY></deltaY>: " + deltaY);

        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(origX, origY),
            new Vector2(deltaX-origX, deltaY-origY),
            new Vector2(origX-deltaX, deltaY-origY),
            new Vector2(origX-deltaX, origY-deltaY),
            new Vector2(deltaX-origX, origY-deltaY)
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
    }

}

public class Triangulator
{
    private List<Vector2> m_points = new List<Vector2>();
 
    public Triangulator (Vector2[] points) {
        m_points = new List<Vector2>(points);
    }
 
    public int[] Triangulate() {
        List<int> indices = new List<int>();
 
        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();
 
        int[] V = new int[n];
        if (Area() > 0) {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }
 
        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2; ) {
            if ((count--) <= 0)
                return indices.ToArray();
 
            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;
 
            if (Snip(u, v, w, nv, V)) {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }
 
        indices.Reverse();
        return indices.ToArray();
    }
 
    private float Area () {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++) {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }
 
    private bool Snip (int u, int v, int w, int n, int[] V) {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++) {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }
 
    private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;
 
        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;
 
        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;
 
        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
}
