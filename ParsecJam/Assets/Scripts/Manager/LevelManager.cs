using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [SerializeField] private float _roundDuration = 60;
    public float roundTimer = 0;
    private bool _isPlaying;

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
        GameManager.instance.onStateChange += () =>
        {
            if (GameManager.instance.state != State.INGAME)
            {
                _isPlaying = false;
            }
            else
            {
                _isPlaying = true;
            }
        };

        roundTimer = _roundDuration;


        _isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlaying)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0)
            {
                GameManager.instance.ChangeState(State.WIN);
            }
        }
    }
}
