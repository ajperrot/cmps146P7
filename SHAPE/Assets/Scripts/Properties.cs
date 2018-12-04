using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour {

	public float latitude;
	public int id = 0;
	public float altitude = 0f;
	public GameObject[] adjacentTiles;
	public int biome = 0; //enumeration
	//biomes are alphabetical:
    // 0 = arctic
    // 1 = desert
    // 2 = forest
    // 3 = jungle
    // 4 = ocean
    // 5 = plains
    // 6 = taiga
				
				

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

    public void changeBiome(int newBiome)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        biome = newBiome;
        switch(newBiome){
            case 0:
                rend.material.SetColor("_Color", Color.cyan);
                break;
            case 1:
                rend.material.SetColor("_Color", Color.red);
                break;
            case 2:
                rend.material.SetColor("_Color", Color.green);
                break;
            case 3:
                rend.material.SetColor("_Color", Color.gray);
                break;
            case 4:
                rend.material.SetColor("_Color", Color.blue);
                break;
            case 5:
                rend.material.SetColor("_Color", Color.yellow);
                break;
            case 6:
                rend.material.SetColor("_Color", Color.white);
                break;
        }
    }

}
