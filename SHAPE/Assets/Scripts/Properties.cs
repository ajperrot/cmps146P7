using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour {

				public int biome = 0; //enumeration
				public float altitude = 0f;
				public float latitude;

				// Use this for initialization
				void Start()
				{
					   latitude = Mathf.Abs(gameObject.transform.position.y);
				}
				
				// Update is called once per frame
				void Update()
				{

				}

				public void changeAltitude(float newAlt)
				{
					   altitude = newAlt;
								Vector3 scale = gameObject.transform.localScale;
								scale.z = ((newAlt + 8001) / 4000) * 0.8506508f;
								gameObject.transform.localScale = scale;
				}
}
