using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Limit of the map")]
    [SerializeField] private float x1 = -30;
    [SerializeField] private float x2 = 20;
    [SerializeField] private float y1 = -14;
    [SerializeField] private float y2 = 15;

    [Header("Enemies")]
    [SerializeField] private GameObject m_MeleeEnemy;
    [SerializeField] private GameObject m_RangedEnemy;

    [SerializeField] private TypeEnemies[] m_typeEnemiesMelee;
    [SerializeField] private TypeEnemies[] m_typeEnemiesRanged;

    [Header("RoundController")]
    [SerializeField] private RoundController m_RoundController;

    [Header("Pool")]
    private List<GameObject> pooledMelee = new List<GameObject>();
    private List<GameObject> pooledRanged = new List<GameObject>();
    [SerializeField] private int amountPool = 75;
    [SerializeField] private Transform MeleeWarehouse;
    [SerializeField] private Transform RangedWarehouse;

    private int m_NumberEnemies;
    private int m_ProbabilityLevel;
    private int m_ProbabilityType;   

    private void Start()
    {
        for (int i = 0; i < amountPool; i++)
        {
            GameObject obj = Instantiate(m_MeleeEnemy, MeleeWarehouse);
            obj.SetActive(false);
            pooledMelee.Add(obj);
        }
        for (int i = 0; i < amountPool; i++)
        {
            GameObject obj = Instantiate(m_RangedEnemy, RangedWarehouse);
            obj.SetActive(false);
            pooledRanged.Add(obj);
        }

        m_RoundController.FirstRound();
    }
    public void NextRound()
    {
        m_NumberEnemies = Random.Range(m_RoundController.Round, m_RoundController.Round * 3 + 1);
        m_ProbabilityLevel = Random.Range(0, 101);
        m_ProbabilityType = Random.Range(0, 101);

        switch (m_RoundController.Round)
        {
            case 1:
                for (int i = 0; i < m_NumberEnemies; i++)
                {
                    InitEnemiesMelee();
                }
                break;
            case 2:
                for (int i = 0; i < m_NumberEnemies; i++)
                {
                    InitEnemiesRanged();
                }
                break; 
            default:
                for (int i = 0; i < m_NumberEnemies; i++)
                {
                    if (Random.Range(0, 101) < m_ProbabilityType)
                        InitEnemiesMelee();
                    else
                        InitEnemiesRanged();
                }
                break;
        }
    }
    public GameObject GetPooledMelee()
    {
        for (int i = 0; i < pooledMelee.Count; i++)
        {
            if (!pooledMelee[i].activeInHierarchy)
            {
                return pooledMelee[i];
            }
        }
        return null;
    }
    public GameObject GetPooledRanged()
    {
        for (int i = 0; i < pooledRanged.Count; i++)
        {
            if (!pooledRanged[i].activeInHierarchy)
            {
                return pooledRanged[i];
            }
        }
        return null;
    }

    private void InitEnemies(TypeEnemies[] types, GameObject obj)
    {
        Vector3 position = getPositionWhithinTheMap();

        obj.transform.position = position;

        if (Random.Range(0, 101) < m_ProbabilityLevel)
        {
            obj.GetComponent<EnemyController>()?.Init(types[0]);
            obj.GetComponent<EnemyPatrolController>()?.Init(types[0]);
        }
        else
        {
            obj.GetComponent<EnemyController>()?.Init(types[1]);
            obj.GetComponent<EnemyPatrolController>()?.Init(types[1]);

        }
    }

    private Vector3 getPositionWhithinTheMap()
    {
        return new Vector3(Random.Range(x1, x2 + 1), Random.Range(y1, y2 + 1), 0);
    }
    private void InitEnemiesMelee()
    {
        GameObject obj = GetPooledMelee();
        if (obj != null)
        {
            InitEnemies(m_typeEnemiesMelee, obj);
        }
    }
    private void InitEnemiesRanged()
    {
        GameObject obj = GetPooledRanged();
        if (obj != null)
        {
            for (int j = 0; j < m_typeEnemiesRanged.Length; j++)
            {
                m_typeEnemiesRanged[j].PatrolPosition = getPositionWhithinTheMap();
            }
            InitEnemies(m_typeEnemiesRanged, obj);
        }
    }
}
