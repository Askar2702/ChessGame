using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAssassin : MonoBehaviour , IAbility
{
    public void TriggerEnter(Collider other)
    {
        // включает пассивку пешек        
        if (other.tag == "Player" && other.GetComponent<Passive>())
        {
            other.transform.GetComponent<Passive>().Miss();
        }
    }

    public void TriggerExit(Collider other)
    {
        // выключает пассивку пешек когда двигается
        if (other.tag == "Player" && other.GetComponent<Passive>())
        {
            other.GetComponent<Passive>().NoMiss();
        }
    }
}
