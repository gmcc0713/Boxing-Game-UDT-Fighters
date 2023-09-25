using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    public GameObject attackCollider;
    public float damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
            // 적 오브젝트가 Player 태그를 가지고 있으면
            if (other.CompareTag("Player"))
            {
                // 공격 범위 안에 들어왔기 때문에 적 오브젝트의 HP를 깎음
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.fillAmount = health / startHealth;
       
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        // 적이 죽었을 때 실행되는 코드
        Destroy(gameObject);
    }
}
