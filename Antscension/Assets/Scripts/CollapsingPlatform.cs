using UnityEngine;
using System.Collections;

public class CollapsingPlatform : MonoBehaviour
{
    private float fallDelay = 0.2f;
    private float destryDelay = 1f;

    [SerializeField] private Rigidbody2D rb;

   private void OnCollisionEnter2D(Collision2D collision)
   {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
   }
   
   private IEnumerator Fall()
   {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destryDelay);
   }
}

