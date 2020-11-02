using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] private GameObject _bulletSpawner;
    [SerializeField] private GameObject _bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShootBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawner.transform.position, _bulletSpawner.transform.rotation);
    }

}
