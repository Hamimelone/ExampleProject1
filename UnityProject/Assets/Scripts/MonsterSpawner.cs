using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    public bool IsSpawning;
    public List<SpawnPortal>spawnPortals = new List<SpawnPortal>();
    [SerializeField] private float actualSpawnInterval;
    private Coroutine spawnCoroutine;
    public void Initialize()
    {
        spawnPortals.Clear();
    }
    public void StartSpawning()
    {
        IsSpawning = true;
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnMonsterRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        IsSpawning = false;
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
    public void SpawnMonster()
    {
        int randomIndex = Random.Range(0, spawnPortals.Count);
        Monster m = MonsterPool.Instance.GetMonster<Monster_A>(spawnPortals[randomIndex].transform.position);
    }
}
