using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] private string EffectName;

    public string GetName()
    {
        return EffectName;
    }

    public abstract void ActivateEffect(bool playerFriendly, Transform activatorCharacter, Vector3 wheelPosition);
}
