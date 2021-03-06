using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polyhedron : MonoBehaviour
{
    public GameObject[] tiles;
    List<GameObject> tileList = new List<GameObject>();

    //bool check = true;
    bool firstUpdate = true;
    float angle = -180 + (Mathf.Acos(-1 / Mathf.Sqrt(5)) * (180/Mathf.PI));
    float angle2;
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
    float smallJump2;
    float bigJump2;
    //float triJump;

    // Use this for initialization
    private void Start()
    {
        //pentagon calculations
        pentRadius = 1f;
        pentInradius = Mathf.Cos(Mathf.PI / 5); //important for calculations
        pentDiam = pentRadius + pentInradius;

        //sidelength gleened from pentagon, sideLength equal for both shapes
        sideLength = 2 * pentRadius * Mathf.Sin(Mathf.PI / 5);

        //hexagon calculations
        hexRadius = sideLength / (2 * Mathf.Sin(Mathf.PI / 6));
        hexInradius = hexRadius * (Mathf.Cos(Mathf.PI / 6)); //important for calculations

        setRadius();

        hexagon = GameObject.Find("hexagon");
        hexagon.transform.localScale = new Vector3(1, 1, 1);
        pentagon = GameObject.Find("pentagon");
        //angle2 = (((2 * pentRadius) + sideLength) / ((4 * pentDiam) + (2 * sideLength))) * 360;
        angle2 = (360 + (angle * 2)) / 2;
        angle2 *= -1;
        Debug.Log("angle 1: " + angle);
        Debug.Log("angle 2: " + angle2);
        //size: 1 = dodecahedron, +1 per hexagon between pentagons
        int T = size * size;
        int facecount = 10 * T + 2;
        int hexcount = facecount - 12;

        tiles = new GameObject[facecount];

        setRadius();
        GameObject[] pentagons = new GameObject[6];
        GameObject[] backPent = new GameObject[6];
        GameObject[] hexagons = new GameObject[hexcount*2];
        GameObject[] backHex = new GameObject[hexcount*2];        
        GameObject[] filler = new GameObject[hexcount*2];
        GameObject[] backFill = new GameObject[hexcount*2];
        int fillerIndex = 0;
        int fillerInit = 0;
        Edge[] pentEdges = pentagon.GetComponent<PentaPrism>().edges;

        hexagon.transform.Translate(Vector3.forward * radius);
        hexagon.transform.Rotate(new Vector3(0, 180, 0));

        //base front pentagon
        pentagons[0] = Object.Instantiate(pentagon, gameObject.transform.position, gameObject.transform.rotation);
        pentagons[0].transform.Translate(Vector3.forward * radius);
        pentagons[0].transform.Rotate(new Vector3(0, 180, 0));
        //back
        backPent[0] = Object.Instantiate(pentagons[0], pentagons[0].transform.position * -1f, pentagons[0].transform.rotation);
        backPent[0].transform.Rotate(new Vector3(0, 180, 180));
        //add to tileList
        tileList.Add(pentagons[0]);
        tileList.Add(backPent[0]);

        //initialize reletive hexagon positions
        for (int i = 0; i < 5; i++)
        {
            int nextPent = i + 1;
            pentagons[nextPent] = Object.Instantiate(pentagons[0], pentagons[0].transform.position, pentagons[0].transform.rotation);
            pentagons[nextPent].transform.RotateAround(Vector3.zero, pentEdges[i].axis, angle);
            pentagons[nextPent].transform.Rotate(new Vector3(0, 0, 180));
            //back
            backPent[nextPent] = Object.Instantiate(pentagons[nextPent], pentagons[nextPent].transform.position * -1f, pentagons[nextPent].transform.rotation);
            backPent[nextPent].transform.Rotate(new Vector3(0, 180, 180));
            //tileList
            tileList.Add(pentagons[nextPent]);
            tileList.Add(backPent[nextPent]);
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
                //back
                backHex[h] = Object.Instantiate(hexagons[h], hexagons[h].transform.position * -1f, hexagons[h].transform.rotation);
                backHex[h].transform.Rotate(new Vector3(0, 180, 180));
                //tileList
                tileList.Add(hexagons[h]);
                tileList.Add(backHex[h]);

                for (int k = 1; k < j; k++)
                {
                    
                       GameObject lastHex = hexagons[h];
                       //this should be perpendicular to the edge axis, i.e. going in the direction of the path
                       Vector3 pentAxis = pentagons[nextPent].transform.position - pentagons[0].transform.position;
                       filler[fillerIndex] = Object.Instantiate(hexagon, lastHex.transform.position, lastHex.transform.rotation);
                       filler[fillerIndex].transform.RotateAround(Vector3.zero, pentEdges[i].axis, bigJump * k / -2f);
                       filler[fillerIndex].transform.RotateAround(Vector3.zero, pentAxis, smallJump * k);
                       //back
                       backFill[fillerIndex] = Object.Instantiate(filler[fillerIndex], filler[fillerIndex].transform.position * -1f, filler[fillerIndex].transform.rotation);
                       backFill[fillerIndex].transform.Rotate(new Vector3(0, 180, 180));
                       //tileList
                       tileList.Add(filler[fillerIndex]);
                       tileList.Add(backFill[fillerIndex]);
                       //increment index
                       fillerIndex += 1;
                       
                }
                
                
            }
            /*
            for (int j = 1; j < size + 1; j++)
            {
                if (j == 1)
                {
                }
                else
                {
                    filler[fillerIndex] = Object.Instantiate(hexagon, pentagons[0].transform.position, pentagons[0].transform.rotation);
                    if (i < 4)
                        filler[fillerIndex].transform.RotateAround(Vector3.zero, (pentEdges[i + 1].axis - pentEdges[i].axis) / 2 + pentEdges[i].axis, smallJump2 + (bigJump2 * (j - 2)) );
                    else
                        filler[fillerIndex].transform.RotateAround(Vector3.zero, (pentEdges[0].axis - pentEdges[4].axis) / 2 + pentEdges[4].axis, smallJump2 + (bigJump2 * (j - 2)));

                    filler[fillerIndex].transform.Rotate(new Vector3(0, 0, 90 + (((i - 2) * 252) + 252 / 2)));
                    backFill[fillerIndex] = Object.Instantiate(filler[fillerIndex], filler[fillerIndex].transform.position * -1f, filler[fillerIndex].transform.rotation);
                    backFill[fillerIndex].transform.Rotate(new Vector3(0, 180, 180));
                    tileList.Add(filler[fillerIndex]);
                    tileList.Add(backFill[fillerIndex]);
                    fillerIndex++;
                }
            }
            */
            /*
            for (int test = 0; test < 2; test++)
            {
                if (fillerIndex < 2)
                    break;
                GameObject lastHex = filler[fillerIndex - 1];
                Vector3 hexAxis = lastHex.transform.position;
                GameObject newHex = Object.Instantiate(hexagon, lastHex.transform.position, lastHex.transform.rotation);
                newHex.transform.RotateAround(Vector3.zero, pentEdges[i].axis, bigJump * test / -2f);
                newHex.transform.RotateAround(Vector3.zero, hexAxis, smallJump * test);
            }
            */
        }
            fillerInit = fillerIndex;

        //copy paths for other pentagons
        for (int i = 1; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int k = 1; k < size; k++)
                {
                    int h = i * 5 * (size - 1) + (k - 1);
                    int baseHex = j * (size - 1) + (k - 1);
                    hexagons[h] = Object.Instantiate(hexagon, hexagons[baseHex].transform.position, hexagons[baseHex].transform.rotation);
                    hexagons[h].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, angle);
                    hexagons[h].transform.RotateAround(pentagons[i].transform.position, pentagons[i].transform.position, 108);
                    //back
                    backHex[h] = Object.Instantiate(hexagons[h], hexagons[h].transform.position * -1f, hexagons[h].transform.rotation);
                    backHex[h].transform.Rotate(new Vector3(0, 180, 180));
                    //tileList
                    tileList.Add(hexagons[h]);
                    tileList.Add(backHex[h]);
                }
                //prevent re-filling unnecessarily
                switch(i){
                    case 1:
                    case 2:
                    case 3:
                        if(j - (i - 1) != 2){
                            fillerIndex += fillerInit / 5;
                            continue;
                        }
                        break;
                    case 4:
                        if(j != 0){
                            fillerIndex += fillerInit / 5;
                            continue;
                        }
                        break;
                    case 5:
                            if(j != 1){
                            fillerIndex += fillerInit / 5;
                            continue;
                        }
                        break;
                }
                //copy triangles
                for(int k = 0; k < fillerInit / 5; k++)
                {
                    int fillerCopy = fillerIndex % fillerInit;
                    filler[fillerIndex] = Object.Instantiate(hexagon, filler[fillerCopy].transform.position, filler[fillerCopy].transform.rotation);
                    filler[fillerIndex].transform.RotateAround(Vector3.zero, pentEdges[i - 1].axis, angle);
                    filler[fillerIndex].transform.RotateAround(pentagons[i].transform.position, pentagons[i].transform.position, 108);
                    //back
                    backFill[fillerIndex] = Object.Instantiate(filler[fillerIndex], filler[fillerIndex].transform.position * -1f, filler[fillerIndex].transform.rotation);
                    backFill[fillerIndex].transform.Rotate(new Vector3(0, 180, 180));
                    //tileList
                    tileList.Add(filler[fillerIndex]);
                    tileList.Add(backFill[fillerIndex]);
                    //increment index
                    fillerIndex += 1;
                }
            }
        }
        //Destroy(hexagon);
        //Destroy(pentagon);
        hexagon.transform.position = new Vector3(0, 0, 0);
        firstUpdate = true;
        hexagon.transform.position = new Vector3(0, 0, 0);
    }

    void LateUpdate()
    {
        if(firstUpdate)
        {
            
            for(int i = tileList.Count - 1; i >=0; i--)
            {
                if (tileList[i] == null)
                {
                    tileList.RemoveAt(i);
                }
            }

            tiles = new GameObject[tileList.Count];
            for(int i = tileList.Count - 1; i >=0; i--)
            {
                //BoxCollider box = tileList[i].GetComponent<BoxCollider>();
                Rigidbody body = tileList[i].GetComponent<Rigidbody>();
                HexPrism hexScript = tileList[i].GetComponent<HexPrism>();
                PentaPrism pentaScript = tileList[i].GetComponent<PentaPrism>();
                //Destroy(box);
                Destroy(body);
                Destroy(hexScript);
                Destroy(pentaScript);
                tiles[i] = tileList[i];
            }
            firstUpdate = false;
            gameObject.AddComponent(typeof(World));
        }
    }

    private void setRadius()
    {
        //BS circumference estimate
        float tempAngle = angle * -1f;
        //Debug.Log(tempAngle);
        float partialDist = ((size - 1) * 2 * hexInradius) + (2 * pentInradius);
        float partialDist2 = ((size - 1) * 2 * hexRadius) + (size * sideLength) + (2 * pentRadius);
        //Debug.Log(partialDist);
        float circ = ((360 / tempAngle) * partialDist);
        radius = circ / (2 * Mathf.PI);

        smallJump = ((pentInradius + hexInradius) / partialDist) * angle;
        bigJump = ((2 * hexInradius) / partialDist) * angle;
        smallJump2 = ((pentRadius + sideLength + hexRadius) / partialDist2) * angle2;
        bigJump2 = (((2 * hexRadius) + sideLength) / partialDist2) * angle2;
    }
    
}
