using UnityEngine;

public class PlayerModelHandler : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    public void Die()
    {
        controller.Dead();
    }
}
