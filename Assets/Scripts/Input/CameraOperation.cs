using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//[ExecuteAlways]
public class CameraOperation : MonoBehaviour
{
    public enum cameraMode
    {
        unset = 0,
        firstPerson = 1,
        thirdPerson = 2,
        //fixedThirdPerson = 3,
        //fixedFocusedThirdPerson = 4,
    };
    [Header("Camera Mode")]
    public bool followingOnStart = false;
    public cameraMode mode;

    [Header("Targeting")]
    public GameObject targetObject;
    //public bool followTargetRotation;
    public Vector3 targetPositionOffset;
    public bool followTargetRotation = true;

    [Header("Snapping")]
    public GameObject snapObject;
    [Range(0f, 1f)]
    public float snapWeight = 1.0f;
    public Vector3 snapPositionOffset;

    public Vector3 snapRotationOffset;
    public bool useAngleInstead = true;

    [Min(0f)]
    public float cameraDistance = 5.0f;
    public Vector2 cameraAngle;

    [Header("Misc")]
    public bool enableRayDetection = false;
    public LayerMask rayLayer;
    public bool enablePositionSlerp = false;
    [Range(0f, 1f)]
    public float positionSlerpWeight = 0.5f;
    public bool enableRotationSlerp = false;
    [Range(0f, 1f)]
    public float rotationSlerpWeight = 0.5f;

    [Header("Development Settings")]
    public float gizmosSize = 0.1f;

    bool isFollowing = false;
    
    /* 
	[Header("")]
	public bool isThirdPersonMode = true;
	public bool moveFollowTargetObject = true;
	public bool rotateFollowTargetObject = true;
	public Vector3 targetPositionOffset;
	public float cameraAngle = 0f;
	public float cameraDistance = 2f;
	public float cameraHeight = 1.3f;
	public Vector3 cameraPositionOffset;
	*/

    /*
	[Header("Camera Self Movement Setting")]
	public bool isCameraSelfRotate = false;
	public float cameraHorizontalRotateSpeed = 0.0f;
	public float cameraVerticalRotateSpeed = 0.0f;
	*/


