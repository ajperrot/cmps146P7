using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polyhedron : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//size: 1 = dodecahedron, +1 per hexagon between pentagons
		int size = 2;
		int T = size * size;
	 int facecount = 10 * T + 2;
	 int hexcount = facecount - 12;
		float radius = (size + 1) * 0.5f * Mathf.Sqrt((5/2) + (11/10) * Mathf.Sqrt(5));
	 GameObject[] pentagons = new GameObject[12];
  GameObject[] hexagons = new GameObject[hexcount];
		GameObject pentagon = GameObject.Find("pentagon");
		GameObject hexagon = GameObject.Find("hexagon");
		Edge[] edges = pentagon.GetComponent<PentaPrism>().edges;

		pentagons[0] = Object.Instantiate(pentagon, gameObject.transform.position, gameObject.transform.rotation);
		pentagons[0].transform.Translate(Vector3.forward*radius);
		pentagons[0].transform.Rotate(new Vector3(0,180,0));
		hexagon.transform.Translate(Vector3.forward*radius);
		hexagon.transform.Rotate(new Vector3(0,180,0));
		Destroy(pentagon);
		//initialize front half
		for(int i = 1; i < 6; i++){
			pentagons[i] = Object.Instantiate(pentagons[0], pentagons[0].transform.position, pentagons[0].transform.rotation);
			pentagons[i].transform.RotateAround(Vector3.zero, edges[i-1].axis, 180 + 116.5f);
			pentagons[i].transform.Rotate(new Vector3(0,0,180));
			for(int j = 0; j < size - 1; j++){
				hexagons[j] = Object.Instantiate(hexagon, pentagons[0].transform.position, pentagons[0].transform.rotation);
				hexagons[j].transform.RotateAround(Vector3.zero, edges[i-1].axis, 270 + (116.5f/2)*(j+1));
				hexagons[j].transform.Rotate(new Vector3(0, 0, 90 + (i - 3) * 252));
			}
		}
		//repeat for other side
		pentagons[6] = Object.Instantiate(pentagons[0], pentagons[0].transform.position*-1f, pentagons[0].transform.rotation);
		pentagons[6].transform.Rotate(new Vector3(180,0,0));
		for(int i = 1; i < 6; i++){
			pentagons[i+5] = Object.Instantiate(pentagons[6], pentagons[6].transform.position, pentagons[6].transform.rotation);
			pentagons[i+5].transform.RotateAround(Vector3.zero, edges[i-1].axis, 180 + 116.5f);
			pentagons[i+5].transform.Rotate(new Vector3(0,0,180));
   for(int j = 0; j < size - 1; j++){
				hexagons[j] = Object.Instantiate(hexagon, pentagons[6].transform.position, pentagons[6].transform.rotation);
				hexagons[j].transform.RotateAround(Vector3.zero, edges[i-1].axis, 270 + (116.5f/2)*(j+1));
				hexagons[j].transform.Rotate(new Vector3(0, 0, 90 - (i - 3) * 252));
			}
		}
		Destroy(hexagon);
	}

}
