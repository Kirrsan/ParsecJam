using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    public float pushPower = 5;

    public float mineMaxRadius;
    public float middleCircleDistance;
    public float innerCircleDistance;

    public bool _isMinePlaced = false;
    
    
    //TODO : reste à faire l'activation posage de mine et l'activation à distance de la mine, sorry j'ai pas trop le temps de le faire là :(
    
    //TODO: pour le posage de mine faudra répertorier l'index du joueur qui l'a posé, pour enlever son pouvoir quand la mine aura bakooooooooooooooooooooooom
    
    //TODO : A appeler quand ya l'activation à distance ;)
    public void TriggerPower()
    {
        TopDownEntity[] players = LevelManager.instance.players;

        for (int i = 0; i < players.Length; i++)
        {
            float distanceToPlayer = CalculateDistance(players[i].transform);
           Debug.Log(distanceToPlayer);
            
            if (distanceToPlayer < innerCircleDistance * innerCircleDistance)
            {
                InnerCircleBehaviour(players[i]);
                Debug.Log("inner");

            }
            else if (distanceToPlayer < middleCircleDistance * middleCircleDistance)
            {
                MiddleCircleBehaviour(players[i]);
                Debug.Log("middle");

            }
            else if (distanceToPlayer < mineMaxRadius * mineMaxRadius)
            {
                MaxRadiusBehaviour(players[i].transform.GetChild(0));
                Debug.Log("max");
            }
        }
        //TODO : enlever le pouvoir du joueur 
        
        
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TriggerPower();
        }
    }

    private void MaxRadiusBehaviour(Transform target)
    {
        Vector3 pushDirection = (target.transform.position - transform.position).normalized;
        
        target.GetComponent<Rigidbody>().AddForce(pushDirection * pushPower, ForceMode.Impulse);
    }
    
    private void MiddleCircleBehaviour(TopDownEntity entity)
    {
        // perd la moitié de sa vie
    }
    
    private void InnerCircleBehaviour(TopDownEntity entity)
    {
        // met sa vie à 0 (si t'as géré ce behaviour là :p)
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

    private float CalculateDistance(Transform targetTransform)
    {
        return ((Vector3)targetTransform.position - (Vector3)transform.position).sqrMagnitude;
    }
    
}
