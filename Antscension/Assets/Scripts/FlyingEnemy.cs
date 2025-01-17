using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{   
    public float speed;
    private GameObject player;
    public bool chase = false;
    public Transform startingPoint;
    public float idleMoveRange = 0.5f; // Range of idle movement
    public float idleMoveSpeed = 2f;  // Speed of idle movement

    private Vector2 idleOffset;       // Offset for idle movement

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        idleOffset = Vector2.zero;
    }

    void Update()
    {   
        if (player == null)
        {
            return;
        }
        if (chase)
        {
            ChasePlayer();
        }
        else
        {
            ReturnStartPointWithIdleMovement();
        }
        Flip();
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.transform.position) <= 0.1f)
        {
            speed = Mathf.Clamp(speed + 1f * Time.deltaTime, 0, 10);
        }
        else
        {
            speed = 2f; 
        }
    }

    private void ReturnStartPointWithIdleMovement()
    {
        // Generate a slight oscillation
        idleOffset.x = Mathf.Sin(Time.time * idleMoveSpeed) * idleMoveRange;
        idleOffset.y = Mathf.Cos(Time.time * idleMoveSpeed) * idleMoveRange;

        // Apply the oscillation to the position
        Vector2 targetPosition = (Vector2)startingPoint.position + idleOffset;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
