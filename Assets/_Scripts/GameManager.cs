using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    public List<GameObject> monsterPool = new List<GameObject>();
    public int maxMonsters = 10;
    public GameObject monster;
    public float createTime = 3.0f;

    [SerializeField] bool isGameOver;

    public static GameManager instance = null;

    public TMP_Text scoreText;
    int totalScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find(AllString.SpawnPointGroup)
                                                                ?.transform;

        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        InvokeRepeating(nameof(CreateMonster), 2.0f, createTime);
        totalScore = PlayerPrefs.GetInt("Total_Score", 0);
        DisplayScore(0);
    }


    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke(nameof(CreateMonster));
            }
        }
    }

    private void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //stantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);
    }

    private void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            var _monster = Instantiate<GameObject>(monster);

            _monster.name = $"Monster_{i:00}";
            _monster.SetActive(false);
            monsterPool.Add(_monster);
        }
    }

    private GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            if(_monster.activeSelf == false)
            {
                return _monster;
            }
        }
        return null;
    }

    public void DisplayScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color><color=#ff0000>{totalScore:#,##0}</color>";
        PlayerPrefs.SetInt("Total_Score", totalScore);
    }
}
