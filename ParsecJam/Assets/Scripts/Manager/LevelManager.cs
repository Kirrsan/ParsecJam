using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private float _roundDuration = 60;
    [SerializeField] private float _roundTimer = 0;
    [SerializeField] private bool _canDecreaseTime = false;

    // Start is called before the first frame update
    void Start()
    {
        _roundTimer = _roundDuration;
        GameManager.instance.onStateChange += () =>
        {
            if(GameManager.instance.state != State.INGAME)
            {
                _canDecreaseTime = false;
            }
            else
            {
                _canDecreaseTime = true;
            }
        };
        _canDecreaseTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canDecreaseTime)
        {
            _roundTimer -= Time.deltaTime;
            if(_roundTimer <= 0)
            {
                GameManager.instance.ChangeState(State.WIN);
            }
        }
    }
}
