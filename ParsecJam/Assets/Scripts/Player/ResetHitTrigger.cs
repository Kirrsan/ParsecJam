using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHitTrigger : MonoBehaviour
{
    private TopDownEntity _playerEntity;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerEntity = transform.parent.GetComponent<TopDownEntity>();
    }


    public void SetIsBeingHitFalse()
    {
        _playerEntity.isBeingHit = false;
    }

    public void InstantiateDeathFx()
    {
        _playerEntity.InstantiateDeathVFX();
    }
}
