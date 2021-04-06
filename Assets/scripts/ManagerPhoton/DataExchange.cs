using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataExchange : MonoBehaviour , IOnEventCallback
{
    public static DataExchange DataExchangeCenter;

    [SerializeField] private List<UnitManager> _playersList = new List<UnitManager>();
    [SerializeField] private List<UnitManager> _enemyList = new List<UnitManager>();
    private int[] _copyPlayerAndEnemy;
    private Transform _players;
    private Transform _enemys;

    private void Awake()
    {
        if (DataExchangeCenter == null)
            DataExchangeCenter = this;
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
            _copyPlayerAndEnemy = (int[])photonEvent.CustomData;
            receiving(_copyPlayerAndEnemy[0], _copyPlayerAndEnemy[1]);// первым идет сам атакующии а потом во втором индексе жертва

        }
        else if (photonEvent.Code == 2)
        {
            object[] data = (object[])photonEvent.CustomData;
            magic(data[0], (string)data[1]);

        }
        else if (photonEvent.Code == 3)
        {
            float data = (float)photonEvent.CustomData;
            GameObject.Find("GameManager").GetComponent<PlayerTurn>().SettingTime(data);
        }
    }

    /// <summary>
    /// Other figures
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    void receiving(int player, int enemy)
    {
        var _player = _enemyList.SingleOrDefault(name => name.Id == player);
        _players = _player.transform;
        var _enemy = _playersList.SingleOrDefault(name => name.Id == enemy);
        _enemys = _enemy.transform;

        _players.GetComponent<IAttack>().Attack(_enemys.gameObject, false);
    }

    /// <summary>
    /// Magic 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="Method"></param>
    void magic(object data, string Method)
    { // находить мага по месту         
        Vector3 player = (Vector3)data;
        var players = _enemyList.SingleOrDefault(name => name.transform.position == player);
        _players = players.transform;
        _players.SendMessage(Method);
    }




    public void AddUnits(UnitManager unit)
    {
        if (unit.tag == "Player")
        {
            _playersList.Add(unit);
            unit.IDAssignment(_playersList.Count-1);
        }
        else
            _enemyList.Add(unit);
    }
    public void RemoveUnits(UnitManager unit)
    {
        if (unit.tag == "Player")
            _playersList.Remove(unit);
        else
            _enemyList.Remove(unit);
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
