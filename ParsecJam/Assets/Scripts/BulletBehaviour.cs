using System.Collections;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TopDownEntity otherPlayer = other.GetComponentInParent<TopDownEntity>();
            otherPlayer.ChangeLife(-1);
            InterfaceManager.instance.AdjustLifeBar(otherPlayer.GetIndex(), otherPlayer.GetLife() * (1/otherPlayer.GetLifeMax()));

            if (otherPlayer.GetIsDead())
            {
                LevelManager.instance.RespawnPlayers();

                ScoreManager.instance.AddToScore(_playerIndex);
            }
        }
        Destroy(this.gameObject);
    }
}
