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
            //other player.dead
            Respawn respawnP1 = other.transform.parent.GetComponent<Respawn>();
            respawnP1.StartCoroutine(respawnP1.PlayerDeathAndRespawn(Respawn.respawnTimeBulletKill));
            Respawn respawnP2 = LevelManager.instance.players[_playerIndex].GetComponent<Respawn>();
            respawnP2.StartCoroutine(respawnP2.PlayerDeathAndRespawn(Respawn.respawnTimeBulletKill));
            ScoreManager.instance.AddToScore(_playerIndex);
        }
        Destroy(this.gameObject);
    }
}
