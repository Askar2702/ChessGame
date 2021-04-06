using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridsPrefab : MonoBehaviour
{
    
    public Renderer Material { get; private set; }
    public int[] NewID { get; private set; }
    public bool HaveEnemy { get; private set; }
    public bool HavePlayer { get; private set; } // чтоб на союзные терретории не заходить 
   



   
    void Start()
    {
        Material = GetComponent<MeshRenderer>();
        hideGrids();
        HaveEnemy = false;
        HavePlayer = false;
    }

  
    public void GridGreen()
    {
        if (!HaveEnemy && !HavePlayer) { 
            Material.material.SetColor("_EmissionColor", Color.green * 1);// для коррекций яркости          
        }
        
      
    }
    public void hideGrids()
    {
       Material.material.SetColor("_EmissionColor", Color.white * 1.3f);// для коррекций яркости
    }


   public void enemySignal(bool haveEnemys) // получет от врага сигнал что он стоит на нем 
   {
        HaveEnemy = haveEnemys;
        if (!haveEnemys) hideGrids();
   }

   

    public void id(int[] ids) // эта функция вызвается при спавне  чтоб дать ему айди
    {
        NewID = ids;       
    }
    public void haveEnemy()
    {
        if (HaveEnemy) Material.material.SetColor("_EmissionColor", Color.red * 1.3f);
        else
            return;

    }
    public void PlayerSignal(bool haveplayer) // получет от врага сигнал что он стоит на нем 
    {
        HavePlayer = haveplayer;        
    }
}
