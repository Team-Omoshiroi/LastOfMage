using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CharacterDataContainer, ClonDataContainer 의 공통된 부분을 모아 둔 부모 클래스
/// </summary>
public class DataContainer : MonoBehaviour
{
    [SerializeField] public Inventory1 Inven { get; }
    [field: SerializeField] public CharacterStats Stats { get; protected set; }
    [field: SerializeField] public EquipSystem Equipments { get; protected set; }
    public Animator Animator { get; protected set; }
    [field: SerializeField] public CharacterAnimationData AnimationData { get; protected set; }
    [field: SerializeField] public CharacterSpriteRotator SpriteRotator { get; protected set; }

    public HealthSystem Health { get; protected set; }
}
