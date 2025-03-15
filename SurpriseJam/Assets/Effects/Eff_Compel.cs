using UnityEngine;
using UnityEngine.EventSystems;

public class Eff_Compel : Effect
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string EffectName { get; }

    public override void ActivateEffect(bool playerFriendly, Transform activaterCharacter)
    {
        Debug.LogWarning(GetType());
    }
}
