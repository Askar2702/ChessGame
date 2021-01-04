using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public PlayerState playerState;
    public GameObject menuBar; //меню бар игрока где есть кнопки атаки
    public int[] idForBrush;//
    public ParticleSystem effectcZnaks;
    public event Action<bool> _notify;
    [SerializeField] Button EndTurn;
    [SerializeField] Button figthBTN;
    [SerializeField] Button MoveButton;
    [SerializeField] Vector3 pos;
    [SerializeField] Transform startRay;
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
        InvokeRepeating("_UpadateProcess", 0f, 0.05f);
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
    }

    protected void Awake()
    {
        idForBrush = new int[2];
        pLayerGrid = GetComponent<IPLayerGrid>();
        Alive = true;
    }




    public virtual void gridsHaveEnemy(int[] idGrids)
    {
        pLayerGrid.GridsHaveEnemy(idGrids);
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
            GameObject.Find("GameManager").GetComponent<PlayerTurn>().time = 0;
            menuBar.SetActive(false);
            float content = 0f;
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent((byte)3, content, options, sendOptions);
            hideGrids();
        }
    }
    protected virtual void _UpadateProcess() //нужен чтоб оптимизировать и не нагружать апдеит
    {
        Ray ray = new Ray(startRay.position, -transform.up);
        RaycastHit hit;
        figthBTN.transform.position = transform.position + pos;//для кнопки иначе она не пашет нормально

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.tag == "grid")
            {
                getPoint(hit.transform.GetComponent<gridsPrefab>().Id); //луч который оперделяет место нахождение 
                if (photon.IsMine)
                    PlayerSignal(hit.transform.GetComponent<gridsPrefab>().Id);
                if (!photon.IsMine)
                {
                    enemySignal(hit.transform.GetComponent<gridsPrefab>().Id);//луч который делает красным там где он есть если она сам враг
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
    }

    public void DetectEnemy() //проверяет где рядом враг во время анимации покоя
    {
        if (transform.tag == "Player")
        {
            if (animator.GetInteger("State") == 1)
            {
                gridsHaveEnemy(idForBrush);
            }
        }
    }

    public  void OffPlayer() // отключение знака мага и чтоб поменять слой дабы дорогу сделать не проходимой
    {
        gameObject.layer = 9;
    }
    public  void moveBool(bool activ)
    {
        if (!Alive) return;
        if (photon.IsMine)
        {
            menuBar.SetActive(activ);
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
            listGrid.GrisItem($"x:{cell[0]} z:{cell[1]}").enemySignal(true);
          //  GameObject.Find($"x:{cell[0]} z:{cell[1]}").SendMessage("enemySignal", true);
            EntredCell = ($"x:{cell[0]} z:{cell[1]}");
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
        if (gameObject.layer == 9)
        {
            listGrid.GrisItem($"x:{cell[0]} z:{cell[1]}").PlayerSignal(true);
          //  GameObject.Find($"x:{cell[0]} z:{cell[1]}").SendMessage("PlayerSignal", true);
            EntredCell = ($"x:{cell[0]} z:{cell[1]}");
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

   
}

public enum PlayerState { Idle , Movement , Attack}
