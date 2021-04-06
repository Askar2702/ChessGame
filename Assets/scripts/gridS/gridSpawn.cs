using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _grid;
    [SerializeField] private int _width; //ширина
    [SerializeField] private int _length;//длина
    [SerializeField] private Transform _instanseGrid;
    [SerializeField] private ListGrid _listGrid;
    private int[] _cellId;
    void Start()
    {
        // инстанс сетки
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _length; j++)
            {
                var Mesh = _grid.GetComponent<MeshRenderer>();
                var MeshSize = Mesh.bounds.size + new Vector3(0.1f, 0, 0.1f); // это нужно для границы сетки
                var position = new Vector3(_instanseGrid.position.x + i * MeshSize.x, 0f, _instanseGrid.position.z + j * MeshSize.z);
                var GridClone = Instantiate(_grid, position, Quaternion.identity);
                _listGrid.AddGrid(GridClone.GetComponent<gridsPrefab>());
                GridClone.name = $"x:{i} z:{j}";
                _cellId = new int[2] { i, j };
                GridClone.SendMessage("id", _cellId);
                GridClone.transform.parent = _instanseGrid;
            }
        }
    }

   
  
}
