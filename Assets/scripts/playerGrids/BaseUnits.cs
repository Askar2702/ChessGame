using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnits : MonoBehaviour
{
    [SerializeField] protected int Radius = 1; // чтоб оперделить радиус вокруг игрока именно чтоб он находился по центру
    [SerializeField] protected int MoveCell = 3; // для пешек он как клетка хождение и поиска врага , но не для берсерка
    protected int[] idForBrush;//
    protected PhotonView photon;
    protected bool detect; // нужен для обнаружение врагов

    protected virtual void Awake()
    {
        idForBrush = new int[2];
        photon = GetComponent<PhotonView>();
        detect = false;
    }
}
