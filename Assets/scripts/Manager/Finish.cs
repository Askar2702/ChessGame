using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [SerializeField] private Text _resultGame;

    private void Start()
    {
        if (!onlineManager.onlineManagers) return;
        if (onlineManager.onlineManagers.isWin)
        {
            _resultGame.text = "You Win!!!";
        }
        else
            _resultGame.text = "You Lose!!!";
    }
    public void Leave()
    {
        SceneManager.LoadScene("Menu");
    }
}
