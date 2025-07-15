using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D.IK;

public enum MonsterState
{
    Moving,
    Attacking
}
public abstract class Monster : MonoBehaviour
{
    protected MonsterState State;
    protected bool IsActive;
    protected NavMeshAgent agent;
    protected T_MainCollector targetPortal;
    protected CircleCollider2D cc2D;
    [Header("BaseSetting")]
    [SerializeField] protected float size;
    [SerializeField] protected float standardHP;
    public float MaxHealth;
    public float CurrentHealth;

    public virtual void Initialize(Vector2 spawnPos)
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;  // 2D游戏不需要旋转
        agent.updateUpAxis = false;
        gameObject.SetActive(true);
        cc2D.radius = size;
        transform.position = spawnPos;
        MaxHealth = standardHP;
        CurrentHealth = MaxHealth;
        IsActive = true;
        targetPortal = FindAnyObjectByType<T_MainCollector>();
        State = MonsterState.Moving;
    }
    public void Start()
    {
        
    }
    public virtual void Awake()
    {
        cc2D = GetComponent<CircleCollider2D>();
    }
    public void Update()
    {
        switch (State)
        {
            case MonsterState.Moving:
                Move();
                break;
            case MonsterState.Attacking:
                //Attack();
                break;
        }
    }
    public virtual void Move()
    {
        if (State == MonsterState.Moving && IsActive && targetPortal != null)
        {
            agent.isStopped = false;
            // 持续更新目标位置
            agent.SetDestination(targetPortal.transform.position);

            // 可选：在2D中朝向移动方向
            if (agent.velocity.magnitude > 0.1f )
            {
                float angle = Mathf.Atan2(agent.velocity.y, agent.velocity.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }  
    }

    public void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
        if (CurrentHealth <= 0) Die();
    }
    protected virtual void Die()
    {
        IsActive = false;
        MonsterPool.Instance.ReturnMonster(this);
    }
}
