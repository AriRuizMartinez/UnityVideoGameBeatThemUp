using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class EnemyPatrolController : MonoBehaviour
{

    private List<GameObject> pooled = new List<GameObject>();
    [SerializeField] private int amountPool = 5;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform m_MagazineTransform;

    public GameObject GetPooled()
    {
        for (int i = 0; i < pooled.Count; i++)
        {
            if (!pooled[i].activeInHierarchy)
            {
                return pooled[i];
            }
        }
        return null;
    }


    private enum SwitchMachineStates { NONE, IDLE, CHASE, ATTACK, PATROL};
    private SwitchMachineStates m_CurrentState;
    private Transform m_PlayerTransform;
    private Coroutine m_CurrentCoroutine;
    [SerializeField] private Vector3[] waypoints;
    public Vector3[] Waypoints 
    {
        get 
        {
            return waypoints;
        }

        set 
        {
            waypoints = value;
        } 
    }
    private int m_NumWaypoints;

    private void ChangeState(SwitchMachineStates newState)
    {

        if (newState == m_CurrentState)
            return;

        ExitState();
        InitState(newState);
    }

    private void InitState(SwitchMachineStates currentState)
    {
        m_CurrentState = currentState;
        switch (m_CurrentState)
        {
            case SwitchMachineStates.IDLE:

                m_Rigidbody.velocity = Vector2.zero;
                m_Animator.Play("IDLEenemy");
                m_CurrentCoroutine = StartCoroutine(WaitingForPlayer());
                break;

            case SwitchMachineStates.CHASE:

                m_Animator.Play("CHASE");

                break;

            case SwitchMachineStates.ATTACK:

                m_CurrentCoroutine = StartCoroutine(Attack());

                break;

            case SwitchMachineStates.PATROL:

                m_Rigidbody.velocity = (waypoints[m_NumWaypoints] - this.transform.position).normalized * m_Speed;
                m_Animator.Play("CHASE");

                break;

            default:
                break;
        }
    }
    private void UpdateState()
    {

        switch (m_CurrentState)
        {
            case SwitchMachineStates.IDLE:

                break;
            case SwitchMachineStates.CHASE:
                m_Rigidbody.velocity = (m_PlayerTransform.position - transform.position).normalized * m_Speed;
                transform.up = m_PlayerTransform.position;
                break;
            case SwitchMachineStates.ATTACK:
                m_Rigidbody.velocity = Vector2.zero;
                break;
            case SwitchMachineStates.PATROL:

                if (Vector3.SqrMagnitude(waypoints[m_NumWaypoints] - this.transform.position) <= m_SQDistancePerFrame)
                {
                    m_NumWaypoints++;
                    if(m_NumWaypoints >= waypoints.Length)
                    {
                        m_NumWaypoints = 0;
                    }
                    m_Rigidbody.velocity = (waypoints[m_NumWaypoints] - this.transform.position).normalized * m_Speed;
                }
                    break;

            default:
                ChangeState(SwitchMachineStates.IDLE);
                break;
        }
    }

    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case SwitchMachineStates.IDLE:
                StopCoroutine(m_CurrentCoroutine);
                break;

            case SwitchMachineStates.CHASE:

                break;

            case SwitchMachineStates.ATTACK:
                StopCoroutine(m_CurrentCoroutine);
                break;
            case SwitchMachineStates.PATROL:

                break;
            default:
                break;
        }
    }
 
    
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_SpriteRenderer;
    private float m_SQDistancePerFrame;
    [SerializeField] private RoundController m_RoundController;

    [Header("Enemy Values")]
    [SerializeField]
    private float m_Speed = 1.75f;
    [SerializeField]
    private int m_Damage = 5;
    [SerializeField]
    private int m_Health = 15;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_NumWaypoints = 0;
        m_SQDistancePerFrame = m_Speed * Time.fixedDeltaTime;
        m_SQDistancePerFrame *= m_SQDistancePerFrame;

    }

    private void Start()
    {
        for (int i = 0; i < amountPool; i++)
        {
            GameObject obj = Instantiate(Bullet, m_MagazineTransform);
            obj.GetComponent<HitboxInfo>().Damage = m_Damage;
            obj.SetActive(false);
            pooled.Add(obj);
        }
    }

    public void ChasePlayer(Transform t)
    {
        m_PlayerTransform = t;
        ChangeState(SwitchMachineStates.CHASE);
    }
    public void AttackPlayer()
    {
        ChangeState(SwitchMachineStates.ATTACK);
    }
    public void IdleMyself()
    {
        ChangeState(SwitchMachineStates.IDLE);
    }
    public void ReturnPatrol()
    {
        ChangeState(SwitchMachineStates.PATROL);
    }
    public IEnumerator WaitingForPlayer()
    {
        yield return new WaitForSeconds(5);
        ChangeState(SwitchMachineStates.PATROL);
    }
    private void Update()
    {
        UpdateState();
    }


    private IEnumerator Attack()
    {
        while (true)
        {
            GameObject bullet = GetPooled();
            if(bullet != null)
            {
                m_Animator.Play("ATTACKPatrol");
                bullet.GetComponent<Transform>().position = transform.position;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody2D>().velocity = (m_PlayerTransform.position - transform.position).normalized * bullet.GetComponent<BulletController>().BulletSpeed;
                bullet.GetComponent<BulletController>().BulletDamage = m_Damage;
            }
            yield return new WaitForSeconds(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitboxInfo>())
        {
            receiveDamage(collision.GetComponent<HitboxInfo>().Damage);
        }
    }
    private void receiveDamage(int damage)
    {
        m_Health -= damage;
        CheckLife();
    }
    private void CheckLife()
    {
        if (m_Health < 0)
        {
            StopAllCoroutines();
            m_RoundController.EnemieDefeated();
            gameObject.SetActive(false);
        }
    }
    public void Init(TypeEnemies t)
    {
        m_RoundController.newEnemy();
        m_Damage = t.damage;
        m_Speed = t.speed;
        m_Health = t.health;
        m_SpriteRenderer.color = t.color;
        GetComponentInChildren<ChaseDetectionPatrol>(true).gameObject.GetComponent<CircleCollider2D>().radius = t.radiusChase;
        GetComponentInChildren<AttackDetectionPatrol>(true).gameObject.GetComponent<CircleCollider2D>().radius = t.radiusAttack;

        waypoints[0] = transform.position;
        waypoints[1] = t.PatrolPosition;

        foreach (HitboxInfo info in GetComponentsInChildren<HitboxInfo>(true))
        {
            if(info != null) 
                info.Damage = m_Damage;
        }
        gameObject.SetActive(true);
        InitState(SwitchMachineStates.PATROL);
    }
}
