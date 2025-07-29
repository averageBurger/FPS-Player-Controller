using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerLegs;
    
    private void Start() {
        playerMovement.OnHitboxDown += PlayerMovement_OnHitboxDown;
        playerMovement.OnHitboxUp += PlayerMovement_OnHitboxUp;
    }

    private void PlayerMovement_OnHitboxDown(object sender, System.EventArgs e) {
        Vector3 rotationAxis = Vector3.Cross(playerMovement.GetSlideStartDirection(), Vector3.up);
        Vector3 rotationDir = Quaternion.AngleAxis(75, rotationAxis) * playerMovement.GetSlideStartDirection();
        transform.rotation = Quaternion.LookRotation(rotationDir, Vector3.up);

        playerLegs.SetActive(true);
    }

    private void PlayerMovement_OnHitboxUp(object sender, System.EventArgs e) {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = Vector3.zero;
        playerLegs.SetActive(false);
    }
}