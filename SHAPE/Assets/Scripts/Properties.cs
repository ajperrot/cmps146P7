using System.Collections;
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
	Color[] forests = { new Color(0f / 255f, 102f / 255f, 0f / 255f), new Color(0f / 255f, 70f / 255f, 0f / 255f), new Color(0f / 255f, 102f / 255f, 34f / 255f) };
    Color[] ocean = { new Color(0f / 255f, 0f / 255f, 255f / 255f), new Color(0f / 255f, 0f / 255f, 204f / 255f), new Color(0f / 255f, 0f / 255f, 153f / 255f) };
    Color[] plains = { new Color(204f / 255f, 255f / 255f, 51f / 255f), new Color(204f / 255f, 255f / 255f, 102f / 255f), new Color(204f / 255f, 255f / 255f, 153f / 255f) };
    Color[] deserts = { new Color(255f / 255f, 204f / 255f, 179f / 255f), new Color(255f / 255f, 204f / 255f, 153f / 255f), new Color(255f / 255f, 204f / 255f, 102f / 255f) };
    Color[] jungles = { new Color(105f / 255f, 255f / 255f, 51f / 255f), new Color(51f / 255f, 204f / 255f, 51f / 255f), new Color(0f / 255f, 255f / 255f, 0f / 255f) };
    Color[] taiga = { new Color(255f / 255f, 255f / 255f, 255f / 255f), new Color(240f / 255f, 240f / 255f, 240f / 255f), new Color(230f / 255f, 230f / 255f, 255f / 255f)};
    Color[] Arctic = { new Color(125f / 255f, 255f / 255f, 255f / 255f), new Color(125f / 255f, 150f / 255f, 255f / 255f), new Color(125f / 255f, 200f / 255f, 255f / 255f) };
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
			return(this.name+", count: "+this.count+"\n  idealTemp: "+this.idealTemp+", birthrate: "+this.birthrate+", carnivore: "+this.carnivore+", herbivore: "+this.herbivore+"\n  flying: "+this.flying+", aquatic: "+this.aquatic+", amphibious: "+this.amphibious+"\n");
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

		public Species Breed(Species mom){
			Species son = new Species();
			//cut and conjoin names for the son
			string firstHalf = this.name.Substring(0, this.name.Length / 2);
			string secondHalf = mom.name.Substring(mom.name.Length / 2);
			son.name = firstHalf + secondHalf;
			son.count = 2;
			son.idealTemp = Random.Range(this.idealTemp, mom.idealTemp);
			son.birthrate = Random.Range(this.birthrate, mom.birthrate);
			float seed = Random.value;
			if(seed < 0.25)
			{
				son.carnivore = true;
				son.herbivore = false;
			}
			else
			{
				son.carnivore = false;
				son.herbivore = true;
				if(seed < 0.5)
				{
					son.flying = true;
				}
				else
				{
					son.flying = this.flying;
					if(seed < 0.75)
					{
						son.amphibious = true;
						
					}
					else
					{
						son.amphibious = mom.amphibious;
					}
				}
			}
			if(son.amphibious == false)
			{
				son.aquatic = this.aquatic;
			}
			else
			{
				son.aquatic = false;
			}
			return son;
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
        if(altitude < 0) {
            altitude = altitude / 100;
        }
        if (newAlt <= 0)
        {
            newAlt = -100;
        }
        else if (newAlt <= 1000)
        {
            newAlt = 500;
        }
        else if (newAlt <= 2000)
        {
            newAlt = 1500;
        }
        else if (newAlt <= 3000)
        {
            newAlt = 2500;
        }
        else if (newAlt <= 4000)
            newAlt = 3500;
        else
            newAlt = 4500;
		Vector3 scale = gameObject.transform.localScale;
        //scale function based around scale 2 = sea level
        scale.z = ((newAlt + 8025) / 4000) * 0.8506508f * 1.2f - 1.8f;
		gameObject.transform.localScale = scale;
	}

    public void changeBiome(int newBiome)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        this.biome = newBiome;
        var temp = Random.Range(0, 3);
        switch (newBiome){
            case 0:
                rend.material.SetColor("_Color", Arctic[temp]);
                break;
            case 1:
                rend.material.SetColor("_Color", taiga[temp]);
                break;
            case 2:
                rend.material.SetColor("_Color", forests[temp]);
                break;
            case 3:
                rend.material.SetColor("_Color", plains[temp]);
                break;
            case 4:
                rend.material.SetColor("_Color", jungles[temp]);
                break;
            case 5:
                rend.material.SetColor("_Color", deserts[temp]);
                break;
            case 6:
                rend.material.SetColor("_Color", ocean[temp]);
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

	public void FixedUpdate()
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
				population.RemoveAt(i);
				return;
			}

			//intraspecies breeding if space remains
			if(currentPop < maxPop && (herd.carnivore == false || population.Count > 2))
			{
				herd.count *= herd.birthrate;
			}

			//we'll add it back at the end
			currentPop -= originalCount;
			
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
			if(biome == 6 && herd.aquatic != true && herd.amphibious != true)
			{
				deathrate = 1;
			}
			if(deathrate > 1)
			{
				deathrate = 1;
			}

			herd.count = Mathf.RoundToInt((1 - deathrate) * herd.count);

			//if the species is dying or overcrowded, consider leaving
			if(herd.count > 1 && (deathrate > 0.5 || currentPop + originalCount > maxPop || (herd.carnivore && population.Count == 1)));
			{

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
					herd.count /= 2;

					bool found = false;
					foreach(Species pop in adjProp.population)
					{
						if(pop.name == emmigrants.name)
						{
							pop.count += emmigrants.count;
							found = true;
							break;
						}
					}
					if(found == false)
					{
						//if the species does not already exist there
						adjProp.population.Add(emmigrants);
					}
					adjProp.currentPop += emmigrants.count;
				}
			}

			//update current population count
			currentPop += herd.count;

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

				return;
			}
		}

		//handle interspecies breeding
		if(currentPop < maxPop && population.Count > 1 && Random.value > 0.05)
		{
			int dadIndex = Random.Range(0, population.Count);
			Species dad = population[dadIndex];
			Species mom;
			if(dadIndex == 0)
			{
				mom = population[dadIndex + 1];
			}
			else
			{
				mom = population[dadIndex - 1];
			}
			Species son = dad.Breed(mom);
			bool exists = false;
			foreach(Species pop in population)
			{
				if(pop.name == son.name)
				{
					//if it already exists just add to the existing population
					exists = true;
					pop.count += son.count;
					break;
				}
			}
			if(exists == false)
			{
				population.Add(son);
			}
			currentPop += son.count;
		}
		
	}

	public void populate()
	{
		//intialize this tile with 2-100 herbivorous species for the biome
		Species newLife = new Species();
		newLife.count = Random.Range(2, 100);
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

		//set name
		switch(biome)
		{
			case 0:
				if(newLife.flying)
				{
					if(newLife.amphibious)
					{
						newLife.name = "Flying Penguin";
					}
					else
					{
						newLife.name = "Reanimated Pterodactyl";
					}
				}
				else
				{
					if(newLife.amphibious)
					{
						newLife.name = "Walrus";
					}
					else
					{
						newLife.name = "Polar Bear";
					}
				}
				break;
			case 1:
				if(newLife.flying)
				{
					if(newLife.amphibious)
					{
						newLife.name = "Seal";
					}
					else
					{
						newLife.name = "Crossbill";
					}
				}
				else
				{
					if(newLife.amphibious)
					{
						newLife.name = "Leopard Frog";
					}
					else
					{
						newLife.name = "Moose";
					}
				}
				break;
			case 2:
				if(newLife.flying)
				{
					if(newLife.amphibious)
					{
						newLife.name = "Flying Tree Frog";
					}
					else
					{
						newLife.name = "Woodpecker";
					}
				}
				else
				{
					if(newLife.amphibious)
					{
						newLife.name = "Poison Dart Frog";
					}
					else
					{
						newLife.name = "Panda Bear";
					}
				}
				break;
			case 3:
				if(newLife.flying)
				{
					if(newLife.amphibious)
					{
						newLife.name = "Airborne Otter";
					}
					else
					{
						newLife.name = "Condor";
					}
				}
				else
				{
					if(newLife.amphibious)
					{
						newLife.name = "Beaver";
					}
					else
					{
						newLife.name = "Groundhog";
					}
				}
				break;
			case 4:
				if(newLife.flying)
				{
					if(newLife.amphibious)
					{
						newLife.name = "Mosquito";
					}
					else
					{
						newLife.name = "Flamingo";
					}
				}
				else
				{
					if(newLife.amphibious)
					{
						newLife.name = "Aligator";
					}
					else
					{
						newLife.name = "Ape";
					}
				}
				break;
			case 5:
				if(newLife.flying)
				{
					if(newLife.amphibious)
					{
						newLife.name = "Aerial Salamander";
					}
					else
					{
						newLife.name = "Vulture";
					}
				}
				else
				{
					if(newLife.amphibious)
					{
						newLife.name = "Salamander";
					}
					else
					{
						newLife.name = "Camel";
					}
				}
				break;
			case 6:
				if(newLife.flying)
				{
					newLife.name = "Seagull";
				}
				else
				{
					newLife.name = "Fish";
				}
				break;
		}

		//add this new species to the population
		population.Add(newLife);
		//update current population count
		currentPop += 2;
	}

	/*---------------------UI----------------------*/
	private void OnMouseOver()
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
				testText.text += "\n"+herd.ToString();
			}


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