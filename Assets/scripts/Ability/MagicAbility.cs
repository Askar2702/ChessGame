using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAbility : MonoBehaviour, IMagicAbility
{
    [SerializeField] private Transform _posCollider;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private int _damage;
    private UnitManager _unitManager;
    private GameObject _targets;
    private PhotonView _photon;
    private healthBar _health;
    bool isMagicCast;
    private Animator _animator;
    private void Awake()
    {
        _unitManager = GetComponent<UnitManager>();
        _animator = GetComponent<Animator>();
        _photon = GetComponent<PhotonView>();
        _health = GetComponent<healthBar>();
    }
    private void Start()
    {
        _unitManager._notify += CanMagic;
    }
    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapBox(_posCollider.position, _scale, _posCollider.rotation, _layerMask);

        foreach (var Currentenemy in hitColliders)
        {

            if (isMagicCast && _animator.GetInteger("State") == 1)
            {
                Currentenemy.GetComponent<UnitManager>().effectOn();
            }
            else
            {
                Currentenemy.GetComponent<UnitManager>().effectOff();
            }
            if (Currentenemy.GetComponent<BaseUnits>())
                _targets = Currentenemy.gameObject;
        }
        if (_animator.GetInteger("State") == 2)
            isMagicCast = false;
    }
    public void Ability_1()
    {
        Debug.Log("Health");
        if (_photon.IsMine)
        {
            isMagicCast = false;
            if (!_targets) return;
        }
        Collider[] hitColliders = Physics.OverlapBox(_posCollider.position, _scale, _posCollider.rotation, _layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            Currentenemy.GetComponent<healthBar>().healthPlayer();
            _animator.SetTrigger("Health");
        }
    }

    public void Ability_2()
    {
        if (_photon.IsMine)
        {
            isMagicCast = false;
            if (!_targets) return;
        }
        Collider[] hitColliders = Physics.OverlapBox(_posCollider.position, _scale, _posCollider.rotation, _layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            Currentenemy.GetComponent<healthBar>().TakeDamage(_damage, this.GetType(), transform);
            _animator.SetTrigger("fire");
        }
    }

    public void Ability_3()
    {
        Collider[] hitColliders = Physics.OverlapBox(_posCollider.position, new Vector3(0.5f, 0.5f, 0.5f), _posCollider.rotation, _layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (!Currentenemy.GetComponent<Passive>()) return;
            Currentenemy.GetComponent<Passive>().UpKnigth();
            _animator.SetTrigger("Health"); // та ж анимация что и при лечении             
            if (_photon.IsMine) isMagicCast = false;
            _health._health -= 150;
            break;
        }
    }
    
    private void CanMagic(bool activ)
    {
        isMagicCast = activ;
    }

    private void OnDestroy()
    {
        _unitManager._notify -= CanMagic;
    }
}
