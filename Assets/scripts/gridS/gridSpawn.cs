using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridSpawn : MonoBehaviour
{
    public GameObject grid;
    public int width; //ширина
    public int length;//длина
    public Transform InstanseGrid;
    [SerializeField] private ListGrid listGrid;
    private int[] CellId;
    void Start()
    {
        // инстанс сетки
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var Mesh = grid.GetComponent<MeshRenderer>();
                var MeshSize = Mesh.bounds.size + new Vector3(0.1f, 0, 0.1f); // это нужно для границы сетки
                var position = new Vector3(InstanseGrid.position.x + i * MeshSize.x, 0f, InstanseGrid.position.z + j * MeshSize.z);
                var GridClone = Instantiate(grid, position, Quaternion.identity);
                listGrid.AddGrid(GridClone.GetComponent<gridsPrefab>());
                GridClone.name = $"x:{i} z:{j}";
                CellId = new int[2] { i, j };
                GridClone.SendMessage("id", CellId);
                GridClone.transform.parent = InstanseGrid;
            }
        }
    }

   
  
}
