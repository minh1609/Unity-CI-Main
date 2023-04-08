
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Generic Ability", fileName = "New Generic Ability")]
public class GenericAbility : ScriptableObject
{
    public float magicCost;
    public float duration;

    public FloatValue playerMagic;
    public Signal usePlayerMagic;
    public virtual void Ability(Vector2 playerPosition, Vector2 playerFacingDirection,
        Animator playerAnimator = null, Rigidbody2D playerRigidbody = null)
    { 

    }
    
}
