using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class MinistrSkills : MonoBehaviour,IMagicAbility
{
    [SerializeField] private Transform _posCollider;
    [SerializeField] private Transform _posCollider2;
    [SerializeField] private Transform _posCollider3;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private UnitManager _parent;
    [SerializeField] private ParticleSystem _telepotsStart;
    [SerializeField] private ParticleSystem _telepotsfinish;
    [SerializeField] private Transform _shield;

    private Transform _player;
    private List<gridsPrefab> _grids;
    private Transform _pointGrid;
    private bool _isTeleport;
    private Vector3 _forward = Vector3.zero;

    void Start()
    {
        _grids = GameObject.Find("GameManager").GetComponent<ListGrid>().Grids;
    }

    public void Ability_1()
    {
        Computation();
        Collider[] hitColliders = Physics.OverlapBox(_posCollider.position, _scale, _posCollider.rotation, _layerMask);
        foreach (var Currentenemy in hitColliders)
        {
            if (Currentenemy.transform.tag == "Player" && _isTeleport)
            {
                if (!_telepotsfinish.isPlaying) 
                { 
                _telepotsStart.Play();
                _telepotsfinish.Play();
                }
                StartCoroutine(teleportTarget(Currentenemy.transform, _pointGrid.position));
                _pointGrid = null;
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
        foreach (var gr in _grids)
        {
            if (gr.name == $"x:{_parent.idForBrush[0] + 2} z:{_parent.idForBrush[1]}")
                gr.GetComponent<gridsPrefab>().PlayerSignal(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_posCollider.position, _scale);
        Gizmos.DrawWireCube(_posCollider2.position, _scale);
        Gizmos.DrawWireCube(_posCollider3.position, new Vector3(2.5f, 1, 1));
    }
    public void Ability_2()
    {
        Computation();
        Collider[] hitCollider = Physics.OverlapBox(_posCollider3.position, new Vector3(2.5f, 1, 1), _posCollider3.rotation, _layerMask);
        StartCoroutine(ShieldEffectStart());
        foreach (var Currentenemy in hitCollider)
        {
            if (!Currentenemy.GetComponent<MovementManager>()) return;
            Currentenemy.GetComponent<MovementManager>().BackMove();
            CellsToIndent(_pointGrid.GetComponent<gridsPrefab>(), Currentenemy.GetComponent<MovementManager>());
            Currentenemy.GetComponent<healthBar>().TakeDamage(50, this.GetType(), transform);
        }
    }

    private void CellsToIndent(gridsPrefab _gridsPrefab , MovementManager enemy)
    {
        if (!_pointGrid) return;
        if (enemy.transform.position.x == _gridsPrefab.transform.GetChild(0).transform.position.x && _isTeleport)
        {
            enemy.target = _gridsPrefab.transform.GetChild(0).transform.position;
        }
        else
        {
            int[] _id = new int[2];
            _id[0] = _gridsPrefab.NewID[0];
            _id[1] = _gridsPrefab.NewID[1];
            _id[0] -= 1;
            foreach (var _grid in _grids)
            {
                if (_id.SequenceEqual(_grid.NewID))
                {
                    if (enemy.transform.position.x == _grid.transform.GetChild(0).transform.position.x)
                    {
                        if (!_grid.HaveEnemy && !_grid.HavePlayer)
                        {
                            enemy.target = _grid.transform.GetChild(0).transform.position;
                        }
                    }
                    else _id[0] += 2;
                    foreach (var _grid1 in _grids)
                    {
                        if (_id.SequenceEqual(_grid1.NewID))
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
        Collider[] hitColliders1 = Physics.OverlapBox(_posCollider2.position, _scale, _posCollider2.rotation, _layerMask);
        foreach (var Currentenemy in hitColliders1)
        {
            if (Currentenemy.GetComponent<gridsPrefab>())
                _pointGrid = Currentenemy.transform;
            if (Currentenemy.GetComponent<UnitManager>())
                _player = Currentenemy.transform;
            else
                _player = null;
        }
        if (!_pointGrid)
        {
            _pointGrid = null;
            return;
        }
        if (!_player)
        {
            _isTeleport = true;
        }
        else if (_player)
        {
            _isTeleport = false;
        }
        

    }



    /// <summary>
    /// двигает щит вперед 
    /// </summary>
    
    IEnumerator ShieldEffectStart()
    {
        _shield.position = new Vector3(_shield.position.x, _shield.position.y, transform.position.z);
        _forward = new Vector3(_shield.position.x, _shield.position.y, _posCollider3.position.z);

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
