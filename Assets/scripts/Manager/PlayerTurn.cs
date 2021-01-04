using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour
{
    public float time;
    public Text timer;
    public static bool CanPlay;
    public Text text;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            CanPlay = true;
        else
            CanPlay = false;
    }

    /// <summary>
    /// контролирует ход игрока по времени
    /// </summary>
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timer.text = time.ToString("f0");

            }
            else if (time <= 0)
            {
                if (onlineManager.CanMove)
                    onlineManager.CanMove = false;
                else
                    onlineManager.CanMove = true;
                foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    player.GetComponent<UnitManager>().hideGrids();
                    player.GetComponent<UnitManager>().menuBar.SetActive(false);
                }
                PlayTurn();
            }


        }
        else
            return;
        text.text = onlineManager.CanMove.ToString();
    }

    /// <summary>
    /// опеределяет чей ход в игре и устанавливает время
    /// </summary>
    public void PlayTurn()
    {// очередность игры                 

        if (PhotonNetwork.IsMasterClient)
        {
            if (onlineManager.CanMove)
                CanPlay = true;
            else
                CanPlay = false;
        }
        else
        {
            if (!onlineManager.CanMove)
            {
                CanPlay = true;
            }
            else
            {
                CanPlay = false;
            }

        }
        time = 90;
    }
}
