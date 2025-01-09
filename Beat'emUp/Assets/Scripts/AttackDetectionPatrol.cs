using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetectionPatrol : MonoBehaviour
{
    [SerializeField] EnemyPatrolController m_EnemyController;
    private Rigidbody2D m_Rigidbody2d;
    private void Awake()
    {
        m_Rigidbody2d = GetComponent<Rigidbody2D>();    
    }
    private void Update()
    {
        m_Rigidbody2d.MovePosition(transform.parent.transform.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            m_EnemyController.AttackPlayer();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_EnemyController.ChasePlayer(collision.transform);
        }
    }
}
