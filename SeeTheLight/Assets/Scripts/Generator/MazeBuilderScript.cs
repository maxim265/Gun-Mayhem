using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilderScript : MazeGeneratorScript
{
    public GameObject RoomPrefab;
    public GameObject T;
    public GameObject B;
    public GameObject L;
    public GameObject R;
    public GameObject TB;
    public GameObject TL;
    public GameObject TR;
    public GameObject LR;
    public GameObject BR;
    public GameObject BL;

    public GameObject BLR;
    public GameObject TBL;
    public GameObject TBR;
    public GameObject TLR;

    public GameObject TBLR;

    public GameObject CorridorVerticalPrefab;
    public GameObject CorridorHorizontalPrefab;

    public GameObject PlayerPlacer;

    public void Start()
    {

    }

    public void GenerateVisualMaze()
    {
        Debug.Log("GENERATE.");
        for (var i = 0; i < N; i++)
        {
            for (var j = 0; j < N; j++)
            {
                var position = new Vector3(i * 40, j * 40, 0);
                Instantiate(RoomPrefab, position, Quaternion.identity);

                if (RoomMatrix[i, j].RoommateB && RoomMatrix[i, j + 1].RoommateT)
                {
                    //Debug.Log("corridor.");
                    position = new Vector3((float)(i * 40 + 1 * 20 - 0.5 * 20), (float)(j * 40 - 0.05 * 20), 0);
                    Instantiate(CorridorVerticalPrefab, position, Quaternion.identity);
                    RoomMatrix[i, j].HasRoommateB = true;
                }

                if (RoomMatrix[i, j].RoommateL && RoomMatrix[i - 1, j].RoommateR)
                {
                    //Debug.Log("corridor.");
                    position = new Vector3((float)(i * 40 + 0.05 * 20), j * 40 + 1 * 20, 0);
                    Instantiate(CorridorHorizontalPrefab, position, Quaternion.identity);
                    RoomMatrix[i, j].HasRoommateL = true;
                }

            }
        }
        GenerateWalls();

        PlayerPlacer.GetComponent<PlayerPlacerScript>().PlacePlayer();

    }

    public void GenerateWalls()
    {
        var sides = 0;
        for (var i = 0; i < N; i++)
        {
            for (var j = 0; j < N; j++)
            {
                sides = MazeGeneratorScript.RoomMatrix[i, j].GetRoommatesPositions();

                //Debug.Log($"[{i}][{j}] side: {sides}");

                var position = new Vector3(i * 40, j * 40, 0);

                switch (sides)
                {
                    case 1:
                        Instantiate(TBLR, position, Quaternion.identity);
                        break;
/////////////////////////////////////////////////////////////////////////
                    case 2:
                        Instantiate(TBL, position, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(TBR, position, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(TLR, position, Quaternion.identity);
                        break;
                    case 5:
                        Instantiate(BLR, position, Quaternion.identity);
                        break;
/////////////////////////////////////////////////////////////////////////
                    case 6:
                        Instantiate(TB, position, Quaternion.identity);
                        break;
                    case 7:
                        Instantiate(TL, position, Quaternion.identity);
                        break;
                    case 8:
                        Instantiate(BL, position, Quaternion.identity);
                        break;
                    case 9:
                        Instantiate(BR, position, Quaternion.identity);
                        break;
                    case 10:
                        Instantiate(TR, position, Quaternion.identity);
                        break;
                    case 11:
                        Instantiate(LR, position, Quaternion.identity);
                        break;
///////////////////////////////////////////////////////////////////////
                    case 12:
                        Instantiate(T, position, Quaternion.identity);
                        break;
                    case 13:
                        Instantiate(B, position, Quaternion.identity);
                        break;
                    case 14:
                        Instantiate(L, position, Quaternion.identity);
                        break;
                    case 15:
                        Instantiate(R, position, Quaternion.identity);
                        break;
                    default:
                        break;

                }
            }
        }
    }
}
