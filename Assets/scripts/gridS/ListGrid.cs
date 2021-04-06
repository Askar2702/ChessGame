using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ListGrid : MonoBehaviour
{
    public List<gridsPrefab> Grids { get; private set; }

    private void Awake()
    {
        Grids = new List<gridsPrefab>();
    }
    public void AddGrid(gridsPrefab gridsPrefab)
    {
        if (Grids.Contains(gridsPrefab)) return;
        Grids.Add(gridsPrefab);
    }

    public gridsPrefab GrisItem(string nameGrid)
    {
        gridsPrefab _item = null;
        _item = Grids.SingleOrDefault(item => item.transform.name == nameGrid);
        return _item;
    }
}
