using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum LegendaryCursorSize
{
    TINY,
    SMALL,
    NORMAL,
    LARGE,
    BIG,
    MASSIVE

}


public class LegendaryBuilder : MonoBehaviour
{
    public static LegendaryBuilder io;

    public LegendaryStructureItem[] structures;

    float cooldown_input = 0.0f;
    int currentIndex = 0;
    [SerializeField] float cursorSpeed = 5f;
    bool isConstructing = false;

    [SerializeField] Color bad;
    [SerializeField] Color good;
    [SerializeField] GameObject constructionEffectSmall;
    [SerializeField] GameObject constructionEffectMedium;
    [SerializeField] GameObject constructionEffectLarge;
    [SerializeField] GameObject legendaryCursor;
    [SerializeField] Transform cursorPlaceholder;
    [SerializeField] Transform cursorVisual;
    [SerializeField] Material cursorMaterial;
    GameObject currentPlaceHolder;
    [SerializeField] GameObject zoneRed;
    [SerializeField] GameObject zoneGreen;

    [SerializeField] GameObject buildingHud;
    [SerializeField] CanvasGroup buildingHudCanvasGroup;
    [SerializeField] Image currentStructureThumb;
    [SerializeField] TextMeshProUGUI currentStructureDescription;


    float currentOffset = 0.0f;

    bool m_Started; // gizmos
    public LayerMask m_LayerMask;

    bool allowPlacement = false;

