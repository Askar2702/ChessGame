using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelleUP : MonoBehaviour, IAttack
{
    [SerializeField] private int _damage;
    [SerializeField] private UnitManager _unitManager;
    private PhotonView _photon;
    private MovementManager _movement;
    private Animator _animator;
    private void Awake()
    {
        _photon = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<MovementManager>();
    }

    public void Attack(GameObject enemyTarget, bool contrAttack)
    {
        _movement.enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(_damage, this.GetType(), transform);
        _animator.SetTrigger("Attack");
        if (_photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            int[] content = new int[] { transform.GetComponent<UnitManager>().Id, enemyTarget.GetComponent<UnitManager>().Id };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            _unitManager.DetectEnemy();
            _unitManager.EnemyMove();
        }
    }
}
