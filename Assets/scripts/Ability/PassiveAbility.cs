using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
    private IAbility _abilityPassive;
    private PhotonView _photon;
   
    private void Start()
    {
        _abilityPassive = GetComponent<IAbility>();
        _photon = GetComponent<PhotonView>();
        if (_photon.IsMine)
        {
            var child = transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
            child.center = new Vector3(0, 1, 0);
            child.size = new Vector3(4, 1, 4);
            child.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _abilityPassive.TriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _abilityPassive.TriggerExit(other);
    }

}
