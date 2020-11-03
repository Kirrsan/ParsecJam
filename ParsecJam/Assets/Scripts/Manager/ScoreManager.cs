using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    private float[] _scores;

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
        _scores = new float[LevelManager.instance.numberOfPlayers];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScore(int index)
    {
        _scores[index] += 1;
    }

}
