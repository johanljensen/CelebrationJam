using System;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    [SerializeField] private Effect thisEffect;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        Instantiate(thisEffect).ActivateEffect(true, FindFirstObjectByType<PlayerController>().transform, transform.position);
    }
}
