using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        int winnerIndex = other.transform.parent.GetComponent<TopDownEntity>().GetIndex();
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (winnerIndex == 1)
            winnerIndex = 0;
        else
            winnerIndex = 1;
        
        Respawn respawnP1 = other.transform.parent.GetComponent<Respawn>();
        respawnP1.StartCoroutine(respawnP1.PlayerDeathAndRespawn(Respawn.respawnTimeFallDeath));
        Respawn respawnP2 = LevelManager.instance.players[winnerIndex].GetComponent<Respawn>();
        respawnP2.StartCoroutine(respawnP2.PlayerDeathAndRespawn(Respawn.respawnTimeFallDeath));
        ScoreManager.instance.AddToScore(winnerIndex);
    }
}
