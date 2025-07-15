using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Projectile : MonoBehaviour
{
    public float Damage { get; protected set; }
    public float Speed { get; protected set; }
    public Vector2 Direction { get; protected set; }
    public float Range { get; protected set; }
    protected Vector2 ShootPos;
    protected Rigidbody2D rb;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void Initialize(Vector2 position,Vector2 direction, float dmg)
    {
        ShootPos = position;
        transform.position = position;
        transform.right = direction; // 使子弹朝向移动方向
        Direction = direction;
        Damage = dmg;
        gameObject.SetActive(true);
    }
    protected virtual void FixedUpdate()
    {
        if (Vector2.Distance(ShootPos, transform.position) <= Range)
            Move();
        else
            ProjectilePool.Instance.ReturnProjectile(this);
    }
    protected virtual void Move()
    {
        rb.velocity = Direction * Speed;
    }
    protected abstract void OnTriggerEnter2D(Collider2D other);
}
