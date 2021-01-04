using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
    IAbility abilityPassive;
    private PhotonView photon;
   
    private void Start()
    {
        abilityPassive = GetComponent<IAbility>();
        photon = GetComponent<PhotonView>();
        if (photon.IsMine)
        {
            var child = transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
            child.center = new Vector3(0, 1, 0);
            child.size = new Vector3(4, 1, 4);
            child.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        abilityPassive.TriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        abilityPassive.TriggerExit(other);
    }

}
