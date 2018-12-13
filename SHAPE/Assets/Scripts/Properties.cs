﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Properties : MonoBehaviour {

	public float latitude;
	public int id = 0;
	public float altitude = 0f;
	public int maxPop = 0;
	public int currentPop = 0;
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

	private string[] biomeNames = {"Arctic", "Taiga", "Forest", "Plains", "Jungle", "Desert", "Ocean"};

	class Species
	{
		//data on species population on one tile
		public string name; //just for output fun
		public int count; //# of creatures of species on this tile
		public int idealTemp; //temperature it prefers
		public int birthrate; //rate of birth per count per fixedUpdate
		public bool carnivore; //can it eat meat
		public bool herbivore; //can it eat non-meat
		public bool flying; //can it make big altitude jumps from tile to tile
		public bool aquatic; //can it survive only in water
		public bool amphibious; // can it survive in both land and water

		public Species()
		{
			name = "";
			count = 0;
			idealTemp = 0;
			birthrate = 0;
			carnivore = false;
			herbivore = false;
			flying = false;
			aquatic = false;
			amphibious = false;
		}

		public override string ToString()
		{
			return("count: "+this.count+"\n  idealTemp: "+this.idealTemp+", birthrate: "+this.birthrate+", carnivore: "+this.carnivore+", herbivore: "+this.herbivore+"\n  flying: "+this.flying+", aquatic: "+this.aquatic+", amphibious: "+this.amphibious+"\n");
		}

		public Species Copy(){
			Species copy = new Species();
			copy.name = this.name;
			copy.count = this.count;
			copy.idealTemp = this.idealTemp;
			copy.birthrate = this.birthrate;
			copy.carnivore = this.carnivore;
			copy.herbivore = this.herbivore;
			copy.flying = this.flying;
			copy.aquatic = this.aquatic;
			copy.amphibious = this.amphibious;
			return copy;
		}
	}
	List<Species> population = new List<Species>(); //all species living on a tile

	private MeshRenderer meshRend;
	private bool active = false;

	// Use this for initialization
	void Start()
	{
		latitude = Mathf.Abs(gameObject.transform.position.y);
		meshRend = GetComponent<MeshRenderer>();
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

	public void Update()
	{

		//update creature birth
		for(int i = 0; i < population.Count; i++)
		{
			Species herd = population[i];
			bool killedAnother = false;
			int extinctIndex = 0;
			int originalCount = herd.count;

			//end if the species is dead to avoid errors
			if(herd.count < 1)
			{
				print("went extinct");//test
				population.RemoveAt(i);
				return;
			}

			//intraspecies breeding if space remains
			if(currentPop < maxPop)
			{
				//print("maxPop = "+maxPop);//test
				herd.count *= herd.birthrate;
			}

			//we'll add it back at the end
			currentPop -= originalCount;
			//print(herd.count);//test
			
			//eat
			if(herd.carnivore)
			{
				Species prey = null;
				int j = i - 1;
				if(j > 0 && j < population.Count)
				{
					prey = population[j];
				}
				else
				{
					j = i + 1;
					if(j > 0 && j < population.Count)
					{
						prey = population[j];
					}
					else
					{
						//no avalible prey == canibalism
						herd.count /= 2;
					}
				}
				if(prey != null)
				{
					int appetite = herd.count / 10;
					int originalPreyCount = prey.count;
					prey.count -= appetite;
					currentPop -= appetite;
					if(prey.count < 0)
					{
						//handle making another species extinct
						currentPop += appetite;
						currentPop -= originalPreyCount;
						killedAnother = true;
						extinctIndex = j;
					}
				}
			}

			//die based on tile's hospitality
			float deathrate = Mathf.Abs(.05f * (herd.idealTemp - temperature));
			if(deathrate > 1)
			{
				deathrate = 1;
			}
			//print("dr = "+deathrate);//test
			herd.count = Mathf.RoundToInt((1 - deathrate) * herd.count);

			//if the species is dying or overcrowded, consider leaving
			if(herd.count > 1 && (deathrate > 0.5 || currentPop + originalCount > maxPop))
			{
				print("considering migration");
				Properties adjProp = null;
				//migrate based on adjacent tiles' hospitality
				float[] modifiers = new float[adjacentTiles.Length];
				for(int j = 0; j < adjacentTiles.Length; j++)
				{
					adjProp = adjacentTiles[j].GetComponent<Properties>();
					//if the creature cannot breathe there
					if(adjProp.biome == 6 && (herd.amphibious == false && herd.aquatic == false) || (adjProp.biome != 6 && herd.aquatic))
					{
						modifiers[j] = 0;
					}
					else
					{
						//if the creature cannot get there
						if(adjProp.biome != 6 && adjProp.altitude - altitude > 250 && herd.flying == false)
						{
							modifiers[j] = 0;
						}
						else //life there is possible
						{
							//make a tile more desireable if it fits the ideal tempurature of the species
							modifiers[j] = j / adjacentTiles.Length + (1 - (Mathf.Abs(.05f * (herd.idealTemp - temperature))));
						}
					}	
				}

				//select a tile to explore based on modifiers and chance
				float exploreChoice = Random.value;
				adjProp = null;
				for(int j = 0; j < adjacentTiles.Length; j++)
				{
					if(exploreChoice < modifiers[j])
					{
						adjProp = adjacentTiles[j].GetComponent<Properties>();
						break;
					}
				}

				//handle move itself
				if(adjProp != null)
				{
					Species emmigrants = herd.Copy();
					//half the species moves at a time. This is just to keep things running
					emmigrants.count /= 2;
					print("migrating");//test
					//adjProp.population.Add(emmigrants);
					//adjProp.currentPop += emmigrants.count;
					//herd.count /= 2;
				}
			}

			//update current population count
			currentPop += herd.count;
			print("count = "+herd.count);//test
			if(herd.count > 1)
			{
				population.RemoveAt(i);
				population.Insert(i, herd);
				if(killedAnother)
				{
					population.RemoveAt(extinctIndex);
					return;
				}
			}
			else
			{
				population.RemoveAt(i);
				print("went extinct");//test
				return;
			}
		}
	}

	public void populate()
	{
		//intialize this tile with 2-100 herbivorous species for the biome
		Species newLife = new Species();
		newLife.count = Mathf.RoundToInt(Random.Range(2, 100));
		newLife.birthrate = Mathf.RoundToInt(Random.Range(2, 10));
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
		//update current population count
		currentPop += 2;
	}

	/*---------------------UI----------------------*/
	private void OnMouseEnter()
    {	
		if(meshRend.isVisible)
 		{
     		// The x axis of Obj points at the camera
			GameObject UI = GameObject.Find("UI");
			Text testText = UI.GetComponent<Text>();
			//set relevant text
			testText.text = "";
			testText.text += biomeNames[biome]+"\n";
			testText.text += "Temperature: "+temperature+", Altitude: "+altitude+"\n Max Population: "+maxPop+"\n";
			for(int i = 0; i < population.Count; i++)
			{
				Species herd = population[i];
				testText.text += herd.ToString() + "\n";
			}

			//print(testText.text);//test
			//set visual
			Color color = meshRend.material.color;
			color.a = 0.5f;
			meshRend.material.color = color;
			active = true;
 		}
    }

	private void OnMouseExit()
    {
		if(active == true)
		{
			GameObject UI = GameObject.Find("UI");
			Text testText = UI.GetComponent<Text>();
			//reset text
			testText.text = "";

			//set visual
			Color color = meshRend.material.color;
			color.a = 1;
			meshRend.material.color = color;
			//return actuve to false
			active = false;
		}

    }

}
