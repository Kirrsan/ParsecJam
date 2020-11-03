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
    private bool _mineSetUp = false;
    private Mine _currentMine;

    [Header("Shield Settings")]
    [SerializeField] private GameObject _shieldPrefab;
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
            _currentMine = Instantiate(_minePrefab, _mineSpawner.position, Quaternion.Euler(0, 0, 90)).GetComponent<Mine>();
        }
        else
        {
            _currentMine.TriggerPower();
            Destroy(_currentMine.gameObject);
            _currentMine = null;
            _mineSetUp = false;
            _currentPower = Power.None;
        }
    }

    private void UseShield()
    {
        Shield _currentShield = Instantiate(_shieldPrefab, _shieldSpawner.position, Quaternion.identity).GetComponentInChildren<Shield>();
        _currentShield.transform.rotation = _shieldSpawner.rotation;
        _currentShield.SetPlayerProtected(_playerEntity.GetIndex());
        _currentPower = Power.None;
    }

}
