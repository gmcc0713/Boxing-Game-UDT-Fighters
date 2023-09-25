using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public float damage = 10;

    public void OnAttacked()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
