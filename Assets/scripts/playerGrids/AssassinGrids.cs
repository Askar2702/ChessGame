using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinGrids : BaseUnits, IPLayerGrid
{
    private int[] _move; // для его ходьбы  сохраняет его позицию для ходьбы вместо родительского который для боя нужен

    public void GetPoint(int[] idCell)
    {
        _move = idCell;
        IdForBrush[0] = idCell[0];
        IdForBrush[1] = idCell[1];
        IdForBrush[0] -= _radius;
        IdForBrush[1] -= _radius;
    }

    public void Grids()
    {
        if (!PlayerTurn.isCanPlay) return;
        for (int i = 1; i < 2; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо в верх
            if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + 2}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо в бок вверх
            if (_listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] + i}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] + i}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        { // право в низ
            if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] - 2}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] - 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        { // право в бок низ
            if (_listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] - i}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] - i}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {// влево вверх
            if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] + 2}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] + 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {// влево вверх бок
            if (_listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] + i}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] + i}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {
            // влево в низ
            if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - 2}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {
            // влево в низ бок
            if (_listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] - i}") == null)
            {
                continue;
            }
            else
                _listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] - i}").GridGreen();
        }
    }

    public void GridsHaveEnemy()
    {
        if (!isdetect)
        {
            for (int i = 0; i < _moveCell; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < _moveCell; j++)
                {
                    if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").haveEnemy();
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
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + 2}") != null)
                { // чтоб закрыть зеление клеки
                    _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    _listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] + i}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] - 2}") != null)
                {
                    _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] - 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] - i}") != null)
                {
                    _listGrid.GrisItem($"x:{_move[0] + 2} z:{_move[1] - i}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] + 2}") != null)
                {
                    _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] + 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] + i}") != null)
                {
                    _listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] + i}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - 2}") != null)
                {
                    _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] - i}") != null)
                {
                    _listGrid.GrisItem($"x:{_move[0] - 2} z:{_move[1] - i}").hideGrids();
                }
            }
            if (isdetect)
            {
                isdetect = false;
                for (int i = 0; i < _moveCell; i++) //закрывает клетки
                {
                    for (int j = 0; j < _moveCell; j++)
                    {
                        if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") == null)
                        {
                            continue;
                        }
                        else
                            _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").hideGrids();
                    }
                }
            }
        }
    }
}
