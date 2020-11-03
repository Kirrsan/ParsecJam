using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        int winnerIndex = other.transform.GetComponent<TopDownEntity>().GetIndex();
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (winnerIndex == 1)
            winnerIndex = 0;
        else
            winnerIndex = 1;

        LevelManager.instance.RespawnPlayers();
        ScoreManager.instance.AddToScore(winnerIndex);
    }
}
