using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShaderPlayers : MonoBehaviour
{
    [SerializeField] private Renderer[] _material;

    private float _time = 0;
    bool isStart = false;
   

    private void Update()
    {
        if (!isStart) return;
        _time += Time.deltaTime;
        foreach (var mat in _material)
        {
            mat.material.SetFloat("_time", _time);
            if (_material.LastOrDefault().material.GetFloat("_time") >= 1)
            {
                isStart = false;
                _time = 0;
            }
        }
    }

    public IEnumerator PlayEffectDissolve()
    {
        yield return new WaitForSeconds(2f); 
        isStart = true;
    } 
}
