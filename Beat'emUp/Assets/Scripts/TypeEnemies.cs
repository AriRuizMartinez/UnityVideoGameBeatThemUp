using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TypeEnemies : ScriptableObject
{
    public float radiusChase;
    public float radiusAttack;
    public int damage;
    public int health;
    public float speed;
    public Color color;
    public Vector3 PatrolPosition;
}
