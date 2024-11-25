using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls { get; private set; }
    public PlayerAim playerAim { get; private set; } 
    public PlayerController playerController { get; private set; }
    public PlayerWeaponController playerWeaponController { get; private set; }
    public PlayerWeaponVisuals playerWeaponVisuals { get; private set; }

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerAim = GetComponent<PlayerAim>();
        playerController = GetComponent<PlayerController>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        playerWeaponVisuals = GetComponent<PlayerWeaponVisuals>();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
