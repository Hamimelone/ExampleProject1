using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class T_LightningTower : AttackTower
{
    [SerializeField] private float spreadRange;
    [SerializeField] private int spreadNumber;
    protected override void Attack(Monster target)
    {
        AudioManager.Instance.Play("LightningShot");
        target.TakeDamage(CurrentAttackDamage);
        ShowLightningEffectBetween(transform.position,target.transform.position);
        SpreadAttack(target);
    }
    protected void SpreadAttack(Monster target)
    {
        // 获取攻击范围内的所有怪物
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(target.transform.position, spreadRange);
        var monsters = hitColliders
            .Select(hit => hit.GetComponent<Monster>())
            .Where(monster => monster != null && monster.IsActive && monster != target)
            .ToList();

        if (monsters.Count > 0)
        {
            int n = Mathf.Min(monsters.Count, spreadNumber);
            // 选择距离最近的怪物作为目标
            var mms = monsters
                .OrderBy(monster => Vector2.Distance(target.transform.position, monster.transform.position))
                .Take(n).ToList();
            foreach ( var monster in mms )
            {
                monster.TakeDamage(CurrentAttackDamage);
                ShowLightningEffectBetween(target.transform.position, monster.transform.position);
            }
        }
    }
    protected void ShowLightningEffectBetween(Vector2 A,Vector2 B)
    {
        Vector2 dir = (B-A).normalized;
        float scale = Vector2.Distance(A, B) / 3f;
        Vector2 mid = 0.5f * (A + B);
        EffectManager.Instance.SetEffect(0, dir, mid, 0.1f, scale);
    }
}
