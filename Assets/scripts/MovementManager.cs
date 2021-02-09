using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour , IPunObservable
{
    private NavMeshAgent agent;
    private UnitManager unitManager;
    private Animator animator;
    private PhotonView photon;
    private bool direction;
    public event Action OnChangestate;
    private int state;// для анимации какая будет играть 
    private int animState
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            if(state == 2)
                OnChangestate?.Invoke();
        }
    }

    public Transform enemy { get; set; }
    public Vector3 target { get; set; }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient) direction = true;
        else direction = false;
        unitManager = GetComponent<UnitManager>();
        photon = GetComponent<PhotonView>();
        target = transform.position;
        transform.position = target;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (unitManager.Alive)
        {

            if (enemy == null) return; // здесь идет поворот к врагу потом при движении враг обнуляется
                                       // Determine which direction to rotate towards
            Vector3 targetDirection = enemy.position - transform.position;
            // The step size is equal to speed times frame time.
            float singleStep = 10f * Time.deltaTime;
            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

        }
    }
    protected void FixedUpdate()
    {
        if (unitManager.Alive)
        {
            if (Vector3.Distance(transform.position, target) > 0.2f)
            {
                agent.SetDestination(target);
                unitManager.playerState = PlayerState.Movement;
                enemy = null; // чтоб не смотрел на врага
            }
            else if (Vector3.Distance(transform.position, target) < 0.2f || transform.position == target)
            {
                animState = 1;
                PositionCentralization(); //выравнивание позиции 
            }
            if (animator.GetInteger("State") == animState) return;
            animator.SetInteger("State", animState);
        }

    }

    public virtual void MovePoint(Transform point)
    {
        target = point.GetChild(0).transform.position;
        animState = 2;
        if (photon.IsMine)
            PlayerTurn.CanPlay = false;
    }


    

    void PositionCentralization()
    {
        unitManager.playerState = PlayerState.Idle;
        if (transform.position == target) return;
        transform.position = target;
        if (photon.IsMine)
        {
            if (enemy != null) return;
            if (direction)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }


    }

    public void BackMove()
    {
        animator.SetTrigger("Back");
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(target);
            stream.SendNext(state);
        }
        else
        {
            target = (Vector3)stream.ReceiveNext();
            state = (int)stream.ReceiveNext();
        }
    }
}
