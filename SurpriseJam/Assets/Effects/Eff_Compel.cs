using UnityEngine;
using UnityEngine.EventSystems;

public class Eff_Compel : Effect
{
    [SerializeField] private float duration;
    [SerializeField] private float speedMultiplier;

    private Transform effectTarget;
    private bool isPlayerFriendly;
    private float durationTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        durationTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        durationTimer += Time.deltaTime;

        if (durationTimer >= duration)
        {
            SetSpeedMod(1);
            Destroy(gameObject);
        }
    }

    private void SetSpeedMod(float speedMod)
    {
        if (effectTarget == null)
        { return; }
        
        if (isPlayerFriendly)
        {
            PlayerController player = effectTarget.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeSpeedAdjustment(speedMod);
            }
        }
        else
        {
            
            EnemyEyEye enemy = effectTarget.GetComponent<EnemyEyEye>();
            if (enemy != null)
            {
                enemy.ChangeSpeedAdjustment(speedMod);
            }
        }
    }

    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        isPlayerFriendly = playerFriendly;
        effectTarget = activatorCharacter;
        
        transform.parent = effectTarget;
        transform.localPosition = Vector3.zero;
        
        SetSpeedMod(speedMultiplier);
    }
}
