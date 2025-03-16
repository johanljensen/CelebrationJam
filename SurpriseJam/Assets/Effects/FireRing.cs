using System;
using UnityEngine;

public class FireRing : MonoBehaviour
{
    private bool isPlayerFriendly;
    [SerializeField] private float fireDamage;

    public void SetAllegiance(bool playerFriendly)
    {
        isPlayerFriendly = playerFriendly;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Burn baby burn!");
        if (isPlayerFriendly)
        {
            EnemyEyEye enemy = other.GetComponent<EnemyEyEye>();
            if (enemy != null)
            {
                enemy.TakeDamage(fireDamage);
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(fireDamage);
            }
        }
    }
}
