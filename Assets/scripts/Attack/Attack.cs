using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private IAttack attack;

    public void AttackEnemyMelle(GameObject enemyTarget, bool contrAttack)
    {
        attack.Attack(enemyTarget, contrAttack);
    }
}
