using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using QFSW.QC;
using QFSW.QC.Actions;
public enum LegendaryAction
{
    NONE,
    PRIMARY_ACTION,
    SECONDARY_ACTION
}

public enum LegendaryWeaponType
{
    UNARMED = 0,
    SWORD_1H = 1,
    SWORD_2H = 2,
    AXE_1H = 3,
    AXE_2H = 4,
    HAMMER = 5,
    STAFF = 6,
    SPEAR = 7,
    DAGGER = 8,
    BOW = 9,
    PHA = 10
}

public enum LegendaryStatistic
{
    IRON,
    MUSCLE,
    HEART,
    GUILE,
    INTUITION,
    HEALTH,
    SPIRIT,
    ACTION,
    MOMENTUM,
    EXPERIENCE
}

public class LegendaryPlayer : MonoBehaviour
{
    Animator landerAnimator;
    CharacterController characterController;
    Camera landerCamera;
    [SerializeField] float velocity = 0.0f;
    [SerializeField] float horizontal = 0.0f;
    [SerializeField] float vertical = 0.0f;
    [SerializeField] private float forceMagnitude = 1.0f;

    // move: walk, run
    // move: rotate, strafe
    // move: crouch, climb, swim, dive
    // look: aim, free, view
    // jump: jump, high jump, flip jump, side jump, fall, high fall
    // dodge: roll, slide
    // action: attack, push, pull, pickup, drop, give, operate, talk
    // state: mounted, idle, busy, move, battle, victory, defeat, hit, primary, secondary, jump, swim, dive, fall, climb

    bool isMounted = false;
    bool isMoving = false;
    bool isWalking = false;
    bool isRunning = false;
    bool isJumping = false;
    bool isFalling = false;
    bool isCrouching = false;
    bool isClimbing = false;
    bool isSwimming = false;
    bool isDiving = false;
    bool isRolling = false;
    bool isSliding = false;
    bool inBattle = false;
    bool inAction = false;
    bool isSailing = false;
    bool isPushing = false;
    bool isPicking = false;
    bool isCrafting = false;
    bool isCooking = false;
    bool isTanning = false;
    bool isFishing = false;
    bool isMenuRingActive = false;
    bool isCharging = false;
    bool isBuilding = false;
    bool isInAquaticBody = false;
    bool isAiming = false;

    LegendaryAction currentAction = LegendaryAction.NONE;

    Transform currentClimableSurface;
    Transform currentSlideableSurface;
    Vector3 currentPushableSurface;
    Vector3 currentCraftObject;
    Vector3 currentCookObject;
    Vector3 currentTanningObject;

    [SerializeField] GameObject currentMountableObject;
    GameObject currentMount;
    Animator currentMountAnimator;

    GameObject currentCraftableBuildingObject;
    [SerializeField] LegendaryActorStats playerStats;

    // animator caches
    int anim_isMounted = Animator.StringToHash("isMounted");
    int anim_isMoving = Animator.StringToHash("isMoving");
    int anim_isWalking = Animator.StringToHash("isWalking");
    int anim_isRunning = Animator.StringToHash("isRunning");
    int anim_isBackstepping = Animator.StringToHash("isBackstepping");
    int anim_move_horizontal = Animator.StringToHash("horizontal");
    int anim_move_vertical = Animator.StringToHash("vertical");
    int anim_isGrounded = Animator.StringToHash("isGrounded");
    int anim_isJumping = Animator.StringToHash("isJumping");
    int anim_isFalling = Animator.StringToHash("isFalling");
    int anim_isCrouching = Animator.StringToHash("isCrouching");
    int anim_isSwimming = Animator.StringToHash("isSwimming");
    int anim_isDiving = Animator.StringToHash("isDiving");
    int anim_isRolling = Animator.StringToHash("isRolling");
    int anim_isSliding = Animator.StringToHash("isSliding");
    int anim_inBattle = Animator.StringToHash("inBattle");
    int anim_inAction = Animator.StringToHash("inAction");
    int anim_isPushing = Animator.StringToHash("isPushing");
    int anim_counter = Animator.StringToHash("counter");
    int anim_weapontype = Animator.StringToHash("WeaponType");
    int anim_gender = Animator.StringToHash("Gender");
    int anim_isSailing = Animator.StringToHash("isSailing");
    int anim_isCrafting = Animator.StringToHash("isCrafting");
    int anim_isPicking = Animator.StringToHash("isPicking");

    int anim_ActionOne = Animator.StringToHash("Action1");
    int anim_ActionTwo = Animator.StringToHash("Action2");
    int anim_Swim = Animator.StringToHash("Swim");
    int anim_Jump = Animator.StringToHash("Jump");
    int anim_Climb = Animator.StringToHash("Climb");
    int anim_Fall = Animator.StringToHash("Fall");

    int anim_Neigh = Animator.StringToHash("Neigh");
    int anim_Mount = Animator.StringToHash("Mount");

    int anim_isBuilding = Animator.StringToHash("isBuilding");

    int anim_Sheath = Animator.StringToHash("Sheath");

    int anim_GetTreasure = Animator.StringToHash("Get_Treasure");
    int anim_GotTreasure = Animator.StringToHash("Got_Treasure");

    int anim_IsAiming = Animator.StringToHash("isAiming");

    // input actions
    LegendaryPlayerInputActions playerLanderInput;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    [SerializeField] float runMultiplier = 5.0f;

    // input states
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;
    bool isCrouchPressed;
    bool isActionOnePressed;
    bool isActionTwoPressed;
    bool isMiniMapPressed;
    bool isWeaponRingPressed;
    bool isMenuItemLeftPressed;
    bool isMenuItemRightPressed;
    bool isSheathPressed;
    bool isBuildModePressed;
    bool isMountPressed;
    bool isStatMenuPressed;
    bool isLookingViewPressed;
    bool isInventoryPressed;
    bool isEscapePressed;


    Transform currentWeaponObject = null;
    GameObject currentToolObject = null;
    GameObject currentProjectileObject = null;
    GameObject godRayEffect = null;

    [SerializeField] LegendaryWeaponType currentWeaponType = 0;
    [SerializeField] LegendaryMountData currentMountData;

