using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class King : MonoBehaviour 
{
    [SerializeField] private ParticleSystem WhiteKing;
    [SerializeField] private ParticleSystem DarkKing;
    private PhotonView photon;
    protected  void Start()
    {
        photon = GetComponent<PhotonView>();
        if (!photon.IsMine)
        {           
            DarkKing.Play();
        }
        else
            WhiteKing.Play();
    }
    


   

    
}
