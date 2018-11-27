using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polyhedron : MonoBehaviour
{
    bool check = true;
    float angle = Mathf.Acos(-1 / Mathf.Sqrt(5)) * (180/Mathf.PI);
    public float testNum;
    public float radius;

    private void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            check = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            testNum += 0.01f;
            check = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            testNum -= 0.01f;
            check = true;
        }

    }

    private void Start()
    {
        testNum = 0;
        Debug.Log(angle);
    }

    public int size;
    public float radMod; //radius modifier
    // Use this for initialization
    void LateUpdate()
    {
        GameObject hexagon = GameObject.Find("hexagon");
        if (check == true)
        {
            //size: 1 = dodecahedron, +1 per hexagon between pentagons
            int T = size * size;
            int facecount = 10 * T + 2;
            int hexcount = facecount - 12;

            radius = testNum + (size * 1.2f) * 0.5f * Mathf.Sqrt((5 / 2) + (11 / 10) * Mathf.Sqrt(5));
            GameObject[] pentagons = new GameObject[12];
            GameObject[] hexagons = new GameObject[hexcount * 2];
            GameObject pentagon = GameObject.Find("pentagon");
            Edge[] pentEdges = pentagon.GetComponent<PentaPrism>().edges;

            hexagon.transform.Translate(Vector3.forward * radius);
            hexagon.transform.Rotate(new Vector3(0, 180, 0));

            //base front pentagon
            pentagons[0] = Object.Instantiate(pentagon, gameObject.transform.position, gameObject.transform.rotation);
            pentagons[0].transform.Translate(Vector3.forward * radius);
            pentagons[0].transform.Rotate(new Vector3(0, 180, 0));
            //Destroy(pentagon);
            //initialize reletive hexagon positions

            for (int i = 0; i < 5; i++)
            {
                for (int j = 1; j < size; j++)
                {
                    int h = i * (size - 1) + (j - 1);
                    hexagons[h] = Object.Instantiate(hexagon, pentagons[0].transform.position, pentagons[0].transform.rotation);
                    hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i].axis, (-(180 - angle) / size) * j);
                    hexagons[h].transform.Rotate(new Vector3(0, 0, 90 + (i - 2) * 252));
                }
            }
            //initialize front half
            for (int i = 1; i < 6; i++)
            {
                pentagons[i] = Object.Instantiate(pentagons[0], pentagons[0].transform.position, pentagons[0].transform.rotation);
                pentagons[i].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, 180 + angle);
                pentagons[i].transform.Rotate(new Vector3(0, 0, 180));

                for (int j = 0; j < 5; j++)
                {
                    for (int k = 1; k < size; k++)
                    {
                        int h = i * 5 * (size - 1) + (k - 1);
                        int baseHex = j * (size - 1) + (k - 1);
                        hexagons[h] = Object.Instantiate(hexagon, hexagons[baseHex].transform.position, hexagons[baseHex].transform.rotation);
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, 180 + angle);
                        hexagons[h].transform.RotateAround(pentagons[i].transform.position, pentagons[i].transform.position, 108);
                    }
                }
            }
            
            //back hemisphere
            pentagons[6] = Object.Instantiate(pentagons[0], pentagons[0].transform.position * -1f, pentagons[0].transform.rotation);
            pentagons[6].transform.Rotate(new Vector3(180, 0, 0));
            //initialize reletive hexagon positions for back
            for (int i = 0; i < 5; i++)
            {
                for (int j = 1; j < size; j++)
                {
                    int h = i * (size - 1) + (j - 1) + ((size - 1) * 30);
                    hexagons[h] = Object.Instantiate(hexagon, pentagons[6].transform.position, pentagons[6].transform.rotation);
                    hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i].axis, (-(180 - angle) / size) * j);
                    hexagons[h].transform.Rotate(new Vector3(0, 0, 90 - (i - 2) * 252));
                }
            }
            //initialize back half
            for (int i = 7; i < 12; i++)
            {
                pentagons[i] = Object.Instantiate(pentagons[6], pentagons[6].transform.position, pentagons[6].transform.rotation);
                pentagons[i].transform.RotateAround(Vector3.zero, pentEdges[i - 7].axis, 180 + angle);
                pentagons[i].transform.Rotate(new Vector3(0, 0, 180));
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 1; k < size; k++)
                    {
                        int h = i * 5 * (size - 1) + (k - 1);
                        int baseHex = j * (size - 1) + (k - 1) + ((size - 1) * 30);
                        hexagons[h] = Object.Instantiate(hexagon, hexagons[baseHex].transform.position, hexagons[baseHex].transform.rotation);
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i - 7].axis, 180 + angle);
                        hexagons[h].transform.RotateAround(pentagons[i].transform.position, pentagons[i].transform.position, 108);
                    }
                }
            }
            //Destroy(hexagon);
            hexagon.transform.position = new Vector3(0, 0, 0);
        }
        check = false;
        hexagon.transform.position = new Vector3(0, 0, 0);
    }

}