    // Use this for initialization
    void Start()
    {
        if(followingOnStart)
            isFollowing = true;
        //this.GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetObject || !isFollowing)
            return;
        switch (mode)
        {
            /*
            case cameraMode.fixedFocusedThirdPerson:
                if (anchorObject != null)
                {
                    this.transform.position = anchorObject.transform.position + anchorPositionOffset;
                }
                this.transform.LookAt(targetObject.transform.position + targetPositionOffset);
                break;
            case cameraMode.fixedThirdPerson:
                if (anchorObject != null)
                {
                    this.transform.position = anchorObject.transform.position + anchorPositionOffset;
                }
                this.transform.rotation = targetObject.transform.rotation * Quaternion.Euler(anchorRotationOffset);
                break;
			*/
            case cameraMode.thirdPerson:
                Vector3 cameraPosOffset;
                Vector3 desiredPos = Vector3.zero;
                Vector3 basePos = Vector3.zero;
                Vector3 posOffset = Vector3.zero;

                //Vector2 calculatedAngle = followTargetRotation ? new Vector2(cameraAngle.x + targetObject.transform.rotation.eulerAngles.y, cameraAngle.y + targetObject.transform.rotation.eulerAngles.x) : new Vector2(cameraAngle.x, cameraAngle.y);
                Vector2 calculatedAngle = followTargetRotation ? new Vector2(cameraAngle.x + targetObject.transform.rotation.eulerAngles.y, cameraAngle.y) : new Vector2(cameraAngle.x, cameraAngle.y);

                /*cameraPosOffset.y = Mathf.Sin(cameraAngle.y * Mathf.Deg2Rad) * cameraDistance;
                float projectDistance = Mathf.Sqrt(cameraDistance * cameraDistance - cameraPosOffset.y * cameraPosOffset.y);
                cameraPosOffset.z = -projectDistance * Mathf.Cos(cameraAngle.x * Mathf.Deg2Rad);
                cameraPosOffset.x = -projectDistance * Mathf.Sin(cameraAngle.x * Mathf.Deg2Rad);*/

                cameraPosOffset.y = Mathf.Sin(calculatedAngle.y * Mathf.Deg2Rad) * cameraDistance;
                float projectDistance = Mathf.Sqrt(cameraDistance * cameraDistance - cameraPosOffset.y * cameraPosOffset.y);
                cameraPosOffset.z = -projectDistance * Mathf.Cos(calculatedAngle.x * Mathf.Deg2Rad);
                cameraPosOffset.x = -projectDistance * Mathf.Sin(calculatedAngle.x * Mathf.Deg2Rad);


                if (snapObject == null)
                {
                    basePos = targetObject.transform.position + targetPositionOffset;
                    posOffset = snapPositionOffset + cameraPosOffset;
                }
                else
                {
                    basePos = snapObject.transform.position * snapWeight + (targetObject.transform.position + targetPositionOffset) * (1f - snapWeight);
                    posOffset = snapPositionOffset * snapWeight + cameraPosOffset * (1f - snapWeight);
                }

                RaycastHit hitInfo;
                Vector3 rayDir = posOffset;
                if(Physics.Raycast(basePos, rayDir.normalized, out hitInfo, rayDir.magnitude, rayLayer) && enableRayDetection)
                {
                    desiredPos = hitInfo.point;
                }
                else
                {
                    desiredPos = basePos + posOffset;
                }
                


                if (enablePositionSlerp)
                {
                    this.transform.position = Vector3.Slerp(this.transform.position, desiredPos, positionSlerpWeight);
                }
                else
                {
                    this.transform.position = desiredPos;
                }
                //this.transform.position = desiredPos + snapPositionOffset + cameraPosOffset;

                /*if (targetObject != null)
                {
                    this.transform.LookAt(targetObject.transform.position + targetPositionOffset);
                }
                else
                {
                    this.transform.rotation = targetObject.transform.rotation; //* Quaternion.Euler(snapRotationOffset);
                }*/

                /*if (followTargetRotation)
                {
                    this.transform.rotation = targetObject.transform.rotation * Quaternion.Euler(cameraAngle.y,0,0);
                }
                else
                {*/
                    if(enableRotationSlerp)
                        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,Quaternion.LookRotation(targetObject.transform.position + targetPositionOffset -this.transform.position),rotationSlerpWeight);
                    else
                        this.transform.LookAt(targetObject.transform.position + targetPositionOffset);
                //}

                break;
            case cameraMode.firstPerson:
                if (snapObject != null)
                {
                    this.transform.position = snapObject.transform.position + snapPositionOffset;
                    if (!useAngleInstead)
                    {
                        this.transform.LookAt(snapObject.transform.TransformPoint(Vector3.forward + Vector3.up * Mathf.Sin(cameraAngle.y * Mathf.Deg2Rad)) + targetPositionOffset);
                    }
                    else
                    {
                        this.transform.rotation = snapObject.transform.rotation * Quaternion.Euler(snapRotationOffset);
                    }
                }
                break;
            default:
                break;
        }
        /*
		if (targetObject==null) {
			return;
		}
		
		if(isThirdPersonMode)
		{
			if(rotateFollowTargetObject)
			{
				this.transform.LookAt (targetObject.transform.position + targetPositionOffset + cameraPositionOffset);
			}
			if(moveFollowTargetObject)
			{
				this.transform.position = targetObject.transform.position - Vector3.forward * cameraDistance * Mathf.Cos(cameraAngle * Mathf.Deg2Rad) - Vector3.right * cameraDistance * Mathf.Sin(cameraAngle * Mathf.Deg2Rad) + Vector3.up * cameraHeight + cameraPositionOffset;
			}
		}
		else
		{
			if(moveFollowTargetObject)
			{
				this.transform.position = targetObject.transform.position + cameraPositionOffset;
			}
			if(rotateFollowTargetObject)
			{
				this.transform.LookAt (targetObject.transform.position + Vector3.forward + targetPositionOffset + cameraPositionOffset);
			}
		}*/
    }

    public void TrackingObject(GameObject other, bool startTracking = true)
    {
        if(other)
        {
            targetObject = other;
            isFollowing = startTracking;
        }
    }

    public void StopTracking()
    {
        targetObject = null;
        isFollowing = false;
    }
    private void OnDrawGizmos()
    {
        if (mode == cameraMode.firstPerson)
        {
            Gizmos.DrawRay(this.transform.position, this.transform.forward * 10f);
        }
        else if (mode == cameraMode.thirdPerson && targetObject)
        {
            Gizmos.DrawWireSphere(targetObject.transform.position, gizmosSize);
            Gizmos.DrawLine(this.transform.position, targetObject.transform.position);
            Gizmos.DrawWireSphere(targetObject.transform.position + targetPositionOffset, gizmosSize);
            Gizmos.DrawLine(this.transform.position, targetObject.transform.position + targetPositionOffset);
            Gizmos.DrawLine(targetObject.transform.position, targetObject.transform.position + targetPositionOffset);
        }
    }
}
