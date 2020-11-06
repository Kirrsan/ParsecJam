using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollision : MonoBehaviour
{

     private bool _isInCollision = false;
    
    
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Wall") ||other.CompareTag("Shield") || other.CompareTag("Mine"))
        {
            _isInCollision = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Wall") ||other.CompareTag("Shield") || other.CompareTag("Mine"))
        {
            _isInCollision = false;
        }
    }

    public bool GetIsInCollision()
    {
        return _isInCollision;
    }
}
