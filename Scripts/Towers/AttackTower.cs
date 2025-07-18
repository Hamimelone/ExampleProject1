using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackTower : Tower
{
    public float CurrentAttackDamage;
    public float CurrentAttackSpeed;
    public float CurrentAttackRange;

    protected Monster currentTarget; // ��ǰ����Ŀ��
    protected float fireCooldown = 0f; // ������ȴʱ��

    protected void Update()
    {
        Fire();
    }
    protected void Fire()
    {
        // ���¿�����ȴʱ��
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }

        // �����ǰĿ�궪ʧ�򳬳�������Χ�������²���Ŀ��
        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            FindTarget();
        }

        // �����Ŀ������ȴʱ��������򹥻�
        if (currentTarget != null && fireCooldown <= 0f && currentTarget.IsActive)
        {
            Attack(currentTarget);
            fireCooldown = 1f / CurrentAttackSpeed; // ������ȴʱ��
        }
    }

    protected void FindTarget()
    {
        // ��ȡ������Χ�ڵ����й���
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, CurrentAttackRange);
        var monsters = hitColliders
            .Select(hit => hit.GetComponent<Monster>())
            .Where(monster => monster != null && monster.IsActive)
            .ToList();

        if (monsters.Count > 0)
        {
            // ѡ���������Ĺ�����ΪĿ��
            currentTarget = monsters
                .OrderBy(monster => Vector2.Distance(this.transform.position, monster.transform.position))
                .First();
        }
        else
        {
            currentTarget = null; // û���ҵ�Ŀ��
        }
    }

    // ���Ŀ���Ƿ��ڹ�����Χ��
    protected bool IsTargetInRange(Monster target)
    {
        return Vector2.Distance(this.transform.position, target.transform.position) <= CurrentAttackRange && target.IsActive;  
    }

    // �������������������д��
    protected abstract void Attack(Monster target);
}
