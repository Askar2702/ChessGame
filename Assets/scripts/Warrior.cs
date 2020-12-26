using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Warrior : BaseUnits , IPunObservable
{
    public int radiusMove;    // это радиус ходьбы вместо родительского который был и для драки и для ходьбы
    private int[] move; // для его ходьбы  сохраняет его позицию для ходьбы вместо родительского который для боя нужен    
    
   
    protected override void Start()
    {
        base.Start();
        
        var child = transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
        child.center = new Vector3(0, 1, 0);
        child.size = new Vector3(4, 1, 4);
        child.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // включает пассивку пешек        
        if (other.tag == transform.tag && other.GetComponent<Passive>())
        {
            other.transform.GetComponent<Passive>().DamagPlus();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // выключает пассивку пешек когда двигается
        if (other.tag == transform.tag && other.GetComponent<Passive>())
        {
            other.GetComponent<Passive>().DamagMinus();          
        }
    }


    public override void grids()
    {
        if (!PlayerTurn.CanPlay) return;
        for (int i = 0; i < radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { // вправо дорогу ищет 
            if (GameObject.Find($"x:{move[0] + i} z:{move[1] + i}") == null)
            {
                continue;
            }
            if (GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").SendMessage("GridGreen");

        }

        for (int j = 0; j < radiusMove; j++)
        { // вверх влево ищет дорогу
            if (GameObject.Find($"x:{move[0] - j} z:{move[1] + j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").SendMessage("GridGreen");
        }

        for (int i = 0; i < radiusMove; i++) //здесь он делает округу зеленым чтоб видеть куда можно ходить
        { //право вниз ищет дорогу
            if (GameObject.Find($"x:{move[0] - i} z:{move[1] - i}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] - i} z:{move[1]- i}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").SendMessage("GridGreen");
        }

        for (int j = 0; j < radiusMove; j++)
        { // влево низ ищет дорогу
            if (GameObject.Find($"x:{move[0] + j} z:{move[1] - j}") == null)
            {
                continue;

            }
            if (GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").GetComponent<gridsPrefab>().HavePlayer
                || GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").GetComponent<gridsPrefab>().HaveEnemy)
                break;
            else
                GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").SendMessage("GridGreen");

        }

    }

    public override void hideGrids()
    {
        if (photon.IsMine)
        {
            for (int i = 0; i < radiusMove; i++)
            {
                if (GameObject.Find($"x:{move[0] + i} z:{move[1] + i}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] + i} z:{move[1] + i}").SendMessage("hideGrids");
                }
            }
            for (int j = 0; j < radiusMove; j++)
            {
                if (GameObject.Find($"x:{move[0] + j} z:{move[1] - j}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] + j} z:{move[1] - j}").SendMessage("hideGrids");
                }
            }
            for (int i = 0; i < radiusMove; i++)
            {
                if (GameObject.Find($"x:{move[0] - i} z:{move[1] - i}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] - i} z:{move[1] - i}").SendMessage("hideGrids");
                }
            }
            for (int j = 0; j < radiusMove; j++)
            {
                if (GameObject.Find($"x:{move[0] - j} z:{move[1] + j}") != null)
                { // чтоб закрыть зеление клеки
                    GameObject.Find($"x:{move[0] - j} z:{move[1] + j}").SendMessage("hideGrids");
                }
            }
            if (detect)
            {
                detect = false;
                for (int i = 0; i < MoveCell; i++) //закрывает клетки с врагами
                {
                    for (int j = 0; j < MoveCell; j++)
                    {
                        if (GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}") == null)
                        {
                            continue;
                        }
                        else
                            GameObject.Find($"x:{idForBrush[0] + i} z:{idForBrush[1] + j}").SendMessage("hideGrids");
                    }
                }
            }

        }
    }


    public override void attack(GameObject enemyTarget, bool contrAttack)
    {
        enemy = enemyTarget.transform; // для ближнего боя нужен это
        if (!enemyTarget.GetComponent<healthBar>()) return;
        enemyTarget.GetComponent<healthBar>().TakeDamage(damage, this.GetType(), transform);
        animator.SetTrigger("Attack");
        if (photon.IsMine)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            Vector3[] content = new Vector3[] { transform.position, enemyTarget.transform.position };
            PhotonNetwork.RaiseEvent((byte)1, content, options, sendOptions);
            gridsHaveEnemy(idForBrush);
            EnemyMove();
        }
    }

    protected override void uiBtnFigt()
    {
        Ray ray = new Ray(startRay.position, -transform.up);
        RaycastHit hit;
        figthBTN.transform.position = transform.position + pos;//для кнопки иначе она не пашет нормально

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.tag == "grid")
            {
                getPoint(hit.transform.GetComponent<gridsPrefab>().Id); //луч который оперделяет место нахождение   
                move = hit.transform.GetComponent<gridsPrefab>().Id;
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

                // Debug.Log(hit.transform.tag);
            }

            Debug.DrawRay(startRay.position, -transform.up, Color.red, 20);
        }
        if (PhotonNetwork.IsMasterClient)
            figthBTN.transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            figthBTN.transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(target);
            stream.SendNext(state);

        }
        else
        {
            target = (Vector3)stream.ReceiveNext();
            state = (int)stream.ReceiveNext();

        }
    }
}
