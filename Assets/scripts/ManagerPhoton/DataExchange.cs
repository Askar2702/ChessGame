using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataExchange : MonoBehaviour , IOnEventCallback
{
    public List<UnitManager> PlayersList, EnemyList = new List<UnitManager>();
    private int[] CopyPlayerAndEnemy;
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

    /// <summary>
    /// Receives signals
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            CopyPlayerAndEnemy = (int[])photonEvent.CustomData;
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

    /// <summary>
    /// Other figures
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    void receiving(int player, int enemy)
    {
        var _player = EnemyList.SingleOrDefault(name => name._id == player);
        Players = _player.transform;
        var _enemy = PlayersList.SingleOrDefault(name => name._id == enemy);
        Enemys = _enemy.transform;

        Players.GetComponent<IAttack>().Attack(Enemys.gameObject, false);
    }

    /// <summary>
    /// Magic 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="Method"></param>
    void magic(object data, string Method)
    { // находить мага по месту         
        Vector3 player = (Vector3)data;
        var players = EnemyList.SingleOrDefault(name => name.transform.position == player);
        Players = players.transform;
        Players.SendMessage(Method);
    }




    public void AddUnits(UnitManager unit)
    {
        if (unit.tag == "Player")
        {
            PlayersList.Add(unit);
            unit.IDAssignment(PlayersList.Count-1);
        }
        else
            EnemyList.Add(unit);
    }
    public void RemoveUnits(UnitManager unit)
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
