using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class grappleScript : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public Transform tip;
    public Transform cameraObj;
    public Transform player;
    public LayerMask whatIsGrappleable;
    private float maxDistance = 100f;
    private SpringJoint joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            stopGrapple();
        }
    }

    void LateUpdate()
    {
        drawGrapple();
    }

    void startGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraObj.position, cameraObj.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromGrapple = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromGrapple * 0.8f;
            joint.minDistance = distanceFromGrapple * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void drawGrapple()
    {
        if (!joint) return;
        lr.SetPosition(0, tip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void stopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    public bool isGrappling()
    {
        return joint != null;
    }

    public Vector3 getPoint()
    {
        return grapplePoint;
    }
}
