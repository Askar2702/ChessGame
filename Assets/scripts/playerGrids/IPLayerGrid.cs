using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPLayerGrid
{
    void GetPoint(int[] idCell);

    void Grids();

    void HideGrids();
    void GridsHaveEnemy(int[] idGrids);
}
