using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoundController : ScriptableObject
{
    private int m_NumberEnemies = 0;
    private int m_Round = 0;
    public int Round {  get { return m_Round; } }
    [SerializeField] private GameEvent m_NextRound;

    public void EnemieDefeated()
    {
        //lock (this)
        //{
            m_NumberEnemies--;
            if (m_NumberEnemies <= 0)
            {
                m_Round++;
                m_NumberEnemies = 0;
                m_NextRound.Raise();
            }
        //}
    }
    public void setEnemies(int number)
    {
        m_NumberEnemies = number;
    }
    public void newEnemy()
    {
        //lock(this) { 
            m_NumberEnemies++;
        //}
    }
    public void FirstRound()
    {
        m_NumberEnemies = 0;
        m_Round = 0;
        EnemieDefeated();
    }
}
