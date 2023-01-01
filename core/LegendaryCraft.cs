using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum LegendaryCraftingType
{
    SMITHING,
    FORGING,
    CARVING,
    TANNING,
    COOKING,
    TAILORING,
    FISHING
}

public class LegendaryCraft : MonoBehaviour
{
    public LegendaryCraftMenuItem[] items;
    public LegendaryCookingMenuItem[] recipes;
    public LegendaryTanningMenuItem[] quilts;

    public static LegendaryCraft io;

    [SerializeField] Sprite thumbSmithing;
    [SerializeField] Sprite thumbForging;
    [SerializeField] Sprite thumbCarving;
    [SerializeField] Sprite thumbTanning;
    [SerializeField] Sprite thumbCooking;
    [SerializeField] Sprite thumbTailoring;

    [SerializeField] Image iconCrafting;

    [SerializeField] Image thumb_item;
    [SerializeField] TextMeshProUGUI item_selected;
    [SerializeField] TextMeshProUGUI item_next;
    [SerializeField] TextMeshProUGUI item_after;
    [SerializeField] TextMeshProUGUI item_previous;
    [SerializeField] TextMeshProUGUI item_before;

    [SerializeField] Image thumb_recipe;
    [SerializeField] TextMeshProUGUI recipe_selected;
    [SerializeField] TextMeshProUGUI recipe_next;
    [SerializeField] TextMeshProUGUI recipe_after;
    [SerializeField] TextMeshProUGUI recipe_previous;
    [SerializeField] TextMeshProUGUI recipe_before;

    [SerializeField] Image thumb_tannable;
    [SerializeField] TextMeshProUGUI tannable_selected;
    [SerializeField] TextMeshProUGUI tannable_next;
    [SerializeField] TextMeshProUGUI tannable_after;
    [SerializeField] TextMeshProUGUI tannable_previous;
    [SerializeField] TextMeshProUGUI tannable_before;

    [SerializeField] TextMeshProUGUI requiredIron; // first slot
    [SerializeField] TextMeshProUGUI requiredCarbon; // second slot
    [SerializeField] TextMeshProUGUI requiredTin; // third slot

    [SerializeField] TextMeshProUGUI requiredMeat; // first slot
    [SerializeField] TextMeshProUGUI requiredLeaf; // second slot
    [SerializeField] TextMeshProUGUI requiredFruit; // third slot
    [SerializeField] TextMeshProUGUI requiredPowder; // fourth slot

    [SerializeField] TextMeshProUGUI availableIron; // first slot
    [SerializeField] TextMeshProUGUI availableCarbon; // second slot
    [SerializeField] TextMeshProUGUI availableTin; // third slot

    [SerializeField] TextMeshProUGUI availableMeat; // first slot
    [SerializeField] TextMeshProUGUI availableLeaf; // second slot
    [SerializeField] TextMeshProUGUI availableFruit; // third slot
    [SerializeField] TextMeshProUGUI availablePowder; // fourth slot

    [SerializeField] TextMeshProUGUI availableLeather;
    [SerializeField] TextMeshProUGUI availableRope;
    [SerializeField] TextMeshProUGUI availableDye;

    [SerializeField] TextMeshProUGUI requiredLeather; // first slot
    [SerializeField] TextMeshProUGUI requiredRope; // second slot
    [SerializeField] TextMeshProUGUI requiredDye; // third slot

    [SerializeField] TextMeshProUGUI message; // smithing
    [SerializeField] TextMeshProUGUI messageCooking;
    [SerializeField] TextMeshProUGUI messageTanning;

    [SerializeField] GameObject smithingTool;
    [SerializeField] GameObject forgingTool;
    [SerializeField] GameObject carvingTool;
    [SerializeField] GameObject tanningTool;
    [SerializeField] GameObject cookingTool;
    [SerializeField] GameObject fishingTool;


    [SerializeField] int currentIndex = 0;

    float cooldown_input = 0.0f;
    LegendaryCraftMenuItem currentCraftedObject;
    LegendaryCookingMenuItem currentCookedObject;
    LegendaryTanningMenuItem currentTanningObject;

