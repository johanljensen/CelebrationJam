using UnityEngine;

public class Eff_PowerOfChrist : Effect
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        Debug.LogWarning(GetType());
    }
}
