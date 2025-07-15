using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Arrow1 : Projectile
{
    public override void Initialize(Vector2 position, Vector2 direction,float dmg)
    {
        Speed = 10f;
        Range = 20f;    
        base.Initialize(position, direction, dmg);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Monster m = other.gameObject.GetComponent<Monster>();
            m?.TakeDamage(Damage);
            ProjectilePool.Instance.ReturnProjectile(this);
        }
    }
}
