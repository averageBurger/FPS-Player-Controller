using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void Start() {
        playerMovement.OnSlideStart += PlayerMovement_OnSlideStart;
        playerMovement.OnSlideEnd += PlayerMovement_OnSlideEnd;
    }

    private void PlayerMovement_OnSlideStart(object sender, System.EventArgs e) {
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, playerMovement.GetSlideStartDirection());

        transform.localRotation = Quaternion.AngleAxis(-75f, rotationAxis) * transform.localRotation;
        transform.localPosition = new Vector3(0f, -0.3f, 0f);
    }

    private void PlayerMovement_OnSlideEnd(object sender, System.EventArgs e) {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = Vector3.zero;
    }
}
