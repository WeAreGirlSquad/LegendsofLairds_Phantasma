using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TRACK_INPUT
{
    TURN,
    JUMP,
    CROUCH,
    ACTION1,
    ACTION2,
    MINIMAP,
    WEAPONRING,
    SHEATH,
    BUILDMODE,
    MOUNT,
    STATSMENU,
    HUNTVIEW,
    INVENTORY
}


public class LegendaryTutorial : MonoBehaviour
{
    LegendaryPlayerInputActions playerLanderInput;

    int turns = 0;
    int jumps = 0;
    int crouch = 0;
    int action1 = 0;
    int action2 = 0;
    int minimap = 0;
    int weaponring = 0;
    int sheath = 0;
    int buildmode = 0;
    int mount = 0;
    int statsmenu = 0;
    int huntview = 0;
    int inventory = 0;

    void OnEnable()
    {
        playerLanderInput.Lander.Enable();
    }

    void OnDisable()
    {
        playerLanderInput.Lander.Disable();
    }

    public void TrackAction()
    {

    }

    public void TrackActionSequence()
    {

    }

    public void ResetAllTracking()
    {
        turns = 0;
        jumps = 0;
        crouch = 0;
        action1 = 0;
        action2 = 0;
        minimap = 0;
        weaponring = 0;
        sheath = 0;
        buildmode = 0;
        mount = 0;
        statsmenu = 0;
        huntview = 0;
        inventory = 0;
    }

    private void Awake()
    {
        //playerLanderInput.Lander.Move.performed += onMovementInput;
        //playerLanderInput.Lander.Run.performed += onRun;
        playerLanderInput.Lander.Turn.performed += onTurn;
        playerLanderInput.Lander.Jump.performed += onJump;
        playerLanderInput.Lander.Crouch.started += onCrouch;
        playerLanderInput.Lander.ActionOne.performed += onActionOne;
        playerLanderInput.Lander.ActionTwo.performed += onActionTwo;
        playerLanderInput.Lander.ToggleMiniMap.performed += onMiniMap;
        playerLanderInput.Lander.ToggleWeaponRing.performed += onWeaponRing;
        //playerLanderInput.Lander.MenuItemLeft.performed += onMenuItemLeft;
        //playerLanderInput.Lander.MenuItemRight.performed += onMenuItemRight;
        playerLanderInput.Lander.Sheath.performed += onSheath;
        playerLanderInput.Lander.BuildMode.performed += onBuildMode;
        playerLanderInput.Lander.ToggleMount.performed += onMount;
        playerLanderInput.Lander.StatsMenu.performed += onStatsMenu;
        playerLanderInput.Lander.Look.performed += onLookView;
        playerLanderInput.Lander.Inventory.performed += onInventoryView;
        //playerLanderInput.Lander.JogDial.performed += onJogDial;
        //playerLanderInput.Lander.EscapeAll.performed += onEscapeAllMenus;
    }


    // InputAction.CallbackContext context

    void onMovementInput(InputAction.CallbackContext context) {
    
    }

    void onRun(InputAction.CallbackContext context) { 
    
    }

    void onTurn(InputAction.CallbackContext context) {
        turns += 1;
    }

    void onJump(InputAction.CallbackContext context) {
        jumps += 1;
    }

    void onCrouch(InputAction.CallbackContext context) {
        crouch += 1;
    }

    void onActionOne(InputAction.CallbackContext context) {
        action1 += 1;
    }

    void onActionTwo(InputAction.CallbackContext context) {
        action2 += 1;
    }

    void onMiniMap(InputAction.CallbackContext context) {
        minimap += 1;
    }

    void onWeaponRing(InputAction.CallbackContext context) {
        weaponring += 1;
    }

    void onMenuItemLeft(InputAction.CallbackContext context) { 
    
    }

    void onMenuItemRight(InputAction.CallbackContext context) { 
    
    }

    void onSheath(InputAction.CallbackContext context) {
        sheath += 1;
    }
    void onBuildMode(InputAction.CallbackContext context) {
        buildmode += 1;
    }

    void onMount(InputAction.CallbackContext context) {
        mount += 1;
    }

    void onStatsMenu(InputAction.CallbackContext context) {
        statsmenu += 1;
    }
    void onLookView(InputAction.CallbackContext context) {
        huntview += 1;
    }
    void onInventoryView(InputAction.CallbackContext context) {
        inventory += 1;
    }
    void onJogDial(InputAction.CallbackContext context) { 
    
    }
    void onEscapeAllMenus(InputAction.CallbackContext context) { 
    
    }

}
