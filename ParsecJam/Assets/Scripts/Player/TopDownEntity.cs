using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEntity : MonoBehaviour
{

    private int _index = 0;

    [Header("Movement Settings")]
    [SerializeField] private float _acceleration = 20f;
    [SerializeField] private float _moveSpeedMax = 10f;
    [SerializeField] private float _friction = 30f;
    [SerializeField] private float _turnFriction = 30f;

    private Vector2 _moveDir;
    private Vector2 _orientDir = Vector2.up;
    private Vector2 _velocity = Vector2.zero;

    [Header("Dash Settings")]
    private Vector2 _dashDir;
    [SerializeField] private float _dashDuration = 0.3f;
    [SerializeField] private float _dashCooldown = 3f;
    [SerializeField] private float _dashspeed = 20f;
    [SerializeField] private float _dashCountdown = -1f;
    private bool _isDashing = false;
    private float _dashTimer;

    [SerializeField] private GameObject _visualObj;
    [HideInInspector] public Shoot shootFunc;

    [Header("Death Settings")]
    [SerializeField] private float _playerDeathTime = 30f;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform mixamoHips;
    [SerializeField] private ParticleSystem deathPoolFX;
    
    [Header("Life Settings")]
    [SerializeField] private float _lifeMax = 15;
    [SerializeField] private GameObject[] _hitFX;
    private float _life;


    [Header("Power Settings")]
    [HideInInspector] public PlayerPowerBehaviour powerBehaviour;
    private bool _canPickUpSomething = false;
    private PickUpPower _pickUp;
    
    [Header("Death Head")]
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _headSpawner;
    [SerializeField] private int _maxHeadsOnScene;
    private int _currentHeadIndex = 0;
    private GameObject[] _headList;
    
    [Header("Animations")]
    public Animator _anim;
    private bool _isFalling = false;
    public float hitOffset;
    [SerializeField] private float strafeOffset;
    [SerializeField] private BoxCollider _gunCollider;
    private float _initialYPosition;
    private bool _yInitialized = false;
    public bool isBeingHit = false;



    
    private void Awake()
    {
        shootFunc = GetComponent<Shoot>();
        powerBehaviour = GetComponent<PlayerPowerBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {

        _dashTimer = _dashCooldown;
        _life = _lifeMax;
        _headList = new GameObject[_maxHeadsOnScene];
        StartCoroutine(WaitAndInitializeY());
    }

    private IEnumerator WaitAndInitializeY()
    {
        yield return new WaitForSeconds(0.2f);
        _initialYPosition = transform.position.y;
        _yInitialized = true;
    }

    public void SetIndex(int newIndex)
    {
        _index = newIndex;
    }
    public int GetIndex()
    {
        return _index;
    }

    private void Update()
    {
        if (GameManager.instance.isPlaying)
        {
            if (_dashTimer < _dashCooldown)
            {
                _dashTimer += Time.deltaTime;
                InterfaceManager.instance.FillCoolDown(_index, _dashTimer * (0.75f/_dashCooldown));
                if(_dashTimer >= _dashCooldown)
                {
                    AudioManager.instance.Play("DashCooldownComplete");
                }
            }
            if(transform.position.y > _initialYPosition && _yInitialized)
            {
                transform.position = new Vector3(transform.position.x, _initialYPosition, transform.position.z);
            }
            if (_isFalling)
            {
                _gunCollider.enabled = false;
            }
            else
            {
                _gunCollider.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.isPlaying)
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

            if(transform.position.y <= _initialYPosition - 0.5f && !_isFalling)
            {
                _isFalling = true;
                int rand = Random.Range(0, 16);
                if (rand == 9)
                {
                    AudioManager.instance.Play("WillScream");
                }
                else
                {
                    AudioManager.instance.Play("PlayerFall");
                }
            }
            if (transform.position.y < -20 || transform.position.x < -125 || transform.position.x > 125 || transform.position.z < -40 || transform.position.z > 40)
            {
                int winnerIndex = LevelManager.instance.GetOtherPlayer(transform.GetComponent<TopDownEntity>().GetIndex());
                GetComponent<Rigidbody>().velocity = Vector3.zero;

                LevelManager.instance.RespawnPlayers(LevelManager.instance.respawnTimeFallDeath);
                ScoreManager.instance.AddToScore(winnerIndex);
            }
            else if(transform.position.y > _initialYPosition - 0.5f)
            {
                _isFalling = false;
            }
        }
    }

    public void Pickup()
    {
        if (GameManager.instance.isPlaying)
        {
            if (_canPickUpSomething)
            {
                if (powerBehaviour.GetPower() != Power.None)
                {
                    if (powerBehaviour.GetMineSetUp())
                    {
                        powerBehaviour.UsePower();
                    }
                }

                AudioManager.instance.Play("PowerPickUp");
                _anim.SetTrigger("PickupItem");
                _canPickUpSomething = false;
                powerBehaviour.SetPower(_pickUp.GivePower());
                BonusManager.instance.RemoveBonus(_pickUp);
                Destroy(_pickUp.gameObject);
                _pickUp = null;

            }
        }
    }

    public void SetPickable(PickUpPower pickable)
    {
        _canPickUpSomething = true;
        _pickUp = pickable;
    }
    public void ResetPickable()
    {
        _canPickUpSomething = false;
        _pickUp = null;
    }

    #region Dash
    public void Dash()
    {
        if (_dashTimer >= _dashCooldown)
        {
            AudioManager.instance.Play("DashWind");
            AudioManager.instance.Play("DashGroan");
            if (_moveDir != Vector2.zero)
            {
                _dashDir = _velocity.normalized;
            }
            else if (_orientDir == Vector2.zero)
            {
                _dashDir = Vector2.up;
            }
            else
            {
                _dashDir = _orientDir;
            }
            _dashCountdown = _dashDuration;
            _isDashing = true;

            _dashTimer = 0;
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

    #endregion
    private void _UpdateVisualOrient()
    {
        float angle = Vector2.SignedAngle(Vector2.up, _orientDir);

        Vector3 eulerAngles = _visualObj.transform.eulerAngles;
        eulerAngles.y = -angle;
        _visualObj.transform.eulerAngles = eulerAngles;
    }

    #region Movement
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
        
        
        Vector2 vision = transform.GetChild(1).forward;
        if (_orientDir != Vector2.zero)
        {
            vision = _orientDir;
        }
        float angle = Vector2.SignedAngle(vision.normalized, _moveDir);

        if (angle > 90 - strafeOffset && angle < 90 + strafeOffset)
        {
            _anim.SetBool("StrafeLeft", true);  
        }
        else
        {
            _anim.SetBool("StrafeLeft", false);
        }
        
        if (angle > -90 - strafeOffset && angle < -90 + strafeOffset)
        {
            _anim.SetBool("StrafeRight", true);
        }
        else
        {
            _anim.SetBool("StrafeRight", false);
        }
        
        if (angle > 180 - strafeOffset && angle < 180 + strafeOffset)
        {
            _anim.SetBool("GoingBack", true);
        }
        else
        {
            _anim.SetBool("GoingBack", false);
        }
        
        _anim.SetFloat("Speed", _velocity.magnitude);
    }
    #endregion

    public void ChangeLife(float lifeToLoose)
    {
        if (!_isDead)
        {
            InterfaceManager.instance.ChangeHealthBarColor(_index);
            _life += lifeToLoose;
            InterfaceManager.instance.AdjustLifeBar(_index, _life * (1 / _lifeMax));
            int rand = Random.Range(0, _hitFX.Length);
            _hitFX[rand].SetActive(false);
            _hitFX[rand].SetActive(true);
        }
        if (_life <= 0 && !_isDead)
        {
            _isDead = true;
            _deathFX.SetActive(false);
            _deathFX.SetActive(true);
            _anim.SetBool("Die", true);
            shootFunc.ResetIsShooting();
            LevelManager levelManager = LevelManager.instance;
            levelManager.players[levelManager.GetOtherPlayer(_index)]._anim.SetTrigger("Dance");
            int rand = Random.Range(0, 16);
            if(rand == 9)
            {
                AudioManager.instance.Play("WillScream");
            }
            else
            {
                AudioManager.instance.Play("DeathSound");
            }

            SpawnHead();
            levelManager.RespawnPlayers(levelManager.respawnTimeBulletKill);
            ScoreManager.instance.AddToScore(levelManager.GetOtherPlayer(_index));
            return;
        }
        AudioManager.instance.Play("PlayerHit");
    }

    private void SpawnHead()
    {
        if (_headList[_maxHeadsOnScene - 1] != null)
        {
            Destroy(_headList[_currentHeadIndex]);
            GameObject head = GameObject.Instantiate(_head, _headSpawner.transform.position , Quaternion.identity);
            _headList[_currentHeadIndex] = head;
            _currentHeadIndex++;
        }
        else
        {
            GameObject head = GameObject.Instantiate(_head, _headSpawner.transform.position , Quaternion.identity);
            _headList[_currentHeadIndex] = head;
            _currentHeadIndex++;
        }

        if (_currentHeadIndex >= _maxHeadsOnScene)
        {
            _currentHeadIndex = 0;
        }
    }

    public void Kill(int ennemyIndex)
    {
        _deathFX.SetActive(false);
        _deathFX.SetActive(true);
        _anim.SetBool("Die", true);
        LevelManager.instance.players[LevelManager.instance.GetOtherPlayer(_index)]._anim.SetTrigger("Dance");
        shootFunc.ResetIsShooting();
        AudioManager.instance.Play("DeathSound");
        _isDead = true;
        SpawnHead();
        LevelManager.instance.RespawnPlayers(LevelManager.instance.respawnTimeBulletKill);
        ScoreManager.instance.AddToScore(LevelManager.instance.GetOtherPlayer(_index));
    }

    public void InstantiateDeathVFX()
    {
        ParticleSystem Fx = Instantiate(deathPoolFX, new Vector3(mixamoHips.transform.position.x, 0.15f, mixamoHips.transform.position.z),deathPoolFX.transform.rotation);
        StartCoroutine(DestroyDeathPoolFX(Fx));
    }

    private IEnumerator DestroyDeathPoolFX(ParticleSystem Fx)
    {
        yield return new WaitForSeconds(deathPoolFX.main.duration);
        Destroy(Fx.gameObject);
    }

    public float GetLife()
    {
        return _life;
    }

    public void SetLife(float newLife)
    {
        _life = newLife;
        InterfaceManager.instance.AdjustLifeBar(_index, _life * (1 / _lifeMax));
    }

    public float GetLifeMax()
    {
        return _lifeMax;
    }

    public void SetIsDead(bool value)
    {
        _isDead = value;
    }

    public bool GetIsDead()
    {
        return _isDead;
    }

    public void SetIsFalling(bool value)
    {
        _isFalling = value;
    }
    


    public Vector2 GetOrientDir()
    {
        return _orientDir;
    }

    public Vector2 GetVelocity()
    {
        return _velocity;
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        _velocity = newVelocity;
    }
}
