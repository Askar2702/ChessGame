using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class King : BaseUnits 
{
    public ParticleSystem WhiteKing;
    public ParticleSystem DarkKing;
    bool isSignalDeath;
    protected  void Start()
    {
        
        if (!photon.IsMine)
        {           
            DarkKing.Play();
        }
        else
            WhiteKing.Play();
        isSignalDeath = false;
        
    }
     void Update()
    {/*
       
        if (!Alive)      
        {            
            if (!isSignalDeath)
            {
                DeathSignal();               
            }
        }*/
    }
    
    private void DeathSignal()
    {
        if (photon.IsMine)
        {
            GameObject.Find("OnlineManager").SendMessage("finished", false);            
        }
        else
            GameObject.Find("OnlineManager").SendMessage("finished", true);
        isSignalDeath = true;        
    }


     public void hideGrids()
    {
        if (photon.IsMine)
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
            //  menuBar.SetActive(false);
            detect = false;

        }
    }



    public  void attack(GameObject enemyTarget , bool contrAttack)
    {
      /*  enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(),transform);
        animator.SetTrigger("Attack");
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            Vector3[] content = new Vector3[] { transform.position, enemyTarget.transform.position };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            gridsHaveEnemy(idForBrush);
            EnemyMove();
        }*/
    }


   

    
}
