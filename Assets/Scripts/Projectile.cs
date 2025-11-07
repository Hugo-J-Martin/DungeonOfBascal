using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;
    public int damage = 10;

    private void Start()
    {
        // Automatically destroy after a few seconds to avoid clutter
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Example: deal damage if it hits something with a health script
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy)
        {
            enemy.TakeDamage(damage);
            
        }

        // Destroy on impact
        Destroy(gameObject);
    }
}
