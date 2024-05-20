using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public string recentLocationName;
    public float[] position;

    public PlayerData(Player player)
    {
        health = player.health;
        recentLocationName = player.recentLocationName;
        position = new float[3];

        position[0] = player.playerTransform.position.x;
        position[1] = player.playerTransform.position.y;
        position[2] = player.playerTransform.position.z;
    }
}
