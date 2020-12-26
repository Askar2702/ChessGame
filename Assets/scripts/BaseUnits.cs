using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseUnits : MonoBehaviour
{
    protected Animator animator;
    protected Transform enemy;
    public int damage;
    public NavMeshAgent agent;
    public int Radius = 1; // чтоб оперделить радиус вокруг игрока именно чтоб он находился по центру
    public int MoveCell = 3; // для пешек он как клетка хождение и поиска врага , но не для берсерка
    public Vector3 target { get; set; }
    protected bool direction;
    protected PhotonView photon;

    public Transform startRay;
    public GameObject menuBar; //меню бар игрока где есть кнопки атаки
    public int[] idForBrush;
    protected bool Move;
    protected string EntredCell = null; // нужны чтоб когда уходил за собой выключал красный свет в клетках
    public Button figthBTN;
    public Vector3 pos;
    public bool Alive;
    protected int state; // для анимации какая будет играть 
    protected bool detect; // нужен для обнаружение врагов
    public bool can;
    public ParticleSystem effectcZnaks;

    protected virtual void Start()
    {        
        if (PhotonNetwork.IsMasterClient) direction = true;
        else direction = false;
        photon = GetComponent<PhotonView>();
        menuBar.SetActive(false);
        figthBTN.gameObject.SetActive(false);
        InvokeRepeating("uiBtnFigt", 0f, 0.05f);
        animator = GetComponent<Animator>();
        target = transform.position;
        transform.position = target;
        detect = false;
        state = 1;
        gameObject.layer = 9;
        Move = true;
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
    }
    
    protected void Update()
    {
        can = PlayerTurn.CanPlay;
        if (Alive)
        {

            if (enemy == null) return; // здесь идет поворот к врагу потом при движении враг обнуляется
                                       // Determine which direction to rotate towards
            Vector3 targetDirection = enemy.position - transform.position;
            // The step size is equal to speed times frame time.
            float singleStep = 10f * Time.deltaTime;
            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

        }
    }


    public virtual void MovePoint(Transform point)
    {
        target = point.GetChild(0).transform.position;
        state = 2;
        Move = true;
        if(photon.IsMine)
            PlayerTurn.CanPlay = false;
        

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
    protected virtual void gridsHaveEnemy(int [] idGrids)
    {
        if (!detect)
        {           
            for (int i = 0; i < MoveCell; i++) //ищет у клеток есть ли рядом враги
            {
                for (int j = 0; j < MoveCell; j++)
                {
                    if (GameObject.Find($"x:{idGrids[0] + i} z:{idGrids[1] + j}") == null)
                    {
                        continue;
                    }
                    else
                    {
                        GameObject.Find($"x:{idGrids[0] + i} z:{idGrids[1] + j}").SendMessage("haveEnemy");
                    }
                   
                }
            }
            detect = true;
        }
        else
        {
            hideGrids();
            detect = false;
        }

    }

    public virtual void grids()
    {
        if (!PlayerTurn.CanPlay) return;
        for (int i = 0; i < MoveCell; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        {
            for (int j = 0; j < MoveCell; j++)
            {
                if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                {
                    continue;
                    // print($"{transform.name}x:{i} z:{j}");
                }
                else
                    GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("GridGreen");

            }
        }

        // чтоб потом закрывать клетки пры вызове

    }
    protected void FixedUpdate()
    {
        if (Alive)
        {
            if (Vector3.Distance(transform.position, target) > 0.2f)
            {
                agent.SetDestination(target);
                enemy = null; // чтоб не смотрел на врага
            }
            else if (Vector3.Distance(transform.position, target) < 0.2f || transform.position == target)
            {

                if (photon.IsMine)
                {
                    state = 1;
                }
                PositionCentralization(); //выравнивание позиции 


            }
            animator.SetInteger("State", state);            
           
        }

    }
    protected virtual void PositionCentralization()
    {
        transform.position = target;
        if (photon.IsMine)
        {
            if (enemy != null) return;
            if (direction)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }


    }
    public virtual void attack(GameObject enemyTarget , bool contrAttack)
    {
        enemy = enemyTarget.transform; // для ближнего боя нужен это
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
        animator.SetTrigger("Attack");
        print(this.GetType());
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            string[] content = new string[] { transform.name, enemyTarget.transform.name };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            gridsHaveEnemy(idForBrush);
            EnemyMove();
        }
    }

    public virtual void moveBool()
    {
        if (!Alive) return;
        if (photon.IsMine)
        {
            if (Move)
            {
                menuBar.SetActive(true);
                Move = false;
                gameObject.layer = 12;
            }
            else
            {
                hideGrids();
                Move = true;
                menuBar.SetActive(false);               
            }

        }
        // Debug.Log("lal");
    } 
    
    public virtual void hideGrids()
    {

    }


    /// <summary>
    /// передает ход игроку и обнуляет время у всех
    /// </summary>
    public virtual void EnemyMove()
    {// ход врага
        if (photon.IsMine)
        {             
            GameObject.Find("GameManager").GetComponent<PlayerTurn>().time = 0;
            menuBar.SetActive(false);
            float content = 0f;
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };            
            PhotonNetwork.RaiseEvent((byte)3,content , options, sendOptions);
            hideGrids();
        }
    }
    protected virtual void uiBtnFigt() //нужен чтоб оптимизировать и не нагружать апдеит
    {
        Ray ray = new Ray(startRay.position, -transform.up);
        RaycastHit hit;
        figthBTN.transform.position = transform.position + pos;//для кнопки иначе она не пашет нормально

        if (Physics.Raycast(ray, out hit, 1f ))
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

    protected virtual void getPoint(int[] idCell) //для высчитавание левого нижнего края откуда пойдет счет границ
    {
        idForBrush[0] = idCell[0];
        idForBrush[1] = idCell[1];
        idForBrush[0] -= Radius;
        idForBrush[1] -= Radius;

    }

    public virtual void OffPlayer() // отключение знака мага и чтоб поменять слой дабы дорогу сделать не проходимой
    {
        gameObject.layer = 9;
    } 
    protected void enemySignal(int[] cell)
    {
        // если он сам враг то отправит сигнал клетке чтоб он стал красным
        if (animator.GetInteger("State") == 1)
        {
            GameObject.Find($"x:{cell[0]} z:{cell[1]}").SendMessage("enemySignal", true);
            EntredCell = ($"x:{cell[0]} z:{cell[1]}");
        }
        else if (animator.GetInteger("State") == 2)
        {
            if (EntredCell == null) return;
            GameObject.Find(EntredCell).SendMessage("enemySignal", false); // здесь сначала отправляю фальш чтоб потом закрыть , иначе если так закрыть то каждый сможет его закрывать поэтому нужна проверка
            GameObject.Find(EntredCell).SendMessage("hideGrids");            
        }
        if (!Alive)
        {
            DataExchange._DataExchange.RemoveUnits(this);
            if (EntredCell == null) return;
            GameObject.Find(EntredCell).SendMessage("enemySignal", false); 
            GameObject.Find(EntredCell).SendMessage("hideGrids");
            Destroy(this.gameObject, 5f);
            
        }
    }
    protected void PlayerSignal(int[] cell)
    {
        // союзный фигура отправит сигнал что клетка не свободна 
        if (gameObject.layer == 9)
        {
            GameObject.Find($"x:{cell[0]} z:{cell[1]}").SendMessage("PlayerSignal", true);
            EntredCell = ($"x:{cell[0]} z:{cell[1]}");
        }
        else if (gameObject.layer ==12 )
        {
            if (EntredCell == null) return;
            GameObject.Find(EntredCell).SendMessage("PlayerSignal", false); // здесь сначала отправляю фальш чтоб потом закрыть , иначе если так закрыть то каждый сможет его закрывать поэтому нужна проверка
        }
        if (animator.GetInteger("State") == 2)
        {
            if (EntredCell == null) return;
            GameObject.Find(EntredCell).SendMessage("PlayerSignal", false);
        }
        if (!Alive)
        {
            DataExchange._DataExchange.RemoveUnits(this);
            if (EntredCell == null) return;
            GameObject.Find(EntredCell).SendMessage("PlayerSignal", false); // здесь он дохнет
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
