using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private InputManager inputManager;
    private const float REFERENCE_BULLET_SPEED = 20;

    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

    [Header("Bullet info")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlots;


    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        HandleInputs();

        EquipWeapon(0);
    }

    private void Update()
    {
        if (isShooting == true)
        {
            Shoot();
        }
    }

    #region Slots management - Pickup/Equip/Drop/Ready - Weapon

    private void EquipWeapon(int i)
    {
        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        inputManager.playerWeaponVisuals.PlayWeaponEquipAnimation();
    }

    public void PickupGun(Weapon newWeapon)
    {
        int maxSlotsAllowed = 2;

        if (weaponSlots.Count >= maxSlotsAllowed)
        {
            return;
        }

        weaponSlots.Add(newWeapon);
        inputManager.playerWeaponVisuals.SwitchOnBackupWeaponModels();
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
        {
            return;
        }

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    private void Shoot()
    {
        if (WeaponReady() == false)
        {
            return;
        }

        if (currentWeapon.CanShoot() == false)
        {
            return;
        }

        if (currentWeapon.shootType == ShootType.Single)
        {
            isShooting = false;
        }

        GameObject newBullet = ObjectPool.instance.GetBullet();

        newBullet.transform.position = BulletSpawnPoint().position;
        newBullet.transform.rotation = bulletPrefab.transform.rotation;

        Rigidbody newBulletRB = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        newBulletRB.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        newBulletRB.velocity = bulletDirection * bulletSpeed;

        inputManager.playerWeaponVisuals.PlayFireAnimation();
    }

    private void Reload()
    {
        if (currentWeapon.CanReload())
        {
            SetWeaponReady(false);
            inputManager.playerWeaponVisuals.PlayReloadAnimation();
        }
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;

    public bool WeaponReady() => weaponReady;

    public Weapon CurrentWeapon() => currentWeapon; 

    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
            {
                return weapon;
            }
        }

        return null;
    }

    public Vector3 BulletDirection()
    {
        Transform aim = inputManager.playerAim.Aim();

        Vector3 direction = (aim.position - BulletSpawnPoint().position).normalized;

        if(inputManager.playerAim.CanAimPrecisly() == false && inputManager.playerAim.Target() == null)
            direction.y = 0;

        return direction;
    }

    public Transform BulletSpawnPoint() => inputManager.playerWeaponVisuals.CurrentWeaponModel().bulletSpawnPoint;

    #endregion

    #region Input Events

    private void HandleInputs()
    {
        PlayerControls controls = inputManager.playerControls;

        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipWeaponSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipWeaponSlot2.performed += context => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }

    #endregion
}
