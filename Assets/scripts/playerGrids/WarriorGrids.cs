using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorGrids : BaseUnits, IPLayerGrid
{

    [SerializeField] private int _radiusMove;    // это радиус ходьбы вместо родительского который был и для драки и для ходьбы
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
        for (int i = 0; i < _radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + i}") == null)
            {
                continue;
            }
            if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + i}").HavePlayer
                || _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + i}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + i}").GridGreen();

        }

        for (int j = 0; j < _radiusMove; j++)
        { // вверх влево ищет дорогу
            if (_listGrid.GrisItem($"x:{_move[0] - j} z:{_move[1] + j}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{_move[0] - j} z:{_move[1] + j}").HavePlayer
                || _listGrid.GrisItem($"x:{_move[0] - j} z:{_move[1] + j}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{_move[0] - j} z:{_move[1] + j}").GridGreen();
        }

        for (int i = 0; i < _radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - i}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - i}").HavePlayer
                || _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - i}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - i}").GridGreen();
        }

        for (int j = 0; j < _radiusMove; j++)
        { // влево низ ищет дорогу
            if (_listGrid.GrisItem($"x:{_move[0] + j} z:{_move[1] - j}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{_move[0] + j} z:{_move[1] - j}").HavePlayer
                || _listGrid.GrisItem($"x:{_move[0] + j} z:{_move[1] - j}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{_move[0] + j} z:{_move[1] - j}").GridGreen();

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
            for (int i = 0; i < _radiusMove; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    _listGrid.GrisItem($"x:{_move[0] + i} z:{_move[1] + i}").hideGrids();
                }
            }
            for (int j = 0; j < _radiusMove; j++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] + j} z:{_move[1] - j}") != null)
                { // чтоб закрыть зеление клеки
                    _listGrid.GrisItem($"x:{_move[0] + j} z:{_move[1] - j}").hideGrids();
                }
            }
            for (int i = 0; i < _radiusMove; i++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - i}") != null)
                { // чтоб закрыть зеление клеки
                    _listGrid.GrisItem($"x:{_move[0] - i} z:{_move[1] - i}").hideGrids();
                }
            }
            for (int j = 0; j < _radiusMove; j++)
            {
                if (_listGrid.GrisItem($"x:{_move[0] - j} z:{_move[1] + j}") != null)
                { // чтоб закрыть зеление клеки
                    _listGrid.GrisItem($"x:{_move[0] - j} z:{_move[1] + j}").hideGrids();
                }
            }
            if (isdetect)
            {
                isdetect = false;
                for (int i = 0; i < _moveCell; i++) //закрывает клетки с врагами
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
