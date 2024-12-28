using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 0.2f;
    Rigidbody2D myRigidBody;
    PlayerMovement player;
    float xSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;

        // Flip the bullet sprite if the player is facing left 
        if (player.transform.localScale.x < 0)
        {
            // Flip the bullet sprite
            Vector3 bulletScale = transform.localScale;
            bulletScale.x = -Mathf.Abs(bulletScale.x);
            transform.localScale = bulletScale;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        {
            Destroy(gameObject);
        }
    }
}
