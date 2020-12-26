using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerMov : BaseUnits , IPunObservable
{
    [SerializeField]
    private bool hod; //первый ход после которого он будет по одной клетке ходить
    int[] idGrisAttack;
    public ParticleSystem Aura;
    healthBar healthBar;
    private int CountMove; //кол во ходов при режиме берсерк
    protected override void Start()
    {
        idGrisAttack = new int[2];
        healthBar = GetComponent<healthBar>();
        base.Start();
        hod = false;
        CountMove = 5;
    }

    protected override void gridsHaveEnemy(int[] idGrids)
    {
        base.gridsHaveEnemy(idGrisAttack);
    }

    protected override void getPoint(int[] idCell)
    {        
        if (PhotonNetwork.IsMasterClient) {
            if (idCell[1] == 1)
            {
                hod = false;
            }
            else
                hod = true;
        }
        else { 
            if (idCell[1] == 6)
            {
                hod = false;
            }
            else
                hod = true;
        }
       
        idGrisAttack[0] = idCell[0] - Radius;
        idGrisAttack[1] = idCell[1] - Radius;
        idForBrush[0] = idCell[0];
        idForBrush[1] = idCell[1];
        idForBrush[0] -= Radius;
        if (!hod) return;                
        idForBrush[1] -= Radius;
    }

    public override void grids()
    {
        if (!hod)
        {          
            if (PhotonNetwork.IsMasterClient) // все это нужно для начально старта 
                base.grids();
            else
            {
                if (!PlayerTurn.CanPlay) return;
                for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
                {
                    for (int j = 0; j < MoveCell; j++)
                    {
                        if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] - j}") == null)
                        {
                            continue;
                            // print($"{transform.name}x:{i} z:{j}");
                        }
                        else
                            GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] - j}").SendMessage("GridGreen");

                    }
                }
                
            }
        }
        else
            base.grids(); 
    }

    public override void MovePoint(Transform point)
    {
        if (Aura.isPlaying)
        {
            CountMove -= 1;
            if (CountMove <= 0)
                healthBar.health -= 150;
        }
        base.MovePoint(point);
    }

    override public void hideGrids()
    {
        if (photon.IsMine)
        {
            if (!hod)
            {                               
                if (PhotonNetwork.IsMasterClient)
                {
                    for (int i = 0; i < MoveCell; i++)
                    {
                        for (int j = 0; j < MoveCell; j++)
                        {
                            if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") != null)
                            { // чтоб закрыть зеление клеки
                                GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("hideGrids");
                            }
                        }
                    }
                }
                else
                {                    
                    for (int i = 0; i < MoveCell; i++)
                    {
                        for (int j = 0; j < MoveCell; j++)
                        {
                            if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] - j}") != null)
                            { // чтоб закрыть зеление клеки
                                GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] - j}").SendMessage("hideGrids");
                            }
                        }
                    }                    
                }
                
            }
            else
            {
                for (int i = 0; i < MoveCell; i++)
                {
                    for (int j = 0; j < MoveCell; j++)
                    {
                        if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") != null)
                        { // чтоб закрыть зеление клеки
                            GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("hideGrids");
                        }
                    }
                }
            }
          
            detect = false;
          
        }
    }

    public void UpKnigth()
    {
        Radius = 2;
        MoveCell = 5;
        damage = 100;
        Aura.Play();
    }
   
    
    public override void attack(GameObject enemyTarget , bool contrAttack)
    {
        enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage,this.GetType(),transform);        
        animator.SetTrigger("Attack");               
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            Vector3[] content = new Vector3[] { transform.position, enemyTarget.transform.position };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);            
            if (contrAttack) return;
            gridsHaveEnemy(idGrisAttack);
            EnemyMove();
        }        
        // print("trueAtack");
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
