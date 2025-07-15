using System.Collections;
using UnityEngine;

public class MeleeWeaponVisuals : MonoBehaviour
{
    [SerializeField] private MeleeWeapon meleeWeapon;

    private void Start() {
        meleeWeapon.OnMeleeWeaponAttack += MeleeWeapon_OnMeleeWeaponAttack;
    }

    private void MeleeWeapon_OnMeleeWeaponAttack(object sender, System.EventArgs e) {
        StartCoroutine(TEMP_MeleeAttackAnimation());
    }

    private IEnumerator TEMP_MeleeAttackAnimation() {
        transform.Rotate(Vector3.right * 70, Space.Self);
        yield return new WaitForSeconds(0.3f);
        transform.Rotate(Vector3.left * 70, Space.Self);
    }
}
