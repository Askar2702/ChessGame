using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAbility : MonoBehaviour, IMagicAbility
{
    [SerializeField] private Transform posCollider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 scale;
    [SerializeField] private int damage;
    private UnitManager unitManager;
    private GameObject Targets;
    private PhotonView photon;
    private healthBar health;
    bool IsMagicCast;
    private Animator animator;
    private void Awake()
    {
        unitManager = GetComponent<UnitManager>();
        animator = GetComponent<Animator>();
        photon = GetComponent<PhotonView>();
        health = GetComponent<healthBar>();
    }
    private void Start()
    {
        unitManager._notify += CanMagic;
    }
    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);

        foreach (var Currentenemy in hitColliders)
        {

            if (IsMagicCast && animator.GetInteger("State") == 1)
            {
                Currentenemy.GetComponent<UnitManager>().effectOn();
            }
            else
            {
                Currentenemy.GetComponent<UnitManager>().effectOff();
            }
            if (Currentenemy.GetComponent<BaseUnits>())
                Targets = Currentenemy.gameObject;
        }
        if (animator.GetInteger("State") == 2)
            IsMagicCast = false;
    }
    public void Ability_1()
    {
        Debug.Log("Health");
        if (photon.IsMine)
        {
            IsMagicCast = false;
            if (!Targets) return;
        }
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            Currentenemy.GetComponent<healthBar>().healthPlayer();
            animator.SetTrigger("Health");
        }
    }

    public void Ability_2()
    {
        if (photon.IsMine)
        {
            IsMagicCast = false;
            if (!Targets) return;
        }
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            Currentenemy.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
            animator.SetTrigger("fire");
        }
    }

    public void Ability_3()
    {
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, new Vector3(0.5f, 0.5f, 0.5f), posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (!Currentenemy.GetComponent<Passive>()) return;
            Currentenemy.GetComponent<Passive>().UpKnigth();
            animator.SetTrigger("Health"); // та ж анимация что и при лечении             
            if (photon.IsMine) IsMagicCast = false;
            health.health -= 150;
            break;
        }
    }
    
    private void CanMagic(bool activ)
    {
        IsMagicCast = activ;
    }

    private void OnDestroy()
    {
        unitManager._notify -= CanMagic;
    }
}
