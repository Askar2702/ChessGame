using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataExchange : MonoBehaviour , IOnEventCallback
{
    [SerializeField]
    private List<BaseUnits> PlayersList, EnemyList = new List<BaseUnits>();
    private Vector3[] CopyPlayerAndEnemy;
    Transform Players;
    Transform Enemys;
    public static DataExchange _DataExchange;

    private void Awake()
    {
        if (_DataExchange == null)
            _DataExchange = this;
        else
            Destroy(this.gameObject);
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            CopyPlayerAndEnemy = (Vector3[])photonEvent.CustomData;
            receiving(CopyPlayerAndEnemy[0], CopyPlayerAndEnemy[1]);// первым идет сам атакующии а потом во втором индексе жертва

        }
        else if (photonEvent.Code == 2)
        {
            object[] data = (object[])photonEvent.CustomData;
            magic(data[0], (string)data[1]);

        }
        else if (photonEvent.Code == 3)
        {
            float data = (float)photonEvent.CustomData;
            GameObject.Find("GameManager").GetComponent<PlayerTurn>().time = data;
        }
    }
    void receiving(Vector3 player, Vector3 enemy)
    {
        var players = EnemyList.FirstOrDefault(name => name.transform.position == player);
        Players = players.transform;
        var enemys = PlayersList.FirstOrDefault(name => name.transform.position == enemy);
        Enemys = enemys.transform;

        Players.GetComponent<BaseUnits>().attack(Enemys.gameObject, false);
    }
    void magic(object data, string Method)
    { // находить мага по месту         
        Vector3 player = (Vector3)data;
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (player == i.transform.position)
            {
                Players = i.transform;
            }
        }
        Players.SendMessage(Method);
    }


    public void AddUnits(BaseUnits unit)
    {
        if (unit.tag == "Player")
            PlayersList.Add(unit);
        else
            EnemyList.Add(unit);
    }
    public void RemoveUnits(BaseUnits unit)
    {
        if (unit.tag == "Player")
            PlayersList.Remove(unit);
        else
            EnemyList.Remove(unit);
    }

  

    /// <summary>
    /// нужен чтоб передача данных работала
    /// </summary>
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

}
