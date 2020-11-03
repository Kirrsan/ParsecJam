﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [SerializeField] private float _roundDuration = 60;
    private float _roundTimer = 0;
    public int numberOfPlayers = 0;

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
        _roundTimer = _roundDuration;
    }

    // Update is called once per frame
    void Update()
    {
        _roundTimer -= Time.deltaTime;
        if (_roundTimer <= 0)
        {
            GameManager.instance.ChangeState(State.WIN);
        }
    }
}
