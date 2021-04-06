using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour 
{
    [SerializeField] private Transform _selectPlayer;
    public Transform SelectPlayer => _selectPlayer;
  
    private Camera _cam;
    private void Awake()
    {
        _cam = Camera.main;
    }
   
    void Update()
    {
        // тут идет выбор кто ходит и выбор перса
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(_cam.transform.position, ray.direction);
        if (PlayerTurn.isCanPlay && GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {//внутри селекта хранится ссылка на героя которого я выделил и снизу сравнивается тот ли если нет то бывшему селектку отправлятся сообщение чтоб онна зкрыла клетки и меню 

                    if (hit.collider.tag == "Player")
                    {
                        if(_selectPlayer == null) _selectPlayer = hit.transform;
                        if (_selectPlayer.name != hit.transform.name)
                        {
                            if (_selectPlayer.GetComponent<BaseUnits>())
                            {
                                _selectPlayer.GetComponent<UnitManager>().moveBool(false);
                                _selectPlayer.GetComponent<UnitManager>().OffPlayer();
                            }
                        }
                        hit.transform.GetComponent<UnitManager>().moveBool(true);
                        _selectPlayer = hit.transform;


                    }
                    if (hit.collider.tag == "grid")
                    {
                        if (hit.transform.GetComponent<Renderer>().material.GetColor("_EmissionColor") == Color.green * 1)
                        {
                            _selectPlayer.GetComponent<MovementManager>().MovePoint(hit.transform);
                            _selectPlayer.GetComponent<UnitManager>().hideGrids();
                        }

                    }
                   
                }
            }
        }
    }
    
  
}
