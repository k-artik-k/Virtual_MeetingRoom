using UnityEngine;

public class ToggleGrab : MonoBehaviour
{
    public Transform handAnchor;
    private bool grabbed = false;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            if (!grabbed)
            {
                Grab();
            }
            else
            {
                Drop();
            }
        }
    }

    void Grab()
    {
        grabbed = true;

        rb.isKinematic = true;
        rb.useGravity = false;

        transform.SetParent(handAnchor);
        transform.localPosition = new Vector3(0f, 0.02f, 0.1f);
        transform.localRotation = Quaternion.Euler(0, 0, 90);
    }

    void Drop()
    {
        grabbed = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
    }
}