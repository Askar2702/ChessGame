using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject Miniknigth;
    [SerializeField] private GameObject DarkPlayer;
    [SerializeField] private GameObject Berserk;
    [SerializeField] private GameObject Magic;
    [SerializeField] private GameObject King;
    [SerializeField] private GameObject Assassin;
    [SerializeField] private GameObject Warrior;
    [SerializeField] private GameObject Minister;
    private GameObject Player;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
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
            Player = PhotonNetwork.Instantiate("Magic/" + Magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White1";
            // маг
            grid = GameObject.Find($"x:{8} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Magic/" + Magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White2";
            //берсерк
            grid = GameObject.Find($"x:{9} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Berserk/" + Berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White1";
            //берсерк
            grid = GameObject.Find($"x:{0} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Berserk/" + Berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White2";
            //Ассасин
            grid = GameObject.Find($"x:{3} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Assassin/" + Assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White1";
            //Ассасин
            grid = GameObject.Find($"x:{6} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Assassin/" + Assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White2";
            //Воин           
            grid = GameObject.Find($"x:{2} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Warrior/" + Warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White1";
            //Воин            
            grid = GameObject.Find($"x:{7} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Warrior/" + Warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White2";
            //Король
            grid = GameObject.Find($"x:{5} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("King/" + King.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            //Министр
            grid = GameObject.Find($"x:{4} z:{0}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Minister/" + Minister.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            Player.name += "White";
            // пешки
            for (int i = 0; i < 10; i++)
            {
                grid = GameObject.Find($"x:{i} z:{1}"); // ставлю на линию пешек нв вторую линию
                pos = grid.transform.GetChild(0).transform.position;
                Player = PhotonNetwork.Instantiate("Knigth/" + Miniknigth.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);    //высота на 3 чтоб после приземления луч увидел землю и взял координат  
                Player.name += i;

            }
        }
        else
        {   // маг
            grid = GameObject.Find($"x:{8} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Magic/" + Magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black1";
            // маг
            grid = GameObject.Find($"x:{1} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Magic/" + Magic.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black2";
            // берсерк
            grid = GameObject.Find($"x:{0} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Berserk/" + Berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black1";
            // берсерк
            grid = GameObject.Find($"x:{9} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Berserk/" + Berserk.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black2";
            // Ассасин
            grid = GameObject.Find($"x:{6} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Assassin/" + Assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black1";
            // Ассасин
            grid = GameObject.Find($"x:{3} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Assassin/" + Assassin.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black2";
            // Воин
            grid = GameObject.Find($"x:{2} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Warrior/" + Warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black1";
            // Воин
            grid = GameObject.Find($"x:{7} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Warrior/" + Warrior.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black2";
            //Король
            grid = GameObject.Find($"x:{4} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("King/" + King.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black";
            //Король
            grid = GameObject.Find($"x:{5} z:{7}");
            pos = grid.transform.GetChild(0).transform.position;
            Player = PhotonNetwork.Instantiate("Minister/" + Minister.name, new Vector3(pos.x, 0f, pos.z), Quaternion.Euler(0f, 180f, 0f));
            Player.name += "Black";
            // пешки
            for (int i = 0; i < 10; i++)
            {
                grid = GameObject.Find($"x:{i} z:{6}"); // ставлю на линию пешек нв вторую линию
                pos = grid.transform.GetChild(0).transform.position;
                Player = PhotonNetwork.Instantiate("DarkKnigth/" + DarkPlayer.name, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);    //высота на 3 чтоб после приземления луч увидел землю и взял координат  
                Player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                Player.name += i;
                cam.transform.position = new Vector3(12.5f, 35.5f, 39f);
                cam.transform.rotation = Quaternion.Euler(50.5f, 180f, 0);
            }
        }
    }

}
