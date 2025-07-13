using UnityEngine;

// CREATED BY: Sean
// Recieves shooting inputs and calls required functions of the equipped weapon.

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private RangedWeapon equippedRangedWeapon;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask rangedWeaponLayerMask;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask enemyWeakpointLayer;

    [Header("Temporary")]
    [SerializeField] private Transform bulletStartPoint;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        InputManager.Instance.OnRangedAttackButtonPressed += WeaponManager_OnRangedAttackButtonPressed;
    }

    // need a way to get a direction from the input class (or use that fire from centre of screen thing?)
    private void WeaponManager_OnRangedAttackButtonPressed(object sender, System.EventArgs e) {
        equippedRangedWeapon.Shoot(bulletStartPoint.position, Vector3.right);
    }

    public LayerMask GetRangedWeaponLayerMask() { return rangedWeaponLayerMask; }
    public LayerMask GetObstacleLayer() { return obstacleLayer; }
    public LayerMask GetEnemyLayer() { return enemyLayer; }
    public LayerMask GetEnemyWeakpointLayer () { return enemyWeakpointLayer; }
}
