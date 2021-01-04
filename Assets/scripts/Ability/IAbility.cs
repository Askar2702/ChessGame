using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility 
{
    // Passive Assasin and Berserk
    void TriggerEnter(Collider other);
    void TriggerExit(Collider other);
}
