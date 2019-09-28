using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Datas")]
    public Datas Datas;

    public Canvas CanvasUI;
    public Image Lifebar;

    public NavMeshAgent Agent;

    [Header("Inventory")]
    public List<ItemObject> Inventory = new List<ItemObject>();

    public List<ItemObject> EquipedItems = new List<ItemObject>();

    //public ItemObject WeaponEquiped;

    public bool TargetIsInRange => Datas.AttackRange > Vector3.Distance(_target.transform.position, transform.position);

    private EnemyBehaviour _target;

    private float _attackCooldown = 0f;

    private float _lifeCooldown = 0f;

    private Datas _baseDatas;

    // Start is called before the first frame update
    void Start()
    {
        _baseDatas = (Datas)Datas.Clone();

        var inv = new List<ItemObject>(Inventory);
        Inventory.Clear();
        inv.ForEach(x =>
        {
            ItemObject i = Instantiate(x) as ItemObject;
            Inventory.Add(i);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(_target == null)
            _target = GameManager.Instance.GetNearestEnemy();

        Agent.SetDestination(_target.transform.position);

        if(_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }
        else if(TargetIsInRange)
        {
            BaseAttack();
        }

        if (_lifeCooldown >= Datas.LifeRegenCooldown)
        {
            _lifeCooldown = 0f;
            Datas.Lifepoints = (uint)Mathf.Clamp(
                Datas.LifeRegen + Datas.Lifepoints, 
                0,
                Datas.MaxLifepoints
            );
        }
        else
        {
            _lifeCooldown += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        CanvasUI.transform.LookAt(Camera.main.transform.position);
        Lifebar.fillAmount = (float)Datas.Lifepoints / Datas.MaxLifepoints;

        Datas.Damages = 5 * GameManager.Instance.Floor;
    }

    #region COMBAT

    void BaseAttack()
    {
        if(_target != null)
        {
            _target.TakeDamage(Datas.Damages);
            _attackCooldown = 1f / Datas.SpeedAttack;
        }
    }

    internal void TakeDamage(uint damages)
    {
        Datas.Lifepoints -= damages;
        _lifeCooldown = 0f;
    }

    #endregion

    #region INVENTORY 

    private void RebuildDatas()
    {
        EquipedItems.ForEach(x => Datas += x.Datas);
    }

    public void AddItem(ItemObject item)
    {
        Inventory?.Add(item);
    }

    public void RemoveItem(ItemObject item)
    {
        if(Inventory.Contains(item))
            Inventory?.Remove(item);
    }

    public void Equip(ItemObject item)
    {
        if (!EquipedItems.Contains(item))
        {
            var currentItem = EquipedItems.Find(x => x.ItemType == item.ItemType);
            if(currentItem != null)
            {
                currentItem.IsEquiped = false;
                EquipedItems.Remove(currentItem);
            }

            EquipedItems.Add(item);
            item.IsEquiped = true;
            Inventory.Sort((a, b) => a.IsEquiped ? b.IsEquiped ? 1 : -1 : 0);
            RebuildDatas();
        }
    }

    public void Unequip(ItemObject item)
    {
        if (EquipedItems.Contains(item))
        {
            EquipedItems.Remove(item);
            RebuildDatas();
        }
    }

    #endregion
}
