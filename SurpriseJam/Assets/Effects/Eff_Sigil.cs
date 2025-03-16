using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Eff_Sigil : Effect
{
    [SerializeField] private SpriteRenderer sigilSprite;

    [SerializeField] private float duration;
    [SerializeField] private float fadeTime;
    private float lifetime;
    
    [SerializeField] private float slowFactor;
    private bool isPlayerFriendly;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifetime = 0;
    }

    void Update()
    {
        Color sigilColour = sigilSprite.material.color;
        sigilSprite.material.color = new Color(sigilColour.r, sigilColour.g, sigilColour.b, 0);
        
        lifetime += Time.deltaTime;
        if (sigilSprite.material.color.a < 1)
        {
            sigilSprite.material.color = new Color(sigilColour.r, sigilColour.g, sigilColour.b, lifetime / fadeTime);
        }
        
        if (lifetime > duration - fadeTime)
        {
            sigilSprite.material.color = new Color(sigilColour.r, sigilColour.g, sigilColour.b, duration - lifetime / fadeTime);
        }

        if (lifetime >= duration)
        {
            Destroy(gameObject);
        }
    }

    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        Debug.LogWarning(GetType());
        isPlayerFriendly = playerFriendly;
        transform.position = wheelPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayerFriendly)
        {
            EnemyEyEye enemy = other.GetComponent<EnemyEyEye>();
            if (enemy != null)
            {
                //SLOW THAT ENEMY
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                //SLOW THAT PLAYER
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayerFriendly)
        {
            EnemyEyEye enemy = other.GetComponent<EnemyEyEye>();
            if (enemy != null)
            {
                //unSLOW THAT ENEMY
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                //unSLOW THAT PLAYER
            }
        }
    }
}
