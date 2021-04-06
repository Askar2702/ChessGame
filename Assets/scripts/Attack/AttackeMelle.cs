using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackeMelle : MonoBehaviour, IAttack
{
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private PawnGrids _pawnGrids;
    [SerializeField] private int _damage;
 
    private PhotonView _photon;
    private MovementManager _movement;
    private Animator _animator;
    public int Damage => _damage;
    public int CountMove { get; private set; } //кол во ходов при режиме берсерк
    private void Awake()
    {
        _photon = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _unitManager = GetComponent<UnitManager>();
        _movement = GetComponent<MovementManager>();
        _movement.OnChangestate += MinusCount;
        CountMove = 5;
    }

    public void SettingDamage(int damage)
    {
        _damage = damage;
    }

    public void Attack(GameObject enemyTarget, bool contrAttack)
    {
        _movement.enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(Damage, this.GetType(), transform);
        _animator.SetTrigger("Attack");
        if (CountMove <= 0)
            _unitManager.ChangetAlivePlayer(false);
        if (_photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            int[] content = new int[] { transform.GetComponent<UnitManager>().Id, enemyTarget.GetComponent<UnitManager>().Id };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            if (contrAttack) return;
            _unitManager.DetectEnemy();
            _unitManager.EnemyMove();
        }
    }

    private void MinusCount()
    {
        if (Damage == 150) 
            CountMove--;
    }
}
