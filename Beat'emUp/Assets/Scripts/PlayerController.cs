using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private enum SwitchMachineStates { NONE, IDLE, WALK, WEAK, STRONG };
    private SwitchMachineStates m_CurrentState;
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
                m_Animator.Play("IDLE");

                break;

            case SwitchMachineStates.WALK:

                m_Animator.Play("WALK");

                break;

            case SwitchMachineStates.WEAK:

                m_ComboAvailable = false;
                //m_Rigidbody.velocity = Vector2.zero;
                m_HitboxInfo.Damage = m_WEAKDamage;
                m_Animator.Play("WEAK");

                break;

            case SwitchMachineStates.STRONG:

                m_ComboAvailable = false;
                //m_Rigidbody.velocity = Vector2.zero;
                m_HitboxInfo.Damage = m_STRONGDamage;
                m_Animator.Play("STRONG");

                break;

            default:
                break;
        }
    }

    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case SwitchMachineStates.IDLE:

                break;

            case SwitchMachineStates.WALK:

                break;

            case SwitchMachineStates.WEAK:
                m_ComboAvailable = false;
                break;

            case SwitchMachineStates.STRONG:
                m_ComboAvailable = false;
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

                if (m_MovementAction.ReadValue<Vector2>() != Vector2.zero)
                    ChangeState(SwitchMachineStates.WALK);

                break;
            case SwitchMachineStates.WALK:

                Walk();

                if (m_Rigidbody.velocity == Vector2.zero)
                    ChangeState(SwitchMachineStates.IDLE);

                break;
            case SwitchMachineStates.WEAK:

                Walk();

                break;

            case SwitchMachineStates.STRONG:

                Walk();

                break;

            default:
                ChangeState(SwitchMachineStates.IDLE);
                break;
        }
    }
    private bool m_ComboAvailable = false;

    public void InitComboWindow()
    {
        m_ComboAvailable = true;
    }

    public void EndComboWindow()
    {
        m_ComboAvailable = false;
    }

    public void EndHit()
    {
        ChangeState(SwitchMachineStates.WALK);
    }
    private void WeakAttackAction(InputAction.CallbackContext actionContext)
    {
        switch (m_CurrentState)
        {
            case SwitchMachineStates.IDLE:
                ChangeState(SwitchMachineStates.WEAK);

                break;

            case SwitchMachineStates.WALK:
                ChangeState(SwitchMachineStates.WEAK);

                break;

            case SwitchMachineStates.WEAK:

                EndComboWindow();
                break;

            case SwitchMachineStates.STRONG:
                if (m_ComboAvailable)
                {
                    
                    MoreDamage();
                    ChangeState(SwitchMachineStates.STRONG);
                }
                else
                {
                    
                    InitialDamage();
                    ChangeState(SwitchMachineStates.WEAK);
                }
                break;

            default:
                break;
        }
    }
    private void StrongAttackAction(InputAction.CallbackContext actionContext)
    {

        switch (m_CurrentState)
        {
            case SwitchMachineStates.IDLE:
                ChangeState(SwitchMachineStates.STRONG);

                break;

            case SwitchMachineStates.WALK:
                ChangeState(SwitchMachineStates.STRONG);

                break;

            case SwitchMachineStates.WEAK:
                if (m_ComboAvailable)
                {
                    
                    MoreDamage();
                    ChangeState(SwitchMachineStates.WEAK);
                }
                else
                {
                    
                    InitialDamage();
                    ChangeState(SwitchMachineStates.STRONG);
                }
                break;

            case SwitchMachineStates.STRONG:
                EndComboWindow();
                break;

            default:
                break;
        }
    }
    private void MoreDamage()
    {
        this.m_WEAKDamage++;
        this.m_STRONGDamage++;
        
    }
    private void InitialDamage()
    {
        this.m_WEAKDamage = this.m_WEAKDamageFinal;
        this.m_STRONGDamage = this.m_STRONGDamageFinal;
        
    }
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    private InputAction m_MovementAction;
    private InputAction m_MouseAction;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private HitboxInfo m_HitboxInfo;
    [SerializeField] private GameEventInt m_UpdateHealth;

    [Header("Character Values")]
    [SerializeField]
    private float m_Speed = 2;
    [SerializeField]
    private int m_WEAKDamage = 2;
    [SerializeField]
    private int m_STRONGDamage = 5;
    [SerializeField]
    private int m_Health = 50;

    private int m_WEAKDamageFinal;
    private int m_STRONGDamageFinal;

    void Awake()
    {
        Assert.IsNotNull(m_InputAsset);

        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("default").FindAction("Move");
        m_MouseAction = m_Input.FindActionMap("default").FindAction("AttackDirection");
        m_Input.FindActionMap("default").FindAction("WeakAttack").performed += WeakAttackAction;
        m_Input.FindActionMap("default").FindAction("StrongAttack").performed += StrongAttackAction;
        m_Input.FindActionMap("default").Enable();

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_HitboxInfo = GetComponentInChildren<HitboxInfo>(true);

        m_WEAKDamageFinal = m_WEAKDamage;
        m_STRONGDamageFinal = m_STRONGDamage;
    }

    private void Start()
    {
        InitState(SwitchMachineStates.IDLE);
    }

    void Update()
    {
        UpdateState();
        FollowMouse();
    }

    private void OnDestroy()
    {
        m_Input.FindActionMap("default").FindAction("WeakAttack").performed -= WeakAttackAction;
        m_Input.FindActionMap("default").FindAction("StrongAttack").performed -= StrongAttackAction;
        m_Input.FindActionMap("default").Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitboxInfo>())
        {
            receiveDamage(collision.GetComponent<HitboxInfo>().Damage);
            collision.GetComponent<BulletController>()?.PlayerHitted();
        }
    }

    private void receiveDamage(int damage)
    {
        m_Health -= damage;
        CheckLife();
        m_UpdateHealth.Raise(m_Health);
    }
    private void CheckLife()
    {
        if(m_Health < 0)
        {
            //gameObject.SetActive(false);
            GameManager.ChangeScene(GameManager.GameScenes.GameOver);
        }
    }
    private void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(m_MouseAction.ReadValue<Vector2>());
        mousePosition.z = 0;

        transform.right = mousePosition - transform.position;
    }
    private void Walk()
    {
        m_Rigidbody.velocity = m_MovementAction.ReadValue<Vector2>() * m_Speed;
    }
}
