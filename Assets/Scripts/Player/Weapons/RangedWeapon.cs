using System;
using System.Collections;
using UnityEngine;

// CREATED BY: Sean
// abstract class to act as a parent to all ranged weapons.
public abstract class RangedWeapon : MonoBehaviour
{
    [SerializeField] protected float range;
    [SerializeField] protected int damage;
    [SerializeField] protected float weakpointMultiplier;

    [SerializeField] protected int magazineSize;
    [SerializeField] protected int startingAmmo;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float fireRate;
    protected int ammoInMagazine;
    protected int ammoInReserve;

    public event EventHandler OnRangedWeaponShot;
    public event EventHandler OnWeaponReload;

    protected bool reloading = false;
    private bool canShoot = true;

    protected virtual void Awake() {
        ammoInReserve = startingAmmo - magazineSize;
        ammoInMagazine = magazineSize;
    }

    public virtual bool Shoot(Vector3 startPos, Vector3 direction) {
        if (ammoInMagazine == 0 || reloading || !canShoot) {
            return false;
        }

        StartCoroutine(FireRateLimiter());
        OnRangedWeaponShot?.Invoke(this, EventArgs.Empty);

        ammoInMagazine -= 1;
        WeaponManager.Instance.CallOnAmmoChanged();
        if (ammoInMagazine == 0 && ammoInReserve > 0) {
            Reload();
        }
        return true;
    }

    private IEnumerator FireRateLimiter() {
        canShoot = false;
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }

    public virtual void Reload() {
        if (ammoInMagazine == magazineSize || reloading || ammoInReserve == 0) {
            return;
        }

        reloading = true;
        OnWeaponReload?.Invoke(this, EventArgs.Empty);
        StartCoroutine(ReloadTimer());
    }

    private IEnumerator ReloadTimer() {
        yield return new WaitForSeconds(reloadTime);

        ammoInReserve += ammoInMagazine;
        if (ammoInReserve >= magazineSize) {
            ammoInReserve -= magazineSize;
            ammoInMagazine = magazineSize;
        } else {
            ammoInMagazine = ammoInReserve;
            ammoInReserve = 0;
        }

            WeaponManager.Instance.CallOnAmmoChanged();
        reloading = false;
    }

    public virtual int GetAmmoInMagazine(){ return ammoInMagazine; }
    public virtual int GetAmmoInReserve(){ return ammoInReserve; }
    public virtual float GetReloadTime(){ return reloadTime; }
}
