using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWarrior : MonoBehaviour, IAbility
{
    public void TriggerEnter(Collider other)
    {
        // включает пассивку пешек        
        if (other.tag == transform.tag && other.GetComponent<Passive>())
        {
            other.transform.GetComponent<Passive>().DamagPlus();
        }
    }

    public void TriggerExit(Collider other)
    {
        // выключает пассивку пешек когда двигается
        if (other.tag == transform.tag && other.GetComponent<Passive>())
        {
            other.GetComponent<Passive>().DamagMinus();
        }
    }
}
