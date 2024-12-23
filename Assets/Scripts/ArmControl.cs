using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKTargetControl : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the FPS camera
    public Transform rightHandIKTarget;  // The IK target for the right hand (or arm)
    
    public Rig rig;  // Reference to the Rig component
    public float distanceInFrontOfCamera = 2f;  // Distance in front of the camera
    public float heightOffset = 1f;  // Height offset to position the hand at a comfortable height
    public float rightOffset = 1f;

    private bool isRightMouseHeld;

    void Update()
    {
        // Detect right mouse button press
        isRightMouseHeld = Input.GetMouseButton(1);

        // When right mouse button is held, enable IK targeting
        if (isRightMouseHeld && rightHandIKTarget && cameraTransform)
        {
            // Enable the rig
            if (rig != null)
            {
                rig.weight = 1f;  // Ensure the IK system is active when right mouse button is pressed
            }

            // Calculate the position in front of the camera
            Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceInFrontOfCamera;
            targetPosition.y += heightOffset;  // Adjust the height of the hand
            targetPosition += cameraTransform.right * rightOffset;
            // Move the IK target to the calculated position
            rightHandIKTarget.position = targetPosition;

            // Optionally, rotate the IK target to align with the camera's forward direction
            rightHandIKTarget.rotation = Quaternion.LookRotation(cameraTransform.forward);
        }
        else
        {
            // Disable the rig when right mouse button is not held
            if (rig != null)
            {
                rig.weight = 0f;  // Set the rig weight to 0 to disable IK
            }
        }
    }
}
