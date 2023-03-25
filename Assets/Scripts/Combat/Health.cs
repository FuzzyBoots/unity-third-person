using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    
    private int health;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public bool isDead => (health == 0); 

    private bool isInvulnerable = false;

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;   
    }

    public void setInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage (int damage)
    {
        if (health <= 0) { return; }

        if (isInvulnerable) { return; }
            
        health = Mathf.Max(0, health - damage);

        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke();
        }
    }
}
