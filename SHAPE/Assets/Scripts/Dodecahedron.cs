using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodecahedron : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//size: 1 = dodecahedron, +1 per hexagon between pentagons
		float radius = 0.5f * Mathf.Sqrt((5/2) + (11/10) * Mathf.Sqrt(5));
	 GameObject[] pentagons = new GameObject[12];
		GameObject pentagon = GameObject.Find("pentagon");
		Edge[] edges = pentagon.GetComponent<PentaPrism>().edges;

		pentagons[0] = Object.Instantiate(pentagon, gameObject.transform.position, gameObject.transform.rotation);
		pentagons[0].transform.Translate(Vector3.forward*radius);
		pentagons[0].transform.Rotate(new Vector3(0,180,0));
		//Destroy(pentagon);
		//initialize front half
		for(int i = 1; i < 6; i++){
			pentagons[i] = Object.Instantiate(pentagons[0], pentagons[0].transform.position, pentagons[0].transform.rotation);
			pentagons[i].transform.RotateAround(Vector3.zero, edges[i-1].axis, 180 + 116.5f);
			pentagons[i].transform.Rotate(new Vector3(0,0,180));
		}
		//repeat for other side
		pentagons[6] = Object.Instantiate(pentagons[0], pentagons[0].transform.position*-1f, pentagons[0].transform.rotation);
		pentagons[6].transform.Rotate(new Vector3(180,0,0));
		for(int i = 1; i < 6; i++){
			pentagons[i] = Object.Instantiate(pentagons[6], pentagons[6].transform.position, pentagons[6].transform.rotation);
			pentagons[i].transform.RotateAround(Vector3.zero, edges[i-1].axis, 180 + 116.5f);
			pentagons[i].transform.Rotate(new Vector3(0,0,180));
		}
	}

}
