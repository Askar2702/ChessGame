using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinGrids : BaseUnits, IPLayerGrid
{
    private int[] move; // для его ходьбы  сохраняет его позицию для ходьбы вместо родительского который для боя нужен

    public void GetPoint(int[] idCell)
    {
        move = idCell;
        idForBrush[0] = idCell[0];
        idForBrush[1] = idCell[1];
        idForBrush[0] -= Radius;
        idForBrush[1] -= Radius;
    }

    public void Grids()
    {
        if (!PlayerTurn.CanPlay) return;
        for (int i = 1; i < 2; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо в верх
            if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + 2}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо в бок вверх
            if (listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] + i}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] + i}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        { // право в низ
            if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] - 2}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] + i} z:{move[1] - 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        { // право в бок низ
            if (listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] - i}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] - i}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {// влево вверх
            if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] + 2}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] - i} z:{move[1] + 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {// влево вверх бок
            if (listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] + i}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] + i}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {
            // влево в низ
            if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - 2}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - 2}").GridGreen();
        }
        for (int i = 1; i < 2; i++)
        {
            // влево в низ бок
            if (listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] - i}") == null)
            {
                continue;
            }
            else
                listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] - i}").GridGreen();
        }
    }

    public void GridsHaveEnemy()
    {
        if (!detect)
        {
            for (int i = 0; i < MoveCell; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < MoveCell; j++)
                {
                    if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").haveEnemy();
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
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + 2}") != null)
                { // чтоб закрыть зеление клеки
                    listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] + i}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] - 2}") != null)
                {
                    listGrid.GrisItem($"x:{move[0] + i} z:{move[1] - 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] - i}") != null)
                {
                    listGrid.GrisItem($"x:{move[0] + 2} z:{move[1] - i}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] + 2}") != null)
                {
                    listGrid.GrisItem($"x:{move[0] - i} z:{move[1] + 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] + i}") != null)
                {
                    listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] + i}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - 2}") != null)
                {
                    listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - 2}").hideGrids();
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] - i}") != null)
                {
                    listGrid.GrisItem($"x:{move[0] - 2} z:{move[1] - i}").hideGrids();
                }
            }
            if (detect)
            {
                detect = false;
                for (int i = 0; i < MoveCell; i++) //закрывает клетки
                {
                    for (int j = 0; j < MoveCell; j++)
                    {
                        if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                        {
                            continue;
                        }
                        else
                            listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").hideGrids();
                    }
                }
            }
        }
    }
}
