using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesign : MonoBehaviour
{
    
    public GameObject respawnPlayer1;
    public GameObject respawnPlayer2;

    public GameObject level;

    public void Activate()
    {
        level.SetActive(true);
    }

    public GameObject GetRespawnPoints(int playerIndex)
    {
        GameObject[] respawns = new GameObject[2];
        respawns[0] = respawnPlayer1;
        respawns[1] = respawnPlayer2;
        return respawns[playerIndex];
    }

}
