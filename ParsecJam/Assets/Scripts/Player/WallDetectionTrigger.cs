using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectionTrigger : MonoBehaviour
{
    private bool _isInWall = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            print("miaou");

            _isInWall = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            print("miaou");

            _isInWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            _isInWall = false;
        }
    }

    public bool GetIsInWall()
    {
        return _isInWall;
    }
}
