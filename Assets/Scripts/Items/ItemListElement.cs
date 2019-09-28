using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemListElement : MonoBehaviour
{
    public Image ItemIconSprite;

    public TMP_Text ItemNameText;
    public TMP_Text ItemBonusText;

    public Button EquipBtn;

    private ItemObject _itemRef;

    // Start is called before the first frame update
    void Start()
    {
        EquipBtn.onClick.AddListener(EquipItem);
    }

    private void LateUpdate()
    {
        EquipBtn.interactable = !_itemRef.IsEquiped;
    }

    public void SetItem(ItemObject item)
    {
        _itemRef = item;

        ItemNameText.text = _itemRef.ItemName;
    }

    private void EquipItem()
    {
        GameManager.EquipItem(_itemRef);
        transform.SetAsFirstSibling();
    }
}
