using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{   
    public float speed;
    private GameObject player;
    public bool chase = false;
    public Transform startingPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {   
        if (player == null)
        {
            return;
        }
        if (chase == true)
        {
            ChasePlayer();
        }
        else
        {
            ReturnStartPoint();
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

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
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
