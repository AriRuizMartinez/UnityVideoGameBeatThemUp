using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerTransform;
    private Vector3 m_Position;
    private Rigidbody2D m_Rigidbody;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        FollowPlayer();
    }
    private void FollowPlayer()
    {
        if (m_PlayerTransform != null)
        {
            m_Position.x = m_PlayerTransform.position.x;
            m_Position.y = m_PlayerTransform.position.y;
            m_Rigidbody.MovePosition(m_Position);
        }
    }
}
