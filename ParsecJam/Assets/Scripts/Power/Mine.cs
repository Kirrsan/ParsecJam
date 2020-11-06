using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{

    public float pushPower = 5;
    public float pushPowerMiddleCircle = 8;

    public float mineMaxRadius;
    public float middleCircleDistance;
    public float innerCircleDistance;

    public bool _isMinePlaced = false;

    [SerializeField] private Text _playerText;
    [SerializeField] private GameObject _inputImage;
    [SerializeField] private GameObject _mine;
    [SerializeField] private GameObject _mineFX;
    [SerializeField] private ParticleSystem _mineFXClouds;

    private int _playersInInner = 0;
    private TopDownEntity _playerInInner;


    private void Start()
    {
        if(LevelManager.instance.numberOfMinesExploded < 4)
        {
            _inputImage.SetActive(true);
        }
        LevelManager.instance.mineList.Add(this);
    }

    public void TriggerPower(bool firstDetonator)
    {
        _playerText.gameObject.SetActive(false);
        _inputImage.SetActive(false);
        LevelManager.instance.numberOfMinesExploded++;
        _mine.SetActive(false);
        _mineFX.SetActive(false);
        _mineFX.SetActive(true);
        TopDownEntity[] players = LevelManager.instance.players;

        for (int i = 0; i < players.Length; i++)
        {
            float distanceToPlayer = CalculateDistance(players[i].transform);
            RaycastHit hit;
            if (Physics.Raycast(players[i].transform.position, transform.position - players[i].transform.position, out hit, mineMaxRadius))
            {
                if (hit.collider.gameObject.CompareTag("Shield") || hit.collider.gameObject.CompareTag("Wall"))
                {
                    float distanceToWall = CalculateDistance(hit.transform);
                    if (distanceToWall < distanceToPlayer)
                    {
                        break;
                    }
                }
            }
            if (distanceToPlayer < innerCircleDistance * innerCircleDistance)
            {
                _playerInInner = players[i];
                _playersInInner++;
            }
            else if (distanceToPlayer < middleCircleDistance * middleCircleDistance)
            {
                MiddleCircleBehaviour(players[i]);
            }
            else if (distanceToPlayer < mineMaxRadius * mineMaxRadius)
            {
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
        }

        if (firstDetonator)
        {
            for (int i = 0; i < LevelManager.instance.mineList.Count; i++)
            {
                if (LevelManager.instance.mineList[i] != this)
                {
                    float distanceToOtherMine = CalculateDistance(LevelManager.instance.mineList[i].transform);
                    if (distanceToOtherMine < mineMaxRadius * mineMaxRadius)
                    {
                        LevelManager.instance.mineList[i].TriggerPower(false);
                    }
                }
            }
        }

        StartCoroutine(WaitAndDestroy());
    }

    private void Update()
    {

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
        Vector3 pushDirection = (entity.transform.position - transform.position).normalized;
        
        entity.GetComponent<Rigidbody>().AddForce(pushDirection * pushPowerMiddleCircle, ForceMode.Impulse);
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

    public void SetPlayerText(int playerIndex)
    {
        _playerText.text = "P" + playerIndex.ToString();
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(_mineFXClouds.main.duration);
        Destroy(gameObject);
    }
    
}
