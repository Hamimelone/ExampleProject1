using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_MainCollector : ResourceTower
{
    [SerializeField] protected float intervalGem;
    [SerializeField] protected int produceGemAmount;
    public float CurrentProduceGemInterval;
    protected float produceGemCooldown;
    [Header("RepairButtons")]
    [SerializeField] private Button Repair1;
    [SerializeField] private Button Repair2;
    public override void Initialize()
    {
        base.Initialize();
        CurrentProduceGemInterval = intervalGem;
        Repair1.onClick.AddListener(() => RepairSelf(new Price(100, 0)));
        Repair2.onClick.AddListener(() => RepairSelf(new Price(0, 5)));
        StartCoroutine(SelfDamageCoroutine());
    }
    #region Producing
    public override void Produce()
    {
        if (produceGemCooldown > 0f)
        {
            produceGemCooldown -= Time.deltaTime;
        }
        if (produceGemCooldown <= 0f)
        {
            GetGem();
            produceGemCooldown = CurrentProduceGemInterval; // ÖØÖÃÀäÈ´Ê±¼ä
        }
        base.Produce();
    }
    public void GetGem()
    {
        GameManager.Instance.AddGem(produceGemAmount);
    }
    #endregion
    public override void Die()
    {
        GameManager.Instance.GameOver();
        base.Die();
    }
    protected override void UpdateHealthBar()
    {
        hpBar.fillAmount = CurrentHealth / MaxHealth;
        hpBar.color = CurrentHealth/MaxHealth < 0.25f? Color.red : Color.white;
    }

    #region SelfDamaging
    IEnumerator SelfDamageCoroutine()
    {
        while (CurrentHealth>0f)
        {
            float RDamage = Random.Range(-5f, 5f);
            float RTime = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(2 +RTime);
            LoseHealth(10f + RDamage);
        }
    }
    public void LoseHealth(float dmg)
    {
        CurrentHealth -= dmg;
        OnHealthChange?.Invoke();
        if (CurrentHealth < 0.25 * MaxHealth)
            AudioManager.Instance.Play("HPAlarm");
        if (CurrentHealth < 0) SelfDemolish();
    }
    public override void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
        OnHealthChange?.Invoke();
        AudioManager.Instance.Play("TowerHit");
        if (CurrentHealth < 0.25 * MaxHealth)
            AudioManager.Instance.Play("HPAlarm");
        if (CurrentHealth < 0) SelfDemolish();
    }
    #endregion
    #region Repair
    public void RepairSelf(Price p)
    {
        if (GameManager.Instance.CanAffordPrice(p))
        {
            GameManager.Instance.AffordPrice(p);
            AddHealth(100);
            AudioManager.Instance.Play("CollectorRepairing");
        }
    }
    public void AddHealth(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        UpdateHealthBar();
    }
    #endregion
}
