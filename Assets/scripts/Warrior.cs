using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Warrior : BaseUnits 
{
    public int radiusMove;    // это радиус ходьбы вместо родительского который был и для драки и для ходьбы
    private int[] move; // для его ходьбы  сохраняет его позицию для ходьбы вместо родительского который для боя нужен    
    
   
    protected  void Start()
    {
        
        var child = transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
        child.center = new Vector3(0, 1, 0);
        child.size = new Vector3(4, 1, 4);
        child.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // включает пассивку пешек        
        if (other.tag == transform.tag && other.GetComponent<Passive>())
        {
            other.transform.GetComponent<Passive>().DamagPlus();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // выключает пассивку пешек когда двигается
        if (other.tag == transform.tag && other.GetComponent<Passive>())
        {
            other.GetComponent<Passive>().DamagMinus();          
        }
    }


    public  void grids()
    {
        

    }

    public  void hideGrids()
    {
        
    }


    public  void attack(GameObject enemyTarget, bool contrAttack)
    {/*
        enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
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

    protected  void _UpadateProcess()
    {
      /*  Ray ray = new Ray(startRay.position, -transform.up);
        RaycastHit hit;
        figthBTN.transform.position = transform.position + pos;//для кнопки иначе она не пашет нормально

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.tag == "grid")
            {
                getPoint(hit.transform.GetComponent<gridsPrefab>().Id); //луч который оперделяет место нахождение   
                move = hit.transform.GetComponent<gridsPrefab>().Id;
                if (photon.IsMine)
                    PlayerSignal(hit.transform.GetComponent<gridsPrefab>().Id);
                if (!photon.IsMine)
                {
                    enemySignal(hit.transform.GetComponent<gridsPrefab>().Id);//луч который делает красным там где он есть если она сам враг
                    if (hit.transform.GetComponent<gridsPrefab>().mat.material.GetColor("_EmissionColor") == Color.red * 1.3f)
                    {
                        figthBTN.gameObject.SetActive(true);
                    }
                    else
                        figthBTN.gameObject.SetActive(false);
                }

                // Debug.Log(hit.transform.tag);
            }

            Debug.DrawRay(startRay.position, -transform.up, Color.red, 20);
        }
        if (PhotonNetwork.IsMasterClient)
            figthBTN.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            figthBTN.transform.rotation = Quaternion.Euler(0, 180f, 0);*/
    }

    
}
