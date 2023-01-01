using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;
using QFSW.QC;
using QFSW.QC.Actions;

public enum PhantaliaWorldItem
{
    IRON,
    CARBON,
    TIN,
    WOOD,
    ROCK,
    MEAT,
    LEAF,
    FRUIT,
    POWDER,
    FLUID,
    LEATHER,
    WOOL,
    GOLD,
    SILVER,
    COPPER,
    DARKIRON,
    BONES,
    DYE,
    ROPE,
}

public class LegendaryInventory : MonoBehaviour
{
    public static LegendaryInventory io;

    Dictionary<PhantaliaWorldItem, int> inventory_phantaliaWorldItems = new Dictionary<PhantaliaWorldItem, int>();
    List<LegendaryItem> inventory_legendaryItems = new List<LegendaryItem>();

    [SerializeField] GameObject projectile;

    bool hasChanged = false;
    [SerializeField] GameObject inventoryItemsUI;
    [SerializeField] Transform inventoryItemsContainer;
    [SerializeField] GameObject legendaryItemUIObject;
    int currentIndexItemUI = 0;

    [SerializeField] Image selected_legendaryitem_thumb;
    [SerializeField] TextMeshProUGUI selected_legendaryitem_title;
    [SerializeField] TextMeshProUGUI selected_legendaryitem_description;
    [SerializeField] GameObject no_items;

    float cooldown_input = 0.0f;


