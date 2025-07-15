using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ArrowTower : AttackTower
{
    protected override void Attack(Monster target)
    {
        Vector2 dir = (target.transform.position - transform.position).normalized;
        ProjectilePool.Instance.GetProjectile<P_Arrow1>(transform.position,dir,CurrentAttackDamage);
    }
}
