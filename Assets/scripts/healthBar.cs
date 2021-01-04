using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
   
    public Slider slider;
    public Vector3 pos;   
    public Image fill;
    public int health;
    protected PhotonView photon;
    public ParticleSystem ellectroEffect;
    public ParticleSystem HealthEffect;
    public Animator animator;
    protected virtual void Start()
    {
        photon = GetComponent<PhotonView>();
        if (photon.IsMine)
            fill.color = Color.green;
        else
            fill.color = Color.red;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        slider.transform.position = transform.position + pos;
        slider.transform.rotation = Quaternion.Euler(0, 0, 0);
        slider.value = health;
        if (health > 100)
            health = 100;
        if (health <= 0)
        {
            animator.SetInteger("State", 3);
            transform.GetComponent<UnitManager>().Alive = false;
        }
        
    }

    public virtual void TakeDamage(int amount , Type DamageType , Transform enemy)
    {
        if (DamageType == typeof(IAttack))
            print("good");
        else if (DamageType == typeof(MagicAbility))
        {
            ellectroEffect.Play();
        }
        StartCoroutine(print(amount));
    }
    
    protected IEnumerator print(int amounts)
    {
        yield return new WaitForSeconds(1f);
        health -= amounts;
    }

    public void healthPlayer()
    {
        health += 100;
        HealthEffect.Play();
    }
    
}
