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
            if (GameObject.Find($"x:{move[0] + i} z:{move[1] + i}") == null)
            {
                continue;
            }
            if (GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").SendMessage("GridGreen");

        }

        for (int j = 0; j < radiusMove; j++)
        { // вверх влево ищет дорогу
            if (GameObject.Find($"x:{move[0] - j} z:{move[1] + j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").SendMessage("GridGreen");
        }

        for (int i = 0; i < radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (GameObject.Find($"x:{move[0] - i} z:{move[1] - i}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").SendMessage("GridGreen");
        }

        for (int j = 0; j < radiusMove; j++)
        { // влево низ ищет дорогу
            if (GameObject.Find($"x:{move[0] + j} z:{move[1] - j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").SendMessage("GridGreen");

        }
    }

    public void GridsHaveEnemy(int[] idGrids)
    {
        if (!detect)
        {
            for (int i = 0; i < MoveCell; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < MoveCell; j++)
                {
                    if (GameObject.Find($"x:{idGrids[0] + i} z:{idGrids[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        GameObject.Find($"x:{idGrids[0] + i} z:{idGrids[1] + j}").SendMessage("haveEnemy");
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
                if (GameObject.Find($"x:{move[0] + i} z:{move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").SendMessage("hideGrids");
                }
            }
            for (int j = 0; j < radiusMove; j++)
            {
                if (GameObject.Find($"x:{move[0] + j} z:{move[1] - j}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").SendMessage("hideGrids");
                }
            }
            for (int i = 0; i < radiusMove; i++)
            {
                if (GameObject.Find($"x:{move[0] - i} z:{move[1] - i}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").SendMessage("hideGrids");
                }
            }
            for (int j = 0; j < radiusMove; j++)
            {
                if (GameObject.Find($"x:{move[0] - j} z:{move[1] + j}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").SendMessage("hideGrids");
                }
            }
            if (detect)
            {
                detect = false;
                for (int i = 0; i < MoveCell; i++) //закрывает клетки с врагами
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
