using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private int _index;

    [SerializeField] private GameObject _bulletSpawner;
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private float _shootRate = 1.5f;
    [SerializeField] private float _shootCapacityMax = 10;
    [SerializeField] private float _shootRecoveryRate = 0.2f;
    [SerializeField] private bool _debugFillEachCase;
    private float _currentShootCapacity;

    private bool _canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        _index = GetComponent<TopDownEntity>().GetIndex();
        _currentShootCapacity = _shootCapacityMax;
    }

    private void Update()
    {
        if (_currentShootCapacity < _shootCapacityMax)
        {
            _currentShootCapacity += _shootRecoveryRate * Time.deltaTime;
            if(_currentShootCapacity > _shootCapacityMax)
            {
                _currentShootCapacity = _shootCapacityMax;
            }
        }
        if (_debugFillEachCase)
        {
            InterfaceManager.instance.AdjustShootBar(_index, ((int)_currentShootCapacity * 1 / _shootCapacityMax));
        }
        else
        {
            InterfaceManager.instance.AdjustShootBar(_index, (_currentShootCapacity * 1 / _shootCapacityMax));
        }
    }

    public void ShootBullet()
    {
        if (_canShoot && _currentShootCapacity >= 1)
        {
            _canShoot = false;
            _currentShootCapacity -= 1;
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawner.transform.position, _bulletSpawner.transform.rotation);
            bullet.GetComponent<BulletBehaviour>().SetPlayerIndex(_index);
            StartCoroutine(WaitBeforeShootingAgain());
        }
    }

    private IEnumerator WaitBeforeShootingAgain()
    {
        yield return new WaitForSeconds(_shootRate);
        _canShoot = true;
    }

}
