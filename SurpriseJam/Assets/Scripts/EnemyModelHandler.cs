using UnityEngine;

public class EnemyModelHandler : MonoBehaviour
{
    [SerializeField] EnemyEyEye enemy;
    void Die()
    {
        enemy.StartFading();
    }
}
