using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinisterGrids : BaseUnits, IPLayerGrid
{
    public void GetPoint(int[] idCell)
    {
        IdForBrush[0] = idCell[0];
        IdForBrush[1] = idCell[1];
        IdForBrush[0] -= _radius;
        IdForBrush[1] -= _radius;
    }

    public void Grids()
    {
        if (!PlayerTurn.isCanPlay) return;
        HorizAndVertical();
        Diagonal();
    }

    public void GridsHaveEnemy()
    {
        IdForBrush[0] -= 1;
        IdForBrush[1] -= 1;
        if (!isdetect)
        {
            isdetect = true;
            for (int i = 0; i < 3; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").HaveEnemy)
                        {
                            _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").haveEnemy();
                        }
                    }
                    print($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}");
                }
            }
        }
        else
        {
            isdetect = false;
            for (int i = 0; i < 3; i++) //закрывает клетки
            {
                for (int j = 0; j < 3; j++)
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

    public void HideGrids()
    {
        if (_photon.IsMine)
        {// использую цифры вместо moveCall потому что тот 3 а тут 6 чтоб быстро закрыть и меньше кода
            IdForBrush[0] -= 10;
            IdForBrush[1] -= 10;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}") != null)
                    { // чтоб закрыть зеление клеки
                        _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + j}").hideGrids();
                    }
                }
            }
            //  menuBar.SetActive(false);
            isdetect = false;

        }
    }

    private void HorizAndVertical()
    {
        for (int i = 0; i < _moveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1]}") == null)
            {
                continue;
            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1]}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1]}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1]}").GridGreen();

        }

        for (int j = 0; j < _moveCell; j++)
        { // вверх ищет дорогу
            if (_listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] + j}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] + j}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] + j}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] + j}").GridGreen();
        }

        for (int i = 0; i < _moveCell; i++)
        { // влево ищет дорогу
            if (_listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1]}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1]}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1]}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1]}").GridGreen();
        }

        for (int j = 0; j < _moveCell; j++)
        { // вниз ищет дорогу
            if (_listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] - j}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] - j}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] - j}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0]} z:{IdForBrush[1] - j}").GridGreen();

        }
    }
    // чтоб мог как и слон ходить
    private void Diagonal()
    {
        for (int i = 0; i < _moveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if(_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + i}") == null)
            {
                continue;
            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + i}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + i}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0] + i} z:{IdForBrush[1] + i}").GridGreen();

        }

        for (int j = 0; j < _moveCell; j++)
        { // вверх влево ищет дорогу
            if (_listGrid.GrisItem($"x:{IdForBrush[0] - j} z:{IdForBrush[1] + j}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0] - j} z:{IdForBrush[1] + j}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0] - j} z:{IdForBrush[1] + j}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0] - j} z:{IdForBrush[1] + j}").GridGreen();
        }

        for (int i = 0; i < _moveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (_listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1] - i}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1] - i}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1] - i}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0] - i} z:{IdForBrush[1] - i}").GridGreen();
        }

        for (int j = 0; j < _moveCell; j++)
        { // влево низ ищет дорогу
            if (_listGrid.GrisItem($"x:{IdForBrush[0] + j} z:{IdForBrush[1] - j}") == null)
            {
                continue;

            }
            if (_listGrid.GrisItem($"x:{IdForBrush[0] + j} z:{IdForBrush[1] - j}").HavePlayer
                || _listGrid.GrisItem($"x:{IdForBrush[0] + j} z:{IdForBrush[1] - j}").HaveEnemy)
                break;
            else
                _listGrid.GrisItem($"x:{IdForBrush[0] + j} z:{IdForBrush[1] - j}").GridGreen();

        }
    }
}
