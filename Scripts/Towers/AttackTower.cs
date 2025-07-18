using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackTower : Tower
{
    public float CurrentAttackDamage;
    public float CurrentAttackSpeed;
    public float CurrentAttackRange;

    protected Monster currentTarget; // 当前攻击目标
    protected float fireCooldown = 0f; // 开火冷却时间

    protected void Update()
    {
        Fire();
    }
    protected void Fire()
    {
        // 更新开火冷却时间
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }

        // 如果当前目标丢失或超出攻击范围，则重新查找目标
        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            FindTarget();
        }

        // 如果有目标且冷却时间结束，则攻击
        if (currentTarget != null && fireCooldown <= 0f && currentTarget.IsActive)
        {
            Attack(currentTarget);
            fireCooldown = 1f / CurrentAttackSpeed; // 重置冷却时间
        }
    }

    protected void FindTarget()
    {
        // 获取攻击范围内的所有怪物
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, CurrentAttackRange);
        var monsters = hitColliders
            .Select(hit => hit.GetComponent<Monster>())
            .Where(monster => monster != null && monster.IsActive)
            .ToList();

        if (monsters.Count > 0)
        {
            // 选择距离最近的怪物作为目标
            currentTarget = monsters
                .OrderBy(monster => Vector2.Distance(this.transform.position, monster.transform.position))
                .First();
        }
        else
        {
            currentTarget = null; // 没有找到目标
        }
    }

    // 检查目标是否在攻击范围内
    protected bool IsTargetInRange(Monster target)
    {
        return Vector2.Distance(this.transform.position, target.transform.position) <= CurrentAttackRange && target.IsActive;  
    }

    // 攻击方法（子类可以重写）
    protected abstract void Attack(Monster target);
}
