using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
    private IAttack _attack;
    public void attackMe()
    {
        //отправляет сообщение о месте нахождение чтоб атаковали себя при нажатие кнопки      
        _attack = GameObject.Find("GameManager").GetComponent<SelectManager>().SelectPlayer.GetComponent<IAttack>();
        _attack.Attack(gameObject, false);
    }
}