    private void Awake()
    {
        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }

        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.IRON, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.CARBON, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.TIN, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.WOOD, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.ROCK, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.MEAT, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.LEAF, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.FRUIT, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.POWDER, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.FLUID, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.LEATHER, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.WOOL, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.GOLD, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.SILVER, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.COPPER, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.DARKIRON, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.BONES, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.DYE, 0);
        inventory_phantaliaWorldItems.Add(PhantaliaWorldItem.ROPE, 0);

    }

    private void Start()
    {
        /*
        inventory_phantaliaWorldItems[PhantaliaWorldItem.IRON] = 20;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.CARBON] = 16;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.TIN] = 11;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.MEAT] = 14;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.LEAF] = 6;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.FRUIT] = 17;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.POWDER] = 13;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.FLUID] = 3;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.LEATHER] = 30;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.ROPE] = 12;
        inventory_phantaliaWorldItems[PhantaliaWorldItem.DYE] = 13;
        */
    }
    
    public GameObject GetProjectile()
    {
        return projectile;
    }
    public int GetCurrentAmountAvailable(PhantaliaWorldItem item)
    {
        return inventory_phantaliaWorldItems[item];
    }

    public bool CanUseItem(PhantaliaWorldItem item, int amount)
    {
        if(inventory_phantaliaWorldItems[item] >= amount)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool IncreaseItem(PhantaliaWorldItem item, int amount)
    {
        // carryweight
        inventory_phantaliaWorldItems[item] = inventory_phantaliaWorldItems[item] + amount;

        return true;
    }

    public bool UseItem(PhantaliaWorldItem item, int amount)
    {
        if(inventory_phantaliaWorldItems[item] >= amount)
        {
            inventory_phantaliaWorldItems[item] = inventory_phantaliaWorldItems[item] - amount;
            return true;
        } else
        {
            return false;
        }
    }

    [Command]
    public void ToggleInventory(bool toggle)
    {
        if(inventory_legendaryItems.Count == 0)
        {
            no_items.SetActive(true);
        } else
        {
            no_items.SetActive(false);
        }


        // play sound
        if(toggle)
        {
            if(inventory_legendaryItems.Count != 0)
            {
                currentIndexItemUI = 0;
                UpdateLegendaryInventory();
                inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
                selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
                selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
                selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
            }
            

        } else
        {
            if (inventory_legendaryItems.Count != 0)
            {
                inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(false);
            }
                
        }
        inventoryItemsUI.SetActive(toggle);
        
    }

    public void InventoryInput(bool actionOne, bool actionTwo, float horizontal, float vertical)
    {
        if(cooldown_input > Time.time)
        {
            return;
        } 
        
        cooldown_input = Time.time + 0.12f;
        

        if(horizontal < -0.5)
        {
            PreviousLegendaryItemUI();
            return;
        }
        if(horizontal > 0.5)
        {
            NextLegendaryItemUI();
            return;
        }
        
        if(vertical < -0.5)
        {
            NextRowLegendaryItemUI();
            return;
        }
        if(vertical > 0.5)
        {
            PreviousRowLegendaryItemUI();
            
            return;
        }

        if(actionOne)
        {
            cooldown_input = Time.time + 0.12f;

            if (inventory_legendaryItems[currentIndexItemUI].isConsumable)
            {
                Debug.Log("consumed:" + inventory_legendaryItems[currentIndexItemUI].itemTitle + " at index " + currentIndexItemUI);
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.CONSUME_FOOD);
                LegendaryCore.io.AddSpirit(inventory_legendaryItems[currentIndexItemUI].ph);
                LegendaryCore.io.AddHealth(inventory_legendaryItems[currentIndexItemUI].hp);
                inventory_legendaryItems.RemoveAt(currentIndexItemUI);
                //LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_SELECT);
                Destroy(inventoryItemsContainer.GetChild(currentIndexItemUI).gameObject);
                hasChanged = true;
                UpdateLegendaryInventory();
                if (inventory_legendaryItems.Count != 0)
                {
                    currentIndexItemUI = 0;
                    UpdateLegendaryInventory();
                    inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
                    selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
                    selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
                    selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
                }

            }

            if (inventory_legendaryItems[currentIndexItemUI].isEquipableWeapon)
            {
                Debug.Log("equiped:" + inventory_legendaryItems[currentIndexItemUI].itemTitle + " at index " + currentIndexItemUI);
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.WEAPON_EQUIP);
                //LegendaryCore.io.AddSpirit(inventory_legendaryItems[currentIndexItemUI].ph);
                //LegendaryCore.io.AddHealth(inventory_legendaryItems[currentIndexItemUI].hp);
                //inventory_legendaryItems.RemoveAt(currentIndexItemUI);
                //LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_MENU_SELECT);
                //Destroy(inventoryItemsContainer.GetChild(currentIndexItemUI).gameObject);
                hasChanged = true;
                LegendaryWeaponRing.io.AddWeaponToRing(inventory_legendaryItems[currentIndexItemUI].weapon);
                LegendaryCore.io.UpdateWeaponRing();
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().DeactivateWeapon();

                UpdateLegendaryInventory();
                if (inventory_legendaryItems.Count != 0)
                {
                    currentIndexItemUI = 0;
                    UpdateLegendaryInventory();
                    inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
                    selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
                    selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
                    selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
                }

            }

            UpdateLegendaryInventory();
        }

    }

    [Command]
    public void NextLegendaryItemUI()
    {
        if(inventory_legendaryItems.Count-1 > currentIndexItemUI)
        {
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(false);
            currentIndexItemUI++;
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
            selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
            selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
            selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
        }

        
    }

    [Command]
    public void PreviousLegendaryItemUI()
    {
        if (currentIndexItemUI > 0)
        {
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(false);
            currentIndexItemUI--;
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
            selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
            selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
            selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
        }
    }

    public void NextRowLegendaryItemUI()
    {
        if (inventory_legendaryItems.Count - 1 > currentIndexItemUI +12)
        {
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(false);
            currentIndexItemUI += 12;
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
            selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
            selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
            selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
        } else
        {
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(false);
            currentIndexItemUI = inventory_legendaryItems.Count - 1;
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
            selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
            selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
            selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
        }
    }

    public void PreviousRowLegendaryItemUI()
    {
        if (currentIndexItemUI - 11 > 0)
        {
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(false);
            currentIndexItemUI -= 12;
            inventoryItemsContainer.GetChild(currentIndexItemUI).GetChild(1).gameObject.SetActive(true);
            selected_legendaryitem_thumb.sprite = inventory_legendaryItems[currentIndexItemUI].thumb;
            selected_legendaryitem_title.text = inventory_legendaryItems[currentIndexItemUI].itemTitle;
            selected_legendaryitem_description.text = inventory_legendaryItems[currentIndexItemUI].itemDescription;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
        }
    }

    public void AddLegendaryItem(LegendaryItem myItem)
    {
        // supply weight 120

        inventory_legendaryItems.Add(myItem);
        hasChanged = true;
        UpdateLegendaryInventory();
    }

    public void UpdateLegendaryInventory()
    {
        if(!hasChanged)
        {
            return;
        }
        
        // clear the inventory
        int uiObjects = inventoryItemsContainer.childCount;
        for(int i = uiObjects -1; i >= 0; i--)
        {
            Destroy(inventoryItemsContainer.GetChild(i).gameObject);
        }

        // Debug.Log(inventory_legendaryItems.Count);

        // populate
        for(int j = 0; j < inventory_legendaryItems.Count; j++)
        {
            GameObject go = Instantiate(legendaryItemUIObject, inventoryItemsContainer);
            go.transform.GetChild(0).GetComponent<Image>().sprite = inventory_legendaryItems[j].thumb;
        }

        hasChanged = false;


    }




    


}
