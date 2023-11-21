using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class EdibleItemSo : BaseItem, IDestroyableItem, IItemAction
{
    [SerializeField]
    private List<ModifierData> modifiersData = new List<ModifierData>();//소비템 효과를 추가할수있는 데이터 리스트 


    public string ActionName => "Consume";

    public bool PerformAction(GameObject character)
    {

        Debug.Log("소비템");
        foreach (ModifierData data in modifiersData)
        {
            data.statModifier.AffectCharacter(character, data.value);
        }
        return true;
    }



    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}
