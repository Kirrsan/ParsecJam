using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int winnerIndex = LevelManager.instance.GetOtherPlayer(other.transform.GetComponent<TopDownEntity>().GetIndex());
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;

        LevelManager.instance.RespawnPlayers(LevelManager.instance.respawnTimeFallDeath);
        ScoreManager.instance.AddToScore(winnerIndex);
    }
}
