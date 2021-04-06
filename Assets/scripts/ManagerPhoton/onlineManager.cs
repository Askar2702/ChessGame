using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class onlineManager : MonoBehaviourPunCallbacks
{
    public static onlineManager onlineManagers;
    public bool isWin { get; private set; }
    public bool isCanMove { get; private set; }

    private void Awake()
    {
        if (onlineManagers == null)
            onlineManagers = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // своя сериализация чтоб передавать данные в фотоне
        PhotonPeer.RegisterType(typeof(Vector3), 27, seriliazeVector3, deseriliazeVector3);
        isWin = false;
        isCanMove = true;
        DontDestroyOnLoad(gameObject);       
    }
    
    /// <summary>
    /// возвращает значение победы
    /// </summary>
    /// <param name="win"></param>
    public void finished(bool win)
    {
        isWin = win;
        StartCoroutine(GoFinish());
    }

    IEnumerator GoFinish()
    {
        yield return new WaitForSeconds(5f); 
        Leave();
        Destroy(gameObject, 3f);
    }


    /// <summary>
    /// ливает с игры
    /// </summary>
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }


    public void SettingIsCanMoveBool(bool can)
    {
        isCanMove = can;
    }
  


    #region Photon Method

    public override void OnLeftRoom()
    {
        // ливает с игры
        PhotonNetwork.LoadLevel("Menu");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Зашел в команту" + newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Ливнул с команату" + otherPlayer.NickName);
    }

    #endregion


    #region для сериализаций Vector3
    public static object deseriliazeVector3(byte[]data)
    {
        Vector3 result = new Vector3();
        result.x = BitConverter.ToSingle(data, 0);
        result.y = BitConverter.ToSingle(data, 4);
        result.z = BitConverter.ToSingle(data, 8);
        return result;
    }

    public static byte[] seriliazeVector3(object obj)
    {
        Vector3 vector = (Vector3)obj;

        byte[] result = new byte[12];

        BitConverter.GetBytes(vector.x).CopyTo(result, 0);
        BitConverter.GetBytes(vector.y).CopyTo(result, 4);
        BitConverter.GetBytes(vector.z).CopyTo(result, 8);

        return result;
    }

    #endregion
}
