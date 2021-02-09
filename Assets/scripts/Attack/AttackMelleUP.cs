using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelleUP : MonoBehaviour, IAttack
{
    [SerializeField] private int damage;
    [SerializeField] private UnitManager unitManager;
    private PhotonView photon;
    private MovementManager Movement;
    private Animator animator;
    private void Awake()
    {
        photon = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        Movement = GetComponent<MovementManager>();
    }

    public void Attack(GameObject enemyTarget, bool contrAttack)
    {
        Movement.enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
        animator.SetTrigger("Attack");
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            int[] content = new int[] { transform.GetComponent<UnitManager>()._id, enemyTarget.GetComponent<UnitManager>()._id };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            unitManager.DetectEnemy();
            unitManager.EnemyMove();
        }
    }
}
