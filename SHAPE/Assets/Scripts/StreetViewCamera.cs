using UnityEngine;
using System;

public class StreetViewCamera : MonoBehaviour {
    public float speed = 1.0f;
    private float X;
    private float Y;
    GameObject pivot;
    public GameObject planet;
    public float cameraRadius;

    private void Start()
    {
        pivot = gameObject.transform.parent.gameObject;


        cameraRadius = planet.GetComponent<Polyhedron>().radius + (float)(Math.Sqrt(planet.GetComponent<Polyhedron>().radius)) + 5f;
        cameraRadius *= -1;
        gameObject.transform.localPosition = new Vector3(0, 0, cameraRadius);
}

    void Update() {
        if (Input.GetMouseButton(0)) {
            //transform.RotateAround(Vector3.zero, new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), speed);
            pivot.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
            //X = transform.rotation.eulerAngles.x;
            //Y = transform.rotation.eulerAngles.y;
            //transform.rotation = Quaternion.Euler(X, Y, 0);
        }
        if (Input.anyKey)
        {
            cameraRadius = planet.GetComponent<Polyhedron>().radius + (float)(Math.Sqrt(planet.GetComponent<Polyhedron>().radius)) + 5f;
            cameraRadius *= -1;
            gameObject.transform.localPosition = new Vector3(0, 0, cameraRadius);
        }
    }
}
