using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour {

    public enum AttackType
    {
        shotting,
        melle
    }

    public enum EnemyState
    {
        idle,
        walk,
        chase,
        attack,
        checkNoise,
        deafened
    }

    SpriteRenderer spriteRenderer;
    [Header("Enemy Properties:")]
    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float bulletSpeed = 3f;
    public float singleWaypointWait = 5; //How long enemy must stay on single waypoint
    public float waypointDestionationAccurancy = 2f;
    public float seeAngle = 45;
    public float seeDistance = 100;
    public float checkNoiseDistance = 2.5f;
    public float shootingDelay = 1f;
    public float deafenedTime = 10f;
    [Range(0.0f,1f)]
    public float playerVisibilityThresold = 0.2f;
    EnemyState currentEnemyState = EnemyState.walk;
    public AttackType currentAttackType = AttackType.shotting;
    public float attackDistance = 2f;
    public List<Vector3> waypoints;
    Vector3 lastNoisePosition;
    Vector2 lookVector = Vector2.right;
    public GameObject bullet;
    int currentWaypoint = 0; //waypoint index
    float timer = 0;
    bool timerEnabled = true;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void UpdateEnemy(Transform player)
    {
        if(timerEnabled) timer += Time.deltaTime;
        spriteRenderer.flipX = lookVector.x > 0 ? false : true;
        switch(currentEnemyState)
        {
            case EnemyState.idle:
                if (!timerEnabled) timerEnabled = true;
                if (LookForPlayer(player) && CheckIfPlayerIsntHidden(player)) currentEnemyState = EnemyState.chase;
                if(timer > singleWaypointWait)
                {
                    currentEnemyState = EnemyState.walk;
                    timer = 0;
                }
                break;
            case EnemyState.walk:
                lookVector = (waypoints[currentWaypoint] - transform.position).x > 0 ? Vector2.right : Vector2.left;
               // if(lookVector == Vector2.right)
                if (LookForPlayer(player) && CheckIfPlayerIsntHidden(player))
                {
                    currentEnemyState = EnemyState.chase;
                    break;
                }
                if(Vector2.Distance(waypoints[currentWaypoint], transform.position) <= waypointDestionationAccurancy)
                {
                    currentWaypoint = currentWaypoint + 1 >= waypoints.Count ? 0 : currentWaypoint + 1;
                    currentEnemyState = EnemyState.idle;
                    timer = 0;
                }
                else
                {
                    transform.Translate((waypoints[currentWaypoint] - transform.position).normalized * walkSpeed * Time.deltaTime);
                }
                break;
            case EnemyState.chase:
                if(LookForPlayer(player))
                {
                    transform.Translate((player.position - transform.position).normalized * chaseSpeed * Time.deltaTime);
                    lookVector = (player.position - transform.position).x > 0 ? Vector2.right : Vector2.left;
                    if(Vector2.Distance(transform.position, player.position) <= attackDistance)
                    {
                        currentEnemyState = EnemyState.attack;
                    }
                }
                else
                {
                    currentEnemyState = EnemyState.idle;
                    timer = 0;
                }
                break;
            case EnemyState.attack:
                if (!LookForPlayer(player))
                {
                    currentEnemyState = EnemyState.idle;
                    timer = 0;
                }
                if (Vector2.Distance(transform.position, player.position) > attackDistance) currentEnemyState = EnemyState.chase;
                switch(currentAttackType)
                {
                    case AttackType.shotting:
                        if(timer >= shootingDelay)
                        {
                            GameObject newBullet = SimplePool.Spawn(bullet, transform.position, Quaternion.identity);
                            Bullet bulletScript = newBullet.GetComponent<Bullet>();
                            if (bulletScript != null)
                            {
                                bulletScript.Init(lookVector * bulletSpeed);
                            }
                            timer = 0;
                        }
                        break;
                    case AttackType.melle:
                        break;
                }


                break;
            case EnemyState.checkNoise:
                lookVector = (lastNoisePosition - transform.position).x > 0 ? Vector2.right : Vector2.left;
                transform.Translate((lastNoisePosition - transform.position).normalized * chaseSpeed * Time.deltaTime);
                if(LookForPlayer(player) && CheckIfPlayerIsntHidden(player))
                {
                    currentEnemyState = EnemyState.chase;
                }
                if(Vector2.Distance(transform.position, lastNoisePosition) < checkNoiseDistance)
                {
                    currentEnemyState = EnemyState.idle;
                    timer = 0;
                }
                break;
            case EnemyState.deafened:
                if(timer>deafenedTime)
                {
                    currentEnemyState = EnemyState.walk;
                    spriteRenderer.color = Color.white;
                    timer = 0;
                }
                else spriteRenderer.color = Color.gray;
                break;
        }

    }

    //Return true if player is visible
    bool LookForPlayer(Transform player)
    {
        //if angle and raycast
        float angle = Vector2.Angle(lookVector, (player.position - transform.position).normalized);

        Vector3 rotatedVector = new Vector2(Mathf.Cos(seeAngle) * lookVector.x - Mathf.Sin(seeAngle) * lookVector.y, Mathf.Sin(seeAngle) * lookVector.x + Mathf.Cos(seeAngle) * lookVector.y);

        // Debug.DrawLine(transform.position, rotatedVector + transform.position, Color.blue);
        // Debug.DrawLine(transform.position, new Vector3(rotatedVector.x, -rotatedVector.y) + transform.position, Color.blue);

        if (angle <= seeAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (player.position-transform.position), seeDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, (player.position - transform.position), Color.blue);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        

        
        return false;
    }

    bool CheckIfPlayerIsntHidden(Transform player)
    {
        if(player.GetComponent<Hidening>().hiddenLevel < playerVisibilityThresold)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("NoiseEmitter") && currentEnemyState != EnemyState.deafened)
        {
            lastNoisePosition = collision.transform.position;
            currentEnemyState = EnemyState.checkNoise;
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("rock"))
        {
            currentEnemyState = EnemyState.deafened;
        }
    }
}
