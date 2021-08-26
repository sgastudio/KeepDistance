using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;

public class PlayerInput : MonoBehaviour
{
    [FoldoutGroup("Mouse")]
    public CursorLockMode cursorLock = CursorLockMode.Confined;
    [FoldoutGroup("Mouse")]
    public bool cursorVisible = true;
    [FoldoutGroup("Mouse")]
    public float aspectRate = 16 / 9;
    [FoldoutGroup("Axises & Buttons")]
    public string positionXAxisName = "Horizontal";
    [FoldoutGroup("Axises & Buttons")]
    public string positionYAxisName = "Vertical";
    [FoldoutGroup("Axises & Buttons"), Space]
    public string rotationXAxisName = "Mouse X";
    [FoldoutGroup("Axises & Buttons")]
    public string rotationYAxisName = "Mouse Y";
    [FoldoutGroup("Axises & Buttons"), Space]
    public Vector2 positionAxisWeight = Vector2.one;
    [FoldoutGroup("Axises & Buttons")]
    public Vector2 rotationAxisWeight = new Vector2(1, 0);
    [FoldoutGroup("Axises & Buttons"), Space]
    public string jumpInputName = "Jump";
    [FoldoutGroup("Axises & Buttons")]
    public string fire1InputName = "Fire1";
    [FoldoutGroup("Axises & Buttons")]
    public string fire2InputName = "Fire2";
    [FoldoutGroup("Axises & Buttons")]
    public string crouchInputName = "Fire3";

    [FoldoutGroup("Movement")]
    [TitleGroup("Movement/Locomotion")]
    public float positionVelocity = 2.5f;
    [TitleGroup("Movement/Locomotion")]
    [Min(0f)]
    public float maxMoveAcceleration = 10.0f;
    [TitleGroup("Movement/Locomotion")]
    [Min(0f)]
    public float maxAirAcceleration = 1.0f;
    [TitleGroup("Movement/Locomotion")]
    public float rotationVelocity = 10.0f;
    [TitleGroup("Movement/Locomotion")]
    public float maxGroundAngle = 30f;
    [TitleGroup("Movement/Locomotion")]
    public float maxStairAngle = 50f;
    public bool onGround => groundContactCount > 0;
    int groundContactCount;
    Vector3 connectionVelocity;
    Vector3 connectionWorldPosition;
    Vector3 connectionLocalPosition;

    [TitleGroup("Movement/Crouch")]
    public float crouchVelocity = 1f;

    [TitleGroup("Movement/Jump")]
    [Min(0f)]
    public float jumpHeight = 2f;
    [TitleGroup("Movement/Jump")]
    [Min(0)]
    public int maxAirJumpTimes = 0;
    [TitleGroup("Movement/Jump")]
    public bool jumpAlignSurfaceNormal = true;
    public bool isJumping => jumpPhase > 0;
    bool desiredJump;
    int jumpPhase;
    Vector3 contactNormal;

    [TitleGroup("Movement/Steep")]
    public bool onSteep => steepContactCount > 0;
    int steepContactCount;
    Vector3 steepNormal;

    [TitleGroup("Movement/Snap")]
    public float maxSnapVelocity = 100f;
    [TitleGroup("Movement/Snap")]
    public float probeDistance = 1f;
    [TitleGroup("Movement/Snap")]
    public LayerMask probeMask = -1;
    [TitleGroup("Movement/Snap")]
    public LayerMask stairsMask = -1;
    float minGroundDotProduct;
    float minStairsDotProduct;


    [TitleGroup("Movement/Status"), DisplayAsString, SerializeField]
    Vector3 currentVelocity;
    [TitleGroup("Movement/Status"), DisplayAsString, SerializeField]
    Vector3 desiredMoveVelocity;
    [TitleGroup("Movement/Status"), DisplayAsString, SerializeField]
    Vector3 caculatedVelocity;
    [Space]
    [TitleGroup("Movement/Status"), DisplayAsString]
    public bool isCrouching = false;

    [FoldoutGroup("Adjustment")]
    public LineDetector AdjustObjects;
    [FoldoutGroup("Adjustment"), Range(0, 1)]
    public float AdjustWeight = 0.8f;
    [FoldoutGroup("Adjustment"), DisplayAsString]
    public float extraValue = 1f;
    [FoldoutGroup("Adjustment"), DisplayAsString]
    public float AdjustValue;
    [FoldoutGroup("Adjustment"), DisplayAsString]
    public Vector3 adjustedVelocity;

    [FoldoutGroup("Input Status"), DisplayAsString]
    public Vector2 positionAxis;
    [FoldoutGroup("Input Status"), DisplayAsString]
    public Vector2 rotationAxis;
    [FoldoutGroup("Input Status"), DisplayAsString]
    public bool jumpButton;
    [FoldoutGroup("Input Status"), DisplayAsString]
    public bool fire1Button;
    [FoldoutGroup("Input Status"), DisplayAsString]
    public bool fire2Button;
    [FoldoutGroup("Input Status"), DisplayAsString]
    public bool crouchButton;