    GameObject tool;
    Transform impactPoint;
    [SerializeField] GameObject smithingSparkEffect;
    [SerializeField] GameObject forgingSmokeEffect;
    [SerializeField] GameObject cookingSmokeEffect;
    [SerializeField] GameObject fishingSplashEffect;
    [SerializeField] GameObject bloodSplashEffect;

    int anim_Smithing = Animator.StringToHash("Crafting_Smithing");
    int anim_Forging = Animator.StringToHash("Crafting_Forging");
    int anim_Carving = Animator.StringToHash("Crafting_Carving");
    int anim_Tanning = Animator.StringToHash("Crafting_Tanning");
    int anim_Cooking = Animator.StringToHash("Crafting_Cooking");
    int anim_Tailoring = Animator.StringToHash("Crafting_Tailoring");
    int anim_Fishing = Animator.StringToHash("Crafting_Fishing");

    [SerializeField] Image skillProgress;
    [SerializeField] Image skillProgressCooking;
    [SerializeField] Image skillProgressTanning;

    // Start is called before the first frame update
    void Awake()
    {
        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }

        currentIndex = 0;
        thumb_item.sprite = items[currentIndex].thumb;

        item_next.text = items[1].itemTitle;
        item_previous.text = items[items.Length - 1].itemTitle;
        item_selected.text = items[currentIndex].itemTitle;
        item_before.text = items[items.Length - 2].itemTitle;
        item_after.text = items[2].itemTitle;
        requiredIron.text = items[currentIndex].iron + "";
        requiredCarbon.text = items[currentIndex].carbon + "";
        requiredTin.text = items[currentIndex].tin + "";

        thumb_recipe.sprite = recipes[currentIndex].thumb;
        recipe_next.text = recipes[1].itemTitle;
        recipe_previous.text = recipes[recipes.Length - 1].itemTitle;
        recipe_selected.text = recipes[0].itemTitle;
        recipe_before.text = recipes[recipes.Length - 2].itemTitle;
        recipe_after.text = recipes[2].itemTitle;

        requiredMeat.text = recipes[currentIndex].meat + "";
        requiredLeaf.text = recipes[currentIndex].leaf + "";
        requiredFruit.text = recipes[currentIndex].fruit + "";
        requiredPowder.text = recipes[currentIndex].powder + "";

        thumb_tannable.sprite = quilts[currentIndex].thumb;
        tannable_next.text = quilts[1].itemTitle;
        tannable_previous.text = quilts[quilts.Length - 1].itemTitle;
        tannable_selected.text = quilts[0].itemTitle;
        tannable_before.text = quilts[quilts.Length - 2].itemTitle;
        tannable_after.text = quilts[2].itemTitle;

        requiredLeather.text = quilts[currentIndex].leather + "";
        requiredRope.text = quilts[currentIndex].rope + "";
        requiredDye.text = quilts[currentIndex].dye + "";


