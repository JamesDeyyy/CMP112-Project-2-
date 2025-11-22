using UnityEngine;

public class rotateGrapple : MonoBehaviour
{
    public grappleScript grappling;

    private Quaternion rotation;
    private float rotationSpeed = 5f;

    void Update()
    {
        if (grappling.isGrappling())
        {
            rotation = transform.parent.rotation;
        }
        else
        {
            rotation = Quaternion.LookRotation(grappling.getPoint() - transform.position);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