    bool freezeMovement = false;
    bool isLootable = false;
    bool inGameMenu = false;
    bool inInventoryMenu = false;
    bool inStatsMenu = false;
    bool inConsoleMode = false;
    bool isLooking = false;

    [SerializeField] Transform leftHand_t;
    [SerializeField] Transform rightHand_t;

    // jump variables
    [SerializeField] float initialJumpVelocity;
    [SerializeField] float maxJumpHeight = 50.0f;
    [SerializeField] float maxJumpTime = 0.5f;
    [SerializeField] float groundedGravity = -0.05f;
    [SerializeField] float gravity = -9.8f;

    [SerializeField] Transform raft_point;
    [SerializeField] GameObject my_raft;

    // SerializeField
    [SerializeField] AudioClip footstep_land1;
    [SerializeField] AudioClip footstep_land2;
    [SerializeField] AudioClip footstep_both;
    [SerializeField] AudioClip footstep_slide;
    [SerializeField] AudioClip footstep_roll;
    [SerializeField] AudioClip Uhra1;
    AudioSource PlayerSfx1;

    // action variables
    int counter = 0;
    float swimming_cooldown;

    [SerializeField] float rotationFactorPerFrame = 1.0f;
    float cooldown_jump = 0.0f;
    float cooldown_action1 = 0.0f;
    float cooldown_fall = 0.0f;
    float cooldown_push = 0.0f;
    float cooldown_sheath = 0.0f;
    float cooldown_buildmode = 0.0f;
    float chargeThrottle = 0.0f;
    float cooldown_mount = 0.0f;
    float cooldown_statsmenu = 0.0f;
    float cooldown_looking = 0.0f;
    float cooldown_inventorymenu = 0.0f;

    void OnEnable()
    {
        playerLanderInput.Lander.Enable();
    }

    void OnDisable()
    {
        playerLanderInput.Lander.Disable();
    }

    void Awake()
    {

        landerAnimator = GetComponent<Animator>();
        playerLanderInput = new LegendaryPlayerInputActions();
        characterController = GetComponent<CharacterController>();
        landerCamera = Camera.main;
        PlayerSfx1 = GetComponent<AudioSource>();
        playerLanderInput.Lander.Move.started += onMovementInput;
        playerLanderInput.Lander.Move.canceled += onMovementInput;
        playerLanderInput.Lander.Move.performed += onMovementInput;
        playerLanderInput.Lander.Run.started += onRun;
        playerLanderInput.Lander.Run.canceled += onRun;
        playerLanderInput.Lander.Turn.started += onTurn;
        playerLanderInput.Lander.Turn.canceled -= onTurn;
        playerLanderInput.Lander.Jump.started += onJump;
        playerLanderInput.Lander.Jump.canceled += onJump;
        playerLanderInput.Lander.Crouch.started += onCrouch;
        playerLanderInput.Lander.Crouch.canceled += onCrouch;
        playerLanderInput.Lander.ActionOne.started += onActionOne;
        playerLanderInput.Lander.ActionOne.performed += onActionOne;
        playerLanderInput.Lander.ActionOne.canceled += onActionOne;
        playerLanderInput.Lander.ActionTwo.started += onActionTwo;
        playerLanderInput.Lander.ActionTwo.performed += onActionTwo;
        playerLanderInput.Lander.ActionTwo.canceled += onActionTwo;
        playerLanderInput.Lander.ToggleMiniMap.started += onMiniMap;
        playerLanderInput.Lander.ToggleMiniMap.canceled += onMiniMap;
        playerLanderInput.Lander.ToggleMiniMap.performed += onMiniMap;
        playerLanderInput.Lander.ToggleWeaponRing.started += onWeaponRing;
        playerLanderInput.Lander.ToggleWeaponRing.canceled += onWeaponRing;
        playerLanderInput.Lander.ToggleWeaponRing.performed += onWeaponRing;
        playerLanderInput.Lander.MenuItemLeft.started += onMenuItemLeft;
        playerLanderInput.Lander.MenuItemLeft.performed += onMenuItemLeft;
        playerLanderInput.Lander.MenuItemLeft.canceled += onMenuItemLeft;
        playerLanderInput.Lander.MenuItemRight.started += onMenuItemRight;
        playerLanderInput.Lander.MenuItemRight.performed += onMenuItemRight;
        playerLanderInput.Lander.MenuItemRight.canceled += onMenuItemRight;
        playerLanderInput.Lander.Sheath.started += onSheath;
        playerLanderInput.Lander.Sheath.performed += onSheath;
        playerLanderInput.Lander.Sheath.canceled += onSheath;
        playerLanderInput.Lander.BuildMode.started += onBuildMode;
        playerLanderInput.Lander.BuildMode.performed += onBuildMode;
        playerLanderInput.Lander.BuildMode.canceled += onBuildMode;
        playerLanderInput.Lander.ToggleMount.started += onMount;
        playerLanderInput.Lander.ToggleMount.performed += onMount;
        playerLanderInput.Lander.ToggleMount.canceled += onMount;
        playerLanderInput.Lander.StatsMenu.started += onStatsMenu;
        playerLanderInput.Lander.StatsMenu.performed += onStatsMenu;
        playerLanderInput.Lander.StatsMenu.canceled += onStatsMenu;
        playerLanderInput.Lander.Look.started += onLookView;
        playerLanderInput.Lander.Look.performed += onLookView;
        playerLanderInput.Lander.Look.canceled += onLookView;
        playerLanderInput.Lander.Inventory.started += onInventoryView;
        playerLanderInput.Lander.Inventory.canceled += onInventoryView;
        playerLanderInput.Lander.Inventory.performed += onInventoryView;
        playerLanderInput.Lander.JogDial.started += onJogDial;
        playerLanderInput.Lander.JogDial.performed += onJogDial;
        playerLanderInput.Lander.JogDial.canceled += onJogDial;
        playerLanderInput.Lander.EscapeAll.started += onEscapeAllMenus;
        playerLanderInput.Lander.EscapeAll.performed += onEscapeAllMenus;
        playerLanderInput.Lander.EscapeAll.canceled += onEscapeAllMenus;


        setupJumpVariables();
        swimming_cooldown = Time.time;
        raft_point = transform.GetChild(5);
    }

    void onEscapeAllMenus(InputAction.CallbackContext context)
    {
        isEscapePressed = context.ReadValueAsButton();
    } 

