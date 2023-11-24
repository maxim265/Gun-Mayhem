using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using Random = UnityEngine.Random;




public class RoomGeneratorScript : MazeGeneratorScript
{
    public bool IsDone;
    public bool IsEdge;
    private bool _posRoommateL;
    private bool _posRoommateR;
    private bool _posRoommateT;
    private bool _posRoommateB;
    public bool RoommateL;
    public bool RoommateR;
    public bool RoommateT;
    public bool RoommateB;
    public bool HasRoommateL;
    public bool HasRoommateR;
    public bool HasRoommateT;
    public bool HasRoommateB;
    public int NumRoommates;
    public int NumCurRoommates = 0;
    private int _numPosRoommates = 4;
    private readonly int[] _randArray = { 0, 1, 2, 3 };

    private void Start()
    {
        IsDone = false;
        IsEdge = false;
        _posRoommateL = true;
        _posRoommateR = true;
        _posRoommateT = true;
        _posRoommateB = true;
        RoommateL = false;
        RoommateR = false;
        RoommateT = false;
        RoommateB = false;
        HasRoommateL = false;
        HasRoommateR = false;
        HasRoommateT = false;
        HasRoommateB = false;
        _numPosRoommates = 4;
        NumCurRoommates = 0;
    }

    public void IsEdgeRoom(int stateX, int stateY)
    {
        _posRoommateT = true;
        _posRoommateB = true;
        _posRoommateL = true;
        _posRoommateR = true;
        RoommateL = false;
        RoommateR = false;
        RoommateT = false;
        RoommateB = false;
        _numPosRoommates = 4;
        switch (stateX)
        {
            case 0:
                _posRoommateL = false;
                IsEdge = true;
                _numPosRoommates--;
                break;
            case 1:
                _posRoommateR = false;
                IsEdge = true;
                _numPosRoommates--;
                break;

        }

        switch (stateY)
        {
            case 0:
                _posRoommateT = false;
                IsEdge = true;
                _numPosRoommates--;
                break;
            case 1:
                _posRoommateB = false;
                IsEdge = true;
                _numPosRoommates--;
                break;
        }
        NumRoommates = RandRoommates();
    }

    private int RandRoommates()
    {
        var randomNumber = Random.value * 100;

        var weight1 = 10f; 
        var weight2 = 20f; 
        var weight3 = 30f; 

        if (randomNumber < weight1 || _numPosRoommates == 1)
        {
            return 1;
        }
        else if (randomNumber < weight1 + weight2 || _numPosRoommates == 2)
        {
            return 2;
        }
        else if (randomNumber < weight1 + weight2 + weight3 || _numPosRoommates == 3)
        {
            return 3; 
        }
        else
        {
            return 4; 
        }
    }

    private void RandomizeArray()
    {
        var n = _randArray.Length;
        for (var s = n - 1; s > 0; s--)
        {
            var randomIndex = Random.Range(0, s + 1);
            (_randArray[s], _randArray[randomIndex]) = (_randArray[randomIndex], _randArray[s]);
        }
    }

    public void CheckRoom(int i, int j)
    {
        RandomizeArray();

        AssignRooms(i, j);

        IsDone = true;
    }

