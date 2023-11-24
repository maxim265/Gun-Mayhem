using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeGeneratorScript : MonoBehaviour
{
    public int N;
    private int[,] _mazeMatrix;
    [SerializeField]
    public static RoomGeneratorScript[,] RoomMatrix;
    public MazeBuilderScript MazeBuilder;



    private void Start()
    {
        _mazeMatrix = new int[N, N];
        RoomMatrix = new RoomGeneratorScript[N, N];

        for (var i = 0; i < N; i++)
        for (var j = 0; j < N; j++)
        {
            _mazeMatrix[i, j] = 0;
            RoomMatrix[i, j] = gameObject.AddComponent<RoomGeneratorScript>();
            //RoomMatrix[i, j] = new RoomGeneratorScript();
        }

        Debug.Log("here"+ this.GetType().Name + ",,m");
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        for (var i = 0; i < N; i++)
        for (var j = 0; j < N; j++)
            CheckEdgeRoom(i, j);

        for (var i = 0; i < N; i++)
        for (var j = 0; j < N; j++)
            RoomMatrix[i, j].CheckRoom(i, j);

        MazeBuilder.GenerateVisualMaze();
    }
    

    private void CheckEdgeRoom(int i, int j)
    {
        var stateX = -1;
        var stateY = -1;

        if (i == 0) stateX = 0;
        else if (i == N - 1) stateX = 1;

        if (j == 0) stateY = 0;
        else if (j == N - 1) stateY = 1;

        RoomMatrix[i, j].IsEdgeRoom(stateX, stateY);
    }
}
