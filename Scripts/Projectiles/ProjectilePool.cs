using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public Projectile bulletPrefab;
        public int initialSize = 10;
    }

    [SerializeField] private List<PoolConfig> poolConfigs;

    private Dictionary<System.Type, Queue<Projectile>> pools;
    private Dictionary<System.Type, Projectile> prefabMap;
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
        pools = new Dictionary<System.Type, Queue<Projectile>>();
        prefabMap = new Dictionary<System.Type, Projectile>();
        poolParents = new Dictionary<System.Type, Transform>();

        foreach (var config in poolConfigs)
        {
            var type = config.bulletPrefab.GetType();
            prefabMap[type] = config.bulletPrefab;

            // 为每种子弹类型创建父对象
            var parent = new GameObject($"{type.Name}Pool").transform;
            parent.SetParent(transform);
            poolParents[type] = parent;

            var queue = new Queue<Projectile>();
            for (int i = 0; i < config.initialSize; i++)
            {
                Projectile p = CreateNewProjectile(type);
                queue.Enqueue(p);
            }

            pools[type] = queue;
        }
    }

    private Projectile CreateNewProjectile(System.Type pType)
    {
        if (!prefabMap.ContainsKey(pType))
        {
            Debug.LogError($"No prefab registered for type {pType}");
            return null;
        }

        Projectile p = Instantiate(prefabMap[pType], poolParents[pType]);
        p.gameObject.SetActive(false);
        return p;
    }

    public T GetProjectile<T>(Vector2 position, Vector2 direction, float dmg) where T : Projectile
    {
        var type = typeof(T);
        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"No pool exists for projectile type {type}");
            return null;
        }

        var queue = pools[type];
        Projectile p;

        if (queue.Count > 0)
        {
            p = queue.Dequeue();
        }
        else
        {
            p = CreateNewProjectile(type);
            Debug.Log($"Expanding pool for {type}");
        }

        p.Initialize(position, direction,dmg);
        return (T)p;
    }

    public void ReturnProjectile(Projectile p)
    {
        if (p == null) return;

        var type = p.GetType();
        if (!pools.ContainsKey(type))
        {
            Debug.LogWarning($"Returning projectile of unregistered type {type}. It will be destroyed.");
            Destroy(p.gameObject);
            return;
        }

        p.gameObject.SetActive(false);
        p.transform.SetParent(poolParents[type]);
        pools[type].Enqueue(p);
    }

    // 动态添加新子弹类型
    public void AddNewProjectileType<T>(T prefab, int initialSize = 10) where T : Projectile
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

        var queue = new Queue<Projectile>();
        for (int i = 0; i < initialSize; i++)
        {
            Projectile p = CreateNewProjectile(type);
            queue.Enqueue(p);
        }

        pools[type] = queue;
    }
}
