using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private int _index;

    [SerializeField] private GameObject _bulletSpawner;
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private float _shootRate = 0.1f;
    [SerializeField] private float _shootCapacityMax = 5;
    [SerializeField] private float _shootRecoveryRate = 0.5f;
    [SerializeField] private float _shootReloadRate = 0.75f;
    [SerializeField] private float _capacityLooseRate = 1f;
    [SerializeField] private float _amountStopReload = 2.5f;
    private float _currentShootCapacity;

    private bool _canShoot = true;
    private bool _isShooting = false;
    private bool _inReloadMode = false;

    // Start is called before the first frame update
    void Start()
    {
        _index = GetComponent<TopDownEntity>().GetIndex();
        _currentShootCapacity = _shootCapacityMax;
    }

    private void Update()
    {
        if (_currentShootCapacity < _shootCapacityMax && !_isShooting)
        {
            _currentShootCapacity += _shootRecoveryRate * Time.deltaTime;
            if (_currentShootCapacity > _shootCapacityMax)
            {
                _currentShootCapacity = _shootCapacityMax;
            }
        }
        if (_inReloadMode)
        {
            _currentShootCapacity += _shootReloadRate * Time.deltaTime;
            if(_currentShootCapacity >= _amountStopReload)
            {
                _inReloadMode = false;
            }
        }
        InterfaceManager.instance.AdjustShootBar(_index, (_currentShootCapacity * 1 / _shootCapacityMax));
    }

    public void ShootBullet()
    {
        if (!_inReloadMode && _canShoot && _currentShootCapacity >= 0)
        {
            _canShoot = false;
            _currentShootCapacity -= Time.deltaTime * _capacityLooseRate;
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawner.transform.position, _bulletSpawner.transform.rotation);
            bullet.GetComponent<BulletBehaviour>().SetPlayerIndex(_index);
            StartCoroutine(WaitBeforeShootingAgain());
            if(_currentShootCapacity <= 0)
            {
                _inReloadMode = true;
            }
        }
    }

    private IEnumerator WaitBeforeShootingAgain()
    {
        yield return new WaitForSeconds(_shootRate);
        _canShoot = true;
    }

    public void SetIsShooting(bool value)
    {
        _isShooting = value;
    }

}
