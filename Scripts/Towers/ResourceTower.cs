using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceTower : Tower
{
    [SerializeField] protected float intervalGold;
    [SerializeField] protected int produceGoldAmount;
    public float CurrentProduceInterval;
    protected float produceGoldCooldownGold;


    public override void Initialize()
    {
        base.Initialize();
        CurrentProduceInterval = intervalGold;
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
            produceGoldCooldownGold = CurrentProduceInterval; // ÖØÖÃÀäÈ´Ê±¼ä
        }
    }
    public virtual void GetGold()
    {
        GameManager.Instance.AddGold(produceGoldAmount);
    }
}
