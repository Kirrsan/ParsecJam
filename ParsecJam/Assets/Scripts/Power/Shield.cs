using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{

    private int _playerProtectedIndex;
    [SerializeField] private float _lifeMax = 7;
    private float _life;
    [SerializeField] private Image _lifeBar;

    // Start is called before the first frame update
    void Start()
    {
        _life = _lifeMax;
        LevelManager.instance.shieldList.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerProtected(int playerIndex)
    {
        _playerProtectedIndex = playerIndex;
    }

    public void LooseLife(float looseAmount)
    {
        _life -= looseAmount;
        _lifeBar.fillAmount = _life * (1/_lifeMax);
        if (_life <= 0)
        {
            DestroyWall();
            return;
        }
        AudioManager.instance.Play("ShieldHit");
    }

    public void DestroyWall()
    {
        AudioManager.instance.Play("ShieldBreak");
        LevelManager.instance.shieldList.Remove(this);
        Destroy(this.transform.parent.gameObject);
    }

    public float GetLifeMax()
    {
        return _lifeMax;
    }

    public int GetPlayerProtected()
    {
        return _playerProtectedIndex;
    }

}
