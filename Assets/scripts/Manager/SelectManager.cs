using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour 
{

    public Transform SelectPlayer;
  
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
   
    void Update()
    {
        // тут идет выбор кто ходит и выбор перса
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, ray.direction);
        if (PlayerTurn.CanPlay && GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {//внутри селекта хранится ссылка на героя которого я выделил и снизу сравнивается тот ли если нет то бывшему селектку отправлятся сообщение чтоб онна зкрыла клетки и меню 

                    if (hit.collider.tag == "Player")
                    {
                        if(SelectPlayer == null) SelectPlayer = hit.transform;
                        if (SelectPlayer.name != hit.transform.name)
                        {
                            if (SelectPlayer.GetComponent<BaseUnits>())
                            {
                                SelectPlayer.GetComponent<UnitManager>().moveBool(false);
                                SelectPlayer.GetComponent<UnitManager>().OffPlayer();
                            }
                        }
                        hit.transform.GetComponent<UnitManager>().moveBool(true);
                        SelectPlayer = hit.transform;


                    }
                    if (hit.collider.tag == "grid")
                    {
                        if (hit.transform.GetComponent<Renderer>().material.GetColor("_EmissionColor") == Color.green * 1)
                        {
                            SelectPlayer.GetComponent<MovementManager>().MovePoint(hit.transform);
                            SelectPlayer.GetComponent<UnitManager>().hideGrids();
                        }

                    }
                   
                }
            }
        }
    }
    
  
}
