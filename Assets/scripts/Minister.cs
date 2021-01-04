using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minister : BaseUnits 
{
    public ParticleSystem WhiteKing;
    public ParticleSystem DarkKing;
   
    protected  void Start()
    {
        if (!photon.IsMine)
        {
            DarkKing.Play();
        }
        else
            WhiteKing.Play();
        
    }

    public  void grids()
    {
        
    }
    

    public  void attack(GameObject enemyTarget, bool contrAttack)
    {
       /* enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
        animator.SetTrigger("Attack");
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            Vector3[] content = new Vector3[] { transform.position, enemyTarget.transform.position };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            if (contrAttack) return;
            gridsHaveEnemy(idForBrush);
            EnemyMove();
        }
        // print("trueAtack");*/
    }
    protected  void gridsHaveEnemy(int[] idGrids)
    {
        

    }
     public void hideGrids()
    {
        
    }
   
}
