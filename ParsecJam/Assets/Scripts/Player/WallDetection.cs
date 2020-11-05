using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{

    private TopDownEntity _entity;
    [SerializeField] private WallDetectionTrigger _detectionTrigger;
    private Vector3 _lastPosition;
    private Vector3 _contactPoint;
    private bool _isOutOfCollision = true;

    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponent<TopDownEntity>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!_isOutOfCollision && _detectionTrigger.GetIsInWall())
        {
            _isOutOfCollision = true;
            _entity.transform.position = _entity.transform.position + _contactPoint * 2;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _isOutOfCollision = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _isOutOfCollision = false;
            _contactPoint = -collision.contacts[0].normal;
        }
    }
}
