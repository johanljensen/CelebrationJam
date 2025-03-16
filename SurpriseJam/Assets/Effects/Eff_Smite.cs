using UnityEngine;

public class Eff_Smite : Effect
{
    [SerializeField] private LightningSmite lightningSmite;

    [SerializeField] private float duration;
    [SerializeField] private float fadeTime;
    private float lifetime;


    public string EffectName { get; }

    // Update is called once per frame
    void Update()
    {
        if (!lightningSmite)
            Destroy(gameObject);

        lifetime += Time.deltaTime;

        if (lifetime >= duration)
        {
            Destroy(gameObject);
            Destroy(lightningSmite.gameObject);
        }
    }
    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        Debug.LogWarning(GetType());
        lightningSmite = Instantiate(lightningSmite);
        lightningSmite.transform.parent = activatorCharacter;
        lightningSmite.transform.localPosition = Vector3.zero + Vector3.up;
        lightningSmite.SetAllegiance(playerFriendly);
    }
}
