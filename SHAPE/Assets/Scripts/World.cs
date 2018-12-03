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
								//set altitude and biome for each tile
								foreach(GameObject tile in tiles){
									
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
								tiles.OrderBy(t=>(t.transform.position - tile.transform.position).sqrMagnitude);
								//skip the closest element (the same one as tile)
								return tiles.Take(sides + 1).Skip(1).Cast<GameObject>().ToArray();
				}
	
}
