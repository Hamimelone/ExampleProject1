using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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
    public List<Monster> AllActiveMonsters = new List<Monster>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void DeleteChildren()
    {
        // 获取所有子物体的Transform组件
        Transform[] children = new Transform[transform.childCount];

        // 先将所有子物体引用存储到数组中
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // 删除所有子物体
        foreach (Transform child in children)
        {
            // 在编辑器中使用DestroyImmediate，在运行时使用Destroy
            if (Application.isPlaying)
            {
                Object.Destroy(child.gameObject);
            }
            else
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }
    public void InitializePools()
    {
        GameObjectExtensions.DestroyAllChildren(this.gameObject);
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
        AllActiveMonsters.Clear();
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

    public T GetMonster<T>(Vector2 spawnPos) where T : Monster
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

        m.Initialize(spawnPos);
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
    public bool CheckMonsterClear()
    {
        return AllActiveMonsters.Count == 0;
    }
}
