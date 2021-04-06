using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffects : MonoBehaviour
{
    [SerializeField] private Color _colorAlphaTrue;
    [SerializeField] private Color _colorAlphaFalse;
    [SerializeField]private Renderer _mat;
    [SerializeField] private ParticleSystem[] _matParticle;
    [SerializeField] [Range(0f, 1f)] float _lerpTime;
    private Color _Matcolor;

    [System.Obsolete]
    private void OnEnable()
    {
        _mat.material.SetColor("_EmissionColor", _colorAlphaTrue * 4);
        _mat.material.SetColor("_BaseColor", _colorAlphaTrue);
        _Matcolor = _colorAlphaTrue;
        foreach (var particle in _matParticle)
            particle.startColor = _colorAlphaTrue;
    }
    [System.Obsolete]
    void Update()
    {
        if (_Matcolor != _colorAlphaFalse)
        {
            _Matcolor = Color32.Lerp(_Matcolor, _colorAlphaFalse, _lerpTime * Time.deltaTime);
            foreach (var particle in _matParticle)
                particle.startColor = Color32.Lerp(particle.startColor, _colorAlphaFalse, _lerpTime * Time.deltaTime);
            _mat.material.SetColor("_BaseColor", _Matcolor);
            _mat.material.SetColor("_EmissionColor", _Matcolor * 4);
        }
        else
            gameObject.SetActive(false);
        
    }
}
