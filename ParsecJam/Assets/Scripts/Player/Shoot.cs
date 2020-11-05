using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private int _index;

    [Header("Bullet Settings")]
    [SerializeField] private GameObject _bulletSpawner;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Pooler")]
    [SerializeField] private List<GameObject> _pooledBullets;
    [SerializeField] private int _amountBulletToPool; 
    public bool shouldExpand = true;

    [Header("Shooting Settings")]
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

        _pooledBullets = new List<GameObject>();
        for (int i = 0; i < _amountBulletToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(_bulletPrefab);
            obj.SetActive(false);
            obj.transform.parent = _bulletSpawner.transform;
            _pooledBullets.Add(obj);
        }
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

            GameObject bullet = GetPooledBullet();
            if (bullet != null)
            {
                bullet.transform.position = _bulletSpawner.transform.position;
                bullet.transform.rotation = _bulletSpawner.transform.rotation;
                bullet.GetComponent<BulletBehaviour>().SetPlayerIndex(_index);
                bullet.SetActive(true);
                StartCoroutine(WaitBeforeShootingAgain());
                if (_currentShootCapacity <= 0)
                {
                    _inReloadMode = true;
                }
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

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < _pooledBullets.Count; i++)
        {
            if (!_pooledBullets[i].activeInHierarchy)
            {
                return _pooledBullets[i];
            }
        }
        if (shouldExpand)
        {
            GameObject obj = (GameObject)Instantiate(_bulletPrefab);
            obj.SetActive(false);
            obj.transform.parent = _bulletSpawner.transform;
            _pooledBullets.Add(obj);
            return obj;
        }
        else
        {
            return null;
        }
    }

}