    #region private variables
    Vector3 previousDirection;
    Rigidbody rigidBody;
    Rigidbody connectedBody;
    Rigidbody previousConnectedBody;
    PhotonView photonView;
    int stepsSinceLastGrounded;
    int stepsSinceLastJump;
    #endregion

    void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        UpdateCursorState();

        photonView = this.GetComponent<PhotonView>();
        rigidBody = this.GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        //if (checkFalseConditions() || rigidBody.IsSleeping())
        if (rigidBody.IsSleeping())
            return;

        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;

        UpdateVelocityFromRigidBody();

        UpdateState();

        CalculateVelocity();

        CalculateJump();

        UpdateRigidBody();

        ResetState();
    }

    void Update()
    {
        if (checkFalseConditions())
        {
            resetInputVectors();
            calculateDesiredVelocity();
            return;
        }

        updateInputVectors();

        calculateDesiredVelocity();

        //translatePlayer();

        rotatePlayer();

        UpdateCursorState();
    }

    #region UpdateFunc
    public void UpdateCursorState()
    {
        Cursor.lockState = cursorLock;
        Cursor.visible = cursorVisible;
    }

    void updateInputVectors()
    {
        //get axis values
        positionAxis = new Vector2(Input.GetAxis(positionXAxisName), Input.GetAxis(positionYAxisName));
        rotationAxis = new Vector2(Input.GetAxis(rotationXAxisName), Input.GetAxis(rotationYAxisName));
        //get button values
        jumpButton = Input.GetButton(jumpInputName);
        fire1Button = Input.GetButton(fire1InputName);
        fire2Button = Input.GetButton(fire2InputName);
        crouchButton = Input.GetButton(crouchInputName);

        desiredJump |= Input.GetButtonDown(jumpInputName);
        isCrouching = crouchButton;
    }

    void calculateDesiredVelocity()
    {
        Vector3 desiredVelocity = new Vector3(positionAxis.x * positionAxisWeight.x, 0, positionAxis.y * positionAxisWeight.y);


        if (AdjustObjects)
        {
            Vector3 direction = (this.transform.rotation * desiredVelocity).normalized;
            if (direction.magnitude == 0)
                direction = previousDirection;
            else
                previousDirection = direction;
            AdjustValue = 1.0f;
            foreach (GameObject o in AdjustObjects.activeList)
            {
                float projection = Vector3.Dot(o.transform.position - this.transform.position, direction);
                float percentage = (projection > 0 ? projection / AdjustObjects.distance : 0) * AdjustWeight;
                AdjustValue *= (1f - percentage);
            }
        }
        adjustedVelocity = desiredVelocity * AdjustValue * extraValue;

        // if (isCrouching)
        //     caculatedVelocity = this.transform.rotation * adjustedVelocity * crouchVelocity;
        // else
        //     caculatedVelocity = this.transform.rotation * adjustedVelocity * positionVelocity;
        if (isCrouching)
            desiredMoveVelocity = adjustedVelocity * crouchVelocity;
        else
            desiredMoveVelocity = adjustedVelocity * positionVelocity;

    }

    void resetInputVectors()
    {
        jumpButton = fire1Button = fire2Button = crouchButton = false;
        positionAxis = Vector2.zero;
        rotationAxis = Vector2.zero;
    }
    bool checkFalseConditions()
    {
        return (photonView.IsMine == false && PhotonNetwork.IsConnected == true) || (DialogueManager.isConversationActive);
    }
    void CaculateState()
    {
        isCrouching = crouchButton;
    }

    void translatePlayer()
    {
        this.transform.Translate(currentVelocity * Time.deltaTime, Space.World);
    }

    void rotatePlayer()
    {
        Vector3 desiredAngularVelocity = new Vector3(rotationAxis.y * rotationAxisWeight.y, rotationAxis.x * rotationAxisWeight.x / aspectRate, 0);
        Vector3 caculatedAngularVelocity = desiredAngularVelocity * rotationVelocity * Mathf.Rad2Deg;
        this.transform.rotation *= Quaternion.Euler(caculatedAngularVelocity * Time.deltaTime);
    }
    #endregion

    #region FixedUpdateFunc
    void UpdateVelocityFromRigidBody()
    {
        if (rigidBody)
        {
            currentVelocity = rigidBody.velocity;
        }
    }

    void UpdateState()
    {
        if (onGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1)
                jumpPhase = 0;
            if (groundContactCount > 1)
                contactNormal.Normalize();
        }
        else
        {
            contactNormal = Vector3.up;
        }

        if (connectedBody)
        {
            if (connectedBody.isKinematic || connectedBody.mass >= rigidBody.mass)
                UpdateConnectionState();
        }
    }

    void CalculateVelocity()
    {
        //Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        //Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
        Vector3 xAxis = ProjectOnContactPlane(transform.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(transform.forward).normalized;

        //Vector3 relativeVelocity = currentVelocity - (Vector3)(transform.worldToLocalMatrix * connectionVelocity);
        Vector3 relativeVelocity = currentVelocity - connectionVelocity;

        float currentX = Vector3.Dot(relativeVelocity, xAxis);
        float currentZ = Vector3.Dot(relativeVelocity, zAxis);

        float acceleration = onGround ? maxMoveAcceleration : maxAirAcceleration;
        float maxMoveVelocityChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredMoveVelocity.x, maxMoveVelocityChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredMoveVelocity.z, maxMoveVelocityChange);
        //float newZ = Mathf.MoveTowards(currentZ, desiredMoveVelocity.z, maxMoveVelocityChange);

        currentVelocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    void CalculateJump()
    {
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }
    }

    void UpdateRigidBody()
    {
        if (rigidBody != null)
        {
            //rigidBody.velocity = this.transform.localToWorldMatrix * currentVelocity;
            rigidBody.velocity = currentVelocity;
        }
    }

    void ResetState()
    {
        //onGround = false;
        groundContactCount = 0;
        contactNormal = Vector3.zero;

        steepContactCount = 0;
        steepNormal = Vector3.zero;

        connectionVelocity = Vector3.zero;
        previousConnectedBody = connectedBody;
        connectedBody = null;
    }

    void Jump()
    {
        Vector3 jumpDirection;

        if (onGround)
        {
            jumpDirection = contactNormal;
        }
        else if (onSteep)
        {
            jumpDirection = steepNormal + Vector3.up;
            jumpDirection.Normalize();
            jumpPhase = 0;
        }
        else if (maxAirJumpTimes > 0 && jumpPhase <= maxAirJumpTimes)
        {
            if (jumpPhase == 0)
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
        }
        else
        {
            return;
        }

        //if (onGround || jumpPhase < maxAirJumpTimes)
        //{
        stepsSinceLastJump = 0;
        jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        //float alignedSpeed = Vector3.Dot(currentVelocity, this.transform.worldToLocalMatrix * jumpDirection);
        float alignedSpeed = Vector3.Dot(currentVelocity, jumpDirection);
        //if (currentVelocity.y > 0f)
        if (alignedSpeed > 0f)
        {
            //jumpSpeed = Mathf.Max(jumpSpeed - currentVelocity.y, 0f);  
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        if (jumpAlignSurfaceNormal || onSteep)
            //currentVelocity += (Vector3)(this.transform.worldToLocalMatrix * jumpDirection) * jumpSpeed;
            currentVelocity += jumpDirection * jumpSpeed;
        else
            currentVelocity += transform.up * jumpSpeed;

        //animation 
        // PlayerAnimatorUpdater updater = this.GetComponent<PlayerAnimatorUpdater>();
        // if (updater != null)
        // {
        //     updater.TriggerJumpTransition();
        // }
        //}
    }

    void UpdateConnectionState()
    {


        if (connectedBody == previousConnectedBody)
        {
            Vector3 connectionMovement = connectedBody.transform.TransformPoint(connectionLocalPosition) - connectionWorldPosition;
            // Vector3 connectionMovement = connectedBody.position - connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }

        connectionWorldPosition = rigidBody.position;
        //connectionLocalPosition = connectedBody.transform.InverseTransformPoint(this.transform.position);
        connectionLocalPosition = connectedBody.transform.InverseTransformPoint(connectionWorldPosition);
        
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
        {
            return false;
        }
        float speed = currentVelocity.magnitude;
        if (speed > maxSnapVelocity)
        {
            return false;
        }
        if (!Physics.Raycast(rigidBody.position, Vector3.down, out RaycastHit hit, probeDistance, probeMask))
        {
            return false;
        }
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        groundContactCount = 1;
        contactNormal = hit.normal;


        float dot = Vector3.Dot(currentVelocity, hit.normal);
        if (dot > 0f)
            currentVelocity = (currentVelocity - hit.normal * dot).normalized * speed;

        connectedBody = hit.rigidbody;
        return true;
    }

    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }
    #endregion

    #region CollisionFunc
    private void OnCollisionEnter(Collision other)
    {
        EvaluateCollision(other);
    }

    private void OnCollisionStay(Collision other)
    {
        EvaluateCollision(other);
    }

    void EvaluateCollision(Collision other)
    {
        float minDot = GetMinDot(other.gameObject.layer);
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.GetContact(i).normal;
            if (normal.y >= minDot)
            {
                //onGround = true;
                groundContactCount += 1;
                contactNormal += normal;
                connectedBody = other.rigidbody;
            }
            else if (normal.y > -0.01f)
            {
                steepContactCount += 1;
                steepNormal += normal;

                if (groundContactCount == 0)
                {
                    connectedBody = other.rigidbody;
                }
            }
        }

        /*foreach (ContactPoint i in other.contacts)
        {
            Vector3 normal = i.normal;
            onGround |= normal.y >= 0.9f;
        }*/
    }
    #endregion

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairAngle * Mathf.Deg2Rad);
    }

    Vector3 ProjectOnContactPlane(Vector3 vec3)
    {
        return vec3 - contactNormal * Vector3.Dot(vec3, contactNormal);
    }

    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairsDotProduct;
    }
}
