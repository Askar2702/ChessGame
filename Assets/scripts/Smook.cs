using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smook : MonoBehaviour
{
    public ParticleSystem ChildParticle;
    private Rigidbody rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       // StartCoroutine(StopChildParticle());
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        if(transform.position.y!=0f)
            rb.velocity = new Vector3(0, transform.position.y * speed, 0);
    }

    IEnumerator StopChildParticle()
    {
        yield return new WaitForSeconds(6f);
        ChildParticle.Stop();
    }
}
