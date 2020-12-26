using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * 2 * Time.deltaTime, Space.World);
        Destroy(this.gameObject, 2f);
    }
}
