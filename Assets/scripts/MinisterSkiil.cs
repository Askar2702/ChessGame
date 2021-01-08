using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinisterSkiil : MonoBehaviour
{
    public Transform posCollider;
    public Transform posCollider2;
    public LayerMask layerMask;
    public Vector3 scale;
    bool teleports;
    public Transform pointGrid;
    public Transform player;
    public UnitManager parent;
    public ParticleSystem telepotsStart;
    public ParticleSystem telepotsfinish;
    private GameObject[] grids;
    void Start()
    {
        grids = GameObject.FindGameObjectsWithTag("grid");
    }

    private void Update()
    {
        Collider[] hitColliders1 = Physics.OverlapBox(posCollider2.position, scale, posCollider2.rotation, layerMask);
        foreach (var Currentenemy in hitColliders1)
        {
            Debug.Log(Currentenemy.transform.name);
            if (Currentenemy.GetComponent<gridsPrefab>())
                pointGrid = Currentenemy.transform;
            if (Currentenemy.GetComponent<BaseUnits>())
                player = Currentenemy.transform;
            else
                player = null;
        }
    }


    public void teleport()
    {

        Collider[] hitColliders1 = Physics.OverlapBox(posCollider2.position, scale, posCollider2.rotation, layerMask);
        foreach (var Currentenemy in hitColliders1)
        {           
            if(Currentenemy.GetComponent<gridsPrefab>())
                pointGrid = Currentenemy.transform;
            if (Currentenemy.GetComponent<BaseUnits>())
                player = Currentenemy.transform;
            else
                player = null;
        }
        if (!pointGrid) return;
        if (!player)
        {
            teleports = true;
        }
        else if(player)
        { 
            teleports = false;
        }
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (Currentenemy.transform.tag == "Player" && teleports)
            {
                // Currentenemy.transform.GetComponent<BaseUnits>().MovePoint(pointGrid);
                telepotsStart.Play();
                telepotsfinish.Play();
                StartCoroutine(teleportTarget(Currentenemy.transform, pointGrid.position));
                pointGrid = null;
                print(Currentenemy.transform.name);
                parent.EnemyMove();
            }
        }
    }    
    IEnumerator teleportTarget(Transform target , Vector3 EndPos)
    {
        yield return new WaitForSeconds(2f);
        var pos = target.position;
        target.position = EndPos;
        target.GetComponent<MovementManager>().target = EndPos;
        foreach(var gr in grids)
        {
            if(gr.name == $"x:{parent.idForBrush[0] + 2} z:{parent.idForBrush[1]}") { }
            gr.GetComponent<gridsPrefab>().HavePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(posCollider.position, scale);
        Gizmos.DrawWireCube(posCollider2.position, scale);
    }
}