    private void AssignRooms(int i, int j)
    {

        NumRoommates = NumRoommates - NumCurRoommates;
        for (var s = 0; s < NumRoommates; s++)
        {
            var side = _randArray[s];

            if (CheckPosRoommate(side))
            {
                switch (side)
                {
                    case 0:
                        if (MazeGeneratorScript.RoomMatrix[i - 1, j].NumCurRoommates != MazeGeneratorScript.RoomMatrix[i - 1, j].NumRoommates)
                        {
                            RoommateL = true;
                            MazeGeneratorScript.RoomMatrix[i - 1, j].RoommateR = true;
                            MazeGeneratorScript.RoomMatrix[i - 1, j]._posRoommateR = false;
                            MazeGeneratorScript.RoomMatrix[i - 1, j].NumCurRoommates++;
                            NumCurRoommates++;
                        }
                        break;
                    case 1:
                        if (MazeGeneratorScript.RoomMatrix[i + 1, j].NumCurRoommates != MazeGeneratorScript.RoomMatrix[i + 1, j].NumRoommates)
                        {
                            RoommateR = true;
                            MazeGeneratorScript.RoomMatrix[i + 1, j].RoommateL = true;
                            MazeGeneratorScript.RoomMatrix[i + 1, j]._posRoommateL = false;
                            MazeGeneratorScript.RoomMatrix[i + 1, j].NumCurRoommates++;
                            NumCurRoommates++;
                        }

                        break;
                    case 2:
                        if (MazeGeneratorScript.RoomMatrix[i, j - 1].NumCurRoommates != MazeGeneratorScript.RoomMatrix[i, j - 1].NumRoommates)
                        {
                            RoommateT = true;
                            MazeGeneratorScript.RoomMatrix[i, j - 1].RoommateB = true;
                            MazeGeneratorScript.RoomMatrix[i, j - 1]._posRoommateB = false;
                            MazeGeneratorScript.RoomMatrix[i, j - 1].NumCurRoommates++;
                            NumCurRoommates++;
                        }

                        break;
                    case 3:
                        if (MazeGeneratorScript.RoomMatrix[i, j + 1].NumCurRoommates != MazeGeneratorScript.RoomMatrix[i, j + 1].NumRoommates)
                        {
                            RoommateB = true;
                            MazeGeneratorScript.RoomMatrix[i, j + 1].RoommateT = true;
                            MazeGeneratorScript.RoomMatrix[i, j + 1]._posRoommateT = false;
                            MazeGeneratorScript.RoomMatrix[i, j + 1].NumCurRoommates++;
                            NumCurRoommates++;
                        }
                        break;
                    default:
                        Debug.Log($"BAD");
                        break;
                }
            }
        }
    }

    private bool CheckPosRoommate(int side)
    {
        return side switch
        {
            0 => _posRoommateL,
            1 => _posRoommateR,
            2 => _posRoommateT,
            3 => _posRoommateB,
            _ => false,
        };
    }

    public int GetRoommatesPositions()
    {
        if (RoommateT && RoommateB && RoommateL && RoommateR && NumCurRoommates == 4) return 1; //TBLR

        // 3
        if (RoommateT && RoommateB && RoommateL && !RoommateR && NumCurRoommates == 3) return 2; //TBL
        if (RoommateT && RoommateB && !RoommateL && RoommateR && NumCurRoommates == 3) return 3; //TBR
        if (RoommateT && !RoommateB && RoommateL && RoommateR && NumCurRoommates == 3) return 4; //TLR
        if (!RoommateT && RoommateB && RoommateL && RoommateR && NumCurRoommates == 3) return 5; //BLR

        // 2
        if (RoommateT && RoommateB && !RoommateL && !RoommateR && NumCurRoommates == 2) return 6; //TB
        if (RoommateT && !RoommateB && RoommateL && !RoommateR && NumCurRoommates == 2) return 7; //TL
        if (!RoommateT && RoommateB && RoommateL && !RoommateR && NumCurRoommates == 2) return 8; //BL
        if (!RoommateT && RoommateB && !RoommateL && RoommateR && NumCurRoommates == 2) return 9; //BR
        if (RoommateT && !RoommateB && !RoommateL && RoommateR && NumCurRoommates == 2) return 10;//TR
        if (!RoommateT && !RoommateB && RoommateL && RoommateR && NumCurRoommates == 2) return 11;//LR

        // 1
        if (RoommateT && !RoommateB && !RoommateL && !RoommateR && NumCurRoommates == 1) return 12;//T
        if (!RoommateT && RoommateB && !RoommateL && !RoommateR && NumCurRoommates == 1) return 13;//B
        if (!RoommateT && !RoommateB && RoommateL && !RoommateR && NumCurRoommates == 1) return 14;//L
        if (!RoommateT && !RoommateB && !RoommateL && RoommateR && NumCurRoommates == 1) return 15;//R
        return 0;
    }
}