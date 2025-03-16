using System;
using System.Collections.Generic;
using UnityEngine;

public class Eff_HolyLight : Effect
{
    [SerializeField] private float duration;
    [SerializeField] private float fadeTime;
    private float lifetime;
    
    private bool isPlayerFriendly;
    [SerializeField] private float lightDamage;

    [SerializeField] private List<EnemyEyEye> enemiesInLight;
    private List<PlayerController> playersInLight;

    [SerializeField] private ParticleSystem lightBeams;
    [SerializeField] private Light floorLight;
    
    [SerializeField] private Color goodColour;
    [SerializeField] private Color badColour;
    
    private void Start()
    {
        enemiesInLight = new List<EnemyEyEye>();
        playersInLight = new List<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemiesInLight.RemoveAll(e => e == null);
        foreach (EnemyEyEye enemy in enemiesInLight)
        {
            if (enemy.transform != null)
            {
                enemy.TakeDamage(lightDamage * Time.deltaTime);
            }
        }

        enemiesInLight.RemoveAll(p => p == null);
        foreach (PlayerController player in playersInLight)
        {
            player.TakeDamage(lightDamage * Time.deltaTime);
        }
        
        lifetime += Time.deltaTime;

        if (lifetime >= duration)
        {
            Destroy(gameObject);
        }
    }

    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        isPlayerFriendly = playerFriendly;
        transform.position = wheelPosition;

        if (isPlayerFriendly)
        {
            ParticleSystem.MainModule lightBeamsMain = lightBeams.main;
            lightBeamsMain.startColor = goodColour;
            floorLight.color = goodColour;
        }
        else
        {
            ParticleSystem.MainModule lightBeamsMain = lightBeams.main;
            lightBeamsMain.startColor = badColour;
            floorLight.color = badColour;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isPlayerFriendly)
        {
            EnemyEyEye enemy = other.GetComponent<EnemyEyEye>();
            if (enemy != null)
            {
                enemiesInLight.Add(enemy);
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                playersInLight.Add(player);
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
                enemiesInLight.Remove(enemy);
            }
        }
        else if (!isPlayerFriendly)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                playersInLight.Remove(player);
            }
        }
    }
}
