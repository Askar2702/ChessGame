using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGrids : BaseUnits, IPLayerGrid
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
            //  menuBar.SetActive(false);
            isdetect = false;

        }
    }
}
