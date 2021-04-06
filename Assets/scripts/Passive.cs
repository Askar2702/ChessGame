using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passive : healthBar , IPunObservable
{

    [SerializeField] private ParticleSystem _aura;
    [SerializeField] private GameObject _misses;
    [SerializeField] private Text _text;
    private AttackeMelle _baseUnits;   
    private int _chanceContrAttack = 20; // шанс контр атаки
    private int _missChance = 10;
    private PawnGrids _pawnGrids;

    protected override void Start()
    {
        base.Start();
        _baseUnits = GetComponent<AttackeMelle>();
        _pawnGrids = GetComponent<PawnGrids>();
    }

    public override void TakeDamage(int amount, Type DamageType, Transform enemy)
    { 
        if (DamageType == typeof(MagicAbility))
        {
            _ellectroEffect.Play();
        }
        if (!_photon.IsMine) return;
        if (DamageType == typeof(IAttack))
        {
            StartCoroutine(delay(enemy));
        }
        int random = UnityEngine.Random.Range(10, 100);
        if (_photon.IsMine && random <= _missChance) 
        {
            MissPlayer();
            return;
        }
        StartCoroutine(DelayChangeHealth(amount));
        
    }


    void Update()
    {
        _text.text = $" XP : 100 \r\n Damage : {_baseUnits.Damage} \r\n Counterattack: {_chanceContrAttack}% \n Miss: {_missChance}% ";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhotonNetwork.IsMasterClient)
                Instantiate(_misses, transform.position, Quaternion.identity);
            else
                Instantiate(_misses, transform.position, Quaternion.Euler(20f,180f,0f));
        }
    }

    public void MissPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
            Instantiate(_misses, transform.position, Quaternion.identity);
        else
            Instantiate(_misses, transform.position, Quaternion.Euler(20f, 180f, 0f));
        if (_photon.IsMine)
        {
            object[] content = new object[2] { (object)transform.position, (object)"MissPlayer" }; //  приходиться массивом отправлять иначе просто vector3 он не принимает отправлять
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent((byte)2, content, options, sendOptions);
        } //сипользовал канал магов


        _misses.transform.position = new Vector3(transform.position.x, transform.position.y * Time.deltaTime, transform.position.z);
    }

    public void Miss()
    {
        _missChance = 40;
    }

    public void NoMiss()
    {
        _missChance = 10;
    }

    // удваивает шанс контратаки
    public void ChanceContrAttack()
    {        
        _chanceContrAttack = 40;
    }
    // возращает исходный шанс
    public void ReturnChance()
    {
        _chanceContrAttack = 20;
    }

    IEnumerator delay(Transform enem) //заддержка контр атаки
    {
        yield return new WaitForSeconds(3f);
        if (_photon.IsMine) {
            if (transform.GetComponent<UnitManager>().isAlive)
            {
                int contAtack = UnityEngine.Random.Range(10, 100);
                if (contAtack < _chanceContrAttack)
                    transform.GetComponent<IAttack>().Attack(enem.gameObject, true);
            }
        }
    }

    // удваивает дамаг пешки  воин
    public void DamagPlus()
    {
        _baseUnits.SettingDamage(150);       
    }

    // приводит в норму дамаг
    public void DamagMinus()
    {
        _baseUnits.SettingDamage(50);
    }

    /// <summary>
    /// UpPower
    /// </summary>
    public void UpKnigth()
    {
        _pawnGrids.SetRadius(_radius: 2, _moveCell: 5);
        _baseUnits.SettingDamage(150);
        
        _aura.Play();

        Debug.Log("11122");
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_healthPlayer);   
        }
        else
        {
            _healthPlayer = (int)stream.ReceiveNext();
        }
    }
}
