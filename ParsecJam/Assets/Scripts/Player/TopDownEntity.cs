using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEntity : MonoBehaviour
{

    private int _index = 0;

    [SerializeField] private float _acceleration = 20f;
    [SerializeField] private float _moveSpeedMax = 10f;
    [SerializeField] private float _friction = 30f;
    [SerializeField] private float _turnFriction = 30f;

    private Vector2 _moveDir;
    private Vector2 _orientDir = Vector2.right;
    private Vector2 _velocity = Vector2.zero;

    private Vector2 _dashDir;
    [SerializeField] private float _dashDuration = 0.3f;
    [SerializeField] private float _dashCooldown = 3f;
    [SerializeField] private float _dashspeed = 20f;
    [SerializeField] private float _dashCountdown = -1f;
    private bool _isDashing = false;
    private float _dashTimer;

    private bool _isPlaying = false;

    [SerializeField] private GameObject _visualObj;
    [HideInInspector] public Shoot shootFunc;
    
    [SerializeField] private float _playerDeathTime = 30f;
    [SerializeField] private bool _isDead = false;

    private void Awake()
    {
        shootFunc = GetComponent<Shoot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onStateChange += () =>
        {
            if(GameManager.instance.state != State.INGAME)
            {
                _isPlaying = false;
            }
            else
            {
                _isPlaying = true;
            }
        };

        _dashTimer = _dashCooldown;
        _isPlaying = true;
    }

    public void SetIndex(int newIndex)
    {
        _index = newIndex;
    }
    public int GetIndex()
    {
        return _index;
    }


    public void Dash()
    {
        if (_dashTimer >= _dashCooldown)
        {
            _dashDir = _velocity.normalized;
            _dashCountdown = _dashDuration;
            _isDashing = true;

            _dashTimer = 0;
        }
    }

    private void Update()
    {
        if (_isPlaying)
        {
            if (_dashTimer < _dashCooldown)
            {
                _dashTimer += Time.deltaTime;
                InterfaceManager.instance.FillCoolDown(_index, _dashTimer * (0.75f/_dashCooldown));
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPlaying)
        {
            if (_isDashing)
            {
                _UpdateDash();
            }
            else
            {
                _UpdateMove();
                _UpdateVisualOrient();
            }

            Vector3 newPosition = transform.position;
            newPosition.x += _velocity.x * Time.fixedDeltaTime;
            newPosition.z += _velocity.y * Time.fixedDeltaTime;
            transform.position = newPosition;
        }
    }

    private void _UpdateDash()
    {
        if (!_isDashing) return;

        _dashCountdown -= Time.fixedDeltaTime;

        if (_dashCountdown <= 0f){
            _isDashing = false;
            if (_moveDir != Vector2.zero)
            {

                _velocity = _velocity.normalized * _moveSpeedMax;
            }
            else
            {
                _velocity = Vector2.zero;
            }
        }
        else
        {
            _velocity = _dashDir * _dashspeed;
        }
    }

    private void _UpdateVisualOrient()
    {
        float angle = Vector2.SignedAngle(Vector2.right, _orientDir);

        Vector3 eulerAngles = _visualObj.transform.eulerAngles;
        eulerAngles.y = -angle;
        _visualObj.transform.eulerAngles = eulerAngles;
    }

    public void Move(Vector2 dir, Vector2 aimDir)
    {
        _moveDir = dir;
        _orientDir = aimDir;
    }

    private void _UpdateMove()
    {
        if(_moveDir != Vector2.zero)
        {
            float turnAngle = Vector2.SignedAngle(_velocity, _moveDir);
            turnAngle = Mathf.Abs(turnAngle);
            float frictionRatio = turnAngle / 360f;
            float turnfrictionWithRatio = _turnFriction * frictionRatio;

            _velocity += _moveDir * _acceleration * Time.fixedDeltaTime;
            if(_velocity.sqrMagnitude > _moveSpeedMax * _moveSpeedMax)
            {
                _velocity = _velocity.normalized * _moveSpeedMax;
            }

            Vector2 frictionDir = _velocity.normalized;
            _velocity -= frictionDir * turnfrictionWithRatio * Time.fixedDeltaTime;

            //_orientDir = _velocity.normalized;
            
        }
        else if (_velocity!= Vector2.zero)
        {
            Vector2 frictionDir = _velocity.normalized;
            float frictionToApply = _friction * Time.fixedDeltaTime;
            if(_velocity.sqrMagnitude <= frictionToApply * frictionToApply)
            {
                _velocity = Vector2.zero;
            }
            else
            {
                _velocity -= frictionToApply * frictionDir;
            }
        }
    }


    public void SetIsDead(bool value)
    {
        _isDead = value;
    }
}
