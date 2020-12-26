using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public Text ResultGame;

    private void Start()
    {
        if (GameObject.Find("OnlineManager") == null) return;
        if (GameObject.Find("OnlineManager").GetComponent<onlineManager>().Win)
        {
            ResultGame.text = "You Win!!!";
        }
        else
            ResultGame.text = "You Lose!!!";
    }
    public void Leave()
    {
        SceneManager.LoadScene("Menu");
    }
}
