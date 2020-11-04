using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EyesFollow : MonoBehaviour
{

    [SerializeField] private float _timerBetweenChecksMax = 3;
    [SerializeField] private Transform _orbite;
    private float _timerBetweenChecks = 3;

    private bool _isDoneChecking = false;

    [Header("Player")]
    [SerializeField] private float _distanceMinToFollowPlayer = 3;
    private Transform _currentPlayerTarget;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _timerBetweenChecks = _timerBetweenChecksMax;
        //square it for distance check
        _distanceMinToFollowPlayer *= _distanceMinToFollowPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isDoneChecking)
            _timerBetweenChecks -= Time.deltaTime;

        if (_timerBetweenChecks < 0)
        {
            _timerBetweenChecks = _timerBetweenChecksMax;
            _isDoneChecking = true;
        }

        if (_isDoneChecking)
        {
            
            TopDownEntity[] players = LevelManager.instance.players;

            float distance = CalculateDistance(players[0].transform);
            float distance2 = CalculateDistance(players[1].transform);

            if (distance < _distanceMinToFollowPlayer && distance2 < _distanceMinToFollowPlayer)
            {
                if (distance < distance2)
                    _currentPlayerTarget = players[0].transform;
                else
                    _currentPlayerTarget = players[1].transform;
            }
            else if (distance < _distanceMinToFollowPlayer)
            {
                _currentPlayerTarget = players[0].transform;
            }
            else if (distance2 < _distanceMinToFollowPlayer)
            {
                _currentPlayerTarget = players[1].transform;
            }
            else
            {
                _currentPlayerTarget = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentPlayerTarget == null) return;
        
        _orbite.LookAt(new Vector3(_currentPlayerTarget.position.x, _currentPlayerTarget.position.y + 1.5f, _currentPlayerTarget.position.z));
    }

    private float CalculateDistance(Transform targetTransform)
    {
        return ((Vector3)targetTransform.position - (Vector3)transform.position).sqrMagnitude;
    }
}
