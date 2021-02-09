using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridsPrefab : MonoBehaviour
{
    
    public Renderer mat { get; private set; }
    public int[] newID { get; private set; }
    public bool HaveEnemy { get; private set; }
    public bool HavePlayer { get; private set; } // чтоб на союзные терретории не заходить 
    public bool _HaveEnemy;
    public bool _HavePlayer;



    private void Awake()
    {
     //   Id = new int[2];
    }
    void Start()
    {
        mat = GetComponent<MeshRenderer>();
        hideGrids();
        HaveEnemy = false;
        HavePlayer = false;
    }

  /*  private void Update()
    {
        _HaveEnemy = HaveEnemy;
        _HavePlayer = HavePlayer;
    }
  */
    public void GridGreen()
    {
        if (!HaveEnemy && !HavePlayer) { 
            mat.material.SetColor("_EmissionColor", Color.green * 1);// для коррекций яркости          
        }
        
      
    }
    public void hideGrids()
    {
       mat.material.SetColor("_EmissionColor", Color.white * 1.3f);// для коррекций яркости
    }


   public void enemySignal(bool haveEnemys) // получет от врага сигнал что он стоит на нем 
   {
        HaveEnemy = haveEnemys;
        if (!haveEnemys) hideGrids();
   }

   

    public void id(int[] ids) // эта функция вызвается при спавне  чтоб дать ему айди
    {
      //  Id = ids;
        newID = ids;       
    }
    public void haveEnemy()
    {
        if (HaveEnemy) mat.material.SetColor("_EmissionColor", Color.red * 1.3f);
        else
            return;

    }
    public void PlayerSignal(bool haveplayer) // получет от врага сигнал что он стоит на нем 
    {
        HavePlayer = haveplayer;        
    }
}
