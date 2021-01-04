using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnGrids : BaseUnits, IPLayerGrid
{

    public int[] idGrisAttack;
    [SerializeField]
    private bool hod; //первый ход после которого он будет по одной клетке ходить
    
    private AttackeMelle attackeMelle;

    protected override void Awake()
    {
        base.Awake();
        attackeMelle = GetComponent<AttackeMelle>();
        idGrisAttack = new int[2];
        hod = false;
    }


    public void GetPoint(int[] idCell)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (idCell[1] == 1)
            {
                hod = false;
            }
            else
                hod = true;
        }
        else
        {
            if (idCell[1] == 6)
            {
                hod = false;
            }
            else
                hod = true;
        }

        idGrisAttack[0] = idCell[0] - 1;
        idGrisAttack[1] = idCell[1] - 1;
        idForBrush[0] = idCell[0];
        idForBrush[1] = idCell[1];
        idForBrush[0] -= Radius;
        if (!hod) return;
        idForBrush[1] -= Radius;
    }

    public void Grids()
    {
        if (attackeMelle.CountMove <= 0) return;
        if (!hod)
        {
            if (PhotonNetwork.IsMasterClient) // все это нужно для начально старта 
                grids();
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
            grids();
    }

    public void GridsHaveEnemy(int[] idGrids)
    {
        if (!detect)
        {
            for (int i = 0; i < 3; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameObject.Find($"x:{idGrisAttack[0] + i} z:{idGrisAttack[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        GameObject.Find($"x:{idGrisAttack[0] + i} z:{idGrisAttack[1] + j}").SendMessage("haveEnemy");
                    }

                }
            }
            detect = true;
        }
        else
        {
            HideGrids();
            detect = false;
        }
    }

    public void HideGrids()
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

    private void grids()
    {
        if (!PlayerTurn.CanPlay) return;
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        {
            for (int j = 0; j < MoveCell; j++)
            {
                if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                {
                    continue;
                    // print($"{transform.name}x:{i} z:{j}");
                }
                else
                    GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("GridGreen");

            }
        }
    }

    public void SetRadius(int _radius , int _moveCell)
    {
        Radius = _radius;
        MoveCell = _moveCell;
    }
}
