using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryUI : MonoBehaviour
{
    public Button CloseBackground;

    public Button WeaponTabBtn;
    public Button ChestTabBtn;
    public Button HeadTabBtn;

    public ScrollRect ScrollRect;

    private GameObject _itemElementPrefab;

    private CanvasGroup _group;

    private ItemType _currentTabOpened;

    // Start is called before the first frame update
    void Start()
    {
        _group = GetComponent<CanvasGroup>();

        CloseBackground.onClick.AddListener(() => DisplayInventory(false));

        WeaponTabBtn.onClick.AddListener(() => OpenTab(ItemType.Weapon));
        ChestTabBtn.onClick.AddListener(() => OpenTab(ItemType.Chest));
        HeadTabBtn.onClick.AddListener(() => OpenTab(ItemType.Head));

        _itemElementPrefab = ScrollRect.content.GetChild(0).gameObject;
        _itemElementPrefab.SetActive(false);

        OpenTab(ItemType.Weapon);

        DisplayInventory(false);

        GameManager.OnItemAdded += RefreshUI;
    }

    public void DisplayInventory(bool b)
    {
        //gameObject.SetActive(b);
        _group.interactable = b;
        _group.alpha = b ? 1 : 0;
        _group.blocksRaycasts = b;
    }

    private void RefreshUI(ItemObject obj)
    {
        if(obj.ItemType == _currentTabOpened)
        {
            OpenTab(_currentTabOpened);
        }
    }

    private void OpenTab(ItemType itemType)
    {
        _currentTabOpened = itemType;

        var list = GameManager.Instance.Player?.Inventory?.FindAll(x => x.ItemType == itemType);

        foreach(Transform t in ScrollRect.content)
        {
            if(t.gameObject.activeSelf)
                Destroy(t.gameObject);
        }

        if(list != null)
        {
            foreach (ItemObject obj in list)
            {
                GameObject go = Instantiate(_itemElementPrefab, ScrollRect.content);
                ItemListElement itemElement = go.GetComponent<ItemListElement>();
                itemElement.SetItem(obj);

                go.SetActive(true);
            }
        }
    }
}
