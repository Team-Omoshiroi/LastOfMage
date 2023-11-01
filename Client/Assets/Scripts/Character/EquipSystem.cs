using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem
{
    private List<BaseItem> equippedItems = new List<BaseItem>();

    [SerializeField] private CharacterDataContainer cdc;

    public void Equip(BaseItem item)
    {
        //������ Ÿ���� ��� ���� ���̶�� ���� ��� �����ϰ� �����Ѵ�.
        if (item is IEquippable equipment)
        {
            for (int i = 0; i < equippedItems.Count; i++)
            {
                if(item.ItemType == equippedItems[i].ItemType)
                {
                    Dequip(equippedItems[i]);
                }
            }

            equippedItems.Add(item);
            equipment.Equip(cdc);
        }
        else { return; }
    }

    public void Dequip(BaseItem item)
    {
        if (item is IEquippable equipment)
        {
            equippedItems.Remove(item);
            equipment.Dequip(cdc);
        }
        else { return; }
    }

    /// <summary>
    /// ������ ��� �������� �����Ѵ�. ĳ���� ��� �� ���� �� ����.
    /// </summary>
    public void DestroyItem()
    {
        //ĳ���� ��� �� ��ü �κ��丮�� �� ������ ó���ϴ� ����

        //������ ��� ������ ����
        equippedItems.Clear();
    }
}
