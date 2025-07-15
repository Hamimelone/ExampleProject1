using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceTower : Tower
{
    [SerializeField] protected float standardProduceGoldSpeed;
    [SerializeField] protected int produceGoldAmount;
    public float CurrentProduceSpeed;
    protected float produceGoldCooldownGold;


    public override void Initialize()
    {
        base.Initialize();
        CurrentProduceSpeed = standardProduceGoldSpeed;
    }
    protected void Update()
    {
        Produce();
    }
    public virtual void Produce()
    {
        if (produceGoldCooldownGold > 0f)
        {
            produceGoldCooldownGold -=Time.deltaTime;
        }
        if (produceGoldCooldownGold <= 0f)
        {
            GetGold();
            produceGoldCooldownGold = 1f / CurrentProduceSpeed; // ÖØÖÃÀäÈ´Ê±¼ä
        }
    }
    public virtual void GetGold()
    {
        GameManager.Instance.AddGold(produceGoldAmount);
    }
}
