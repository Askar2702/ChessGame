using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passive : healthBar , IPunObservable
{

    [SerializeField] private ParticleSystem Aura;
    private AttackeMelle baseUnits;   
    private int chance = 20; // шанс контр атаки
    public Text text;
    private int miss = 10;
    private PawnGrids _pawnGrids;
    private MovementManager movementManager;
    public GameObject misses;

    protected override void Start()
    {
        base.Start();
        baseUnits = GetComponent<AttackeMelle>();
        _pawnGrids = GetComponent<PawnGrids>();
        movementManager = GetComponent<MovementManager>();
    }

    public override void TakeDamage(int amount, Type DamageType, Transform enemy)
    {
        if (DamageType == typeof(MagicAbility))
        {
            ellectroEffect.Play();
        }
        if (photon.IsMine) 
        {
            int random = UnityEngine.Random.Range(10, 100);
            if (random <= miss) return;
            StartCoroutine(print(amount));
            if (DamageType == typeof(MagicMove)) return; 
            else
            {
                StartCoroutine(delay(enemy));
            }            
        }        
    }

    protected override void Update()
    {
        base.Update();
        text.text = $" XP : 100 \r\n Damage : {baseUnits.damage} \r\n Counterattack: {chance}% \n Miss: {miss}% ";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhotonNetwork.IsMasterClient)
                Instantiate(misses, transform.position, Quaternion.identity);
            else
                Instantiate(misses, transform.position, Quaternion.Euler(20f,180f,0f));


            // Misses.transform.position = new Vector3(transform.position.x, transform.position.y * Time.deltaTime, transform.position.z);
        }
    }

    public void Miss()
    {
        miss = 40;
    }

    public void NoMiss()
    {
        miss = 10;
    }

    // удваивает шанс контратаки
    public void ChanceContrAttack()
    {        
        chance = 40;
    }
    // возращает исходный шанс
    public void ReturnChance()
    {
        chance = 20;
    }

    IEnumerator delay(Transform enem) //заддержка контр атаки
    {
        yield return new WaitForSeconds(3f);
        if (transform.GetComponent<UnitManager>().Alive) { 
            int contAtack = UnityEngine.Random.Range(10, 100);
            if (contAtack < chance)
                transform.GetComponent<IAttack>().Attack(enem.gameObject, true); 
        }
    }

    // удваивает дамаг пешки  воин
    public void DamagPlus()
    {
        baseUnits.damage = 150;        
    }

    // приводит в норму дамаг
    public void DamagMinus()
    {
        baseUnits.damage = 50;
    }

    /// <summary>
    /// UpPower
    /// </summary>
    public void UpKnigth()
    {
        _pawnGrids.SetRadius(_radius: 2, _moveCell: 5);
        baseUnits.damage = 150;
        
        Aura.Play();

        Debug.Log("11122");
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);   
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }
}
