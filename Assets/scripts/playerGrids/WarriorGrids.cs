using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorGrids : BaseUnits, IPLayerGrid
{

    public int radiusMove;    // это радиус ходьбы вместо родительского который был и для драки и для ходьбы
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
        for (int i = 0; i < radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + i}") == null)
            {
                continue;
            }
            if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + i}").HavePlayer
                || listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + i}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + i}").GridGreen();

        }

        for (int j = 0; j < radiusMove; j++)
        { // вверх влево ищет дорогу
            if (listGrid.GrisItem($"x:{move[0] - j} z:{move[1] + j}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{move[0] - j} z:{move[1] + j}").HavePlayer
                || listGrid.GrisItem($"x:{move[0] - j} z:{move[1] + j}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{move[0] - j} z:{move[1] + j}").GridGreen();
        }

        for (int i = 0; i < radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - i}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - i}").HavePlayer
                || listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - i}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - i}").GridGreen();
        }

        for (int j = 0; j < radiusMove; j++)
        { // влево низ ищет дорогу
            if (listGrid.GrisItem($"x:{move[0] + j} z:{move[1] - j}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{move[0] + j} z:{move[1] - j}").HavePlayer
                || listGrid.GrisItem($"x:{move[0] + j} z:{move[1] - j}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{move[0] + j} z:{move[1] - j}").GridGreen();

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
            for (int i = 0; i < radiusMove; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    listGrid.GrisItem($"x:{move[0] + i} z:{move[1] + i}").hideGrids();
                }
            }
            for (int j = 0; j < radiusMove; j++)
            {
                if (listGrid.GrisItem($"x:{move[0] + j} z:{move[1] - j}") != null)
                { // чтоб закрыть зеление клеки
                    listGrid.GrisItem($"x:{move[0] + j} z:{move[1] - j}").hideGrids();
                }
            }
            for (int i = 0; i < radiusMove; i++)
            {
                if (listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - i}") != null)
                { // чтоб закрыть зеление клеки
                    listGrid.GrisItem($"x:{move[0] - i} z:{move[1] - i}").hideGrids();
                }
            }
            for (int j = 0; j < radiusMove; j++)
            {
                if (listGrid.GrisItem($"x:{move[0] - j} z:{move[1] + j}") != null)
                { // чтоб закрыть зеление клеки
                    listGrid.GrisItem($"x:{move[0] - j} z:{move[1] + j}").hideGrids();
                }
            }
            if (detect)
            {
                detect = false;
                for (int i = 0; i < MoveCell; i++) //закрывает клетки с врагами
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
