using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polyhedron : MonoBehaviour
{
    bool check = true;
    float angle = -180 + (Mathf.Acos(-1 / Mathf.Sqrt(5)) * (180/Mathf.PI));
    public float testNum;
    public float radius;
    public int size;
    public float radMod; //radius modifier

    public GameObject pentagon;
    public GameObject hexagon;

    //Shape parameters for radius calculation
    float pentRadius;
    float pentInradius;
    float pentDiam;
    float hexRadius;
    float hexInradius;
    float hexDiam;
    float sideLength;   


    //angle jumps
    float smallJump;
    float bigJump;


    // Use this for initialization
    private void Start()
    {
        //testNum = 0;
        //Debug.Log(angle);

        //pentagon calculations
        pentRadius = 1f;
        pentInradius = Mathf.Cos(Mathf.PI / 5); //important for calculations
        Debug.Log(pentInradius);

        //sidelength gleened from pentagon, sideLength equal for both shapes
        sideLength = 2 * pentRadius * Mathf.Sin(Mathf.PI / 5);

        //hexagon calculations
        hexRadius = sideLength / (2 * Mathf.Sin(Mathf.PI / 6));
        hexInradius = hexRadius * (Mathf.Cos(Mathf.PI / 6)); //important for calculations

        setRadius();

        //Debug.Log(radius);




    }

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

    void LateUpdate()
    {
        hexagon = GameObject.Find("hexagon");
        pentagon = GameObject.Find("pentagon");
        if (check == true)
        {
            //size: 1 = dodecahedron, +1 per hexagon between pentagons
            int T = size * size;
            int facecount = 10 * T + 2;
            int hexcount = facecount - 12;

            //radius = testNum + (size * 1.2f) * 0.5f * Mathf.Sqrt((5 / 2) + (11 / 10) * Mathf.Sqrt(5));
            setRadius();
            //Debug.Log(radius);
            GameObject[] pentagons = new GameObject[12];
            GameObject[] hexagons = new GameObject[hexcount * 2];
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
                    if (j == 1)
                    {
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i].axis, smallJump);
                    }
                    else
                    {
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i].axis, smallJump + (bigJump * (j - 1)));
                    }
                    hexagons[h].transform.Rotate(new Vector3(0, 0, 90 + (i - 2) * 252));

                }
            }
            //initialize front half
            for (int i = 1; i < 6; i++)
            {
                pentagons[i] = Object.Instantiate(pentagons[0], pentagons[0].transform.position, pentagons[0].transform.rotation);
                pentagons[i].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, angle);
                pentagons[i].transform.Rotate(new Vector3(0, 0, 180));

                for (int j = 0; j < 5; j++)
                {
                    for (int k = 1; k < size; k++)
                    {
                        int h = i * 5 * (size - 1) + (k - 1);
                        int baseHex = j * (size - 1) + (k - 1);
                        hexagons[h] = Object.Instantiate(hexagon, hexagons[baseHex].transform.position, hexagons[baseHex].transform.rotation);
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, angle);
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
                    if (j == 1) {
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i].axis, smallJump);
                    } else
                    {
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i].axis, smallJump + (bigJump * (j - 1)));
                    }
                    hexagons[h].transform.Rotate(new Vector3(0, 0, 90 - (i - 2) * 252));
                }
            }
            //initialize back half
            for (int i = 7; i < 12; i++)
            {
                pentagons[i] = Object.Instantiate(pentagons[6], pentagons[6].transform.position, pentagons[6].transform.rotation);
                pentagons[i].transform.RotateAround(Vector3.zero, pentEdges[i - 7].axis, angle);
                pentagons[i].transform.Rotate(new Vector3(0, 0, 180));
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 1; k < size; k++)
                    {
                        int h = i * 5 * (size - 1) + (k - 1);
                        int baseHex = j * (size - 1) + (k - 1) + ((size - 1) * 30);
                        hexagons[h] = Object.Instantiate(hexagon, hexagons[baseHex].transform.position, hexagons[baseHex].transform.rotation);
                        hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i - 7].axis, angle);
                        hexagons[h].transform.RotateAround(pentagons[i].transform.position, pentagons[i].transform.position, 108);
                    }
                }
            }
            //Destroy(hexagon);
            hexagon.transform.position = new Vector3(0, 0, 0);
        }
/*
        //I WROTE THIS BY ALEX PERROTTIvvvvv
        for (int l = 1; l < k; l++)
        {
            GameObject lastHex = hexagons[h + l - 1];
            Vector3 pentAxis = Vector3.Cross(pentEdges[i].axis, Vector3.up);
            hexagons[h + l] = Object.Instantiate(hexagon, lastHex.transform.position, lastHex.transform.rotation);
            hexagons[h + l].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, bigJump / -2f);
            hexagons[h + l].transform.RotateAround(Vector3.zero, pentAxis, bigJump * 0.75f);

        }*/
        check = false;
        hexagon.transform.position = new Vector3(0, 0, 0);
    }


    private void setRadius()
    {
        //BS circumference estimate
        float tempAngle = angle * -1f;
        //Debug.Log(tempAngle);
        float partialDist = ((size - 1) * 2 * hexInradius) + (2 * pentInradius);
        Debug.Log(partialDist);
        float circ = ((360 / tempAngle) * partialDist);
        radius = circ / (2 * Mathf.PI);

        smallJump = ((pentInradius + hexInradius) / partialDist) * angle;
        bigJump = ((2 * hexInradius) / partialDist) * angle;

    }
    
}
