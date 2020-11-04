using UnityEngine;
using UnityEngine.UI;

public class AddScroreBehaviour : MonoBehaviour
{

    private Vector3 _initialPosition;
    private Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _text = GetComponent<Text>();
        _text.text = "+1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationEnd()
    {
        transform.position = _initialPosition;
        gameObject.SetActive(false);
    }
}
