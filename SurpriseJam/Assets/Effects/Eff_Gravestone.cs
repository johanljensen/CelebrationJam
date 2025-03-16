using UnityEngine;

public class Eff_Gravestone : Effect
{
    [SerializeField] private GameObject gravePrefab;
    
    public override void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition)
    {
        transform.position = wheelPosition;
    }
}
