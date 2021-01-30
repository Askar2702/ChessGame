using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackeMelle : MonoBehaviour, IAttack
{
    public int damage;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private PawnGrids pawnGrids;
    private PhotonView photon;
    private MovementManager Movement;
    private Animator animator;
    public int CountMove { get; private set; } //кол во ходов при режиме берсерк
    private void Awake()
    {
        photon = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        unitManager = GetComponent<UnitManager>();
        Movement = GetComponent<MovementManager>();
        Movement.OnChangestate += MinusCount;
        CountMove = 5;
    }

    public void Attack(GameObject enemyTarget, bool contrAttack)
    {
        Movement.enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
        animator.SetTrigger("Attack");
        if (CountMove <= 0)
            unitManager.Alive = false;
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            Vector3[] content = new Vector3[] { transform.position, enemyTarget.transform.position };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            if (contrAttack) return;
            unitManager.DetectEnemy();
            unitManager.EnemyMove();
        }
    }

    private void MinusCount()
    {
        if (damage == 150) 
            CountMove--;
    }
}
