using UnityEngine;
using System;

public class StreetViewCamera : MonoBehaviour {
    public float speed = 3.0f;
    private float X;
    private float Y;
    GameObject pivot;
    public GameObject planet;
    public float cameraRadius;
    float minimum;

    private void Start()
    {
        pivot = gameObject.transform.parent.gameObject;


        cameraRadius = minimum = planet.GetComponent<Polyhedron>().radius + (float)(Math.Sqrt(planet.GetComponent<Polyhedron>().radius)) + 5f;
        cameraRadius *= -1;
        minimum *= -1;
        gameObject.transform.localPosition = new Vector3(0, 0, cameraRadius);
}

    void Update() {
        if (Input.GetMouseButton(0)) {
            //transform.RotateAround(Vector3.zero, new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), speed);
            Vector3 rVec = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            rVec *= speed;
            pivot.transform.Rotate(rVec);

            //X = transform.rotation.eulerAngles.x;
            //Y = transform.rotation.eulerAngles.y;
            //transform.rotation = Quaternion.Euler(X, Y, 0);
        }
        //cameraRadius = planet.GetComponent<Polyhedron>().radius + (float)(Math.Sqrt(planet.GetComponent<Polyhedron>().radius)) + 5f;
        cameraRadius += Input.GetAxis("Vertical") + Input.GetAxis("Mouse ScrollWheel") * 8;
        gameObject.transform.localPosition = new Vector3(0, 0, cameraRadius);
        if (cameraRadius > minimum)
            cameraRadius = minimum;
    }
}
