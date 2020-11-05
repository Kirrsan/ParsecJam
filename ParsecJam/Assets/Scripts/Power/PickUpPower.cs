using UnityEngine;

public enum Power
{
    None,
    Mine,
    Shield,
    Rocket,
    Cancel,
}

public class PickUpPower : MonoBehaviour
{

    [SerializeField] private Power _powerToGive;
    [SerializeField] private GameObject[] _powerObject;
    private Animation _animation;

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TopDownEntity entity = other.GetComponent<TopDownEntity>();
        entity.SetPickable(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TopDownEntity entity = other.GetComponent<TopDownEntity>();
        entity.ResetPickable();
    }

    public Power GivePower()
    {
        return _powerToGive;
    }

    public void SetPower(Power power)
    {
        _powerToGive = power;
        _powerObject[(int)power - 1].SetActive(true);
    }

    public void StartLevitating()
    {
        _animation.Play("PickUp");
    }

}
