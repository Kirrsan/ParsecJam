﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [SerializeField] private float _roundDuration = 60;
    public float roundTimer = 0;
    public TopDownEntity[] players = new TopDownEntity[2];
    
    [SerializeField] private LevelDesign[] _levelDesigns;
    [SerializeField] private int currentLevelDesign = 0;

    public System.Action onInstanceCreated;
    public System.Action onRespawnPlayers;

    public List<Shield> shieldList = new List<Shield>();

    public float respawnTimeBulletKill = 1;
    public float respawnTimeFallDeath = 1;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (onInstanceCreated != null) onInstanceCreated.Invoke();

        _levelDesigns[currentLevelDesign].Activate();

        roundTimer = _roundDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlaying)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0)
            {
                GameManager.instance.ChangeState(State.WIN);
            }
        }
    }

    public LevelDesign GetCurrentLevelDesign()
    {
        return _levelDesigns[currentLevelDesign];
    }

    public void RespawnPlayers(float respawnTime)
    {
        if (onRespawnPlayers != null) onRespawnPlayers.Invoke();
        GameManager.instance.isPlaying = false;
        Respawn respawn;

        for (int i = 0; i < players.Length; i++)
        {
            respawn = players[i].GetComponent<Respawn>();
            respawn.StartCoroutine(respawn.PlayerDeathAndRespawn(respawnTime));
        }
    }

    public int GetOtherPlayer(int playerIndex)
    {
        int other = -1;
        for (int i = 0; i < players.Length; i++)
        {
            if (i != playerIndex)
            {
                other = players[i].GetIndex();
            }
        }
        return other;
    }
}
