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

    private int _playersInInner = 0;
    private TopDownEntity _playerInInner;

    public void TriggerPower()
    {
        TopDownEntity[] players = LevelManager.instance.players;

        for (int i = 0; i < players.Length; i++)
        {
            float distanceToPlayer = CalculateDistance(players[i].transform);
            
            if (distanceToPlayer < innerCircleDistance * innerCircleDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(players[i].transform.position, transform.position, out hit, mineMaxRadius))
                {
                    if (hit.collider.gameObject.CompareTag("Wall"))
                    {
                        if(hit.collider.GetComponent<Shield>().GetPlayerProtected() == i)
                        {
                            break;
                        }
                    }
                }

                _playerInInner = players[i];
                _playersInInner++;
            }
            else if (distanceToPlayer < middleCircleDistance * middleCircleDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(players[i].transform.position, transform.position, out hit, mineMaxRadius))
                {
                    if (hit.collider.gameObject.CompareTag("Wall"))
                    {
                        if (hit.collider.GetComponent<Shield>().GetPlayerProtected() == i)
                        {
                            return;
                        }
                    }
                }

                MiddleCircleBehaviour(players[i]);

            }
            else if (distanceToPlayer < mineMaxRadius * mineMaxRadius)
            {
                RaycastHit hit;
                if (Physics.Raycast(players[i].transform.position, transform.position, out hit, mineMaxRadius))
                {
                    if (hit.collider.gameObject.CompareTag("Wall"))
                    {
                        if (hit.collider.GetComponent<Shield>().GetPlayerProtected() == i)
                        {
                            return;
                        }
                    }
                }

                MaxRadiusBehaviour(players[i].transform);
            }
        }

        if (_playersInInner == 2)
        {
            InnerCircleBehaviour(players[0]);
            InnerCircleBehaviour(players[1]);
        }
        else if (_playerInInner != null)
        {
            InnerCircleBehaviour(_playerInInner);
            _playerInInner = null;
        }



        for (int i = 0; i < LevelManager.instance.shieldList.Count; i++)
        {
            float distanceToPlayer = CalculateDistance(LevelManager.instance.shieldList[i].transform);

            if (distanceToPlayer < innerCircleDistance * innerCircleDistance)
            {
                InnerCircleBehaviour(LevelManager.instance.shieldList[i]);
            }
            else if (distanceToPlayer < middleCircleDistance * middleCircleDistance)
            {
                MiddleCircleBehaviour(LevelManager.instance.shieldList[i]);

            }
            else if (distanceToPlayer < mineMaxRadius * mineMaxRadius)
            {
                MaxRadiusBehaviour(players[i].transform);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TriggerPower();
        }
    }

    #region Shield
    private void MiddleCircleBehaviour(Shield entity)
    {
        entity.LooseLife((entity.GetLifeMax() / 3));
    }

    private void InnerCircleBehaviour(Shield entity)
    {
        entity.DestroyWall();
    }
    #endregion

    #region Player
    private void MaxRadiusBehaviour(Transform target)
    {
        Vector3 pushDirection = (target.transform.position - transform.position).normalized;
        
        target.GetComponent<Rigidbody>().AddForce(pushDirection * pushPower, ForceMode.Impulse);
    }
    
    private void MiddleCircleBehaviour(TopDownEntity entity)
    {
        entity.ChangeLife((entity.GetLifeMax()/-2));
    }
    
    private void InnerCircleBehaviour(TopDownEntity entity)
    {
        entity.Kill(entity.GetIndex());
    }
    #endregion

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
