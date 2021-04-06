using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miss : MonoBehaviour
{
   
    void Update()
    {
        transform.Translate(transform.up * 2 * Time.deltaTime, Space.World);
        Destroy(this.gameObject, 2f);
    }
}
