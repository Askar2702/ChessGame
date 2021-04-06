using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnGrids : BaseUnits, IPLayerGrid
{

    [SerializeField] private int[] _idGrisAttack; // чисто посмотреть что за клетка под ним
    [SerializeField]
    private bool isTurn; //первый ход после которого он будет по одной клетке ходить
    
    private AttackeMelle attackeMelle;

    protected override void Awake()
    {
        base.Awake();
        attackeMelle = GetComponent<AttackeMelle>();
        _idGrisAttack = new int[2];
        isTurn = false;
    }


    public void GetPoint(int[] idCell)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (idCell[1] == 1)
            {
                isTurn = false;
            }
            else
                isTurn = true;
        }
        else
        {
            if (idCell[1] == 6)
            {
                isTurn = false;
            }
            else
                isTurn = true;
        }

        _idGrisAttack[0] = idCell[0] - 1;
        _idGrisAttack[1] = idCell[1] - 1;
        IdForBrush[0] = idCell[0];
        IdForBrush[1] = idCell[1];
        IdForBrush[0] -= _radius;
        if (!isTurn) return;
        IdForBrush[1] -= _radius;
    }

    public void Grids()
    {
        if (attackeMelle.CountMove <= 0) return;
        if (!isTurn)
        {
            if (PhotonNetwork.IsMasterClient) // все это нужно для начально старта 
                grids();
            else
            {
                if (!PlayerTurn.isCanPlay) return;
                for (int i = 0; i < _moveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
                {
                    for (int j = 0; j < _moveCell; j++)
                    {
                        if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] - j}") == null)
                        {
                            continue;
                            // print($"{transform.name}x:{i} z:{j}");
                        }
                        else
                            _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] - j}").GridGreen();

                    }
                }

            }
        }
        else
            grids();
    }

    public void GridsHaveEnemy()
    {
        if (!isdetect)
        {
            for (int i = 0; i < 3; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameObject.Find($"x:{_idGrisAttack[0] + i} z:{_idGrisAttack[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        GameObject.Find($"x:{_idGrisAttack[0] + i} z:{_idGrisAttack[1] + j}").SendMessage("haveEnemy");
                    }

                }
            }
            isdetect = true;
        }
        else
        {
            HideGrids();
            isdetect = false;
        }
    }

    public void HideGrids()
    {
        if (_photon.IsMine)
        {
            if (!isTurn)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    for (int i = 0; i < _moveCell; i++)
                    {
                        for (int j = 0; j < _moveCell; j++)
                        {
                            if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") != null)
                            { // чтоб закрыть зеление клеки
                                _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").hideGrids();
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _moveCell; i++)
                    {
                        for (int j = 0; j < _moveCell; j++)
                        {
                            if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] - j}") != null)
                            { // чтоб закрыть зеление клеки
                                _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] - j}").hideGrids();
                            }
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < _moveCell; i++)
                {
                    for (int j = 0; j < _moveCell; j++)
                    {
                        if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") != null)
                        { // чтоб закрыть зеление клеки
                            _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").hideGrids();
                        }
                    }
                }
            }

            isdetect = false;
        }
    }

    private void grids()
    {
        if (!PlayerTurn.isCanPlay) return;
        for (int i = 0; i < _moveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        {
            for (int j = 0; j < _moveCell; j++)
            {
                if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") == null)
                {
                    continue;
                    // print($"{transform.name}x:{i} z:{j}");
                }
                else
                    _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").GridGreen();

            }
        }
    }

    public void SetRadius(int _radius , int _moveCell)
    {
        base._radius = _radius;
        base._moveCell = _moveCell;
    }
}
