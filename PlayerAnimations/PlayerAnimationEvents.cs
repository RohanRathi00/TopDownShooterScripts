using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals playerVisualController;
    private PlayerWeaponController playerWeaponController;

    private void Start()
    {
        playerVisualController = GetComponentInParent<PlayerWeaponVisuals>();
        playerWeaponController = GetComponentInParent<PlayerWeaponController>();
    }

    public void ReloadIsOver()
    {
        playerVisualController.MaximizeRigWeight();
        playerWeaponController.CurrentWeapon().RefillBullets();

        playerWeaponController.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        playerVisualController.MaximizeRigWeight();
        playerVisualController.MaximizeLeftHandRigWeight();
    }

    public void WeaponEquipingIsOver()
    {
        playerWeaponController.SetWeaponReady(true);
    }

    public void SwitchOnWeaponModel() => playerVisualController.SwitchOnCurrentWeaponModel();
}
