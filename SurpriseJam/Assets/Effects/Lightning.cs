using UnityEngine;

public class Lightning : MonoBehaviour
{
    bool isPlayerFriendly;
    float damage;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Lightning");
        if (isPlayerFriendly)
        {
            EnemyEyEye enemy = other.GetComponent<EnemyEyEye>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    public void GetLightningVariables(bool isPlayerFriendly, float damage)
    {
        this.isPlayerFriendly = isPlayerFriendly;
        this.damage = damage;
    }
}