    void onInventoryView(InputAction.CallbackContext context)
    {
        isInventoryPressed = context.ReadValueAsButton();
    }

    void onStatsMenu(InputAction.CallbackContext context)
    {
        isStatMenuPressed = context.ReadValueAsButton();
    }

    void onLookView(InputAction.CallbackContext context)
    {
        isLookingViewPressed = context.ReadValueAsButton();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpHeight / 2; 
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
    }

    void onMount(InputAction.CallbackContext context)
    {
        isMountPressed = context.ReadValueAsButton();
        if(!isBuilding || !inGameMenu)
        {
            Mount(isMountPressed);
        }
        
    }
    void onSheath(InputAction.CallbackContext context)
    {
        isSheathPressed = context.ReadValueAsButton();
    }

    void onBuildMode(InputAction.CallbackContext context)
    {
        isBuildModePressed = context.ReadValueAsButton();
    }

    void onMiniMap(InputAction.CallbackContext context)
    {
        isMiniMapPressed = context.ReadValueAsButton();
    }

    void onMenuItemLeft(InputAction.CallbackContext context)
    {
        isMenuItemLeftPressed = context.ReadValueAsButton();
    }

    void onJogDial(InputAction.CallbackContext context)
    {
        float Y = context.ReadValue<float>();
        if(Y > 0.1f)
        {
            isMenuItemLeftPressed = true;
        }

        if(Y < -0.1f)
        {
            isMenuItemRightPressed = true;
        }

        if(Y > -0.1f && Y < 0.1f)
        {
            isMenuItemLeftPressed = false;
            isMenuItemRightPressed = false;
        }
        
    }

    void onMenuItemRight(InputAction.CallbackContext context)
    {
        isMenuItemRightPressed = context.ReadValueAsButton();
    }

    void onWeaponRing(InputAction.CallbackContext context)
    {
        isWeaponRingPressed = context.ReadValueAsButton();
    }

    void onActionTwo(InputAction.CallbackContext context)
    {
        isActionTwoPressed = context.ReadValueAsButton();
    }

