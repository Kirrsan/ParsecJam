using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EyesFollow : MonoBehaviour
{

    [SerializeField] private float _timerBetweenChecksMax = 3;
    [SerializeField] private Transform _orbite;
    [SerializeField] private float _rotationSpeed = 3;
    [SerializeField] private float _timeBetweenFollowPlayerChecks = 0.3f;
    private float _timerBetweenChecks = 3;

    private bool _isDoneChecking = false;
    private bool _isDoneTurning = true;
    private bool _isDoneTurningWhileFollowingPlayer = false;
    private Vector3 _lastRotation;
    private Quaternion _lastRotationWhileFollowingPlayer;
    private Vector3 _initialRotation;
    private Vector3 _lastPlayerPosition;
    private Vector3 _eyeAngle;
    private bool _hasToCheckPlayerPos = false;
    private float _lerpTimeWhileFollowingPlayer = 0;

    [Header("Player")]
    [SerializeField] private float _distanceMinToFollowPlayer = 3;
    [SerializeField] private float _angleToFollowPlayer = 90;
    [SerializeField] private bool _isClamped = true;
    [SerializeField] private float _positiveAngleMin = 150;
    [SerializeField] private float _positiveAngleMax = 175;
    [SerializeField] private float _negativeAngleMin = -175;
    [SerializeField] private float _negativeAngleMax = -150;
    private Transform _currentPlayerTarget;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _timerBetweenChecks = _timerBetweenChecksMax;
        //square it for distance check
        _distanceMinToFollowPlayer *= _distanceMinToFollowPlayer;
        _initialRotation = _orbite.transform.rotation.eulerAngles;
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

            float distance = Mathf.Infinity;
            float distance2 = Mathf.Infinity;

            float player1Angle = Vector3.Angle(transform.position - players[0].transform.position , Vector3.forward);
            float player2Angle = Vector3.Angle(transform.position - players[1].transform.position, Vector3.forward);

            if (player1Angle <= _angleToFollowPlayer)
            {
                distance = CalculateDistance(players[0].transform);
            }
            if (player2Angle <= _angleToFollowPlayer)
            {
                distance2 = CalculateDistance(players[1].transform);
            }

            if (distance < _distanceMinToFollowPlayer && distance2 < _distanceMinToFollowPlayer)
            {
                if (distance < distance2)
                {
                    SetNewPlayer(players[0].transform);
                }
                else
                {
                    SetNewPlayer(players[1].transform);
                }
            }
            else if (distance < _distanceMinToFollowPlayer)
            {
                SetNewPlayer(players[0].transform);
            }
            else if (distance2 < _distanceMinToFollowPlayer)
            {
                SetNewPlayer(players[1].transform);
            }
            else
            {
                _currentPlayerTarget = null;
                _lastRotation = _orbite.transform.rotation.eulerAngles;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentPlayerTarget == null && _isDoneTurning) return;

        if (_currentPlayerTarget == null && !_isDoneTurning)
        {
            Vector3 newRotation = Vector3.Lerp(_lastRotation, _initialRotation, Time.deltaTime * _rotationSpeed);
            _orbite.transform.eulerAngles = newRotation;
            if (Mathf.Abs(((Vector3)_orbite.transform.eulerAngles - _initialRotation).sqrMagnitude) <= 0.2f)
            {
                _isDoneTurning = true;
            }
        }
        else
        {
            if (_hasToCheckPlayerPos)
            {
                _hasToCheckPlayerPos = false;
                _isDoneTurningWhileFollowingPlayer = false;
                StartCoroutine(WaitAndCheckNewPlayerPosition());
                _lastPlayerPosition = _currentPlayerTarget.transform.position;

                float angle = Vector2.SignedAngle(Vector2.up, _lastPlayerPosition - _orbite.transform.position);
                if (_isClamped)
                {
                    if (angle > 0)
                    {
                        angle = Mathf.Clamp(angle, _positiveAngleMin, _positiveAngleMax);
                    }
                    else
                    {
                        angle = Mathf.Clamp(angle, _negativeAngleMin, _negativeAngleMax);
                    }
                }
                _eyeAngle = _orbite.transform.eulerAngles;
                _eyeAngle.y = -angle;
            }

            if (!_isDoneTurningWhileFollowingPlayer)
            {
                _lerpTimeWhileFollowingPlayer += Time.deltaTime * _rotationSpeed;

                Quaternion newRotation = Quaternion.Lerp(_lastRotationWhileFollowingPlayer, Quaternion.Euler(_eyeAngle), _lerpTimeWhileFollowingPlayer);
                _orbite.transform.rotation = newRotation;

                if (Mathf.Abs(((Vector3)_orbite.transform.eulerAngles - _eyeAngle).sqrMagnitude) <= 0.2f)
                {
                    _isDoneTurningWhileFollowingPlayer = true;
                    ResetRotationLerp();
                }
            }
        }
    }

    private void SetNewPlayer(Transform newPlayer)
    {
        if (_currentPlayerTarget != newPlayer.transform)
        {
            ResetRotationLerp();
        }
        _currentPlayerTarget = newPlayer.transform;
        _isDoneTurning = false;
    }

    private void ResetRotationLerp()
    {
        _hasToCheckPlayerPos = true;
        _lastRotationWhileFollowingPlayer = _orbite.transform.rotation;
        _lerpTimeWhileFollowingPlayer = 0;
        _isDoneTurningWhileFollowingPlayer = false;
    }

    private IEnumerator WaitAndCheckNewPlayerPosition()
    {
        yield return new WaitForSeconds(_timeBetweenFollowPlayerChecks);
        ResetRotationLerp();
    }

    private float CalculateDistance(Transform targetTransform)
    {
        return ((Vector3)targetTransform.position - (Vector3)transform.position).sqrMagnitude;
    }
}
