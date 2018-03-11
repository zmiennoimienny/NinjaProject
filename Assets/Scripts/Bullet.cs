using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {


    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 velocity)
    {
        rigidbody2D.velocity = velocity;
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.drag = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        SimplePool.Despawn(gameObject);
    }
}
