 using UnityEngine;
 
 public class StreetViewCamera:MonoBehaviour {
     public float speed = 1.0f;
     private float X;
     private float Y;
 
     void Update() {
         if(Input.GetMouseButton(0)) {
             transform.RotateAround(Vector3.zero, new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), speed);
             //X = transform.rotation.eulerAngles.x;
             //Y = transform.rotation.eulerAngles.y;
             //transform.rotation = Quaternion.Euler(X, Y, 0);
         }
     }
 }
