using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
    public void attackMe()
    {
        //отправляет сообщение о месте нахождение чтоб атаковали себя при нажатие кнопки      
        GameObject.Find("GameManager").GetComponent<SelectManager>().SelectPlayer.GetComponent<BaseUnits>().attack(gameObject , false);      
        
    }
}
