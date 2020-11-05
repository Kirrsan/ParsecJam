﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private float _speed;
    private int _playerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * _speed;
    }

    public void SetPlayerIndex(int index)
    {
        _playerIndex = index;
    }

    public int GetPlayerIndex()
    {
        return _playerIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Unshootable"))
        {
            return;
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            if(other.GetComponent<BulletBehaviour>().GetPlayerIndex() == _playerIndex)
                return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<TopDownEntity>().GetIndex() != _playerIndex)
            {
                TopDownEntity otherPlayer = other.GetComponent<TopDownEntity>();
                otherPlayer.ChangeLife(-1);
            }
            else
            {
                return;
            }
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            other.GetComponent<Shield>().LooseLife(1);
        }
        gameObject.SetActive(false);
    }
}
