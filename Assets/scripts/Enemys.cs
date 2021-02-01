using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
    IAttack attack;
    public void attackMe()
    {
        //отправляет сообщение о месте нахождение чтоб атаковали себя при нажатие кнопки      
        attack = GameObject.Find("GameManager").GetComponent<SelectManager>().SelectPlayer.GetComponent<IAttack>();
        attack.Attack(gameObject, false);
    }
}
