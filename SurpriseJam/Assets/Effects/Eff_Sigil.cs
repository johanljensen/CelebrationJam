using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<Transform> slowTargets;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slowTargets = new List<Transform>();
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
            WipeEffect();
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
                enemy.ChangeSpeedAdjustment(slowFactor);
                slowTargets.Add(enemy.transform);
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeSpeedAdjustment(slowFactor);
                slowTargets.Add(player.transform);
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
                enemy.ChangeSpeedAdjustment(1);
                slowTargets.Remove(enemy.transform);
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeSpeedAdjustment(1);
                slowTargets.Remove(player.transform);
            }
        }
    }

    private void WipeEffect()
    {
        if (isPlayerFriendly)
        {
            foreach (Transform slowEnemy in slowTargets)
            {
                if (slowEnemy != null)
                {
                    slowEnemy.GetComponent<EnemyEyEye>().ChangeSpeedAdjustment(1);
                }
            }
        }
        else
        {
            foreach (Transform slowPlayer in slowTargets)
            {
                if (slowPlayer != null)
                {
                    slowPlayer.GetComponent<PlayerController>().ChangeSpeedAdjustment(1);
                }
            }
        }
    }
}
