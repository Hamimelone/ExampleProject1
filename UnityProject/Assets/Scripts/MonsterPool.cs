using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public Monster bulletPrefab;
        public int initialSize = 10;
    }

    [SerializeField] private List<PoolConfig> poolConfigs;

    private Dictionary<System.Type, Queue<Monster>> pools;
    private Dictionary<System.Type, Monster> prefabMap;
    private Dictionary<System.Type, Transform> poolParents;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializePools();
    }

    private void InitializePools()
    {
        pools = new Dictionary<System.Type, Queue<Monster>>();
        prefabMap = new Dictionary<System.Type, Monster>();
        poolParents = new Dictionary<System.Type, Transform>();

        foreach (var config in poolConfigs)
        {
            var type = config.bulletPrefab.GetType();
            prefabMap[type] = config.bulletPrefab;

            // 为每种子弹类型创建父对象
            var parent = new GameObject($"{type.Name}Pool").transform;
            parent.SetParent(transform);
            poolParents[type] = parent;

            var queue = new Queue<Monster>();
            for (int i = 0; i < config.initialSize; i++)
            {
                Monster m = CreateNewMonster(type);
                queue.Enqueue(m);
            }

            pools[type] = queue;
        }
    }

    private Monster CreateNewMonster(System.Type mType)
    {
        if (!prefabMap.ContainsKey(mType))
        {
            Debug.LogError($"No prefab registered for type {mType}");
            return null;
        }

        Monster m = Instantiate(prefabMap[mType], poolParents[mType]);
        m.gameObject.SetActive(false);
        return m;
    }

    public T GetMonster<T>() where T : Monster
    {
        var type = typeof(T);
        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"No pool exists for bullet type {type}");
            return null;
        }

        var queue = pools[type];
        Monster m;

        if (queue.Count > 0)
        {
            m = queue.Dequeue();
        }
        else
        {
            m = CreateNewMonster(type);
            Debug.Log($"Expanding pool for {type}");
        }

        m.Initialize();
        return (T)m;
    }

    public void ReturnMonster(Monster m)
    {
        if (m == null) return;

        var type = m.GetType();
        if (!pools.ContainsKey(type))
        {
            Debug.LogWarning($"Returning monster of unregistered type {type}. It will be destroyed.");
            Destroy(m.gameObject);
            return;
        }

        m.gameObject.SetActive(false);
        m.transform.SetParent(poolParents[type]);
        pools[type].Enqueue(m);
    }

    // 动态添加新子弹类型
    public void AddNewMonsterType<T>(T prefab, int initialSize = 10) where T : Monster
    {
        var type = typeof(T);
        if (pools.ContainsKey(type))
        {
            Debug.LogWarning($"Pool for {type} already exists");
            return;
        }

        prefabMap[type] = prefab;

        var parent = new GameObject($"{type.Name}Pool").transform;
        parent.SetParent(transform);
        poolParents[type] = parent;

        var queue = new Queue<Monster>();
        for (int i = 0; i < initialSize; i++)
        {
            Monster m = CreateNewMonster(type);
            queue.Enqueue(m);
        }

        pools[type] = queue;
    }
}
