using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ListGrid : MonoBehaviour
{
    private List<gridsPrefab> grids;

    private void Awake()
    {
        grids = new List<gridsPrefab>();
    }
    public void AddGrid(gridsPrefab gridsPrefab)
    {
        if (grids.Contains(gridsPrefab)) return;
        grids.Add(gridsPrefab);
    }

    public gridsPrefab GrisItem(string nameGrid)
    {
        gridsPrefab _item = null;
        _item = grids.SingleOrDefault(item => item.transform.name == nameGrid);
        return _item;
    }
}
