using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGrids : BaseUnits, IPLayerGrid
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
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        {
            for (int j = 0; j < MoveCell; j++)
            {
                if (listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                {
                    continue;
                    // print($"{transform.name}x:{i} z:{j}");
                }
                else
                    listGrid.GrisItem($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").GridGreen();

            }
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
            for (int i = 0; i < MoveCell; i++)
            {
                for (int j = 0; j < MoveCell; j++)
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
}
