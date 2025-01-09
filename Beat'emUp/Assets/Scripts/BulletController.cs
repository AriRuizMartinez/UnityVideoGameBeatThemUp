using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Values")]
    [SerializeField]
    private float m_BulletSpeed = 5f;
    private int m_BulletDamage;
    private SpriteRenderer m_SpriteRenderer;
    private void OnEnable()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public float BulletSpeed
    {
        get 
        { 
            return m_BulletSpeed; 
        } 
    }
    public int BulletDamage
    {
        get
        {
            return m_BulletDamage;
        }
        set
        {
            m_BulletDamage = value;
        }
    }
    private void Update()
    {
        if (!m_SpriteRenderer.isVisible)
        {
            gameObject.SetActive(false);
        }
    }
    public void PlayerHitted()
    {
        gameObject.SetActive(false);
    }
}
