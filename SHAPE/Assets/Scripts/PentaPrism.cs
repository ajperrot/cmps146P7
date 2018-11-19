using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaPrism : MonoBehaviour
{
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //center at (0,0,0)
        //going in clockwise order from top
        float pi = Mathf.PI;
        float c1 = Mathf.Cos(2*pi/5);
        float c2 = Mathf.Cos(pi/5);
        float s1 = Mathf.Sin(2*pi/5);
        float s2 = Mathf.Sin(4*pi/5);
        //depth of the prism
        float d = 1;
        //front face
        Vector3 p0 = new Vector3(0,0,0);
        Vector3 p1 = new Vector3(0,1,0);
        Vector3 p2 = new Vector3(s1,c1,0);
        Vector3 p3 = new Vector3(s2,-c2,0);
        Vector3 p4 = new Vector3(-s2,-c2,0);
        Vector3 p5 = new Vector3(-s1,c1,0);
        //back face
        Vector3 p6 = new Vector3(0,1,d);
        Vector3 p7 = new Vector3(s1,c1,d);
        Vector3 p8 = new Vector3(s2,-c2,d);
        Vector3 p9 = new Vector3(-s2,-c2,d);
        Vector3 p10 = new Vector3(-s1,c1,d);

        mesh.Clear();
        mesh.vertices = new Vector3[]{p0,p1,p2,p3,p4,p5,
                                         p6,p7,p8,p9,p10};
        mesh.triangles = new int[]{
            //front face
            0,1,2,
            0,2,3,
            0,3,4,
            0,4,5,
            0,5,1,
            //rectangular edges
            6,2,1,
            6,7,2,

            7,3,2,
            7,8,3,

            8,4,3,
            8,9,4,

            9,5,4,
            9,10,5,

            10,1,5,
            10,6,1,
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        ;
    }
}