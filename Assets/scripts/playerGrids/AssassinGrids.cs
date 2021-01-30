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
            if (GameObject.Find($"x:{move[0] + i} z:{move[1] + 2}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] + i} z:{move[1] + 2}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо в бок вверх
            if (GameObject.Find($"x:{move[0] + 2} z:{move[1] + i}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] + 2} z:{move[1] + i}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++)
        { // право в низ
            if (GameObject.Find($"x:{move[0] + i} z:{move[1] - 2}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] + i} z:{move[1] - 2}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++)
        { // право в бок низ
            if (GameObject.Find($"x:{move[0] + 2} z:{move[1] - i}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] + 2} z:{move[1] - i}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++)
        {// влево вверх
            if (GameObject.Find($"x:{move[0] - i} z:{move[1] + 2}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] - i} z:{move[1] + 2}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++)
        {// влево вверх бок
            if (GameObject.Find($"x:{move[0] - 2} z:{move[1] + i}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] - 2} z:{move[1] + i}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++)
        {
            // влево в низ
            if (GameObject.Find($"x:{move[0] - i} z:{move[1] - 2}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] - i} z:{move[1] - 2}").SendMessage("GridGreen");
        }
        for (int i = 1; i < 2; i++)
        {
            // влево в низ бок
            if (GameObject.Find($"x:{move[0] - 2} z:{move[1] - i}") == null)
            {
                continue;
            }
            else
                GameObject.Find($"x:{move[0] - 2} z:{move[1] - i}").SendMessage("GridGreen");
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
                    if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("haveEnemy");
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
                if (GameObject.Find($"x:{move[0] + i} z:{move[1] + 2}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] + i} z:{move[1] + 2}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] + 2} z:{move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] + 2} z:{move[1] + i}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] + i} z:{move[1] - 2}") != null)
                {
                    GameObject.Find($"x:{move[0] + i} z:{move[1] - 2}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] + 2} z:{move[1] - i}") != null)
                {
                    GameObject.Find($"x:{move[0] + 2} z:{move[1] - i}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] - i} z:{move[1] + 2}") != null)
                {
                    GameObject.Find($"x:{move[0] - i} z:{move[1] + 2}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] - 2} z:{move[1] + i}") != null)
                {
                    GameObject.Find($"x:{move[0] - 2} z:{move[1] + i}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] - i} z:{move[1] - 2}") != null)
                {
                    GameObject.Find($"x:{move[0] - i} z:{move[1] - 2}").SendMessage("hideGrids");
                }
            }
            for (int i = 1; i < 2; i++)
            {
                if (GameObject.Find($"x:{move[0] - 2} z:{move[1] - i}") != null)
                {
                    GameObject.Find($"x:{move[0] - 2} z:{move[1] - i}").SendMessage("hideGrids");
                }
            }
            if (detect)
            {
                detect = false;
                for (int i = 0; i < MoveCell; i++) //закрывает клетки
                {
                    for (int j = 0; j < MoveCell; j++)
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
    }
}
