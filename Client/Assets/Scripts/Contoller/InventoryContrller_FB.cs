
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    // 인벤토리 ui및 모델 과 통신 두 요소 모두에 종속된다  

    public class InventoryContrller_FB : MonoBehaviour
    {
        [SerializeField]
        private FarmingBox farmingBox;

        [SerializeField]
        private UIInventoryPage_FB inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        [SerializeField]
        private Button BtnCancel;

        private Action OnOpened;
        private Action OnClosed;

        UIInventoryPage _inventorypage_player;
        UIInventoryPage_FB _inventorypage_FB;
        PlayerInput playerInput;


        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
            playerInput = GetComponent<PlayerInput>();
            OnOpened = farmingBox.OnOpened;
            OnClosed = farmingBox.OnClosed;

            BtnCancel.onClick.AddListener(() => CloseInventoryUI());
        }

        private void PrepareInventoryData()//인벤토리 데이터가 바뀔 때 호출 
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

            //foreach (InventoryItem item in initialItems)
            //{
            //    if (item.IsEmpty)
            //        continue;
            //    inventoryData.AddItem(item);
            //}
        }
        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();//인벤토리데이터 업데이트 할때 한번초기화 
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemIcon,item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeinventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }
                
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemIcon, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            BaseItem item = inventoryItem.item;
            
            if(item.ItemType == eItemType.Weapon)
            {
                inventoryUI.UpdateDescription(itemIndex, item.ItemIcon, $"{item.name}\n{item.Description}", item.Description);
            }
            else
            {
                inventoryUI.UpdateDescription(itemIndex, item.ItemIcon, item.name, item.Description);           
            }
            
        }

        private void OnMouseDown()
        {
            //플레이어 캐릭터와 일정 거리 이하라면 플레이어의 인벤토리와 보관함 인벤토리를 동시에 연다.
            if (/*Vector3.Distance(this.transform.position, 플레이어.transform.position) < 20*/ true)
            {
                OpenInventoryUI();
            }
        }

        private void OpenInventoryUI()
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                OnOpened?.Invoke();

                //playerInput.CanControl = false;
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemIcon, item.Value.quantity);
                }
            }
        }

        private void CloseInventoryUI()
        {
            if (inventoryUI.isActiveAndEnabled == true)
            {
                inventoryUI.Hide();
                OnClosed?.Invoke();
                //playerInput.CanControl = true;
            }
        }
    }
}