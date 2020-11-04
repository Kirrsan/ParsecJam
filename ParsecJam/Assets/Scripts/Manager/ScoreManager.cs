using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    public int numberOfPlayers = 0;
    private float[] _scores;
    [SerializeField] private Text[] _scoreTexts;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _scores = new float[numberOfPlayers];
        for(int i = 0; i < _scores.Length; i++)
        {
            _scoreTexts[i].text = _scores[i].ToString();
            _scoreTexts[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScore(int index)
    {
        _scores[index] += 1;
        _scoreTexts[index].text = _scores[index].ToString();
        _scoreTexts[index].transform.GetChild(0).gameObject.SetActive(true);
    }

    public string GetWinner()
    {
        string temp = "";
        if(_scores[0] > _scores[1])
        {
            AudioManager.instance.Play("EndVictory");
            temp = "1-" + _scores[0].ToString();
        }
        else if(_scores[0] < _scores[1])
        {
            AudioManager.instance.Play("EndVictory");
            temp = "2-" + _scores[1].ToString();
        }
        else
        {
            AudioManager.instance.Play("EndTie");
            temp = "tie";
        }
        return temp;
    }

}
