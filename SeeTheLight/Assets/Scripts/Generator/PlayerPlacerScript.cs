using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacerScript : MonoBehaviour
{
    public GameObject Player;
    public int RoomNumX;
    public int RoomNumY;
    
    // Update is called once per frame
    public void PlacePlayer()
    {
        Debug.Log($"BAD");
        var position = new Vector3((float)(RoomNumX * 2 + 0.2), (float) (RoomNumY * 2 + 0.2), 2.35F);
        Player.transform.position = position;
    }
}
