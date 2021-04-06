using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
   
    [SerializeField] protected Slider _slider;
    [SerializeField] protected Image _fill;
    [SerializeField] protected int _healthPlayer;
    [SerializeField] protected ParticleSystem _ellectroEffect;
    [SerializeField] protected ParticleSystem _HealthEffect;
    protected ShaderPlayers _shaderTest;
    protected Vector3 _pos = new Vector3(0, 3, 0);
    protected PhotonView _photon;
    protected Animator _animator;
    private Camera _cam;

    public int _health { get { return _healthPlayer; } set { _healthPlayer = value; } }
    protected virtual void Start()
    {
        _photon = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _slider.maxValue = _healthPlayer;
        _slider.value = _healthPlayer;
        if (_photon.IsMine)
            _fill.color = Color.green;
        else
            _fill.color = Color.red;
        _shaderTest = GetComponent<ShaderPlayers>();
        _cam = Camera.main;
        StartCoroutine(UpdateHealth());
    }

   
    

    protected void LateUpdate()
    {
        _slider.transform.position = transform.position + _pos;
        _slider.transform.LookAt(_slider.transform.position + _cam.transform.forward);
    }
    public virtual void TakeDamage(int amount , Type DamageType , Transform enemy)
    {
        if (DamageType == typeof(MagicAbility))
        {
            _ellectroEffect.Play();
        }
        StartCoroutine(DelayChangeHealth(amount));
    }
    
    protected  IEnumerator DelayChangeHealth(int amounts)
    {
        yield return new WaitForSeconds(1f);
        _healthPlayer -= amounts;
    }

    protected IEnumerator UpdateHealth()
    {
        while (true)
        {
            _slider.value = _healthPlayer;

            if (_healthPlayer <= 0 && _animator.GetInteger("State")!=3)
            {
                _animator.SetInteger("State", 3);
                StartCoroutine(_shaderTest.PlayEffectDissolve());
                transform.GetComponent<UnitManager>().ChangetAlivePlayer(false);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    public void healthPlayer()
    {
        _healthPlayer += 100;
        if (_healthPlayer > 100)
            _healthPlayer = 100;
        _HealthEffect.Play();
    }
    
}
