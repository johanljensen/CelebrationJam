using System;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    [SerializeField] private Effect thisEffect;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.GetComponent<PlayerController>() || other.GetComponent<EnemyEyEye>())
        {
            Instantiate(thisEffect).ActivateEffect(true, FindFirstObjectByType<PlayerController>().transform,
                transform.position);
        }
    }
}
