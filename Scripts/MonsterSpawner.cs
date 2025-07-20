using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    public bool IsSpawning;
    public List<SpawnPortal>spawnPortals = new List<SpawnPortal>();
    [SerializeField] private float actualSpawnInterval;
    private Coroutine spawnCoroutine;
    [SerializeField] private Text timerTxt;
    private float timer = 0f;
    [SerializeField] private float spawningTime;
    [SerializeField] private float restingTime;
    [SerializeField] private int maxWaveNum;
    private int currentWave;
    public bool SpawnFinished;
    private List<float> weightList = new List<float>();
    public void Initialize()
    {
        spawnPortals.Clear();
        weightList.Clear();
        timer = 0f;
        currentWave = 0;
        weightList.Add(50f);
        weightList.Add(0f);
        StopCoroutine(StartTimeLine());
        StopSpawning();
        MonsterPool.Instance.InitializePools();
        StartCoroutine(StartTimeLine());
    }
    public void Update()
    {
        timer += Time.deltaTime;
        timerTxt.text = timer.ToString("f2");
    }

    public void StartSpawning(float interval)
    {
        if (!IsSpawning)
        {
            IsSpawning = true;
            actualSpawnInterval = interval;
            AudioManager.Instance.Play("WaveWarning");
            if (spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);

            spawnCoroutine = StartCoroutine(SpawnMonsterRoutine());
        }
    }

    public void StopSpawning()
    {
        if (IsSpawning)
        {
            if (spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);

            IsSpawning = false;
        }
    }

    IEnumerator SpawnMonsterRoutine()
    {
        while (IsSpawning)
        {
            yield return new WaitForSeconds(actualSpawnInterval);

            if (IsSpawning)
            {
                SpawnMonster();
            }
        }
    }
    IEnumerator StartTimeLine()
    {
        int currentLevel = LevelManager.Instance.CurrentLevelIndex;
        IsSpawning = false;
        SpawnFinished = false;
        while (currentWave < maxWaveNum && currentLevel == LevelManager.Instance.CurrentLevelIndex)
        {
            yield return new WaitForSeconds(restingTime);
            currentWave++;
            StartSpawning(1.2f - 0.2f * (float)currentWave);
            actualSpawnInterval -= Mathf.Max( 0.5f * Time.deltaTime / spawningTime,0.1f);
            UIManager.Instance.ShowText("Monster coming",1.5f);
            yield return new WaitForSeconds(spawningTime);
            weightList[0] -= 10f;
            weightList[1] += 15f;
            StopSpawning();
        }
        SpawnFinished = false;
        SpawnFinished = true;
    }
    public void SpawnMonster()
    {
        int randomIndex = Random.Range(0, spawnPortals.Count);
        float sumW = 0f;
        foreach (var w in weightList)
        {
            sumW += w;
        }
        float r = Random.Range(0,sumW);
        if (r >0 && r <= weightList[0])
        {
            Monster m = MonsterPool.Instance.GetMonster<Monster_A>(spawnPortals[randomIndex].transform.position);
        }
        else
        {
            Monster m = MonsterPool.Instance.GetMonster<Monster_B>(spawnPortals[randomIndex].transform.position);
        }
    }
}
