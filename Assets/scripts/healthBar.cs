using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
   
    [SerializeField] protected Slider slider;
    [SerializeField] protected Vector3 pos;
    [SerializeField] protected Image fill;
    [SerializeField] protected int health;
    [SerializeField] protected PhotonView photon;
    [SerializeField] protected ParticleSystem ellectroEffect;
    [SerializeField] protected ParticleSystem HealthEffect;
    [SerializeField] protected Animator animator;
    private Camera cam;

    public int _health { get { return health; } set { health = value; } }
    protected virtual void Start()
    {
        photon = GetComponent<PhotonView>();
        slider.maxValue = health;
        slider.value = health;
        if (photon.IsMine)
            fill.color = Color.green;
        else
            fill.color = Color.red;
        cam = Camera.main;
    }

   
    

    protected void LateUpdate()
    {
        slider.transform.position = transform.position + pos;
        slider.transform.LookAt(slider.transform.position + cam.transform.forward);
    }
    public virtual void TakeDamage(int amount , Type DamageType , Transform enemy)
    {
        if (DamageType == typeof(IAttack))
            print("good");
        else if (DamageType == typeof(MagicAbility))
        {
            ellectroEffect.Play();
        }
        StartCoroutine(DelayChangeHealth(amount));
    }
    
    protected IEnumerator DelayChangeHealth(int amounts)
    {
        yield return new WaitForSeconds(1f);
        health -= amounts; 
        slider.value = health;
        if (health <= 0)
        {
            animator.SetInteger("State", 3);
            transform.GetComponent<UnitManager>().Alive = false;
        }
    }

    public void healthPlayer()
    {
        health += 100;
        if (health > 100)
            health = 100;
        HealthEffect.Play();
    }
    
}
