using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private int _currentPlayerIndex = 0;
    private GameObject _respawnPoint;
    [SerializeField] private Transform _children;

    private void Start()
    {
        _currentPlayerIndex = GetComponent<TopDownEntity>().GetIndex();
        _respawnPoint = LevelManager.instance.GetCurrentLevelDesign().GetRespawnPoints(_currentPlayerIndex);
    }


    
    public IEnumerator PlayerDeathAndRespawn(float deathTimer)
    {
        //animation or thing to do
        yield return new WaitForSeconds(deathTimer); 
        transform.position = new Vector3( _respawnPoint.transform.position.x,  _respawnPoint.transform.position.y,  _respawnPoint.transform.position.z);
        GetComponent<TopDownEntity>().SetIsDead(false);
        if (_currentPlayerIndex == 0)
        {
            AudioManager.instance.Play("PlayerSpawn");
        }
    }
}
