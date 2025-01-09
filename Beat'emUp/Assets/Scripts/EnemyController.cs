using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    private enum SwitchMachineStates { NONE, IDLE, CHASE, ATTACK};
    private SwitchMachineStates m_CurrentState;
    private Transform m_PlayerTransform;
    private Coroutine m_Coroutine;

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

                break;

            case SwitchMachineStates.CHASE:

                m_Animator.Play("CHASE");

                break;

            case SwitchMachineStates.ATTACK:

                m_Coroutine = StartCoroutine(Attack());

                break;

            default:
                break;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            m_Animator.Play("ATTACK");
            yield return new WaitForSeconds(1);
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
                transform.up = m_PlayerTransform.position - transform.position;
                break;
            case SwitchMachineStates.ATTACK:
               
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

                break;

            case SwitchMachineStates.CHASE:

                break;

            case SwitchMachineStates.ATTACK:
                StopCoroutine(m_Coroutine);
                break;

            default:
                break;
        }
    }
 
    
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private RoundController m_RoundController;

    [Header("Enemy Values")]
    [SerializeField]
    private float m_Speed = 1.75f;
    [SerializeField]
    private int m_Damage = 3;
    [SerializeField]
    private int m_Health = 25;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
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
    private void Update()
    {
        UpdateState();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HitboxInfo>())
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
        GetComponentInChildren<ChaseDetection>(true).gameObject.GetComponent<CircleCollider2D>().radius = t.radiusChase;
        GetComponentInChildren<HitboxInfo>(true).Damage = m_Damage;
        gameObject.SetActive(true);
        InitState(SwitchMachineStates.IDLE);
    }
}
