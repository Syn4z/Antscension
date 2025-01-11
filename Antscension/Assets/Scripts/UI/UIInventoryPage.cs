using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIInventoryPage : MonoBehaviour
{

    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UIInventoryDescription itemDescription;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    public Sprite itemImage1;
    public int itemQuantity1;
    public string itemTitle1, itemDescription1;
    // public Sprite itemImage2;
    // public int itemQuantity2;
    // public string itemTitle2, itemDescription2;

    // public event Action<int> OnDescriptionRequested;

    public void InitializeInventoryUI(int inventorySize)
    {
        ClearInventoryUI();
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }

        if (listOfUIItems.Count > 0)
        {
            listOfUIItems[0].SetData(itemImage1, itemQuantity1);
        }
    }

    private void ClearInventoryUI()
    {
        foreach (var item in listOfUIItems)
        {
            Destroy(item.gameObject);
        }
        listOfUIItems.Clear();
    }

    // public void UpdateData(int itemIndex, Sprite itemImage,
    //     int itemQuantity)
    // {
    //     if (listOfUIItems.Count > itemIndex)
    //     {
    //         listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
    //     }
    // }

    public void Awake()
    {
        Hide();
        itemDescription.ResetDescription();
    }

    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        // int index = listOfUIItems.IndexOf(inventoryItemUI);
        // if (index == -1)
        // {
        //     return;
        // }
        // OnItemActionRequested?.Invoke(index);
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        // ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        // int index = listOfUIItems.IndexOf(inventoryItemUI);
        // if (index == -1)
        // {
        //     return;
        // }
        // OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        // HandleItemSelection(inventoryItemUI);
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        // int index = listOfUIItems.IndexOf(inventoryItemUI);
        // if (index == -1)
        //     return;
        // currentlyDraggedItemIndex = index;
        // HandleItemSelection(inventoryItemUI);
        // OnStartDragging?.Invoke(index);
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        itemDescription.SetDescription(itemImage1, itemTitle1, itemDescription1);  
        // {
        //     int index = listOfUIItems.IndexOf(inventoryItemUI);
        //     if (index == -1)
        //         return;
        //     OnDescriptionRequested?.Invoke(index);
        // } 
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        // ResetSelection();

        listOfUIItems[0].SetData(itemImage1, itemQuantity1);
    }

    // private void ResetSelection()
    // {
    //     itemDescription.ResetDescription();
    //     DeselectAllItems();
    // }

    // public void DeselectAllItems()
    // {
    //     foreach (UIInventoryItem item in listOfUIItems)
    //     {
    //         item.Deselect();
    //     }
    // }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
