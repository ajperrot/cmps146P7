using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour {
				GameObject[] tiles;

				// Use this for initialization
				void Start () {
					   Polyhedron polyhedron = gameObject.GetComponent<Polyhedron>();
								tiles = polyhedron.tiles;
								float radius = polyhedron.radius;
								//set id, altitude, adjacency, and biome for each tile
								for(int i = 0; i < tiles.Length; i++)
								{
								    GameObject tile = tiles[i];
											 Properties props = tile.GetComponent<Properties>();
												//set id
												props.id = i;
												//set altitude
												float x = (tile.transform.rotation.y / Mathf.PI) * 10;
												print(x);
												float y = (tile.transform.position.y / radius) * 10;
												print(y);
												float noise = Mathf.PerlinNoise(x, y);
												props.changeAltitude((noise * 16000) - 8000);
												//set adjacency
												props.adjacentTiles = adjTiles(tile);
												//set tempurature

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
								GameObject[] temp = (GameObject[])tiles.Clone();
								temp.OrderBy(t=>(t.transform.position - tile.transform.position));
								//skip the closest element (the same one as tile)
								return temp.Take(sides + 1).Skip(1).Cast<GameObject>().ToArray();
				}
	
}
