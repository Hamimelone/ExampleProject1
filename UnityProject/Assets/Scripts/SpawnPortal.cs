using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public bool IsActive;
    [SerializeField]private float actualSpawnInterval;
    private Coroutine spawnCoroutine;
    [SerializeField]private float spawnRange;

    public void Initialize()
    {
        MapManager.Instance.NotPlacableList.Add(transform.position);
        MapManager.Instance.DicPosToTileType.Add(transform.position, TileType.SpawnPoint);
        IsActive = true;
        StartSpawning();
    }
    public void StartSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnMonsterRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        IsActive = false;
    }

    IEnumerator SpawnMonsterRoutine()
    {
        while (IsActive)
        {
            yield return new WaitForSeconds(actualSpawnInterval);

            if (IsActive)
            {
                SpawnMonster();
            }
        }
    }
    public void SpawnMonster()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRange;
        Vector2 spawnPos = (Vector2)transform.position + randomCircle;
        Monster m = MonsterPool.Instance.GetMonster<Monster_A>(spawnPos);
        Debug.Log("spawn a monster");
    }
}
