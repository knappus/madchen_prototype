using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClipperLib;

public class ClipperInterface : MonoBehaviour {

    const int SCALE_FACTOR = 10000;




    public static List<IntPoint> MeshToPath(Vector2[] mesh) {
        List<IntPoint> subj = new List<IntPoint>(mesh.Length);
        for (int i=0; i<mesh.Length; i++) {
            subj.Add(new IntPoint((int)(mesh[i].x * SCALE_FACTOR), (int)(mesh[i].y * SCALE_FACTOR)));
        }
        return subj;
    }
    public static List<IntPoint> MeshToPath3D(Vector3[] mesh) {
        List<IntPoint> subj = new List<IntPoint>(mesh.Length);
        for (int i=0; i<mesh.Length; i++) {
            subj.Add(new IntPoint((int)(mesh[i].x * SCALE_FACTOR), (int)(mesh[i].y * SCALE_FACTOR)));
        }
        return subj;
    }

    public static Vector2[] PathToMesh(List<IntPoint> path) {
        Vector2[] mesh = new Vector2[path.Count];
        for (int i=0; i<mesh.Length; i++) {
            mesh[i] = new Vector2(path[i].X/(float)SCALE_FACTOR, path[i].Y/(float)SCALE_FACTOR);
        }
        return mesh;
    }
    public static Vector3[] PathToMesh3D(List<IntPoint> path) {
        Vector3[] mesh = new Vector3[path.Count];
        for (int i=0; i<mesh.Length; i++) {
            mesh[i] = new Vector3(path[i].X/(float)SCALE_FACTOR, path[i].Y/(float)SCALE_FACTOR, 0);
        }
        return mesh;
    }


    public static Vector2[] MeshDifference(Vector2[] mesh1, Vector2[] mesh2) {
        List<IntPoint> subj = MeshToPath(mesh1);
        List<IntPoint> clip = MeshToPath(mesh2);
        List<List<IntPoint>> solution = new List<List<IntPoint>>();

        Clipper c = new Clipper();
        c.AddPath(subj, PolyType.ptSubject, true);
        c.AddPath(clip, PolyType.ptClip, true);
        c.Execute(ClipType.ctDifference, solution, 
          PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

        return PathToMesh(solution[0]);
    }
    public static Vector3[] MeshDifference(Vector3[] mesh1, Vector3[] mesh2) {
        List<IntPoint> subj = MeshToPath3D(mesh1);
        List<IntPoint> clip = MeshToPath3D(mesh2);
        List<List<IntPoint>> solution = new List<List<IntPoint>>();

        Clipper c = new Clipper();
        c.AddPath(subj, PolyType.ptSubject, true);
        c.AddPath(clip, PolyType.ptClip, true);
        c.Execute(ClipType.ctDifference, solution, 
          PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

        return PathToMesh3D(solution[0]);
    }
}
