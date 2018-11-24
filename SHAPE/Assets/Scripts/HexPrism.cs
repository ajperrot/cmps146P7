using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPrism : MonoBehaviour
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
        float xd = Mathf.Sqrt(3)/2f;
        float yd = (1f/2f);
        //depth of prism
        float d = -0.5f;
        //front face
        Vector3 p0 = new Vector3(0,0,d);
        Vector3 p1 = new Vector3(0,1,d);
        Vector3 p2 = new Vector3(xd,yd,d);
        Vector3 p3 = new Vector3(xd,-yd,d);
        Vector3 p4 = new Vector3(0,-1,d);
        Vector3 p5 = new Vector3(-xd,-yd,d);
		Vector3 p6 = new Vector3(-xd,yd,d);
		//back face
        Vector3 p7 = new Vector3(0,1,0);
        Vector3 p8 = new Vector3(xd,yd,0);
        Vector3 p9 = new Vector3(xd,-yd,0);
        Vector3 p10 = new Vector3(0,-1,0);
        Vector3 p11 = new Vector3(-xd,-yd,0);
		Vector3 p12 = new Vector3(-xd,yd,0);

        mesh.vertices = new Vector3[]{p0,p1,p2,p3,p4,p5,p6,
								         p7,p8,p9,p10,p11,p12};
        mesh.triangles = new int[]{
			//front face
            0,1,2,
            0,2,3,
            0,3,4,
            0,4,5,
            0,5,6,
            0,6,1,
            //rectangular edges
            7,2,1,
            7,8,2,

            8,3,2,
            8,9,3,

            9,4,3,
            9,10,4,

            10,5,4,
            10,11,5,

            11,6,5,
            11,12,6,

            12,1,6,
            12,7,1
        };
		mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        ;
    }
}
