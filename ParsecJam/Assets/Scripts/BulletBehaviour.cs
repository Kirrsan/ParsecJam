using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private float _speed;
    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += direction * Time.deltaTime * _speed;
    }

    private void ChangeDirection(Vector3 dir)
    {
        direction = dir;
    }
}
