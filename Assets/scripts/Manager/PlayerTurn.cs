using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private Text _timer;
    [SerializeField] private Text _text;

    public static bool isCanPlay;
    public float TimeTurn => _time;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            isCanPlay = true;
        else
            isCanPlay = false;
    }

    /// <summary>
    /// контролирует ход игрока по времени
    /// </summary>
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            if (TimeTurn > 0)
            {
                _time -= Time.deltaTime;
                _timer.text = TimeTurn.ToString("f0");

            }
            else if (TimeTurn <= 0)
            {
                if (onlineManager.onlineManagers.isCanMove)
                    onlineManager.onlineManagers.SettingIsCanMoveBool(false);
                else
                    onlineManager.onlineManagers.SettingIsCanMoveBool(true);
                foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    player.GetComponent<UnitManager>().moveBool(false);
                  //  player.GetComponent<UnitManager>()._canvasMenuBar.SetActive(false);
                }
                PlayTurn();
            }


        }
        else
            return;
        _text.text = onlineManager.onlineManagers.isCanMove.ToString();
    }

    /// <summary>
    /// опеределяет чей ход в игре и устанавливает время
    /// </summary>
    public void PlayTurn()
    {// очередность игры                 

        if (PhotonNetwork.IsMasterClient)
        {
            if (onlineManager.onlineManagers.isCanMove)
                isCanPlay = true;
            else
                isCanPlay = false;
        }
        else
        {
            if (!onlineManager.onlineManagers.isCanMove)
            {
                isCanPlay = true;
            }
            else
            {
                isCanPlay = false;
            }

        }
        _time = 90;
    }

    public void SettingTime(float time)
    {
        _time = time;
    }
}
