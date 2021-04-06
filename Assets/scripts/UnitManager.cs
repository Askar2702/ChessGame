using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour , IPunObservable
{
    [SerializeField] private ParticleSystem _effectcZnaks;
    [SerializeField] private ParticleSystem _SelectParticle;
    [SerializeField] private Button _endTurn;
    [SerializeField] private Button _figthBTN;
    [SerializeField] private Button _moveButton;
    [SerializeField] private Transform _startRay;
    [SerializeField] private int _id; // отображает в редакторе сетевой  id игрока
    [SerializeField] private GameObject _canvasMenuBar; //меню бар игрока где есть кнопки атаки
    public PlayerState _playerState { get; private set; }
    public int[] idForBrush { get; private set; }
    public int Id => _id;

    public event Action<bool> _notify;
    public bool isAlive { get; private set; }
    private Vector3 _posistionfigthBTN = new Vector3(0f, 5f, 0f);

    private Animator _animator;
    private PhotonView _photon;
    private string _entredCell = null; // нужны чтоб когда уходил за собой выключал красный свет в клетках
    private IPLayerGrid _pLayerGrid;
    private ListGrid _listGrid;

    void Start()
    {
        ChangetStatusPlayer(PlayerState.Idle);
        _listGrid = GameObject.Find("GameManager").GetComponent<ListGrid>();
        _photon = GetComponent<PhotonView>();
        _canvasMenuBar.SetActive(false);
        _figthBTN.gameObject.SetActive(false);
        _animator = GetComponent<Animator>();
        _moveButton.onClick.AddListener(() => grids());
        _endTurn.onClick.AddListener(() => EnemyMove());
        gameObject.layer = 9;
        isAlive = true;
        if (!_photon.IsMine)
        {
            transform.tag = "Enemy";
        }
        DataExchange.DataExchangeCenter.AddUnits(this);
        StartCoroutine("UpdateProccess");
    }

    protected void Awake()
    {
        idForBrush = new int[2];
        _pLayerGrid = GetComponent<IPLayerGrid>();
        isAlive = true;
    }


    public void ChangetStatusPlayer(PlayerState playerState)
    {
        _playerState = playerState;
    }

    private  void getPoint(int[] idCell) //для высчитавание левого нижнего края откуда пойдет счет границ
    {
        _pLayerGrid.GetPoint(idCell);
    }
    private  void grids()
    {
        _pLayerGrid.Grids();
    }
    public  void hideGrids()
    {
        _pLayerGrid.HideGrids();
    }

    /// <summary>
    /// передает ход игроку и обнуляет время у всех
    /// </summary>
    public  void EnemyMove()
    {// ход врага
        if (_photon.IsMine)
        {
            OffPlayer();
            GameObject.Find("GameManager").GetComponent<PlayerTurn>().SettingTime(0);
            _canvasMenuBar.SetActive(false);
            float content = 0f;
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent((byte)3, content, options, sendOptions);
            hideGrids();
        }
    }
    
    public void ChangetAlivePlayer(bool activ)
    {
        isAlive = activ;
    }

    public void DetectEnemy() //проверяет где рядом враг во время анимации покоя
    {
        if (transform.tag == "Player")
        {
            if (_animator.GetInteger("State") == 1)
            {
                _pLayerGrid.GridsHaveEnemy();
            }
        }
    }

    public  void OffPlayer() // отключение знака мага и чтоб поменять слой дабы дорогу сделать не проходимой
    {
        gameObject.layer = 9;
        _SelectParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    public  void moveBool(bool activ)
    {
        if (!isAlive) return;
        if (_photon.IsMine)
        {
            _canvasMenuBar.SetActive(activ);
            if (activ)
            {
                _SelectParticle.Play();
                gameObject.layer = 12;
            }
            else
            {
                hideGrids();
                OffPlayer();
            }
        }
        _notify?.Invoke(activ);
    }

    protected void enemySignal(int[] cell)
    {
        // если он сам враг то отправит сигнал клетке чтоб он стал красным
        if (_playerState == PlayerState.Idle)
        {
            _entredCell = ($"x:{cell[0]} z:{cell[1]}");
            _listGrid.GrisItem(_entredCell).enemySignal(true);
        }
        else if (_playerState == PlayerState.Movement)
        {
            if (_entredCell == null) return;
            _listGrid.GrisItem(_entredCell).enemySignal( false); // здесь сначала отправляю фальш чтоб потом закрыть , иначе если так закрыть то каждый сможет его закрывать поэтому нужна проверка
        }
        if (!isAlive)
        {
            DataExchange.DataExchangeCenter.RemoveUnits(this);
            if (_entredCell == null) return;
            _listGrid.GrisItem(_entredCell).enemySignal(false);
            Destroy(this.gameObject, 5f);
        }
    }
    protected void PlayerSignal(int[] cell)
    {
        // союзный фигура отправит сигнал что клетка не свободна 
        if (gameObject.layer == 9 && _playerState == PlayerState.Idle)
        {
            _entredCell = ($"x:{cell[0]} z:{cell[1]}");
            _listGrid.GrisItem(_entredCell).PlayerSignal(true);
        }
        else if (gameObject.layer == 12 || _playerState == PlayerState.Movement)
        {
            if (_entredCell == null) return;
            _listGrid.GrisItem(_entredCell).PlayerSignal( false); // здесь сначала отправляю фальш чтоб потом закрыть , иначе если так закрыть то каждый сможет его закрывать поэтому нужна проверка
        }
        
        if (!isAlive)
        {
            DataExchange.DataExchangeCenter.RemoveUnits(this);
            if (_entredCell == null) return;
            _listGrid.GrisItem(_entredCell).PlayerSignal(false); // здесь он дохнет
            Destroy(this.gameObject, 5f);

        }

    }

    public void effectOn()
    {
        _effectcZnaks.Play();
    }
    public void effectOff()
    {
        _effectcZnaks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private IEnumerator UpdateProccess()
    {
        while (true)
        {
            Ray ray = new Ray(_startRay.position, -transform.up);
            RaycastHit hit;
            _figthBTN.transform.position = transform.position + _posistionfigthBTN;//для кнопки иначе она не пашет нормально

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.transform.tag == "grid")
                {
                    idForBrush = hit.transform.GetComponent<gridsPrefab>().NewID;
                    getPoint(idForBrush); //луч который оперделяет место нахождение 
                    if (_photon.IsMine)
                        PlayerSignal(idForBrush);
                    if (!_photon.IsMine)
                    {
                        enemySignal(idForBrush);//луч который делает красным там где он есть если она сам враг
                        if (hit.transform.GetComponent<gridsPrefab>().Material.material.GetColor("_EmissionColor") == Color.red * 1.3f)
                        {
                            _figthBTN.gameObject.SetActive(true);
                        }
                        else
                            _figthBTN.gameObject.SetActive(false);
                    }


                }


                Debug.DrawRay(_startRay.position, -transform.up, Color.red, 20);
                // Debug.Log("1");
            }
            if (PhotonNetwork.IsMasterClient)
                _figthBTN.transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                _figthBTN.transform.rotation = Quaternion.Euler(0, 180f, 0);
            yield return new WaitForSeconds(1);
        }
    }


    /// <summary>
    /// присвоение id к каждому игроку с помощью которого будет обмен данными 
    /// </summary>
    /// <param name="idPlayer"></param>
    public void IDAssignment(int idPlayer)
    {
        _id = idPlayer;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) { 
        
            stream.SendNext(_id);
        }
        else
        {
            _id = (int)stream.ReceiveNext();
        }
    }
}
   


public enum PlayerState { Idle , Movement , Attack}
