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
    public PlayerState playerState;
    public GameObject menuBar; //меню бар игрока где есть кнопки атаки
    public int[] idForBrush;//
    [SerializeField] ParticleSystem effectcZnaks;
    [SerializeField] ParticleSystem SelectParticle;
    public event Action<bool> _notify;
    [SerializeField] Button EndTurn;
    [SerializeField] Button figthBTN;
    [SerializeField] Button MoveButton;
    [SerializeField] Vector3 pos;
    [SerializeField] Transform startRay;
    [SerializeField] private int id;
    public int _id => id;

    private Animator animator;
    private PhotonView photon;//
    private string EntredCell = null; // нужны чтоб когда уходил за собой выключал красный свет в клетках
    public bool Alive;//
    private IPLayerGrid pLayerGrid;
    private ListGrid listGrid;

    void Start()
    {
        playerState = PlayerState.Idle;
        listGrid = GameObject.Find("GameManager").GetComponent<ListGrid>();
        photon = GetComponent<PhotonView>();
        menuBar.SetActive(false);
        figthBTN.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        MoveButton.onClick.AddListener(() => grids());
        EndTurn.onClick.AddListener(() => EnemyMove());
        gameObject.layer = 9;
        Alive = true;
        if (!photon.IsMine)
        {
            transform.tag = "Enemy";
        }
        DataExchange._DataExchange.AddUnits(this);
        StartCoroutine("UpdateProccess");
    }

    protected void Awake()
    {
        idForBrush = new int[2];
        pLayerGrid = GetComponent<IPLayerGrid>();
        Alive = true;
    }


   

    private  void getPoint(int[] idCell) //для высчитавание левого нижнего края откуда пойдет счет границ
    {
        pLayerGrid.GetPoint(idCell);
    }
    private  void grids()
    {
        pLayerGrid.Grids();
    }
    public  void hideGrids()
    {
        pLayerGrid.HideGrids();
    }

    /// <summary>
    /// передает ход игроку и обнуляет время у всех
    /// </summary>
    public  void EnemyMove()
    {// ход врага
        if (photon.IsMine)
        {
            OffPlayer();
            GameObject.Find("GameManager").GetComponent<PlayerTurn>().time = 0;
            menuBar.SetActive(false);
            float content = 0f;
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent((byte)3, content, options, sendOptions);
            hideGrids();
        }
    }
    

    public void DetectEnemy() //проверяет где рядом враг во время анимации покоя
    {
        if (transform.tag == "Player")
        {
            if (animator.GetInteger("State") == 1)
            {
                pLayerGrid.GridsHaveEnemy();
            }
        }
    }

    public  void OffPlayer() // отключение знака мага и чтоб поменять слой дабы дорогу сделать не проходимой
    {
        gameObject.layer = 9;
        SelectParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    public  void moveBool(bool activ)
    {
        if (!Alive) return;
        if (photon.IsMine)
        {
            menuBar.SetActive(activ);
            SelectParticle.Play();
            if (activ)
            {
                gameObject.layer = 12;
            }
            else
            {
                hideGrids();
            }
        }
        _notify?.Invoke(activ);
    }

    protected void enemySignal(int[] cell)
    {
        // если он сам враг то отправит сигнал клетке чтоб он стал красным
        if (playerState == PlayerState.Idle)
        {
            EntredCell = ($"x:{cell[0]} z:{cell[1]}");
            listGrid.GrisItem(EntredCell).enemySignal(true);
        }
        else if (playerState == PlayerState.Movement)
        {
            if (EntredCell == null) return;
            listGrid.GrisItem(EntredCell).enemySignal( false); // здесь сначала отправляю фальш чтоб потом закрыть , иначе если так закрыть то каждый сможет его закрывать поэтому нужна проверка
        }
        if (!Alive)
        {
            DataExchange._DataExchange.RemoveUnits(this);
            if (EntredCell == null) return;
            listGrid.GrisItem(EntredCell).enemySignal(false);
            Destroy(this.gameObject, 5f);
        }
    }
    protected void PlayerSignal(int[] cell)
    {
        // союзный фигура отправит сигнал что клетка не свободна 
        if (gameObject.layer == 9 && playerState == PlayerState.Idle)
        {
            EntredCell = ($"x:{cell[0]} z:{cell[1]}");
            listGrid.GrisItem(EntredCell).PlayerSignal(true);
        }
        else if (gameObject.layer == 12 || playerState == PlayerState.Movement)
        {
            if (EntredCell == null) return;
            listGrid.GrisItem(EntredCell).PlayerSignal( false); // здесь сначала отправляю фальш чтоб потом закрыть , иначе если так закрыть то каждый сможет его закрывать поэтому нужна проверка
        }
        
        if (!Alive)
        {
            DataExchange._DataExchange.RemoveUnits(this);
            if (EntredCell == null) return;
            listGrid.GrisItem(EntredCell).PlayerSignal(false); // здесь он дохнет
            Destroy(this.gameObject, 5f);

        }

    }

    public void effectOn()
    {
        effectcZnaks.Play();
    }
    public void effectOff()
    {
        effectcZnaks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private IEnumerator UpdateProccess()
    {
        while (true)
        {
            Ray ray = new Ray(startRay.position, -transform.up);
            RaycastHit hit;
            figthBTN.transform.position = transform.position + pos;//для кнопки иначе она не пашет нормально

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.transform.tag == "grid")
                {
                    idForBrush = hit.transform.GetComponent<gridsPrefab>().newID;
                    getPoint(idForBrush); //луч который оперделяет место нахождение 
                    if (photon.IsMine)
                        PlayerSignal(idForBrush);
                    if (!photon.IsMine)
                    {
                        enemySignal(idForBrush);//луч который делает красным там где он есть если она сам враг
                        if (hit.transform.GetComponent<gridsPrefab>().mat.material.GetColor("_EmissionColor") == Color.red * 1.3f)
                        {
                            figthBTN.gameObject.SetActive(true);
                        }
                        else
                            figthBTN.gameObject.SetActive(false);
                    }


                }


                Debug.DrawRay(startRay.position, -transform.up, Color.red, 20);
                // Debug.Log("1");
            }
            if (PhotonNetwork.IsMasterClient)
                figthBTN.transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                figthBTN.transform.rotation = Quaternion.Euler(0, 180f, 0);
            yield return new WaitForSeconds(1);
        }
    }


    /// <summary>
    /// присвоение id к каждому игроку с помощью которого будет обмен данными 
    /// </summary>
    /// <param name="_idPlayer"></param>
    public void IDAssignment(int _idPlayer)
    {
        id = _idPlayer;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) { 
        
            stream.SendNext(id);
        }
        else
        {
            id = (int)stream.ReceiveNext();
        }
    }
}
   


public enum PlayerState { Idle , Movement , Attack}
