using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackRange : MonoBehaviour
{
    [SerializeField] private Monster owner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Tower") && owner.State == MonsterState.Moving)
        {
            owner.SetTargetTower(other.GetComponent<Tower>());
        }
    }
}
