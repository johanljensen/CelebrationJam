using UnityEngine;

public class Eff_RingOfFire : Effect
{
    [SerializeField] private FireRing fireRing;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float ringHeight;
    
    [SerializeField] private float duration;
    [SerializeField] private float fadeTime;
    private float lifetime;
    

    // Update is called once per frame
    void Update()
    {
        if (!fireRing)
            Destroy(gameObject);

        fireRing.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        
        lifetime += Time.deltaTime;

        if (lifetime >= duration)
        {
            Destroy(gameObject);
            Destroy(fireRing.gameObject);
        }
    }

    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        Debug.LogWarning(GetType());
        fireRing.transform.parent = activatorCharacter;
        fireRing.transform.localPosition = Vector3.zero + Vector3.up * ringHeight;
        fireRing.SetAllegiance(playerFriendly);
    }
}
