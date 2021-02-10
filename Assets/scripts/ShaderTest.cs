using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    [SerializeField] private Renderer [] material;
    [SerializeField]
    [Range(0.0f , 1.0f)]
    private float time;
    private UnitManager unitManager;
    bool isStart = false;
    void Start()
    {
        unitManager = GetComponent<UnitManager>();
    }

    private void Update()
    {
        if (!isStart) return;
        time += Time.deltaTime;
        foreach (var mat in material)
            mat.material.SetFloat("_time", time);
    }


    public IEnumerator PlayEffectDissolve()
    {
        yield return new WaitForSeconds(2f);
        isStart = true;
    } 
}
