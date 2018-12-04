using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour {

				public float latitude;
				public int id = 0;
				public float altitude = 0f;
				public GameObject[] adjacentTiles;
				public int biome = 0; //enumeration
				
				

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
								//scale function based around scale 2 = sea level
								scale.z = ((newAlt + 8025) / 4000) * 0.8506508f;
								gameObject.transform.localScale = scale;
				}
}
