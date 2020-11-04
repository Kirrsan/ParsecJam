using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private Animator _anim;
    
    [SerializeField] float _randomMin;
    [SerializeField] float _randomMax;

    private float randomEyeOpening = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();

        randomEyeOpening = Random.Range(_randomMin, _randomMax);
        StartCoroutine(EyeOpening());
    }

    private IEnumerator EyeOpening()
    {
        yield return new WaitForSeconds(randomEyeOpening);
        OpenEye();
    }

    
    public void OpenEye()
    {
        _anim.SetTrigger("OpenEye");
    }
    
    public void CloseEye()
    {
        _anim.SetTrigger("CloseEye");
    }
}
