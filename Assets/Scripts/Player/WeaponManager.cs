using System;
using UnityEngine;

// CREATED BY: Sean
// Recieves shooting inputs and calls required functions of the equipped weapon.

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Equipped Weapons")]
    [SerializeField] private RangedWeapon equippedRangedWeapon;
    [SerializeField] private MeleeWeapon equippedMeleeWeapon;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask weaponInteractableLayerMask;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;

    [Header("References")]
    [SerializeField] private Transform bulletSpawnPointTransform;
    [SerializeField] private Transform meleeAttackShapeSpawnPointTransform;

    public event EventHandler OnAmmoChanged;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        InputManager.Instance.OnRangedAttackButtonPressed += InputManager_OnRangedAttackButtonPressed;
        InputManager.Instance.OnReloadButtonPressed += InputManager_OnReloadButtonPressed;
        InputManager.Instance.OnMeleeAttackButtonPressed += InputManager_OnMeleeAttackButtonPressed;
    }

    private void InputManager_OnMeleeAttackButtonPressed(object sender, EventArgs e) {
        equippedMeleeWeapon.Attack(Vector3.forward);
    }

    private void InputManager_OnReloadButtonPressed(object sender, System.EventArgs e) {
        equippedRangedWeapon.Reload();
    }

    // likely switch out for ViewportPointToRay() later (for hitscan)? OR just get look direction
    private void InputManager_OnRangedAttackButtonPressed(object sender, System.EventArgs e) {
        equippedRangedWeapon.Shoot(Vector3.forward);
    }

    public RangedWeapon GetEquippedRangedWeapon(){ return equippedRangedWeapon;}

    public LayerMask GetWeaponInteractableLayerMask() { return weaponInteractableLayerMask; }

    // layers are stored in binary, its weird. Have to bitshift
    public bool HasHitObstacle(int layerHit) {
        return 1 == obstacleLayer.value >> layerHit;
    }

    public bool HasHitEnemy(int layerHit) {
        return 1 == enemyLayer.value >> layerHit;
    }

    public Vector3 GetBulletSpawnPoint() { return bulletSpawnPointTransform.position; }

    public Vector3 GetMeleeAttackShapeSpawnPoint() { return meleeAttackShapeSpawnPointTransform.position; }

    public void CallOnAmmoChanged() {
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UnequipRangedWeapon() {
        equippedRangedWeapon = null;
    }

    public void EquipRangedWeapon(RangedWeapon newWeapon) {
        if (equippedRangedWeapon == null) {
            equippedRangedWeapon = newWeapon;
        } else {
            Debug.Log("Tried to equip a ranged weapon when there is already one equipped!");
        }
    }

    public void ChangeMeleeWeapon(MeleeWeapon newWeapon) {
        equippedMeleeWeapon = newWeapon;
    }
}
