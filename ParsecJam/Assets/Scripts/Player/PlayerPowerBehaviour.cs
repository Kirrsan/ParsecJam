using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerBehaviour : MonoBehaviour
{

    private TopDownEntity _playerEntity;

    private Power _currentPower;
    [Header("Mine Settings")]
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private Transform _mineSpawner;
    [SerializeField] private GameObject _mineFXprefab;
    private bool _mineSetUp = false;
    private Mine _currentMine;

    [Header("Shield Settings")]
    [SerializeField] private GameObject[] _shieldPrefab;
    [SerializeField] private Transform _shieldSpawner;

    // Start is called before the first frame update
    void Start()
    {
        _playerEntity = GetComponent<TopDownEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPower(Power newPower)
    {
        _currentPower = newPower;
        InterfaceManager.instance.ChangePowerIcon(_playerEntity.GetIndex(), _currentPower);
    }
    public Power GetPower()
    {
        return _currentPower;
    }

    public void UsePower()
    {
        switch (_currentPower)
        {
            case Power.None:
                print("you don't have any power looser");
                break;
            case Power.Mine:
                UseMine();
                break;
            case Power.Shield:
                UseShield();
                break;
            case Power.Rocket:
                break;
            case Power.Cancel:
                break;
            default:
                print("can't find power");
                break;
        }
    }

    private void UseMine()
    {
        if (!_mineSetUp)
        {
            _mineSetUp = true;
            _currentMine = Instantiate(_minePrefab, _mineSpawner.position, Quaternion.identity).GetComponent<Mine>();
            _currentMine.SetPlayerText(_playerEntity.GetIndex() + 1);
        }
        else
        {
            AudioManager.instance.Play("Mine");
;            _currentMine.TriggerPower(true);
            _currentMine = null;
            _mineSetUp = false;
            _currentPower = Power.None;
            InterfaceManager.instance.ChangePowerIcon(_playerEntity.GetIndex(), _currentPower);
        }
    }

    private void UseShield()
    {
        if (_shieldSpawner.GetComponent<SpawnCollision>().GetIsInCollision())
        {
            Debug.Log("You shall not place that here ! (shield)");
            return;
        }
    
        AudioManager.instance.Play("ShieldDrop");
        int rand = Random.Range(0, 11);
        Shield _currentShield;
        if (rand == 6)
        {
            _currentShield = Instantiate(_shieldPrefab[1], _shieldSpawner.position, Quaternion.identity).GetComponentInChildren<Shield>();
        }
        else
        {
            _currentShield = Instantiate(_shieldPrefab[0], _shieldSpawner.position, Quaternion.identity).GetComponentInChildren<Shield>();
        }
        _currentShield.transform.rotation = _shieldSpawner.rotation;
        _currentShield.SetPlayerProtected(_playerEntity.GetIndex());
        _currentPower = Power.None;
        InterfaceManager.instance.ChangePowerIcon(_playerEntity.GetIndex(), _currentPower);
    }

    public bool GetMineSetUp()
    {
        return _mineSetUp;
    }

    public void ResetPower()
    {
        _mineSetUp = false;
        _currentPower = Power.None;
        InterfaceManager.instance.ChangePowerIcon(_playerEntity.GetIndex(), _currentPower);
    }
}
