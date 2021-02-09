using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class MinistrSkills : MonoBehaviour,IMagicAbility
{
    [SerializeField] private Transform posCollider;
    [SerializeField] private Transform posCollider2;
    [SerializeField] private Transform posCollider3;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 scale;
    [SerializeField] private UnitManager parent;
    [SerializeField] private ParticleSystem telepotsStart;
    [SerializeField] private ParticleSystem telepotsfinish;
    [SerializeField] private Transform _shield;

    private Transform player;
    [SerializeField] private List<gridsPrefab> grids;
    private Transform pointGrid;
    private bool IsTeleport;
    private Vector3 _forward = Vector3.zero;

    void Start()
    {
        grids = GameObject.Find("GameManager").GetComponent<ListGrid>().grids;
    }

    public void Ability_1()
    {
        Computation();
        Collider[] hitColliders = Physics.OverlapBox(posCollider.position, scale, posCollider.rotation, layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (Currentenemy.transform.tag == "Player" && IsTeleport)
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
            if (gr.name == $"x:{parent.idForBrush[0] + 2} z:{parent.idForBrush[1]}")
                gr.GetComponent<gridsPrefab>().PlayerSignal(false);
            Debug.Log($"x:{parent.idForBrush[0] + 2} z:{parent.idForBrush[1]}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(posCollider.position, scale);
        Gizmos.DrawWireCube(posCollider2.position, scale);
        Gizmos.DrawWireCube(posCollider3.position, new Vector3(4, 1, 1));
    }

    public void Ability_2()
    {
        Computation();
        Collider[] hitCollider = Physics.OverlapBox(posCollider3.position, new Vector3(4, 1, 1), posCollider3.rotation, layerMask);
        StartCoroutine(ShieldEffectStart());
        foreach (var Currentenemy in hitCollider)
        {
            if (!Currentenemy.GetComponent<MovementManager>()) return;
            Currentenemy.GetComponent<MovementManager>().BackMove();
            CellsToIndent(pointGrid.GetComponent<gridsPrefab>(), Currentenemy.GetComponent<MovementManager>());
            Currentenemy.GetComponent<healthBar>().TakeDamage(50, this.GetType(), transform);
        }
    }

    private void CellsToIndent(gridsPrefab _gridsPrefab , MovementManager enemy)
    {
        if (!pointGrid) return;
        if (enemy.transform.position.x == _gridsPrefab.transform.GetChild(0).transform.position.x && IsTeleport)
        {
            enemy.target = _gridsPrefab.transform.GetChild(0).transform.position;
        }
        else
        {
            int[] _id = new int[2];
            _id[0] = _gridsPrefab.newID[0];
            _id[1] = _gridsPrefab.newID[1];
            _id[0] -= 1;
            foreach (var _grid in grids)
            {
                if (_id.SequenceEqual(_grid.newID))
                {
                    if (enemy.transform.position.x == _grid.transform.GetChild(0).transform.position.x)
                    {
                        if (!_grid.HaveEnemy && !_grid.HavePlayer)
                        {
                            enemy.target = _grid.transform.GetChild(0).transform.position;
                        }
                    }
                    else _id[0] += 2;
                    foreach (var _grid1 in grids)
                    {
                        if (_id.SequenceEqual(_grid1.newID))
                        {
                            if (enemy.transform.position.x == _grid1.transform.GetChild(0).transform.position.x && !_grid1.HaveEnemy && !_grid1.HavePlayer)
                            {
                                enemy.target = _grid1.transform.GetChild(0).transform.position;
                                break;
                            }
                        }
                        else break;
                    }
                }
            }
        }
    }

    /// <summary>
    ///  использует коллайдеры чтоб знать не занята ли клетка или существует ли она 
    /// </summary>
    private void Computation()
    {
        // эта часть нужна чтоб оперделить можно ли толкать врага и не выйдет ли он за придел карты 
        Collider[] hitColliders1 = Physics.OverlapBox(posCollider2.position, scale, posCollider2.rotation, layerMask);
        foreach (var Currentenemy in hitColliders1)
        {
            if (Currentenemy.GetComponent<gridsPrefab>())
                pointGrid = Currentenemy.transform;
            if (Currentenemy.GetComponent<UnitManager>())
                player = Currentenemy.transform;
            else
                player = null;
        }
        if (!pointGrid)
        {
            pointGrid = null;
            return;
        }
        if (!player)
        {
            IsTeleport = true;
        }
        else if (player)
        {
            IsTeleport = false;
        }
        

    }



    /// <summary>
    /// двигает щит вперед 
    /// </summary>
    
    IEnumerator ShieldEffectStart()
    {
        _shield.position = new Vector3(_shield.position.x, _shield.position.y, transform.position.z);
        _forward = new Vector3(_shield.position.x, _shield.position.y, posCollider3.position.z);

        while (Vector3.Distance( _shield.position, _forward) >= 0.3f)
        {
            ShieldEffect();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ShieldEffect()
    {
        _shield.gameObject.SetActive(true);
        _shield.position = Vector3.Lerp(_shield.position, _forward, 2 * Time.deltaTime);
    }


    public void Ability_3()
    {
        //didn't come up with a skill
    }
}
