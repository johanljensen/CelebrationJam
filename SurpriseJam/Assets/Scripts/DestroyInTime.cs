using System;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private float lifeTimer;

    private void Start()
    {
        lifeTimer = 0;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
