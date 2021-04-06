using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class King : MonoBehaviour 
{
    [SerializeField] private ParticleSystem _whiteKing;
    [SerializeField] private ParticleSystem _darkKing;
    private PhotonView _photon;
    protected  void Start()
    {
        _photon = GetComponent<PhotonView>();
        if (!_photon.IsMine)
        {           
            _darkKing.Play();
        }
        else
            _whiteKing.Play();
    }
    


   

    
}