    void onActionOne(InputAction.CallbackContext context)
    {
        isActionOnePressed = context.ReadValueAsButton();
        LegendaryCore.io.isActionOnePressed = isActionOnePressed;
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onCrouch(InputAction.CallbackContext context)
    {
        isCrouchPressed = context.ReadValueAsButton();
        isCrouching = !isCrouching;
    }

    void onTurn(InputAction.CallbackContext context)
    {
        if(!inBattle)
        {
            landerAnimator.SetTrigger(anim_isBackstepping);
        }
        
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement = (context.ReadValue<Vector2>().x * GetCameraRight(landerCamera)) + (context.ReadValue<Vector2>().y * GetCameraForward(landerCamera));
        currentRunMovement = currentMovement * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    Vector3 GetCameraForward(Camera l_camera)
    {
        Vector3 forward = l_camera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    Vector3 GetCameraRight(Camera l_camera)
    {
        Vector3 right = l_camera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    public void UpdateStats(LegendaryActorStats stats)
    {
        playerStats = stats;
    }

    public LegendaryActorStats GetActorStats()
    {
        return playerStats;
    }

    public int GetStatistic(LegendaryStatistic stat)
    {
        switch(stat)
        {
            case LegendaryStatistic.IRON:
                return playerStats.iron;
            case LegendaryStatistic.MUSCLE:
                return playerStats.muscle;
            case LegendaryStatistic.HEART:
                return playerStats.heart;
            case LegendaryStatistic.GUILE:
                return playerStats.guile;
            case LegendaryStatistic.INTUITION:
                return playerStats.intuition;
        }
        return 0;
    }

    public void GetTreasure()
    {
        landerAnimator.SetTrigger(anim_GetTreasure);
    }

    public void GotTreasure()
    {
        landerAnimator.SetTrigger(anim_GotTreasure);
    }
    void handleGravity()
    {
        if(isClimbing)
        {
            return;
        }

        if(characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
            //landerAnimator.SetBool(anim_isJumping, false);
            //landerAnimator.SetBool(anim_isGrounded, true);
        } else
        {
            //landerAnimator.SetBool(anim_isGrounded, false);
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }


    public void Battle(bool battleStatus)
    {
        inBattle = battleStatus;
        AllowControls(!battleStatus);
        if (isMounted)
        {
            Mount(false);
        }
        
    }

    public void EscapeCrafting()
    {
        landerAnimator.SetBool(anim_isCrafting, false);
        isCrafting = false;
        isCooking = false;
        isTanning = false;
        AllowControls(true);

    }

    public void AimingWeapon()
    {
        isAiming = true;
        landerAnimator.SetBool(anim_IsAiming, isAiming);
        
    }

    public void Building(bool buildingStatus)
    {
        if(isFalling || isSwimming || isCrafting || isCooking || isFishing || isTanning || inBattle)
        {
            return;
        }
        //currentCookObject = craftingObject;
        landerAnimator.SetBool(anim_isCrafting, buildingStatus);
        landerAnimator.SetBool(anim_isBuilding, buildingStatus);
        isBuilding = buildingStatus;
        LegendaryCore.io.ActivateBuildingCamera(buildingStatus);
        AllowControls(!buildingStatus);
        if (isMounted)
        {
            Mount(false);
        }
    }

    public void Cooking(bool cookingStatus, Vector3 craftingObject)
    {
        currentCookObject = craftingObject;
        landerAnimator.SetBool(anim_isCrafting, cookingStatus);
        isCooking = cookingStatus;
        AllowControls(!cookingStatus);
        if (isMounted)
        {
            Mount(false);
        }
    }

    public void Tanning(bool tanningStatus, Vector3 craftingObject)
    {
        currentTanningObject = craftingObject;
        landerAnimator.SetBool(anim_isCrafting, tanningStatus);
        isTanning = tanningStatus;
        AllowControls(!tanningStatus);
        if (isMounted)
        {
            Mount(false);
        }
    }

    public void Crafting(bool craftingStatus, Vector3 craftingObject)
    {
        currentCraftObject = craftingObject;
        landerAnimator.SetBool(anim_isCrafting, craftingStatus);
        isCrafting = craftingStatus;
        AllowControls(!craftingStatus);
        if (isMounted)
        {
            Mount(false);
        }
    }

    public void Fishing(bool craftingStatus)
    {
        isFishing = craftingStatus;
        if (isMounted)
        {
            Mount(false);
        }

    }

    public void Slide(bool toggle, Transform surface)
    {
        isSliding = toggle;
        //AllowControls(toggle);
        currentSlideableSurface = surface;
        landerAnimator.SetBool(anim_isSliding, toggle);
    }

    public void AllowControls(bool toggle)
    {
        characterController.enabled = toggle;
    }

    public void ActivateWeapon(Transform weapon)
    {
        landerAnimator.SetTrigger(anim_ActionTwo);
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_WEAPON_PICKUP);
        LegendaryFx.io.PlayEffect(LegendaryVisualEffect.GOLD_AURA, transform.position);
        StartCoroutine(SetWeaponDelayed(weapon));

        currentProjectileObject = LegendaryInventory.io.GetProjectile();
       
    }

    public void ActivateTool(GameObject tool)
    {
        if(currentToolObject != null)
        {
            DeactivateTool();
        }
        currentToolObject = tool;

        if(!isFishing)
        {
            tool.transform.parent = rightHand_t;
            tool.transform.position = rightHand_t.position;
            tool.transform.rotation = rightHand_t.rotation;
        } else
        {
            tool.transform.parent = leftHand_t;
            tool.transform.position = leftHand_t.position;
            tool.transform.rotation = leftHand_t.rotation;
        }


    }

    public void DeactivateTool()
    {
        if(currentToolObject != null)
        {
            Destroy(currentToolObject.gameObject);
        }
        
    }

    public void DeactivateWeapon()
    {
        if(currentWeaponObject != null)
        {
            currentWeaponType = LegendaryWeaponType.UNARMED;
            landerAnimator.SetInteger(anim_weapontype, (int)currentWeaponType);

            Destroy(currentWeaponObject.gameObject);
        }
        
    }

    public void Strike(int id)
    {
        switch(id)
        {
            case 1:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.ITEM_CRAFTING_HAMMER_STRIKE);
                LegendaryCraft.io.StrikeHammer();
                break;
            default:
                break;
        }

    }

    IEnumerator DelayObject(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }


    IEnumerator SetWeaponDelayed(Transform weapon)
    {
        currentWeaponObject = weapon;
        yield return new WaitForSeconds(0.2f);
        weapon.parent = leftHand_t;
        weapon.position = leftHand_t.position;
        weapon.rotation = leftHand_t.rotation;
        currentWeaponType = weapon.GetComponent<LegendaryWeapon>().GetWeaponType();
        landerAnimator.SetInteger(anim_weapontype, ((int)currentWeaponType));
    }

    public void inAquaticBody(bool toggle)
    {
        if(toggle)
        {
            isInAquaticBody = true;
        } else
        {
            isInAquaticBody = false;
        }
    }


    void handleRotation()
    {
        //transform.RotateAround(transform.position, transform.up, 180f);

        if(isClimbing)
        {
            transform.rotation = Quaternion.LookRotation(-currentClimableSurface.forward);
            
            return;
        }

        if(isSliding)
        {
            transform.rotation = Quaternion.LookRotation(currentSlideableSurface.forward);
            return;
            
        }

        if(isPushing)
        {
            
            // currentPushableSurface = new Vector3(0, 0, currentMovement.z);
       
            transform.rotation = Quaternion.LookRotation(currentPushableSurface);
            return;
        }

        if (isCrafting)
        {
            transform.rotation = Quaternion.LookRotation(currentCraftObject);
            return;
        }

        if (isCooking)
        {
            transform.rotation = Quaternion.LookRotation(currentCookObject);
            return;
        }

        if (isTanning)
        {
            transform.rotation = Quaternion.LookRotation(currentTanningObject);
            return;
        }

        if (isCharging)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward);
            return;
        }

        if (isBuilding)
        {
            transform.rotation = Quaternion.LookRotation(LegendaryBuilder.io.GetCursorLocation());
            return;
        }

        if(inGameMenu)
        {
            //transform.rotation = Quaternion.LookRotation(-transform.forward);
            return;
        }


        Vector3 positionToLookat;
        //targetRotation = Quaternion.Euler(0, currentMovement.x, 0);
        
            positionToLookat.x = currentMovement.x;
            positionToLookat.y = 0.0f;
            positionToLookat.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //float angle = Mathf.Atan2(currentMovement.y, currentMovement.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

            Quaternion targetRotation = Quaternion.LookRotation(positionToLookat);
            // transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime);
            
            if(isSailing)
            {
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime);
            } else
            {
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 5f);
                //transform.rotation = Quaternion.LookRotation(positionToLookat);
            }

        }

    }


    public void SetRaft(GameObject raft)
    {
        my_raft = raft;
    }
    

    public void Sail()
    {
        if (isMounted)
        {
            Mount(false);
        }

        if (isActionTwoPressed)
        {
            return;
        }

        if(!isSailing)
        {
            transform.rotation = Quaternion.LookRotation(my_raft.GetComponent<LegendaryRaft>().pole.forward);
        }

        characterController.center = new Vector3(0, 0.69f, 0);
        landerAnimator.SetBool(anim_isSailing, true);
        
        my_raft.transform.parent = raft_point;
        isSailing = true;
        LegendaryCore.io.ActivateSailCamera(isSailing);

    }

    void handleJump()
    {
        if(isMounted) {
            return;
        }

        if(isAiming)
        {
            return;
        }

        if(!characterController.isGrounded)
        {
            return;
        }

        if(isCrouching)
        {
            return;
        }

        if(isSailing)
        {
            return;
        }

        if(isCharging)
        {
            return;
        }

        if(isJumpPressed && isJumping)
        {
            return;
        }

        if(isSliding)
        {
            return;
        }

        if(inAction)
        {
            return;
        }

        if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
            landerAnimator.SetBool(anim_isJumping, false);
        }

        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            landerAnimator.SetTrigger(anim_Jump);
            currentMovement.y = initialJumpVelocity;
            currentRunMovement.y = initialJumpVelocity;
            landerAnimator.SetBool(anim_isJumping, true);
            landerAnimator.SetBool(anim_isSwimming, false);
        }
        
        
    }

    public void Climbable(Transform surface)
    {
        currentClimableSurface = surface;
        landerAnimator.SetTrigger(anim_Climb);
        //characterController.enabled = false;
    }

    public void inClimb()
    {
        isClimbing = true;
        //characterController.enabled = false;
        //transform.Translate(new Vector3(transform.forward.x, transform.forward.y, currentClimableSurface.forward.z));
    }

    public void unClimb()
    {
        FallDown();
        //clear();
        //landerAnimator.SetTrigger(anim_Jump);
        return;
    }

    public void FallDown()
    {
        isClimbing = false;
        isJumping = false;
        isFalling = true;
        landerAnimator.SetTrigger(anim_Fall);
        landerAnimator.SetBool(anim_isFalling, true);
        characterController.enabled = true;
    }

    void handleAnimation()
    {
        landerAnimator.SetFloat(anim_move_horizontal, currentMovement.x);
        landerAnimator.SetFloat(anim_move_vertical, currentMovement.z);
        //landerAnimator.SetBool(anim_isGrounded, characterController.isGrounded);

        isWalking = landerAnimator.GetBool(anim_isWalking);
        isRunning = landerAnimator.GetBool(anim_isRunning);

        if(inGameMenu) {
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);
            return;
        }

        if(inAction)
        {
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);
            landerAnimator.SetBool(anim_inAction, true);
            return;
        }

        if (isMovementPressed && !isWalking)
        {
 
           landerAnimator.SetBool(anim_isWalking, true);
  
        } 
        else if (!isMovementPressed && isWalking)
        {
            landerAnimator.SetBool(anim_isWalking, false);
        }

        if((isMovementPressed && isRunPressed) && !isRunning)
        {
            landerAnimator.SetBool(anim_isRunning, true);
                
        }
        else if ((!isMovementPressed || !isRunPressed && isRunning ))
        {
            landerAnimator.SetBool(anim_isRunning, false);
        }

    }

    void handleCrouch()
    {
        landerAnimator.SetBool(anim_isCrouching, isCrouching);   
    }

    public void AllowLoot() 
    {
        isLootable = true;
    }



    void handleActions()
    {
        if(isMounted)
        {
            return;
        }


        if(isBuilding && isActionTwoPressed)
        {
            isCrafting = false;
            landerAnimator.SetBool(anim_isCrafting, isCrafting);
            landerAnimator.SetBool(anim_isCrafting, isCrafting);
            AllowControls(true);
        }


        if(isCrafting && isActionTwoPressed)
        {
            isCrafting = false;
            landerAnimator.SetBool(anim_isCrafting, isCrafting);
            currentCraftObject = Vector3.forward;
            AllowControls(true);

        }

        if (isCooking && isActionTwoPressed)
        {
            isCooking = false;
            landerAnimator.SetBool(anim_isCrafting, isCooking);
            currentCookObject = Vector3.forward;
            AllowControls(true);

        }

        if (isTanning && isActionTwoPressed)
        {
            isTanning = false;
            landerAnimator.SetBool(anim_isCrafting, isTanning);
            currentCookObject = Vector3.forward;
            AllowControls(true);

        }


        if (isSailing && isActionTwoPressed)
        {
            StopSailing();
        }

        if(isClimbing && isActionTwoPressed)
        {
            unClimb();
            //clear();
            //landerAnimator.SetTrigger(anim_Jump);
            return;
        }

        if (inAction)
        {
            return;
        }

        if(isClimbing || isFalling || isSliding || isSwimming || isSailing || isCrafting || isCooking || isFishing || isTanning || isBuilding)
        {
            return;
        }

        if(isActionOnePressed)
        {
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);

            counter += 1;
            if (counter > 3)
            {
                landerAnimator.SetInteger(anim_counter, 0);
                counter = 0;
            }
            else
            {
                landerAnimator.SetInteger(anim_counter, counter);
            }
            landerAnimator.SetTrigger(anim_ActionOne);

            inAction = true;
            

        }

        if(isActionTwoPressed)
        {
            if(landerAnimator.GetBool(anim_inAction))
            {
                return;
            }
            if(isLootable)
            {

            }

            landerAnimator.SetTrigger(anim_ActionTwo);
            landerAnimator.SetBool(anim_inAction, true);
        }
    }

    public void FootstepLeft()
    {
        if(isInAquaticBody)
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_STEP);
        } else
        {
            PlayerSfx1.clip = footstep_land1;
            PlayerSfx1.Play();
        }
        
    }

    public void FootstepRight()
    {
        if (isInAquaticBody)
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_STEP_2);
        }
        else
        {
            PlayerSfx1.clip = footstep_land2;
            PlayerSfx1.Play();
        }


    }

    public void FootstepBoth()
    {
        if (isInAquaticBody)
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_SPLASH);
        }
        PlayerSfx1.clip = footstep_both;
        PlayerSfx1.Play();
    }

    public void SwimStroke()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.WATER_SPLASH);
    }

    public void FootstepSlide()
    {
        PlayerSfx1.clip = footstep_slide;
        PlayerSfx1.Play();
    }

    public void FootstepRoll()
    {
        PlayerSfx1.clip = footstep_roll;
        PlayerSfx1.Play();
    }

    public void Uhra()
    {
        LegendaryCore.io.PlaySfx2(Uhra1);
        //PlayerSfx1.clip = Uhra1;
        //PlayerSfx1.Play();
    }

    public void Hurt()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.BATTLE_PLAYER_HURT);

    }

    public void PushObject()
    {
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.FEMALE_PUSH);
    }

    public bool isUnarmed()
    {
        if(currentWeaponType == LegendaryWeaponType.UNARMED)
        {
            return true;
        } else
        {
            return false;
        }

    }

    public bool CheckForWeapon(LegendaryWeaponType weaponType)
    {
        if(currentWeaponType == weaponType)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public LegendaryWeaponType GetCurrentPlayerWeaponType()
    {
        return currentWeaponType;
    }

    public void PlayBattleSound(int index)
    {
        switch(index)
        {
            case 0:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.FEMALE_HURAH_1);
                break;
            case 1:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.FEMALE_HURAH_2);
                break;
            case 3:
                LegendaryAudio.io.PlaySfx(LegendaryAudioType.FEMALE_HIT_1);
                break;
        }
    }

    public void CancelAim()
    {
        isAiming = false;
        landerAnimator.SetBool(anim_IsAiming, isAiming);
    }

    public void ShootWeapon()
    {
        // play sound
        if (currentWeaponObject == null)
        {
            return;
        }
        
        GameObject go = Instantiate(currentProjectileObject, currentWeaponObject.GetChild(0).GetChild(0).position, currentWeaponObject.GetChild(0).GetChild(0).rotation);
        StartCoroutine(ShootProjectile(go));
        isAiming = false;
        // currentWeaponObject.GetChild(0).GetChild(0)


    }

    IEnumerator ShootProjectile(GameObject go)
    {
        landerAnimator.SetBool(anim_IsAiming, false);
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.ARROW_RELEASE);
        go.GetComponent<Rigidbody>().velocity = transform.forward * 8.0f;
        yield return new WaitForSeconds(5.0f);
        Destroy(go);
    }

    public void LoadWeapon()
    {
        
    }

    public void ChargePha()
    {
        if(isCharging)
        {
            return;
        }
        AllowControls(false);
        isCharging = true;
        godRayEffect = Instantiate(LegendaryCore.io.GetGodRay(), transform);
    }

    public void EndCharge()
    {
        isCharging = false;
        godRayEffect.transform.DOScale(0f, 0.8f);
        clear();
        Destroy(godRayEffect, 1f);
    }


    public void SwingWeapon(int weapontype)
    {
        if(isFishing)
        {
            currentToolObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            return;
        }


        if(currentWeaponObject == null)
        {
            return;
        }

        switch(weapontype)
        {
            case 3:
                currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(true); // Axe Swing
                break;
            case 5:
                currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(true); // Hammer Swing
                break;
            case 6:
                //currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(true); // Staff Swing
                break;
            case 7:
                currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(true); // Spear Swing
                break;
        }


        currentWeaponObject.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
        LegendaryAudio.io.PlaySfx(LegendaryAudioType.WEAPON_SWING_1);
    }

    public void EndSwingWeapon()
    {
        if (isFishing)
        {
            currentToolObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            return;
        }

        if (currentWeaponObject == null)
        {
            return;
        }

        switch (currentWeaponType)
        {
            case LegendaryWeaponType.AXE_1H:
                currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(false); // Axe Swing
                break;
            case LegendaryWeaponType.HAMMER:
                currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(false); // Hammer Swing
                break;
            case LegendaryWeaponType.STAFF:
                break;
            case LegendaryWeaponType.SPEAR:
                currentWeaponObject.GetChild(0).GetChild(0).gameObject.SetActive(false); // Spear Swing
                break;
        }

        currentWeaponObject.GetChild(0).GetComponent<TrailRenderer>().enabled = false;
    }

    public void StopSailing()
    {
        if(isSailing)
        {
            my_raft.GetComponent<LegendaryRaft>().cooldownRaft = Time.time + 2.25f;
            isSailing = false;
            characterController.center = new Vector3(0, 1.02f, 0);
            landerAnimator.SetBool(anim_isSailing, false);
            my_raft.transform.parent = null;
            isSailing = false;
            LegendaryCore.io.ActivateSailCamera(isSailing);
        }
    }

    public void clear()
    {
        inAction = false;
        landerAnimator.SetBool(anim_inAction, false);
        landerAnimator.SetBool(anim_isWalking, false);
        landerAnimator.SetBool(anim_isRunning, false);
        landerAnimator.SetBool(anim_isFalling, false);
        //landerAnimator.SetBool(anim_IsAiming, false);
        //landerAnimator.SetBool(anim_isSwimming, false);
        characterController.enabled = true;
        isClimbing = false;
        isFalling = false;
        isSliding = false;
        isPushing = false;
        //isAiming = false;
    }


    public void Mount(bool toggle)
    {
        if(currentMountableObject == null)
        {
            return;
        }


        if(inGameMenu || isSwimming)
        {
            return;
        }


        if(cooldown_mount > Time.time)
        {
            return;
        }

       
        cooldown_mount = Time.time + 0.25f;


        if (isMounted || !toggle)
        {
            if(isMounted)
            {
                LegendaryCore.io.PlayMountEffect();
            }
            Destroy(currentMount);
            //Debug.Log("unmounted");
            isMounted = false;
            landerAnimator.SetBool(anim_isMounted, isMounted);
            currentMountAnimator = null;
            
            
            characterController.radius = 0.11f;
        } else 
        {
            currentMount = Instantiate(currentMountableObject, transform.position, transform.rotation, raft_point);
            
            
            // set mount to null
            //Debug.Log("mounted");
            isMounted = true;
            characterController.radius = 0.63f;
            landerAnimator.SetBool(anim_isMounted, isMounted);
            landerAnimator.SetTrigger(anim_Mount);
            currentMountAnimator = currentMount.GetComponent<Animator>();
            LegendaryCore.io.PlayMountEffect();
        }
    }

    void handleMovement()
    {
        if (inAction || isClimbing || inBattle || isCrafting || isCooking || isCharging || isTanning || isBuilding || inGameMenu || isAiming)
        {
            
            if (isRunPressed)
            {
                characterController.Move(currentMovement * 0);
            }
            else
            {
                characterController.Move(currentMovement * 0);
            }
            return;
        }

        if(isMounted)
        {
            characterController.Move(currentRunMovement * 3.0f * Time.deltaTime);

            //landerAnimator.SetFloat(anim_move_horizontal, currentMovement.x);
            //landerAnimator.SetFloat(anim_move_vertical, currentMovement.z);

            return;
        }

        if(isCrouching)
        {
            if (isRunPressed)
            {
                characterController.center = new Vector3(0, 0.3f, 0);
                characterController.height = 0.6f;
                characterController.Move(currentMovement * 0.1f * Time.deltaTime);
            } else
            {
                characterController.center = new Vector3(0, 1.02f, 0);
                characterController.height = 1.8f;
            }
            return;
        }

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }

    public void SeaIn()
    {
        if(isMounted)
        {
            Mount(false);
        }


        if(isFalling)
        {
            clear();
            isFalling = false;
            landerAnimator.SetBool(anim_isFalling, false);
        }

        if(isSailing)
        {
            return;
        }

        if(isSwimming)
        {
            return;
        }
        clear();
        swimming_cooldown = Time.time + 0.2f;
        landerAnimator.SetTrigger(anim_Swim);
        isSwimming = true;
        DeactivateWeapon();
        landerAnimator.SetBool(anim_isSwimming, true);
        LegendaryCore.io.CloseCamAdjust(0.47f);
        //isSwimming = true;
        //landerAnimator.SetBool(anim_isSwimming, true);
    }

    public void SeaOut()
    {
        if(swimming_cooldown > Time.time)
        {
            return;
        }

        if (isSailing)
        {
            return;
        }

        isSwimming = false;
        landerAnimator.SetBool(anim_isSwimming, false);
        LegendaryCore.io.CloseCamAdjust(1.54f);
    }

    public void Swim()
    {
        isSwimming = true;
        landerAnimator.SetBool(anim_isSwimming, true);
    }

    public void SetMountable(GameObject mountable, LegendaryMountData mountable_data)
    {
        // better use index
        // use the mountable_data
        // update hud
        currentMountableObject = mountable;
    }

    void Start()
    {
        LegendaryCore.io.RegisterPlayer(gameObject);
        HudUpdates();
        landerAnimator.SetInteger(anim_gender, gameObject.GetComponent<LegendaryActor>().gender);

    }

    public void HudUpdates()
    {
            float hp = (float) gameObject.GetComponent<LegendaryActor>().currentHP / (float) gameObject.GetComponent<LegendaryActor>().maxHP;
            float ph = (float) gameObject.GetComponent<LegendaryActor>().currentPh / (float) gameObject.GetComponent<LegendaryActor>().maxPh;
            LegendaryCore.io.UpdateWorldHud(hp, ph, gameObject.GetComponent<LegendaryActor>().currentHP, gameObject.GetComponent<LegendaryActor>().currentPh);
            LegendaryCore.io.UpdateAvatarStats(gameObject.GetComponent<LegendaryActor>().avatar, gameObject.GetComponent<LegendaryActor>().actorLevel);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(cooldown_push > Time.time)
        {
            if(forceMagnitude < 0.7f)
            {
                forceMagnitude += 0.1f;
            }
            return;
        }


        if (hit.collider.gameObject.CompareTag("Pushable"))
        {
            isPushing = true;
            cooldown_push = Time.time + 0.2f;
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                rb.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
                currentPushableSurface = forceDirection;
            }
        } else
        {
            forceMagnitude = 0.0f;
            isPushing = false;
        }

        landerAnimator.SetBool(anim_isPushing, isPushing);


    }

    private void FixedUpdate()
    {
        landerAnimator.SetBool(anim_isGrounded, characterController.isGrounded);
        if (characterController.isGrounded)
        {
            isFalling = false;
            landerAnimator.SetBool(anim_isFalling, false);
            landerAnimator.SetBool(anim_isGrounded, true);
            // cooldown_fall = Time.time + 0.02f;
            if(!isCrouching && characterController.height < 1.8f)
            {
                characterController.center = new Vector3(0, 1.02f, 0);
                characterController.height = 1.8f;
            }

        } else
        {
            /*
            if (cooldown_fall < Time.time)
            {
                return;
            }*/

            landerAnimator.SetBool(anim_isGrounded, false);

            if (!isSwimming || !isClimbing || !isJumping)
            {
                if(!Physics.Raycast(transform.position, Vector3.down, 3f))
                {
                    isFalling = true;
                    landerAnimator.SetBool(anim_isFalling, true);
                    //Debug.Log("From Fixed Update");
                    FallDown();
                }

            }
        }

        


    }

    public void InGameMenu(bool toggle)
    {
        inGameMenu = toggle;
    }

    public void InStatsMenu(bool toggle)
    {
        inStatsMenu = toggle;
    }

    public void InInventory(bool toggle)
    {
        inInventoryMenu = toggle;
    }

    public void InConsoleMode(bool toggle)
    {
        inConsoleMode = toggle;
    }

    public LegendaryMountData GetCurrentMount()
    {
        return currentMountData;
    }

    private void Update()
    {
  
        if(isEscapePressed)
        {
            if(!inBattle)
            {
                LegendaryCore.io.EscapeAll();
                return;
            }
        }



        if(inConsoleMode)
        {
            return;
        }

        if(inInventoryMenu)
        {
            //Debug.Log("x: " + currentMovementInput.x + " y: " + currentMovementInput.y);
            LegendaryInventory.io.InventoryInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.x, currentMovementInput.y);
        }

        if(isInventoryPressed)
        {
            if (inBattle)
            {
                return;
            }

            if (cooldown_inventorymenu > Time.time)
            {
                return;
            }

            if (!inInventoryMenu && !inGameMenu && !inStatsMenu)
            {
                inGameMenu = true;
                AllowControls(false);
                LegendaryInventory.io.ToggleInventory(inGameMenu);
                inInventoryMenu = true;
            }
            else
            {
                inGameMenu = false;
                LegendaryInventory.io.ToggleInventory(inGameMenu);
                AllowControls(true);
                inInventoryMenu = false;
            }
            cooldown_inventorymenu = Time.time + 0.5f;
            return;

        }

        if (isStatMenuPressed)
        {
            if (inBattle)
            {
                return;
            }

            if(isCrafting || isCooking || isTanning)
            {
                return;
            }

            if (cooldown_statsmenu > Time.time) 
            {
                return;
            }


            if (!inStatsMenu && !inGameMenu && !inInventoryMenu)
            {
                LegendaryCore.io.ToggleMiniMap(false);
                inGameMenu = true;
                AllowControls(false);
                LegendaryCore.io.ShowStatisticsMenu(inGameMenu);
                inStatsMenu = true;
            }
            else
            {
                inGameMenu = false;
                LegendaryCore.io.ShowStatisticsMenu(inGameMenu);
                AllowControls(true);
                inStatsMenu = false;
            }
            cooldown_statsmenu = Time.time + 0.5f;
            return;


        }

        if(inGameMenu)
        {
            
            return;
        }



        if (isLookingViewPressed)
        {
            if(inBattle)
            {
                return;
            }

            if (cooldown_looking > Time.time)
            {
                return;
            }


            if (!isLooking)
            {
                LegendaryCore.io.SwitchCamera(LegendaryView.CLOSE);
                isLooking = true;
                cooldown_looking = Time.time + 0.5f;
            }
            else
            {
                LegendaryCore.io.SwitchCamera(LegendaryView.NORMAL);
                isLooking = false;
                cooldown_looking = Time.time + 0.5f;
            }

            


        }

        /*
        if(isFalling && characterController.isGrounded)
        {
            isFalling = false;
            landerAnimator.SetBool(anim_isFalling, false);
        }*/



        if(inBattle)
        {
            inAction = false;
            
            landerAnimator.SetBool(anim_inAction, false);
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);
            landerAnimator.SetBool(anim_isFalling, false);
            //landerAnimator.SetBool(anim_isSwimming, false);
            //characterController.enabled = true;
            isClimbing = false;
            isFalling = false;
            isSliding = false;
            LegendaryBattle.io.BattleInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.y);
            //Debug.Log("detect movement:" + currentMovementInput.y);
            //Debug.Log("detect action:" + isActionOnePressed);
            return;
        }


        handleMovement();
        handleRotation();
        handleActions();


        if (isCrafting)
        {
            inAction = false;
            landerAnimator.SetBool(anim_inAction, false);
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);
            landerAnimator.SetBool(anim_isFalling, false);

            LegendaryCraft.io.CraftingInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.y);

            //landerAnimator.SetBool(anim_isSwimming, false);
            //characterController.enabled = true;
            isClimbing = false;
            isFalling = false;
            isSliding = false;

            return;

        }

        if (isCooking)
        {
            inAction = false;
            landerAnimator.SetBool(anim_inAction, false);
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);
            landerAnimator.SetBool(anim_isFalling, false);

            LegendaryCraft.io.CookingInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.y);

            //landerAnimator.SetBool(anim_isSwimming, false);
            //characterController.enabled = true;
            isClimbing = false;
            isFalling = false;
            isSliding = false;

            return;
        }

        if (isTanning)
        {
            inAction = false;
            landerAnimator.SetBool(anim_inAction, false);
            landerAnimator.SetBool(anim_isWalking, false);
            landerAnimator.SetBool(anim_isRunning, false);
            landerAnimator.SetBool(anim_isFalling, false);

            LegendaryCraft.io.TanningInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.y);

            //landerAnimator.SetBool(anim_isSwimming, false);
            //characterController.enabled = true;
            isClimbing = false;
            isFalling = false;
            isSliding = false;

            return;
        }

        if (isCharging)
        {



            if(gameObject.GetComponent<LegendaryActor>().currentPh < gameObject.GetComponent<LegendaryActor>().maxPh)
            {
                if(chargeThrottle > Time.time)
                {
                    return;
                } else
                {
                    gameObject.GetComponent<LegendaryActor>().currentPh++;
                    chargeThrottle = Time.time + 0.52f;
                    HudUpdates();
                }
                
            }
            return;
        }

        if(isFishing)
        {
            LegendaryCraft.io.FishingInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.y);
        }




        handleAnimation();
        handleGravity();
        handleJump();

        if (isMounted)
        {
            //isMoving
            //Jump
            //Neigh

            currentMountAnimator.SetBool(anim_isMoving, isMovementPressed);

            if(cooldown_action1 > Time.time)
            {
                
            } else
            {
                if (isActionOnePressed)
                {
                    //currentMountAnimator.SetTrigger(anim_Neigh);
                    //landerAnimator.SetTrigger(anim_Neigh);
                    cooldown_action1 = Time.time + 0.12f;
                }     
            }

            if (cooldown_jump > Time.time)
            {

            }
            else
            {
                if (isJumpPressed)
                {
                    //currentMountAnimator.SetTrigger(anim_Jump);
                    cooldown_jump = Time.time + 0.22f;
                }
            }


        }        

        handleCrouch();




        if(isMiniMapPressed)
        {
            LegendaryCore.io.ToggleMiniMap();
        }
        if(isWeaponRingPressed)
        {
            LegendaryCore.io.ToggleWeaponRing();
        }
        if(LegendaryCore.io.WeaponRingStatus())
        {
            if(isMenuItemLeftPressed) { LegendaryCore.io.WeaponRingLeft(); }
            if(isMenuItemRightPressed) { LegendaryCore.io.WeaponRingRight(); }
            // check OK or CANCEL
        }

        if(isSheathPressed)
        {
            if(cooldown_sheath > Time.time)
            {
                return;
            }
            if(currentWeaponObject == null)
            {
                return;
            }

            cooldown_sheath = Time.time + 0.22f;
            landerAnimator.SetTrigger(anim_Sheath);
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.SHEATH);
            

        }

        if(isBuilding)
        {
            LegendaryBuilder.io.BuildingInput(isActionOnePressed, isActionTwoPressed, currentMovementInput.x, currentMovementInput.y);

            if (isMenuItemLeftPressed) { LegendaryBuilder.io.DecreaseCurrentIndex(); }
            if (isMenuItemRightPressed) { LegendaryBuilder.io.IncreaseCurrentIndex(); }

            Vector3 targetPostition = new Vector3(LegendaryBuilder.io.GetCursorLocation().x,
                                       this.transform.position.y,
                                       LegendaryBuilder.io.GetCursorLocation().z);
            transform.LookAt(targetPostition);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            //transform.LookAt(LegendaryBuilder.io.GetCursorLocation());
        }

        if(isBuildModePressed)
        {
            if(cooldown_buildmode > Time.time)
            {
                return;
            }

            cooldown_buildmode = Time.time + 0.22f;
            isBuilding = !isBuilding;
            Building(isBuilding);
            LegendaryBuilder.io.ActivateBuildCursor(isBuilding);

            if (isBuilding)
            {
                inAction = false;
                landerAnimator.SetBool(anim_inAction, false);
                landerAnimator.SetBool(anim_isWalking, false);
                landerAnimator.SetBool(anim_isRunning, false);
                landerAnimator.SetBool(anim_isFalling, false);
                
                

                //landerAnimator.SetBool(anim_isSwimming, false);
                //characterController.enabled = true;
                isClimbing = false;
                isFalling = false;
                isSliding = false;

                return;
            }
        }




    }

    void CheckPlayerInput()
    {

        


    }

    




}
