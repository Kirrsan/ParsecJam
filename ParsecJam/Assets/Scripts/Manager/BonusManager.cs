using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusManager : MonoBehaviour
{

    public static BonusManager instance;

    
    [SerializeField]private GameObject _powerPrefab;
    
    [SerializeField]private float _powerDisappearAfterThisAmountOfTime;
    [SerializeField]private float _powerAppearAfterThisAmountOfTime;
    
    private PickUpPosition[] _bonusPosition;
    private List<Transform> _bonusPositionList = new List<Transform>();
    private int _bonusToSpawnForCurrentLd;

    
    private List<PickUpPower> _bonusSpawned = new List<PickUpPower>();

    private bool _isSpawned;

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
        LevelManager levelManager = LevelManager.instance;
        _bonusPosition = levelManager.GetCurrentLevelDesign().bonusPosition;
        _bonusToSpawnForCurrentLd = levelManager.GetCurrentLevelDesign().numberOfBonusToSpawn;

        if (_bonusToSpawnForCurrentLd > _bonusPosition.Length)
        {
            _bonusToSpawnForCurrentLd = _bonusPosition.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isSpawned)
            StartCoroutine(SpawnBonuses());
    }

    private IEnumerator SpawnBonuses()
    {
        _isSpawned = true;

        yield return new WaitForSeconds(_powerAppearAfterThisAmountOfTime);
        
        FillList();
        List<int> randomPosition = new List<int>();
        for (int i = 0; i < _bonusPosition.Length; i++)
        {
            randomPosition.Add(i);
        }
        
        for (int i = 0; i < _bonusToSpawnForCurrentLd; i++)
        {
            int random = Random.Range(0, randomPosition.Count);
            int position = randomPosition[random];
            randomPosition.Remove(random);
            
            GameObject bonus = GameObject.Instantiate(_powerPrefab, _bonusPosition[position].transform.position, Quaternion.identity);
            PickUpPower bonusPower = bonus.GetComponent<PickUpPower>();
            bonusPower.SetPower(_bonusPosition[position].powerThatWillSpawnHere);
            _bonusSpawned.Add(bonusPower);
        }

        StartCoroutine(RemoveBonusesLeft());
    }

    private IEnumerator RemoveBonusesLeft()
    {
        yield return  new WaitForSeconds(_powerDisappearAfterThisAmountOfTime);
        for (int i = 0; i < _bonusSpawned.Count; i++)
        {
            Destroy(_bonusSpawned[i].gameObject);
        }
        _bonusSpawned.Clear();
        _isSpawned = false;
    }

    void FillList()
    {
        _bonusPositionList.Clear();
        for (int i = 0; i < _bonusPosition.Length; i++)
        {
            _bonusPositionList.Add(_bonusPosition[i].transform);
        }
    }

    public void RemoveBonus(PickUpPower power)
    {
        _bonusSpawned.Remove(power);
    }
}
