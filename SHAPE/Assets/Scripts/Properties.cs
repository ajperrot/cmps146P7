using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour {

	public float latitude;
	public int id = 0;
	public float altitude = 0f;
	public GameObject[] adjacentTiles;
	public int temperature;
	public int biome; //enumeration
	//biomes are in order of heat:
    // 0 = arctic
    // 1 = taiga
    // 2 = forest
    // 3 = plains
    // 4 = jungle
    // 5 = desert
    // 6 = ocean

	class Species
	{
		//data on species population on one tile
		public int count; //# of creatures of species on this tile
		public int idealTemp; //temperature it prefers
		public bool carnivore; //can it eat meat
		public bool herbivore; //can it eat non-meat
		public bool flying; //can it make big altitude jumps from tile to tile
		public bool aquatic; //can it survive only in water
		public bool amphibious; // can it survive in both land and water

		public Species()
		{
			count = 0;
			idealTemp = 0;
			carnivore = false;
			herbivore = false;
			flying = false;
			aquatic = false;
			amphibious = false;
		}

		public string ToString()
		{
			return("count: "+this.count+", idealTemp: "+this.idealTemp+", carnivore: "+this.carnivore+", herbivore: "+this.herbivore+", flying: "+this.flying+", aquatic: "+this.aquatic+", amphibious: "+this.amphibious+"\n");
		}
	}
	List<Species> population = new List<Species>(); //all species living on a tile

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
        this.biome = newBiome;
        switch(newBiome){
            case 0:
                rend.material.SetColor("_Color", Color.cyan);
                break;
            case 1:
                rend.material.SetColor("_Color", Color.white);
                break;
            case 2:
                rend.material.SetColor("_Color", Color.gray);
                break;
            case 3:
                rend.material.SetColor("_Color", Color.yellow);
                break;
            case 4:
                rend.material.SetColor("_Color", Color.green);
                break;
            case 5:
                rend.material.SetColor("_Color", Color.red);
                break;
            case 6:
                rend.material.SetColor("_Color", Color.blue);
                break;
        }
    }

	//sets a new temperature and biome if necessary
	public void changeTemperature(int newTemp)
	{
		if(temperature / 10 != newTemp / 10 || temperature == 0)
		{
			if(newTemp > 40)
			{
				//all 40+ temperatures go to desert
				changeBiome(5);
			}else if(newTemp < 0)
			{
				//all negative temperatures go to arctic
				changeBiome(0);
			}else
			{
				changeBiome(newTemp / 10 + 1);
			}
		}
		this.temperature = newTemp;
	}

	public void creatureAct()
	{
		//update creature population via possible creature actions
	}

	public void populate()
	{
		//intialize this tile with 2 herbivorous species for the biome
		Species newLife = new Species();
		newLife.count = 2;
		newLife.herbivore = true;
		if(Random.value < 0.5)
		{
			//50% chance to be flight-capable
			newLife.flying = true;
		}
		if(biome == 6)
		{
			//if this is an ocean
			newLife.idealTemp = Mathf.RoundToInt(Random.value * 30);
			if(Random.value < 0.5)
			{
				newLife.aquatic = true;
			}else
			{
				newLife.amphibious = true;
			}
		}else
		{
			//if this is not an ocean
			if(Random.value < 0.5)
			{
				newLife.amphibious = true;
			}
			newLife.idealTemp = Mathf.RoundToInt((biome - 1) * 10 + Random.value * 10);
		}
		//add this new species to the population
		population.Add(newLife);
	}

}
