using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinistrSkills : MonoBehaviour,IMagicAbility
{
    [SerializeField] private Transform posCollider;
    [SerializeField] private Transform posCollider2;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 scale;
    [SerializeField] private UnitManager parent;
    [SerializeField] private ParticleSystem telepotsStart;
    [SerializeField] private ParticleSystem telepotsfinish;

    public Transform player;
    public GameObject[] grids;
    private Transform pointGrid;
    private bool teleports;

    void Start()
    {
        grids = GameObject.FindGameObjectsWithTag("grid");
    }

    

    public void Ability_1()
    {
        Collider[] hitColliders1 = Physics.OverlapBox(posCollider2.position, scale, posCollider2.rotation, layerMask);
        foreach (var Currentenemy in hitColliders1)
        {
            Debug.Log("123");
            if (Currentenemy.GetComponent<gridsPrefab>())
                pointGrid = Currentenemy.transform;
            if (Currentenemy.GetComponent<UnitManager>())
                player = Currentenemy.transform;
            else
                player = null;
        }
        if (!pointGrid) return;
        if (!player)
        {
            teleports = true;
        }
        else if (player)
        {
            teleports = false;
        }
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (Currentenemy.transform.tag == "Player" && teleports)
            {
                if (!telepotsfinish.isPlaying) 
                { 
                telepotsStart.Play();
                telepotsfinish.Play();
                }
                StartCoroutine(teleportTarget(Currentenemy.transform, pointGrid.position));
                pointGrid = null;
                Debug.Log(Currentenemy.transform.name);
            }
        }
    }

    

    IEnumerator teleportTarget(Transform target, Vector3 EndPos)
    {
        yield return new WaitForSeconds(2f);
        var pos = target.position;
        target.position = EndPos;
        target.GetComponent<MovementManager>().target = EndPos;
        foreach (var gr in grids)
        {
            if (gr.name == $"x:{parent.idForBrush[0] + 2} z:{parent.idForBrush[1]}") { }
            gr.GetComponent<gridsPrefab>().HavePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(posCollider.position, scale);
        Gizmos.DrawWireCube(posCollider2.position, scale);
    }

    public void Ability_2()
    {
        foreach(var enemy in DataExchange._DataExchange.EnemyList)
        {
            if(Vector3.Distance(transform.position , enemy.transform.position) <= 2f)
            {
                if(enemy.transform.position.z > transform.position.z)
                    Debug.Log(enemy.name);
            }
        }
    }

    public void Ability_3()
    {
        //didn't come up with a skill
    }
}
