using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffects : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Color _color;
    private Color _Matcolor;
    [SerializeField]private Renderer mat;
    [SerializeField] private ParticleSystem[] matParticle;
    [SerializeField] [Range(0f, 1f)] float lerpTime;

    [System.Obsolete]
    private void OnEnable()
    {
        mat.material.SetColor("_EmissionColor", color * 4);
        mat.material.SetColor("_BaseColor", color);
        _Matcolor = color;
        foreach (var particle in matParticle)
            particle.startColor = color;
    }
    [System.Obsolete]
    void Update()
    {
        if (_Matcolor != _color)
        {
            _Matcolor = Color32.Lerp(_Matcolor, _color, lerpTime * Time.deltaTime);
            foreach (var particle in matParticle)
                particle.startColor = Color32.Lerp(particle.startColor, _color, lerpTime * Time.deltaTime);
            mat.material.SetColor("_BaseColor", _Matcolor);
            mat.material.SetColor("_EmissionColor", _Matcolor * 4);
        }
        else
            gameObject.SetActive(false);
        
    }
}
