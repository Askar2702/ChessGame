using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text Log;
    void Start()
    {
        // здесь начинается фотон
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9000);
        PhotonNetwork.AutomaticallySyncScene = true;
        Logs(PhotonNetwork.NickName);
        PhotonNetwork.GameVersion = "1";
        if (PhotonNetwork.IsConnected) return;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Logs("Connected to master");
    }

    public void createRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }
    public void joinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Logs("Join Room");
        PhotonNetwork.LoadLevel("GameLvl");
    }
    private void Logs(string message)
    {
        Log.text += "\n";
        Log.text += message;
    }
}
