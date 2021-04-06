using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnits : MonoBehaviour
{
    [SerializeField] protected int _radius = 1; // чтоб оперделить радиус вокруг игрока именно чтоб он находился по центру
    [SerializeField] protected int _moveCell = 3; // для пешек он как клетка хождение и поиска врага , но не для берсерка
    protected ListGrid _listGrid;
    public int[] IdForBrush { get; protected set; }//
    protected PhotonView _photon;
    protected bool isdetect; // нужен для обнаружение врагов

    protected virtual void Awake()
    {
        IdForBrush = new int[2];
        _photon = GetComponent<PhotonView>();
        _listGrid = GameObject.Find("GameManager").GetComponent<ListGrid>();
        isdetect = false;
    }
}
