using System;
using UnityEngine;

// CREATED BY: Sean
// Recieves shooting inputs and calls required functions of the equipped weapon.

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Equipped Weapons")]
    [SerializeField] private RangedWeapon equippedRangedWeapon;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask rangedWeaponLayerMask;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask enemyWeakpointLayer;

    [Header("References")]
    [SerializeField] private Transform bulletSpawnPointTransform;

    public event EventHandler OnAmmoChanged;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        InputManager.Instance.OnRangedAttackButtonPressed += WeaponManager_OnRangedAttackButtonPressed;
        InputManager.Instance.OnReloadButtonPressed += WeaponManager_OnReloadButtonPressed;
    }

    private void WeaponManager_OnReloadButtonPressed(object sender, System.EventArgs e) {
        equippedRangedWeapon.Reload();
    }

    // likely switch out for ViewportPointToRay() later (for hitscan)? OR just get look direction
    private void WeaponManager_OnRangedAttackButtonPressed(object sender, System.EventArgs e) {
        equippedRangedWeapon.Shoot(Vector3.right);
    }

    public RangedWeapon GetEquippedRangedWeapon(){ return equippedRangedWeapon;}

    public LayerMask GetRangedWeaponLayerMask() { return rangedWeaponLayerMask; }

    // layers are stored in binary, its weird. Have to bitshift
    public bool HasHitObstacle(int layerHit) {
        return 1 == obstacleLayer.value >> layerHit;
    }

    public bool HasHitEnemyWeakpoint(int layerHit) {
        return 1 == enemyWeakpointLayer.value >> layerHit;
    }

    public Vector3 GetBulletSpawnPoint() { return bulletSpawnPointTransform.position; }

    public void CallOnAmmoChanged() {
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }
}
