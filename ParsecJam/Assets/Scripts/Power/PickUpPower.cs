using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPower : MonoBehaviour
{

    [SerializeField]private PowerEnum.Power powerToGive;    
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TopDownEntity entity = other.GetComponent<TopDownEntity>();
        entity.currentPower = powerToGive;
    }
}
