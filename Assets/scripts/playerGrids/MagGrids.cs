﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGrids : BaseUnits, IPLayerGrid
{
    public void GetPoint(int[] idCell)
    {
        idForBrush[0] = idCell[0];
        idForBrush[1] = idCell[1];
        idForBrush[0] -= Radius;
        idForBrush[1] -= Radius;
    }

    public void Grids()
    {
        if (!PlayerTurn.CanPlay) return;
        HorizAndVertical();
        Diagonal();
    }

    public void GridsHaveEnemy()
    {
        Debug.Log("//");
    }

    public void HideGrids()
    {
        if (photon.IsMine)
        {// использую цифры вместо moveCall потому что тот 3 а тут 6 чтоб быстро закрыть и меньше кода
            idForBrush[0] -= 3;
            idForBrush[1] -= 3;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") != null)
                    { // чтоб закрыть зеление клеки
                        listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").hideGrids();
                    }
                }
            }
            //  menuBar.SetActive(false);
            detect = false;

        }
    }
    private void HorizAndVertical()
    {
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1]}") == null)
            {
                continue;
            }
            if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1]}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1]}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1]}").GridGreen();

        }

        for (int j = 0; j < MoveCell; j++)
        { // вверх ищет дорогу
            if (listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] + j}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] + j}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] + j}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] + j}").GridGreen();
        }

        for (int i = 0; i < MoveCell; i++)
        { // влево ищет дорогу
            if (listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1]}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1]}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1]}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1]}").GridGreen();
        }

        for (int j = 0; j < MoveCell; j++)
        { // вниз ищет дорогу
            if (listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] - j}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] - j}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] - j}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0]} z:{idForBrush[1] - j}").GridGreen();

        }
    }
    private void Diagonal()
    {
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}") == null)
            {
                continue;
            }
            if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + i}").GridGreen();

        }

        for (int j = 0; j < MoveCell; j++)
        { // вверх влево ищет дорогу
            if (listGrid.GrisItem($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0] - j} z:{idForBrush[1] + j}").GridGreen();
        }

        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0] - i} z:{idForBrush[1] - i}").GridGreen();
        }

        for (int j = 0; j < MoveCell; j++)
        { // влево низ ищет дорогу
            if (listGrid.GrisItem($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}") == null)
            {
                continue;

            }
            if (listGrid.GrisItem($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}").HavePlayer
                || listGrid.GrisItem($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}").HaveEnemy)
                break;
            else
                listGrid.GrisItem($"x:{idForBrush[0] + j} z:{idForBrush[1] - j}").GridGreen();

        }
    }
    
}