        skillProgress.fillAmount = 0;
        skillProgressCooking.fillAmount = 0;
        skillProgressTanning.fillAmount = 0;

    }

    private void Start()
    {
        availableIron.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.IRON) + "";
        availableCarbon.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.CARBON) + "";
        availableTin.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.TIN) + "";


        UpdateSatchel();
        UpdateInventory();
        UpdateBasket();
    }

    public void SetupCrafting(LegendaryCraftingType craftingType)
    {
        currentIndex = 0;

        switch(craftingType)
        {
            case LegendaryCraftingType.FISHING:
                if (tool == null)
                {
                    LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().DeactivateWeapon();
                    tool = Instantiate(fishingTool);
                    LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().ActivateTool(tool);
                }
                break;
        }
    }

    // smithing list
    public void UpdateInventory()
    {
        availableIron.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.IRON) + "";
        availableCarbon.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.CARBON) + "";
        availableTin.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.TIN) + "";
    }


    // cooking list
    public void UpdateSatchel()
    {
        availableMeat.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.MEAT) + "";
        availableLeaf.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.LEAF) + "";
        availableFruit.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.FRUIT) + "";
        availablePowder.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.POWDER) + "";
    }

    public void UpdateBasket()
    {
        availableLeather.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.LEATHER) + "";
        availableRope.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.ROPE) + "";
        availableDye.text = LegendaryInventory.io.GetCurrentAmountAvailable(PhantaliaWorldItem.DYE) + "";
    }

    public void StrikeHammer()
    {
        Instantiate(smithingSparkEffect, impactPoint);
    }

    public void StrikeRock(Vector3 minePoint)
    {
        GameObject go = Instantiate(smithingSparkEffect, minePoint, Quaternion.identity);
        Destroy(go, 5.0f);
    }

    public void BurnPot()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_CRAFTING_COOKING);
        GameObject go = Instantiate(cookingSmokeEffect, impactPoint);
        go.transform.DOMoveY(2f, 4.5f);
        Destroy(go, 5.0f);
    }

    public void StrikeFish(Vector3 splashPoint)
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_SPLASH);
        GameObject go = Instantiate(fishingSplashEffect, splashPoint, Quaternion.identity);
        //go.transform.DOMoveY(2f, 4.5f);
        Destroy(go, 5.0f);
    }

    public void StrikeAnimal(Vector3 strikePoint)
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BLOOD_SQUISH);
        GameObject go = Instantiate(bloodSplashEffect, strikePoint, Quaternion.identity);
        //go.transform.DOMoveY(2f, 4.5f);
        Destroy(go, 5.0f);
    }

    IEnumerator DoneCrafting()
    {
        skillProgress.DOFillAmount(1.0f, 1.5f);
        LegendaryItem newWeaponPart = new LegendaryItem();
        newWeaponPart.category = LegendaryItemCategory.WEAPON_PART;
        newWeaponPart.itemTitle = currentCraftedObject.itemTitle;
        newWeaponPart.itemDescription = currentCraftedObject.itemDescription;
        newWeaponPart.thumb = currentCraftedObject.thumb;
        newWeaponPart.isComposite = true;
        newWeaponPart.compositeCategory = currentCraftedObject.compositeCategory;
        newWeaponPart.power = currentCraftedObject.power;
        LegendaryInventory.io.AddLegendaryItem(newWeaponPart);
        yield return new WaitForSeconds(2.0f);
        LegendaryCore.io.SetItem(currentCraftedObject.itemTitle, currentCraftedObject.itemDescription, currentCraftedObject.thumb);
        LegendaryCore.io.ShowItem(1);        
        LegendaryCore.io.ToggleCraftingdHud(true);
        skillProgress.fillAmount = 0;
    }

    IEnumerator DoneCooking()
    {
        BurnPot();
        skillProgressCooking.DOFillAmount(1.0f, 1.5f);
        LegendaryItem newConsumable = new LegendaryItem();
        newConsumable.category = LegendaryItemCategory.CONSUMABLE_ITEM;
        newConsumable.itemTitle = currentCookedObject.itemTitle;
        newConsumable.itemDescription = currentCookedObject.itemDescription;
        newConsumable.thumb = currentCookedObject.thumb;
        newConsumable.isConsumable = true;
        newConsumable.hp = currentCookedObject.recover_hp;
        newConsumable.ph = currentCookedObject.recover_ph;
        LegendaryInventory.io.AddLegendaryItem(newConsumable);
        yield return new WaitForSeconds(2.0f);
        LegendaryCore.io.SetItem(currentCookedObject.itemTitle, currentCookedObject.itemDescription, currentCookedObject.thumb);
        LegendaryCore.io.ShowItem(1);
        LegendaryCore.io.ToggleCookingdHud(true);
        skillProgressCooking.fillAmount = 0;
    }

    IEnumerator DoneTanning()
    {
        //BurnPot();
        skillProgressTanning.DOFillAmount(1.0f, 1.5f);
        LegendaryItem newWearable = new LegendaryItem();
        newWearable.category = LegendaryItemCategory.WEARABLE;
        newWearable.itemTitle = currentTanningObject.itemTitle;
        newWearable.itemDescription = currentTanningObject.itemDescription;
        newWearable.thumb = currentTanningObject.thumb;
        newWearable.isWearable = true;
        newWearable.basecolor = currentTanningObject.basecolor;
        newWearable.tartan = currentTanningObject.tartan;
        LegendaryInventory.io.AddLegendaryItem(newWearable);
        yield return new WaitForSeconds(2.0f);
        LegendaryCore.io.SetItem(currentTanningObject.itemTitle, currentTanningObject.itemDescription, currentTanningObject.thumb);
        LegendaryCore.io.ShowItem(1);
        LegendaryCore.io.ToggleTanningdHud(true);
        skillProgressTanning.fillAmount = 0;
    }


    // TODO

    public void FishingInput(bool isActionOnePressed, bool isActionTwoPressed, float direction)
    {
        if (cooldown_input > Time.time)
        {
            return;
        }

        if (isActionOnePressed)
        {
            LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetTrigger(anim_Fishing);
            cooldown_input = Time.time + 0.16f;
            return;
        }

    }

    public void TanningInput(bool isActionOnePressed, bool isActionTwoPressed, float direction)
    {
        if (cooldown_input > Time.time)
        {
            return;
        }

        if (!isActionOnePressed && !isActionTwoPressed && (int)direction == 0)
        {
            return;
        }

        if (isActionOnePressed)
        {
            currentTanningObject = quilts[currentIndex];
            messageTanning.text = "";
            if (tool == null)
            {
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().DeactivateWeapon();
                tool = Instantiate(tanningTool);
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().ActivateTool(tool);
            }

            bool enoughLeather = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.LEATHER, currentTanningObject.leather);
            bool enoughRope = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.ROPE, currentTanningObject.rope);
            bool enoughDye = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.DYE, currentTanningObject.dye);

            if (enoughLeather && enoughRope && enoughDye)
            {
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.LEATHER, currentTanningObject.leather);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.ROPE, currentTanningObject.rope);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.DYE, currentTanningObject.dye);
            }
            else
            {
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_CRAFTING_FAILED);
                messageTanning.text = "You need more resources to stitch this item";
                return;
            }

            LegendaryCraft.io.UpdateBasket();

            LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetTrigger(anim_Tanning);
            LegendaryCore.io.ToggleTanningdHud(false);

            StartCoroutine(DoneTanning());
            cooldown_input = Time.time + 5.0f;
            return;
        }

        // move up
        if (direction > 0)
        {
            currentIndex--;
            cooldown_input = Time.time + 0.16f;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
            messageTanning.text = "";
        }
        // move down
        if (direction < 0)
        {
            currentIndex++;
            cooldown_input = Time.time + 0.16f;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
            messageTanning.text = "";
        }

        if (currentIndex == -1)
        {
            currentIndex = quilts.Length - 1;
        }
        else if (currentIndex == quilts.Length)
        {
            currentIndex = 0;
        }

        tannable_selected.text = quilts[currentIndex].itemTitle;
        thumb_tannable.sprite = quilts[currentIndex].thumb;

        requiredLeather.text = quilts[currentIndex].leather + "";
        requiredRope.text = quilts[currentIndex].rope + "";
        requiredDye.text = quilts[currentIndex].dye + "";

        if (currentIndex == 1)
        {
            tannable_next.text = quilts[2].itemTitle;
            tannable_previous.text = quilts[0].itemTitle;

            tannable_before.text = quilts[quilts.Length - 1].itemTitle;
            tannable_after.text = quilts[3].itemTitle;

            return;
        }

        if (currentIndex == quilts.Length - 2)
        {
            tannable_next.text = quilts[quilts.Length - 1].itemTitle;
            tannable_previous.text = quilts[quilts.Length - 3].itemTitle;

            tannable_before.text = quilts[quilts.Length - 4].itemTitle;
            tannable_after.text = quilts[0].itemTitle;

            return;
        }

        if (currentIndex == 0)
        {

            tannable_next.text = quilts[1].itemTitle;
            tannable_previous.text = quilts[quilts.Length - 1].itemTitle;

            tannable_before.text = quilts[quilts.Length - 2].itemTitle;
            tannable_after.text = quilts[2].itemTitle;

            return;
        }

        if (currentIndex == quilts.Length - 1)
        {
            tannable_next.text = quilts[0].itemTitle;
            tannable_previous.text = quilts[quilts.Length - 2].itemTitle;

            tannable_before.text = quilts[quilts.Length - 3].itemTitle;
            tannable_after.text = quilts[1].itemTitle;

            return;
        }

        tannable_before.text = quilts[currentIndex - 2].itemTitle;
        tannable_previous.text = quilts[currentIndex - 1].itemTitle;
        tannable_next.text = quilts[currentIndex + 1].itemTitle;
        tannable_after.text = quilts[currentIndex + 2].itemTitle;

    }



    public void CookingInput(bool isActionOnePressed, bool isActionTwoPressed, float direction)
    {
        if (cooldown_input > Time.time)
        {
            return;
        }

        if (!isActionOnePressed && !isActionTwoPressed && (int)direction == 0)
        {
            return;
        }

        if (isActionOnePressed)
        {
            currentCookedObject = recipes[currentIndex];
            message.text = "";
            if (tool == null)
            {
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().DeactivateWeapon();
                tool = Instantiate(cookingTool);
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().ActivateTool(tool);
            }

            bool enoughMeat = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.MEAT, currentCookedObject.meat);
            bool enoughLeaf = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.LEAF, currentCookedObject.leaf);
            bool enoughFruit = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.FRUIT, currentCookedObject.fruit);
            bool enoughPowder = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.POWDER, currentCookedObject.powder);

            if (enoughMeat && enoughLeaf && enoughFruit && enoughPowder )
            {
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.MEAT, currentCookedObject.meat);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.LEAF, currentCookedObject.leaf);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.FRUIT, currentCookedObject.fruit);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.POWDER, currentCookedObject.powder);
            }
            else
            {
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_CRAFTING_FAILED);
                messageCooking.text = "You need more resources to cook this recipe";
                return;
            }

            LegendaryCraft.io.UpdateSatchel();

            LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetTrigger(anim_Cooking);
            LegendaryCore.io.ToggleCookingdHud(false);

            StartCoroutine(DoneCooking());
            cooldown_input = Time.time + 5.0f;
            return;
        }

        // move up
        if (direction > 0)
        {
            currentIndex--;
            cooldown_input = Time.time + 0.16f;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
            messageCooking.text = "";
        }
        // move down
        if (direction < 0)
        {
            currentIndex++;
            cooldown_input = Time.time + 0.16f;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
            messageCooking.text = "";
        }

        if (currentIndex == -1)
        {
            currentIndex = recipes.Length - 1;
        }
        else if (currentIndex == recipes.Length)
        {
            currentIndex = 0;
        }

        recipe_selected.text = recipes[currentIndex].itemTitle;
        thumb_recipe.sprite = recipes[currentIndex].thumb;

        requiredMeat.text = recipes[currentIndex].meat + "";
        requiredLeaf.text = recipes[currentIndex].leaf + "";
        requiredFruit.text = recipes[currentIndex].fruit + "";
        requiredPowder.text = recipes[currentIndex].powder + "";

        if (currentIndex == 1)
        {
            recipe_next.text = recipes[2].itemTitle;
            recipe_previous.text = recipes[0].itemTitle;

            recipe_before.text = recipes[recipes.Length - 1].itemTitle;
            recipe_after.text = recipes[3].itemTitle;

            return;
        }

        if (currentIndex == recipes.Length - 2)
        {
            recipe_next.text = recipes[recipes.Length - 1].itemTitle;
            recipe_previous.text = recipes[recipes.Length - 3].itemTitle;

            recipe_before.text = recipes[recipes.Length - 4].itemTitle;
            recipe_after.text = recipes[0].itemTitle;

            return;
        }

        if (currentIndex == 0)
        {

            recipe_next.text = recipes[1].itemTitle;
            recipe_previous.text = recipes[recipes.Length - 1].itemTitle;

            recipe_before.text = recipes[recipes.Length - 2].itemTitle;
            recipe_after.text = recipes[2].itemTitle;

            return;
        }

        if (currentIndex == recipes.Length - 1)
        {
            recipe_next.text = recipes[0].itemTitle;
            recipe_previous.text = recipes[recipes.Length - 2].itemTitle;

            recipe_before.text = recipes[recipes.Length - 3].itemTitle;
            recipe_after.text = recipes[1].itemTitle;

            return;
        }

        recipe_before.text = recipes[currentIndex - 2].itemTitle;
        recipe_previous.text = recipes[currentIndex - 1].itemTitle;
        recipe_next.text = recipes[currentIndex + 1].itemTitle;
        recipe_after.text = recipes[currentIndex + 2].itemTitle;

    }



    public void CraftingInput(bool isActionOnePressed, bool isActionTwoPressed, float direction)
    {
        if(cooldown_input > Time.time)
        {
            return;
        }

        if(!isActionOnePressed && !isActionTwoPressed && (int) direction == 0)
        {
            return;
        }

        if(isActionOnePressed)
        {
            currentCraftedObject = items[currentIndex];
            message.text = "";
            if (tool == null)
            {
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().DeactivateWeapon();
                tool = Instantiate(smithingTool);
                LegendaryCore.io.GetLocalPlayer().GetComponent<LegendaryPlayer>().ActivateTool(tool);
            }

            bool enoughIron = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.IRON, currentCraftedObject.iron);
            bool enoughCarbon = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.CARBON, currentCraftedObject.carbon);
            bool enoughTin = LegendaryInventory.io.CanUseItem(PhantaliaWorldItem.TIN, currentCraftedObject.tin);

            if(enoughIron && enoughCarbon && enoughTin)
            {
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.IRON, currentCraftedObject.iron);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.CARBON, currentCraftedObject.carbon);
                LegendaryInventory.io.UseItem(PhantaliaWorldItem.TIN, currentCraftedObject.tin);

            } else
            {
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_CRAFTING_FAILED);
                message.text = "You need more resources to craft this item";
                return;
            }

            LegendaryCraft.io.UpdateInventory();

            LegendaryCore.io.GetLocalPlayer().GetComponent<Animator>().SetTrigger(anim_Smithing);
            LegendaryCore.io.ToggleCraftingdHud(false);

            StartCoroutine(DoneCrafting());
            cooldown_input = Time.time + 5.0f;
            return;
        }

        // move up
        if(direction > 0)
        {
            currentIndex--;
            cooldown_input = Time.time + 0.16f;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
            message.text = "";
        } 
        // move down
        if(direction < 0)
        {
            currentIndex++;
            cooldown_input = Time.time + 0.16f;
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.BASIC_MENU_SELECT);
            message.text = "";
        }

        if(currentIndex == -1)
        {
            currentIndex = items.Length - 1;
        } else if (currentIndex == items.Length)
        {
            currentIndex = 0;
        }

        item_selected.text = items[currentIndex].itemTitle;
        thumb_item.sprite = items[currentIndex].thumb;

        requiredIron.text = items[currentIndex].iron + "";
        requiredCarbon.text = items[currentIndex].carbon + "";
        requiredTin.text = items[currentIndex].tin + "";

        if (currentIndex == 1)
        {
            item_next.text = items[2].itemTitle;
            item_previous.text = items[0].itemTitle;

            item_before.text = items[items.Length - 1].itemTitle;
            item_after.text = items[3].itemTitle;

            return;
        }

        if (currentIndex == items.Length - 2)
        {
            item_next.text = items[items.Length - 1].itemTitle;
            item_previous.text = items[items.Length -3].itemTitle;

            item_before.text = items[items.Length - 4].itemTitle;
            item_after.text = items[0].itemTitle;

            return;
        }

        if (currentIndex == 0)
        {

            item_next.text = items[1].itemTitle;
            item_previous.text = items[items.Length - 1].itemTitle;

            item_before.text = items[items.Length - 2].itemTitle;
            item_after.text = items[2].itemTitle;

            return;
        } 

        if(currentIndex == items.Length-1)
        {
            item_next.text = items[0].itemTitle;
            item_previous.text = items[items.Length - 2].itemTitle;

            item_before.text = items[items.Length - 3].itemTitle;
            item_after.text = items[1].itemTitle;

            return;
        }

        item_before.text = items[currentIndex - 2].itemTitle;
        item_previous.text = items[currentIndex - 1].itemTitle;
        item_next.text = items[currentIndex + 1].itemTitle;
        item_after.text = items[currentIndex + 2].itemTitle;

    }

    public void SetImpactPoint(Transform point)
    {
        impactPoint = point;
    }


}
