using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Test()
    {
        Debug.Log("Moving forward");
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
