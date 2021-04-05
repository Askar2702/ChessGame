using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShaderPlayers : MonoBehaviour
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
        {
            mat.material.SetFloat("_time", time);
            if (material.LastOrDefault().material.GetFloat("_time") >= 1)
            {
                isStart = false;
                time = 0;
            }
        }
    }

    public IEnumerator PlayEffectDissolve()
    {
        yield return new WaitForSeconds(2f); 
        isStart = true;
    } 
}
