using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private int _index;

    [SerializeField] private GameObject _bulletSpawner;
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private float _shootRate = 1.5f;
    private bool _canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        _index = GetComponent<TopDownEntity>().GetIndex();
    }

    public void ShootBullet()
    {
        if (_canShoot)
        {
            _canShoot = false;
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
