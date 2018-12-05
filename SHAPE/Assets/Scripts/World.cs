using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour {
	GameObject[] tiles;
	private List<Properties> liveTiles = new List<Properties>();

	// Use this for initialization
	void Start () {
        float addx = Random.Range(0f, 255f);
        float addy = Random.Range(0f, 255f);

		Polyhedron polyhedron = gameObject.GetComponent<Polyhedron>();
		tiles = polyhedron.tiles;
		float radius = polyhedron.radius;
		//set id, altitude, adjacency for each tile, as well as bome for ocean tiles
		for(int i = 0; i < tiles.Length; i++)
		{
			GameObject tile = tiles[i];
			Properties props = tile.GetComponent<Properties>();
			//set id
			props.id = i;
			//set altitude
			float x = (tile.transform.rotation.y / Mathf.PI) * 100 * 0.1f + addx;
			float y = (tile.transform.position.y / radius) * 30 * 0.1f + addy;
			float noise = Mathf.PerlinNoise(x, y);
			props.changeAltitude((noise * 16000) - 8000);
			//set adjacency
			props.adjacentTiles = adjTiles(tile);
			//set tiles below sea level to ocean
			if(props.altitude < 0)
			{
				props.changeBiome(6);
			}

		}

		//set tempurature and biome based on distance to water
		for(int i = 0; i < tiles.Length; i++){
			GameObject tile = tiles[i];
			Properties props = tile.GetComponent<Properties>();
			//warmth: value 0-1 determining likelyhood to be warm based on latitude
			float warmth = (1 - (props.latitude / radius));
			if(props.biome == 6)
			{
				//if this is an ocean
				//temperature is 0-30 for ocean
				props.temperature = Mathf.RoundToInt((((Random.value / 4f) + warmth) / 1.25f) * 30f);
			}else
			{
				//if this is not an ocean
				int distToWater = closestWater(tile);
				float baseTemp;
				if(distToWater < 0)
				{
					//landlocked planet conditions
					baseTemp = Random.Range(-50, 50);
				}else{
					baseTemp = Random.Range(distToWater * -5, distToWater * 10);
				}
				float altAffect = props.altitude / -200f;
				int tempTemp = Mathf.RoundToInt(baseTemp + altAffect + warmth * 50);
				if(tempTemp > 50)
				{
					tempTemp = 50;
				}else if(tempTemp < -50)
				{
					tempTemp = -50;
				}
				props.changeTemperature(tempTemp);
			}
			//populate a certain number of tiles, adding them to liveTiles
			if(i == 0)
			{
				//populate only the first tile
				props.populate();
				liveTiles.Add(props);
			}
		}
	}

	void Update()
	{
		//update all tiles with life on them
		foreach(Properties props in liveTiles)
		{
			props.creatureAct();
		}
	}

	GameObject[] adjTiles(GameObject tile)
	{
		int sides;
		if(tile.GetComponent<PentaPrism>())
			{
				//if it has 5 sides
				sides = 5;
			}else
			{
				//if it has 6 sides
				sides = 6;
			}
		;
		//skip the closest element (the same one as tile)
		return tiles.OrderBy(t=> Vector3.Distance(t.transform.position, tile.transform.position)).Take(sides + 1).Skip(1).Cast<GameObject>().ToArray();
	}

	//use A* to find closest ocean tile and return the distance in tiles
	int closestWater(GameObject tile)
	{
		//priority queue
		List<Properties> queue = new List<Properties>();
		queue.Add(tile.GetComponent<Properties>());
		//distance to tile
		int[] distances = new int[tiles.Length];
		distances[queue[0].id] = 0;
		//distance + heuristic
		float[] costs = new float[tiles.Length];
		costs[queue[0].id] = 0;
		
		//main A* loop
		while(queue.Count != 0)
		{
			//sort queue by costs
			queue = queue.OrderBy(p=> costs[p.id]).Cast<Properties>().ToList();
			Properties currentNode = queue[0];
			queue.RemoveAt(0);
			//Check if current node is the destination
        	if(currentNode.biome == 6)
			{
				return distances[currentNode.id];
			}else
			{
				int dist = distances[currentNode.id] + 1;
				GameObject[] adjNodes = currentNode.adjacentTiles;
				for(int i = 0; i < adjNodes.Length; i++)
				{
					Properties adjNode = adjNodes[i].GetComponent<Properties>();
					//if tile not yet hit or only from a less optimal path
					if(distances[adjNode.id] == 0 || dist < distances[adjNode.id])
					{
						//add this version of this tile to the queue
						distances[adjNode.id] = dist;
						//up to +/- 1 for altitude, to prioritize sea-level tiles
						costs[adjNode.id] = dist + (adjNode.altitude / 8000f);
						queue.Add(adjNode);
					}
				}
			}

		}
		// -1 indicates no water on the planet
		return -1;
	}
	
}
