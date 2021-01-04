using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBersek : MonoBehaviour , IAbility
{
    public void TriggerEnter(Collider other)
    {
        // включает пассивку пешек        
        if (other.tag == "Player" && other.GetComponent<Passive>())
        {
            other.transform.GetComponent<Passive>().ChanceContrAttack();
        }
    }

    public void TriggerExit(Collider other)
    {
        // выключает пассивку пешек когда двигается
        if (other.tag == "Player" && other.GetComponent<Passive>())
        {
            other.GetComponent<Passive>().ReturnChance();
        }
    }
}
