using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Power
{
    None,
    Mine,
    Shield,
    Rocket,
    Cancel,
}

public class PickUpPower : MonoBehaviour
{

    [SerializeField]private Power powerToGive;    
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TopDownEntity entity = other.GetComponent<TopDownEntity>();
        entity.SetPickable(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TopDownEntity entity = other.GetComponent<TopDownEntity>();
        entity.ResetPickable();
    }

    public Power GivePower()
    {
        return powerToGive;
    }

}
