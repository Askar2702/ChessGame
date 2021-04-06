using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private IAttack _attack;

    public void AttackEnemyMelle(GameObject enemyTarget, bool contrAttack)
    {
        _attack.Attack(enemyTarget, contrAttack);
    }
}
