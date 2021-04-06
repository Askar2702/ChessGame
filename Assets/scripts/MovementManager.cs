using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour , IPunObservable
{
    public event Action OnChangestate;
    [SerializeField] ParticleSystem _particleFire;
    private NavMeshAgent _agent;
    private UnitManager _unitManager;
    private Animator _animator;
    private PhotonView _photon;
    private bool isFirstMove;
    private int _state;// для анимации какая будет играть 
    private int _animState
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            if(_state == 2)
                OnChangestate?.Invoke();
        }
    }

    public Transform enemy { get; set; }
    public Vector3 target { get; set; }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient) isFirstMove = true;
        else isFirstMove = false;
        _unitManager = GetComponent<UnitManager>();
        _photon = GetComponent<PhotonView>();
        target = transform.position;
        transform.position = target;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_unitManager.isAlive)
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
        if (_unitManager.isAlive)
        {
            if (Vector3.Distance(transform.position, target) > 0.2f)
            {
                _agent.SetDestination(target);
                _unitManager.ChangetStatusPlayer(PlayerState.Movement);
                enemy = null; // чтоб не смотрел на врага
            }
            else if (Vector3.Distance(transform.position, target) < 0.2f || transform.position == target)
            {
                _animState = 1;
                PositionCentralization(); //выравнивание позиции 
            }
            if (_animator.GetInteger("State") == _animState) return;
            _animator.SetInteger("State", _animState);
        }

    }

    public virtual void MovePoint(Transform point)
    {
        target = point.GetChild(0).transform.position;
        _animState = 2;
        if (_photon.IsMine)
            PlayerTurn.isCanPlay = false;
    }


    

    void PositionCentralization()
    {
        _unitManager.ChangetStatusPlayer(PlayerState.Idle);
        if (transform.position == target) return;
        transform.position = target;
        if (_photon.IsMine)
        {
            if (enemy != null) return;
            if (isFirstMove)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }


    }

    public void BackMove()
    {
        _animator.SetTrigger("Back");
        _particleFire.Play();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(target);
            stream.SendNext(_state);
        }
        else
        {
            target = (Vector3)stream.ReceiveNext();
            _state = (int)stream.ReceiveNext();
        }
    }
}