    private void Awake()
    {
        if(io == null)
        {
            io = this;
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_Started = true;
    }

    private void SetSizeCursor(LegendaryCursorSize size)
    {
        switch(size)
        {
            case LegendaryCursorSize.TINY:
                cursorPlaceholder.DOScale(1f, 0.5f);
                cursorVisual.DOScale(new Vector3(1f, 0.01440749f, 1f), 0.5f);
                cursorPlaceholder.DOMoveY(-0.25f, 0.5f);
                break;
            case LegendaryCursorSize.SMALL:
                cursorPlaceholder.DOScale(2f, 0.5f);
                cursorVisual.DOScale(new Vector3(2f, 0.01440749f, 2f), 0.5f);
                cursorPlaceholder.DOMoveY(0.2f, 0.5f);
                break;
            case LegendaryCursorSize.NORMAL:
                cursorPlaceholder.DOScale(3f, 0.5f);
                cursorVisual.DOScale(new Vector3(3f, 0.01440749f, 3f), 0.5f);
                cursorPlaceholder.DOMoveY(0.6f, 0.5f);
                break;
            case LegendaryCursorSize.LARGE:
                cursorPlaceholder.DOScale(4f, 0.5f);
                cursorPlaceholder.DOMoveY(1.1f, 0.5f);
                cursorVisual.DOScale(new Vector3(4f, 0.01440749f, 4f), 0.5f);
                break;
            case LegendaryCursorSize.BIG:
                cursorPlaceholder.DOScale(5f, 0.5f);
                cursorVisual.DOScale(new Vector3(5f, 0.01440749f, 5f), 0.5f);
                cursorPlaceholder.DOMoveY(1.6f, 0.5f);
                break;
            case LegendaryCursorSize.MASSIVE:
                cursorPlaceholder.DOScale(10f, 0.5f);
                cursorVisual.DOScale(new Vector3(10f, 0.01440749f, 10f), 0.5f);
                cursorPlaceholder.DOMoveY(4.2f, 0.5f);
                break;
        }
    }

    private void FixedUpdate()
    {
        if(legendaryCursor.activeSelf)
        {

            bool noObstructions = CheckForCursorCollisions();
            bool onLand = RayShoot();
            if(onLand && noObstructions)
            {
                allowPlacement = true;
                zoneGreen.SetActive(true);
                zoneRed.SetActive(false);
            } else
            {
                allowPlacement = false;
                zoneRed.SetActive(true);
                zoneGreen.SetActive(false);
            }

        }
        
    }

    public Vector3 GetCursorLocation()
    {
        return cursorPlaceholder.position;
    }

    bool RayShoot()
    {
        float range = 1.0f;
        Vector3 DirectionRay = legendaryCursor.transform.TransformDirection(Vector3.down);
        //Debug.DrawRay(legendaryCursor.transform.position, DirectionRay * range, Color.blue);
        RaycastHit Hit;
        if (Physics.Raycast(legendaryCursor.transform.position, DirectionRay, out Hit, range))
        {
            if (Hit.collider.CompareTag("Land") || Hit.collider.CompareTag("Floor"))
            {
                currentOffset = Hit.distance;
                //Debug.Log("distance:" + Hit.distance);
                //Debug.Log("Land or Floor");
                return true;
            } else
            {
                return false;
            }
        }

        return false;
    }

    private bool CheckForCursorCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(cursorPlaceholder.position + Vector3.up, cursorPlaceholder.localScale / 2, Quaternion.identity, m_LayerMask);

        int i = 0;
        while (i < hitColliders.Length)
        {
            //Debug.Log("Hit : " + hitColliders[i].name + i);
            i++;
        }

        if(hitColliders.Length > 0)
        {
            return false;
        } else
        {
            return true;
        }

    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_Started)
            Gizmos.DrawWireCube(cursorPlaceholder.position + Vector3.up, cursorPlaceholder.localScale);
    }*/

    IEnumerator ConstructLegendaryStructure()
    {
        yield return new WaitForSeconds(0.12f);

        Vector3 placementStructure = new Vector3(legendaryCursor.transform.position.x, legendaryCursor.transform.position.y - currentOffset, legendaryCursor.transform.position.z);
        // add to save structure
        Instantiate(structures[currentIndex].structure, placementStructure, currentPlaceHolder.transform.rotation);
        isConstructing = false;
        yield return null;
    }

    public void ActivateBuildCursor(bool toggle)
    {
        UpdateBuilderItemHud();
        buildingHud.SetActive(toggle);
        legendaryCursor.transform.position = LegendaryCore.io.GetLocalPlayer().transform.position + Vector3.forward * 2;
        if(LegendaryCore.io.GetLocalPlayer().transform.position.y > 1.0f)
        {

        } else
        {
            legendaryCursor.transform.DOMoveY(0.1f, 0.1f);
        }
        
        legendaryCursor.SetActive(toggle);
        if(currentPlaceHolder == null)
        {
            SetSizeCursor(structures[currentIndex].size);
            currentPlaceHolder = Instantiate(structures[currentIndex].placeholder, legendaryCursor.transform);
            
        }
        if(!toggle)
        {
            Destroy(currentPlaceHolder);
            // save placement
        }
        

    }

    public void UpdateBuilderItemHud()
    {
        currentStructureDescription.text = structures[currentIndex].itemTitle;
        currentStructureThumb.sprite = structures[currentIndex].thumb;
    }

    public void IncreaseCurrentIndex()
    {
        if (cooldown_input > Time.time)
        {
            return;
        }

        if (isConstructing)
        {
            return;
        }

        if(currentIndex == structures.Length-1)
        {
            return;
        }
        currentIndex++;
        Destroy(currentPlaceHolder);
        SetSizeCursor(structures[currentIndex].size);
        currentPlaceHolder = Instantiate(structures[currentIndex].placeholder, legendaryCursor.transform);
        cooldown_input = Time.time + 0.16f;
        UpdateBuilderItemHud();
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.MENU_BUILD_NEXT);
    }

    public void DecreaseCurrentIndex()
    {
        if (cooldown_input > Time.time)
        {
            return;
        }

        if (isConstructing)
        {
            return;
        }
        if(currentIndex == 0)
        {
            return;
        }
        currentIndex--;
        Destroy(currentPlaceHolder);
        SetSizeCursor(structures[currentIndex].size);
        currentPlaceHolder = Instantiate(structures[currentIndex].placeholder, legendaryCursor.transform);
        cooldown_input = Time.time + 0.16f;
        UpdateBuilderItemHud();
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.MENU_BUILD_NEXT);
    }

    public void BuildingInput(bool isActionOnePressed, bool isActionTwoPressed, float directionX, float directionZ)
    {
        if(isConstructing)
        {
            return;
        }

        if (cooldown_input > Time.time)
        {
            return;
        }

        if (isActionOnePressed)
        {
            if(!allowPlacement)
            {
                return;
            }

            //Debug.Log("structure placed: " + structures[currentIndex].itemTitle);
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.MENU_BUILD_PLACE);
            isConstructing = true;
            StartCoroutine(ConstructLegendaryStructure());
            cooldown_input = Time.time + 0.16f;
            return;
        }

        if (isActionTwoPressed)
        {
            if (!allowPlacement)
            {
                return;
            }

            currentPlaceHolder.transform.DORotate(new Vector3(0f, 45f, 0f), 0.5f, RotateMode.LocalAxisAdd);
            //currentPlaceHolder.transform.DOLocalRotate(new Vector3(0f, 45f, 0f), 0.5f, RotateMode.LocalAxisAdd);
            cooldown_input = Time.time + 0.16f;
            return;
        }

        legendaryCursor.transform.Translate(new Vector3(directionX * Time.deltaTime * cursorSpeed, 0, directionZ * Time.deltaTime * cursorSpeed), Space.Self);
        if(allowPlacement)
        {
            cursorMaterial.DOColor(good, 0.2f);
        }
        else
        {
            cursorMaterial.DOColor(bad, 0.2f);
        }
    }





}
