using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMove : BaseUnits 
{

    public Transform posCollider;
    public LayerMask layerMask;
    private GameObject Targets;
    bool MagicCast;
    public Vector3 scale;

    private void Start()
    {
        MagicCast = true;
    }
    private  void Update()
    {      
       /* Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);

        foreach (var Currentenemy in hitColliders)
        {

            if (MagicCast && animator.GetInteger("State") == 1)
            {
                Currentenemy.GetComponent<BaseUnits>().effectOn();
            }
            else
            {
                Currentenemy.GetComponent<BaseUnits>().effectOff();
            }
            if (Currentenemy.GetComponent<BaseUnits>())
                Targets = Currentenemy.gameObject;
        }
        if (animator.GetInteger("State") == 2)
            MagicCast = false;*/
    }
    public void ClickAttack()
    {
        if (!Targets) return;
        attackEnemy();
        Events(transform.position, "attackEnemy");
        EnemyMove();        
    }
    public void attackEnemy()
    {
       /* Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation,layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            Currentenemy.GetComponent<healthBar>().TakeDamage(damage, this.GetType(),transform);
            animator.SetTrigger("fire");
        }*/
    }

  
    public void ClickHealth()
    {
        if (!Targets) return;
        PlayerHealt();
        Events(transform.position, "PlayerHealt");        
        EnemyMove();
    }

    public void PlayerHealt()
    {
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation,layerMask); 
        foreach (var Currentenemy in hitColliders)
        {
            Currentenemy.GetComponent<healthBar>().healthPlayer();
            //animator.SetTrigger("Health");
        }
    }

    public void ClickUpPower()
    {
        UpPower();
        Events(transform.position, "UpPower");               
    }
    public void UpPower()
    {
       /* print("gazaru");
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, new Vector3(0.5f,0.5f,0.5f), posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (!Currentenemy.GetComponent<playerMov>()) return;
            Currentenemy.GetComponent<playerMov>().UpKnigth();
            animator.SetTrigger("Health"); // та ж анимация что и при лечении             
            if (photon.IsMine) EnemyMove();            
            health.health -= 150;
            break;
        }*/
       
    }
    public  void grids()
    {
        if (!PlayerTurn.CanPlay) return;
        HorizAndVertical();
        Diagonal();
    }
    private void HorizAndVertical()
    {       
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1]}") == null)
            {
                continue;
            }
            if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1]}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1]}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1]}").SendMessage("GridGreen");

        }

        for (int j = 0; j < MoveCell; j++)
        { // вверх ищет дорогу
            if (GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] + j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] + j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] + j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] + j}").SendMessage("GridGreen");
        }

        for (int i = 0; i < MoveCell; i++)
        { // влево ищет дорогу
            if (GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1]}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1]}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1]}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1]}").SendMessage("GridGreen");
        }

        for (int j = 0; j < MoveCell; j++)
        { // вниз ищет дорогу
            if (GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] - j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] - j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] - j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0]} z:{idForBrush[1] - j}").SendMessage("GridGreen");

        }
    }
    // чтоб мог как и слон ходить
    private void Diagonal()
    {
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}") == null)
            {
                continue;
            }
            if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}").SendMessage("GridGreen");

        }

        for (int j = 0; j < MoveCell; j++)
        { // вверх влево ищет дорогу
            if (GameObject.Find($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}").SendMessage("GridGreen");
        }

        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}").SendMessage("GridGreen");
        }

        for (int j = 0; j < MoveCell; j++)
        { // влево низ ищет дорогу
            if (GameObject.Find($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}").SendMessage("GridGreen");

        }
    }
    public void hideGrids()
    {
        if (photon.IsMine)
        {// использую цифры вместо moveCall потому что тот 3 а тут 6 чтоб быстро закрыть и меньше кода
            idForBrush[0] -= 3;
            idForBrush[1] -= 3;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(posCollider.position, scale);
    }

    public  void moveBool()
    {
      /*  if (!Alive) return;
        if (photon.IsMine)
        {
            if (Move)
            {
                menuBar.SetActive(true);
                Move = false;
                gameObject.layer = 12;
                MagicCast = true;
            }
            else
            {
                hideGrids();
                Move = true;
                menuBar.SetActive(false);                
                MagicCast = false;
            }

        }*/
    }
    public  void OffPlayer()
    {
        MagicCast = false;
        gameObject.layer = 9;
    }

    private void Events(Vector3 pos , string Method)
    {
        object[] content = new object[2] { (object)pos, (object)Method }; //  приходиться массивом отправлять иначе просто vector3 он не принимает отправлять
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)2, content, options, sendOptions);
        MagicCast = false;
    }
    public  void EnemyMove()
    {
        MagicCast = false;
    }
    
}
