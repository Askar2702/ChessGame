﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minister : BaseUnits , IPunObservable
{
    public ParticleSystem WhiteKing;
    public ParticleSystem DarkKing;
   
    protected override void Start()
    {
        base.Start();
        if (!photon.IsMine)
        {
            DarkKing.Play();
        }
        else
            WhiteKing.Play();
        
    }

    public override void grids()
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

    public override void attack(GameObject enemyTarget, bool contrAttack)
    {
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
            if (contrAttack) return;
            gridsHaveEnemy(idForBrush);
            EnemyMove();
        }
        // print("trueAtack");
    }
    protected override void gridsHaveEnemy(int[] idGrids)
    {
        idForBrush[0] -= 1;
        idForBrush[1] -= 1;
        if (!detect)
        {
            detect = true;
            for (int i = 0; i < 3; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").GetComponent<gridsPrefab>().HaveEnemy)
                        {
                            GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("haveEnemy");
                        }
                    }
                    print($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}");
                }
            }
        }
        else
        {
            detect = false;
            for (int i = 0; i < 3; i++) //закрывает клетки
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                        GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("hideGrids");
                }
            }
        }

    }
    override public void hideGrids()
    {
        if (photon.IsMine)
        {// использую цифры вместо moveCall потому что тот 3 а тут 6 чтоб быстро закрыть и меньше кода
            idForBrush[0] -= 10;
            idForBrush[1] -= 10;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
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