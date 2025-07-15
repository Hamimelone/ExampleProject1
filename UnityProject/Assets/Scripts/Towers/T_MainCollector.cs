using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_MainCollector : ResourceTower
{
    [SerializeField] protected float standardProduceGemSpeed;
    [SerializeField] protected int produceGemAmount;
    public float CurrentProduceGemSpeed;
    protected float produceGemCooldown;
    public override void Initialize()
    {
        base.Initialize();
        CurrentProduceGemSpeed = standardProduceGemSpeed;
    }
    public override void Produce()
    {
        if (produceGemCooldown > 0f)
        {
            produceGemCooldown -= Time.deltaTime;
        }
        if (produceGemCooldown <= 0f)
        {
            GetGem();
            produceGemCooldown = 1f / CurrentProduceGemSpeed; // ÖØÖÃÀäÈ´Ê±¼ä
        }
        base.Produce();
    }
    public void GetGem()
    {
        GameManager.Instance.AddGem(produceGemAmount);
    }
}
