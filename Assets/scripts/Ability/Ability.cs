﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private IMagicAbility magicAbility;
    private PhotonView photon;
    private UnitManager unitManager;

    private void Start()
    {
        magicAbility = GetComponent<IMagicAbility>(); 
        photon = GetComponent<PhotonView>();
        unitManager = GetComponent<UnitManager>();
    }

    /// <summary>
    /// Health
    /// </summary>
    public void Ability1()
    {
        magicAbility.Ability_1();
        if (photon.IsMine) Events(transform.position, "Ability1");
    }


    /// <summary>
    /// Attack enemy
    /// </summary>
    public void Ability2()
    {
        magicAbility.Ability_2();
        if (photon.IsMine) Events(transform.position, "Ability2");
    }

    /// <summary>
    /// UpPower
    /// </summary>
    public void Ability3()
    {
        magicAbility.Ability_3();
        if (photon.IsMine) Events(transform.position, "Ability3");
    }

    /// <summary>
    /// sends a message of its copies about the functions that will work
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Method"></param>
    private void Events(Vector3 pos, string Method)
    {
        object[] content = new object[2] { (object)pos, (object)Method }; //  приходиться массивом отправлять иначе просто vector3 он не принимает отправлять
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)2, content, options, sendOptions);
        unitManager.EnemyMove();
    }
}