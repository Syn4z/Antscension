using UnityEngine;

public class ChaseControl : MonoBehaviour
{   
    public FlyingEnemy[] enemyArray;

   private void OnTriggerEnter2D(Collider2D collision)
   {
        if (collision.gameObject.CompareTag("Player"))
       {
           foreach (FlyingEnemy enemy in enemyArray)
           {
               enemy.chase = true;
           }
       }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
    if (collision.gameObject.CompareTag("Player"))
       {
           foreach (FlyingEnemy enemy in enemyArray)
           {
               enemy.chase = false;
           }
       }
   }
}
