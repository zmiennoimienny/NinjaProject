using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    public Animator animator;
    public NoiseEmitter noiseEmitter;
    [Header("Player Properties")]
    public float landNoiseValue = 5f;
    public float walkNoiseValue = 3f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float climpingSpeed = 1f;
    [SerializeField] private float distToGround;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbLayer;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;
    private float xSpeed;
    private float ySpeed;
    private bool grounded;

    private Vector2 oldPos;

    private void Awake()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        distToGround = collider.bounds.extents.y;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        oldPos = transform.position;
    }

    private void Update()
    {
        HandleInput();
        HandleAnimations();
    }

    void HandleInput()
    {
        transform.Translate(Vector3.right * speed * Input.GetAxis("Horizontal") * Time.deltaTime);
       // Debug.Log(IsGrounded());
       // Debug.LogFormat("Can Climp: {0}", CanClimb());
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.gravityScale = 1f;
        if (CanClimb())
        {
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = Vector2.zero;
            transform.Translate(Vector3.up * climpingSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        }
        else if (Input.GetAxis("Vertical")>0 && IsGrounded())
        {
            Jump();
        }

        if (IsGrounded() && !grounded)
        {
            noiseEmitter.EmitNoise(landNoiseValue);
        }
        else if(grounded)
        {
            noiseEmitter.EmitNoise(walkNoiseValue * Mathf.Abs(Input.GetAxis("Horizontal")));
        }
        grounded = IsGrounded();
    }

    void HandleAnimations()
    {
        xSpeed = ((transform.position.x - oldPos.x) / Time.deltaTime) / speed;
        ySpeed = ((transform.position.y - oldPos.y) / Time.deltaTime);
        animator.SetFloat("xSpeed", Mathf.Abs(xSpeed));
        animator.SetFloat("ySpeed", ySpeed);

        animator.SetFloat("yVelocity", rigidbody2D.velocity.y);

        if (xSpeed < -0.02f) spriteRenderer.flipX = true;
        else if (xSpeed > 0.02f) spriteRenderer.flipX = false;
        if (CanClimb()) animator.SetBool("climbing", true);
        else animator.SetBool("climbing", false);
        //Debug.Log(xSpeed);
        //Debug.Log(ySpeed);
        oldPos = new Vector2(transform.position.x, transform.position.y);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position, distToGround + 0.1f, groundLayer);
    }

    bool CanClimb()
    {
        return Physics2D.OverlapCircle(transform.position, distToGround + 0.1f, climbLayer);
    }

    void Jump()
    {
        rigidbody2D.velocity = Vector2.up * jumpSpeed;
    }


}
