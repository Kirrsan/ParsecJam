using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    public float mineMaxRadius;
    public float middleCircleDistance;
    public float innerCircleDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TriggerPower()
    {
        TopDownEntity[] players = LevelManager.instance.players;
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, mineMaxRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, middleCircleDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, innerCircleDistance);
    }
}
