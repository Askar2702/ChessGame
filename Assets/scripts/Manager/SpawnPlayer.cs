using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _miniknigth;
    [SerializeField] private GameObject _darkPlayer;
    [SerializeField] private GameObject _berserk;
    [SerializeField] private GameObject _magic;
    [SerializeField] private GameObject _king;
    [SerializeField] private GameObject _assassin;
    [SerializeField] private GameObject _warrior;
    [SerializeField] private GameObject _minister;
    private GameObject _player;
    private Camera _cam;
    void Start()
    {
        _cam = Camera.main;
        SpawnUnits();
    }

    
    private void SpawnUnits()
    {
        var grid = new GameObject();
        var pos = new Vector3();
        if (PhotonNetwork.IsMasterClient)
        {   // маг
            grid = GameObject.Find($"x:{1} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Magic/" + _magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White1";
            // маг
            grid = GameObject.Find($"x:{8} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Magic/" + _magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White2";
            //берсерк
            grid = GameObject.Find($"x:{9} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Berserk/" + _berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White1";
            //берсерк
            grid = GameObject.Find($"x:{0} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Berserk/" + _berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White2";
            //Ассасин
            grid = GameObject.Find($"x:{3} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Assassin/" + _assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White1";
            //Ассасин
            grid = GameObject.Find($"x:{6} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Assassin/" + _assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White2";
            //Воин           
            grid = GameObject.Find($"x:{2} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Warrior/" + _warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White1";
            //Воин            
            grid = GameObject.Find($"x:{7} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Warrior/" + _warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White2";
            //Король
            grid = GameObject.Find($"x:{5} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("King/" + _king.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            //Министр
            grid = GameObject.Find($"x:{4} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Minister/" + _minister.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            _player.name += "White";
            // пешки
            for (int i = 0; i < 10; i++)
            {
                grid = GameObject.Find($"x:{i} z:{1}"); // ставлю на линию пешек нв вторую линию
                pos = grid.transform.GetChild(0).transform.position;
                _player = PhotonNetwork.Instantiate("Knigth/" + _miniknigth.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);    //высота на 3 чтоб после приземления луч увидел землю и взял координат  
                _player.name += i;

            }
        }
        else
        {   // маг
            grid = GameObject.Find($"x:{8} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Magic/" + _magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black1";
            // маг
            grid = GameObject.Find($"x:{1} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Magic/" + _magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black2";
            // берсерк
            grid = GameObject.Find($"x:{0} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Berserk/" + _berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black1";
            // берсерк
            grid = GameObject.Find($"x:{9} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Berserk/" + _berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black2";
            // Ассасин
            grid = GameObject.Find($"x:{6} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Assassin/" + _assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black1";
            // Ассасин
            grid = GameObject.Find($"x:{3} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Assassin/" + _assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black2";
            // Воин
            grid = GameObject.Find($"x:{2} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Warrior/" + _warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black1";
            // Воин
            grid = GameObject.Find($"x:{7} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Warrior/" + _warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black2";
            //Король
            grid = GameObject.Find($"x:{4} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("King/" + _king.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black";
            //Король
            grid = GameObject.Find($"x:{5} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            _player = PhotonNetwork.Instantiate("Minister/" + _minister.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            _player.name += "Black";
            // пешки
            for (int i = 0; i < 10; i++)
            {
                grid = GameObject.Find($"x:{i} z:{6}"); // ставлю на линию пешек нв вторую линию
                pos = grid.transform.GetChild(0).transform.position;
                _player = PhotonNetwork.Instantiate("DarkKnigth/" + _darkPlayer.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);    //высота на 3 чтоб после приземления луч увидел землю и взял координат  
                _player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                _player.name += i;
                _cam.transform.position = new Vector3(12.5f, 35.5f, 39f);
                _cam.transform.rotation = Quaternion.Euler(50.5f, 180f, 0);
            }
        }
    }

}
