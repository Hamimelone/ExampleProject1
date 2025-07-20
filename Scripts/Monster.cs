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
    public MonsterState State {  get; protected set; }
    public bool IsActive {  get; protected set; }
    protected NavMeshAgent agent;
    protected T_MainCollector targetPortal;
    protected CircleCollider2D cc2D;
    [Header("BaseSetting")]
    [SerializeField] protected float size;
    [SerializeField] protected float standardHP;
    public float MaxHealth;
    public float CurrentHealth;
    public float CurrentAttackDamage;
    public float CurrentAttackSpeed;
    public Tower targetTower {  get; protected set; }
    protected float fireCooldown = 0f;
    protected SpriteRenderer sr;
    protected Coroutine flashRoutine;
    protected float RedTime = 0.1f; //���ֺ�ɫ��ʱ�䣬�ɵ���
    protected float changeColorTime = 0f; //���ԭ����ɫ��ʱ��

    public virtual void Initialize(Vector2 spawnPos)
    {
        transform.position = spawnPos;
        agent.updateRotation = false;  // 2D��Ϸ����Ҫ��ת
        agent.updateUpAxis = false;
        gameObject.SetActive(true);
        cc2D.radius = size;
        MaxHealth = standardHP;
        CurrentHealth = MaxHealth;
        IsActive = true;
        targetPortal = FindAnyObjectByType<T_MainCollector>();
        State = MonsterState.Moving;
        targetTower = null;
        changeColorTime = 0f;
        MonsterPool.Instance.AllActiveMonsters.Add(this);
    }
    public void Start()
    {
        
    }
    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cc2D = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        switch (State)
        {
            case MonsterState.Moving:
                Move();
                break;
            case MonsterState.Attacking:
                Attack();
                break;
        }
        sr.color = changeColorTime>0f? Color.red: Color.white;
        if(changeColorTime >0)
        changeColorTime -= Time.deltaTime;
    }
    public virtual void Move()
    {
        if (State == MonsterState.Moving && IsActive && targetPortal != null)
        {
            agent.isStopped = false;
            // ��������Ŀ��λ��
            agent.SetDestination(targetPortal.transform.position);

            // ��ѡ����2D�г����ƶ�����
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
        changeColorTime += RedTime;
    }
    protected virtual void Die()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = null;
        IsActive = false;
        MonsterPool.Instance.AllActiveMonsters.Remove(this);
        //GameManager.Instance.CheckVictory();
        MonsterPool.Instance.ReturnMonster(this);
    }

    public void SetTargetTower(Tower t)
    {
        targetTower = t;
        State = MonsterState.Attacking;
    }
    public virtual void Attack()
    {
        agent.isStopped = true;
        if (targetTower != null)
        {
            // ���¿�����ȴʱ��
            if (fireCooldown > 0f)
            {
                fireCooldown -= Time.deltaTime;
            }
            // �����Ŀ������ȴʱ��������򹥻�
            if (fireCooldown <= 0f)
            {
                targetTower.TakeDamage(CurrentAttackDamage);
                fireCooldown = 1f / CurrentAttackSpeed; // ������ȴʱ��
            }
        }
        else
        {
            State = MonsterState.Moving;
        }
    }
}
