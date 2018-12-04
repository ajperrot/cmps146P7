using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour {
    public GameObject polyhedron;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        float radius = polyhedron.GetComponent<Polyhedron>().radius + 0f;
        gameObject.GetComponent<Transform>().localScale = 2 * new Vector3(radius, radius, radius);

    }
}
