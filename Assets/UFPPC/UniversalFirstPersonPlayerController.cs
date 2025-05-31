using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UFPPC
{
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    [HelpURL("https://www.jakebrotherton.com/Contact")]
    public class UniversalFirstPersonPlayerController : MonoBehaviour
    {
        // Public/Serialized Variables
        #region
        // Main Attributes
        [SerializeField][Range(0f, 20f)][Tooltip("Default player speed.")] private float walkSpeed = 6f;
        [SerializeField][Range(0f, 15f)][Tooltip("Default player jump height.")] private float jumpHeight = 2f;
        [SerializeField][Range(0f, 30f)][Tooltip("Default run speed.")] private float runSpeed = 10f;
        [SerializeField][Range(0f, 10f)][Tooltip("Default slowed walk speed.")] private float slowWalkSpeed = 3f;
        [SerializeField][Tooltip("default strength of gravity.")] private float gravity = 18.36f;
        [SerializeField][Tooltip("Enables jumping for the player.")] private bool enableJumping = true;
        [Tooltip("Enables running for the player.")] public bool enableRunning = true;
        [SerializeField][Tooltip("Enables double jump for the player.")] private bool doubleJump = false;
        [Tooltip("Enables slow walking for the player.")] public bool enableSlowWalk = false;

        // Crouching System
        [Tooltip("Enables crouching for the player.")] public bool enableCrouch = false;
        [Tooltip("Setting to true will make crouch toggle rather than hold.")] public bool toggleCrouch = false;
        [Tooltip("Enables prone for the player. [REQUIRES TOGGLE CROUCH ON]")] public bool enableProne = false;
        [SerializeField][Tooltip("The height to set the character controller when crouched.")] private float characterCrouchHeight = 1.5f;
        [SerializeField][Tooltip("The height to set the character controller when prone.")] private float characterProneHeight = 1f;
        [SerializeField][Tooltip("The amount to drop the camera down when prone to help prevent camera clipping above.")] private float proneCameraOffset = 0.1f;
        [SerializeField][Range(0f, 20f)][Tooltip("The speed to smoothly lerp the camera when going prone and crouching.")] private float cameraLerpSpeed = 10f;

        // Keybinds
        [SerializeField][Tooltip("Set the button for sprinting.")] public KeyCode sprintButton = KeyCode.LeftShift;
        [SerializeField][Tooltip("Set the button for crouch.")] public KeyCode crouchButton = KeyCode.C;
        [SerializeField][Tooltip("Set the button for crouch.")] public KeyCode proneButton = KeyCode.Z;
        [SerializeField][Tooltip("Set the button for slow walking.")] public KeyCode slowWalkButton = KeyCode.LeftAlt;

        // Stamina System
        [Tooltip("Enables the stamina system.")] public bool useStamina;
        [SerializeField][Tooltip("The max amount of stamina the player has.")] private float maxStamina = 100f;
        [SerializeField][Range(0, 50)][Tooltip("How quickly the stamina drains.")] private float staminaDrain = 15f;
        [SerializeField][Range(0, 50)][Tooltip("How quickly the stramina regenerates.")] private float staminaRegen = 20f;
        [SerializeField][Tooltip("Put the stamina bar UI here.")] private Image staminaBar;
        [SerializeField][Tooltip("Put the stamina canvas group here.")] private CanvasGroup staminaCanvasGroup;

        // Simple Camera System
        [Tooltip("Enables the simple camera system.")] public bool useCameraSystem = true;
        [SerializeField][Range(0f, 30f)][Tooltip("Default mouse sensitivity.")] private float mouseSensitivity = 12f;

        // Other Settings
        [SerializeField][Tooltip("Hides the player mesh when disabled.")] private bool renderPlayerMesh = false;
        [SerializeField][Tooltip("Set the body object to hide when render player mesh is disabled")] private Renderer body;
        [SerializeField][Tooltip("Lock the cursor to the screen when game starts.")] private bool lockCursorToScreen = true;
        [SerializeField]
        [Tooltip("If you are using Cinemachine enable this to automatically " +
            "turn the player in the direction of the camera.")]
        private bool useCinemachine;
        [SerializeField][Tooltip("The velocity pulling the player down when grounded.")] private float antiBump = 4.5f;
        [SerializeField][Tooltip("Set the collision box size for the ground check.")] private Vector3 groundBoxSize = new(0.7f, 0.3f, 0.7f);
        [SerializeField]
        [Tooltip("The layer to use as the mask for the ground check, " + "Recommended: Create 'Ground' Layer -> Set " +
            "all ground objects to the new layer -> Set this layer mask to the new layer.")]
        private LayerMask groundLayerMask = ~0;
        #endregion

        // Private & Other Variables
        #region
        private CharacterController characterController;

        // Variables containing the double jump info
        private int jumpCount = 0;
        private const int extraJumps = 2;

        // Vector responsible for player velocity
        private Vector3 velocity;

        // Player state varaibles such as their current stamina, default speed and heights as well as their current state.
        private float playerStamina = 100f;
        private float crouchSpeedDivision = 1f;
        private float defaultWalkSpeed;
        private float defaultRunSpeed;
        private float originalHeight;
        private float originalCameraHeight;
        private bool hasRegenerated = true;
        private bool isSprinting = false;
        private bool isCrouching = false;
        private bool isProne = false;

        // Variables for the simple camera system
        private Camera cam;
        private float xRotation = 0f;
        #endregion

        // Awake
        #region
        private void Awake()
        {
            Cursor.lockState = lockCursorToScreen ? CursorLockMode.Locked : CursorLockMode.None; // Locks the cursor to screen if enabled
            characterController = GetComponent<CharacterController>();

            cam = Camera.main; // Sets the camera as the current main camera
        }
        #endregion

        // Start
        #region
        private void Start()
        {
            // Sets all the default speed and height variables to the defaults set in the inspector upon starting.
            defaultWalkSpeed = walkSpeed;
            defaultRunSpeed = runSpeed;
            originalHeight = characterController.height;
            originalCameraHeight = cam.transform.localPosition.y;

            // Attaches the camera as a child to the player and sets its position to the respected height.
            if (useCameraSystem && !useCinemachine)
            {
                cam.transform.parent = transform;
                cam.transform.position = new Vector3(0f, characterController.height - characterController.height / 6, 0f);
            }
        }
        #endregion

        // Update
        #region
        private void Update()
        {
            if (IsGrounded() && velocity.y < 0) // Sets the velocity to anti bump to hold the player down and resets the double jump count if on the ground.
            {
                velocity.y = -antiBump;
                jumpCount = 0;
            }

            // Gets the horizontal and vertical inputs of the player
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Sets the speed and state of the player depending on whether they are sprinting or not.
            float speed = Input.GetKey(sprintButton) ? runSpeed : walkSpeed;
            bool isWalking = x != 0 || z != 0;
            bool isRunning = isWalking && Input.GetKey(sprintButton);

            // Sets the move direction based on the input and clamps it to ensure going diagonal isnt faster than just forwards.
            Vector3 moveDir = transform.right * x + transform.forward * z;
            moveDir = Vector3.ClampMagnitude(moveDir, 1f);

            // Sets the current speed if running is enabled and moves the player using speed, time and the direction.
            float currentSpeed = enableRunning ? speed : walkSpeed;
            characterController.Move((currentSpeed / crouchSpeedDivision) * Time.deltaTime * moveDir);

            if (enableJumping) // If grounded or the jump count is below the set jumps allow the player to jump
            {
                if (Input.GetButtonDown("Jump") && !isProne && (IsGrounded() || (doubleJump && jumpCount < extraJumps)))
                {
                    velocity.y = 0f;
                    velocity.y += Mathf.Sqrt(jumpHeight * -2f * -gravity);
                    jumpCount++;
                }

                if (!IsGrounded()) // If a player hits a roof bounce them off of it (Prevents the player sticking to the roof for a moment).
                {
                    Vector3 checkPosition = transform.position + Vector3.up * (characterController.height / 2);

                    if (Physics.Raycast(checkPosition, Vector3.up, 0.1f, groundLayerMask))
                    {
                        velocity.y = -jumpHeight / 2;
                    }
                }
            }

            // Applies gravity to the velocity and moves the character based on this.
            velocity.y += -gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            if (enableCrouch) // Only runs if crouching is enabled for the player
            {
                if (Input.GetKeyDown(crouchButton)) // Crouches or uncrouches the player by setting the character controllers height based on their current state.
                {
                    if (isProne && !RoofAboveProne())
                    {
                        SetStance(characterCrouchHeight, (characterCrouchHeight / originalHeight) * originalCameraHeight);
                        isProne = false;
                        isCrouching = true;
                    }
                    else if (!isCrouching && !isProne)
                    {
                        SetStance(characterCrouchHeight, (characterCrouchHeight / originalHeight) * originalCameraHeight);
                        isCrouching = true;
                    }
                    else if (!RoofAbove())
                    {
                        SetStance(originalHeight, originalCameraHeight);
                        isCrouching = false;
                    }
                }

                if (enableProne && toggleCrouch && Input.GetKeyDown(proneButton)) // Prone is only available when toggle crouch is enabled and does a similar routine to the crouch code.
                {
                    if (isCrouching || !isProne)
                    {
                        SetStance(characterProneHeight, (characterProneHeight / originalHeight) * originalCameraHeight - proneCameraOffset);
                        isProne = true;
                        isCrouching = false;
                    }
                    else if (!RoofAbove())
                    {
                        SetStance(originalHeight, originalCameraHeight);
                        isProne = false;
                    }
                }

                if (Input.GetKeyUp(crouchButton) && !toggleCrouch) // If toggle crouch is not enabled the player will uncrouch immediately when the let go of the button if there is no roof above.
                {
                    if (!RoofAbove())
                    {
                        SetStance(originalHeight, originalCameraHeight);
                        isCrouching = false;
                    }
                    else
                    {
                        StartCoroutine(TryToUncrouch());
                    }
                }

                crouchSpeedDivision = isCrouching ? 2 : isProne ? 3 : 1; // Sets the player speed based on their crouch state. I used a division to keep it consitent even when sprinting.
            }

            if (enableSlowWalk) // If slow walk is enabled and the button is being pressed down set the walk speed to the slowed walk speed.
            {
                walkSpeed = Input.GetKey(slowWalkButton) ? slowWalkSpeed : defaultWalkSpeed;
            }

            if (useStamina) // If using the stamina system this will check if they are sprinting and set states in return.
            {
                if (!isRunning || !hasRegenerated)
                {
                    isSprinting = false;
                }
                else if (isWalking && isRunning && hasRegenerated)
                {
                    if (playerStamina > 0)
                    {
                        isSprinting = true;
                        Sprinting();
                    }
                }

                if (!isSprinting) // Regenerates the stamina when not sprinting and will set the run speed back to default when fully regenerated.
                {
                    if (playerStamina <= maxStamina)
                    {
                        playerStamina += staminaRegen * Time.deltaTime;
                        UpdateStamina();
                        if (staminaCanvasGroup != null)
                        {
                            staminaCanvasGroup.alpha = 1;
                        }

                        if (playerStamina >= maxStamina)
                        {
                            SetRunSpeed(defaultRunSpeed);
                            if (staminaCanvasGroup != null)
                            {
                                staminaCanvasGroup.alpha = 0;
                            }
                            hasRegenerated = true;
                        }
                    }
                }
            }

            if (useCameraSystem && !useCinemachine) // If using the simple camera system get the inputs of the mouse and apply this to the rotation of the player and camera.
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 100;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 100;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90, 90);

                cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                transform.Rotate(Vector3.up * mouseX);
            }

            if (useCinemachine && !useCameraSystem) // If using Cinemachine only rotate the player based on the camera direction and cinemachine will handle camera rotation.
            {
                transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
            }

            if (useCameraSystem && useCinemachine) // If both camera system are enabled it will return an error to make sure there are no issues with the camera trying to be controlled by two things at once.
            {
                Debug.LogError("Multiple camera systems in use! Please either use the cinemachine camera or the simple camera system. Only one can be active at a time.");
            }

            if (!renderPlayerMesh) // Will hide the player mesh of the player or body if set in the inspector by disabling all mesh renderers on these objects and vice versa.
            {
                Renderer[] meshRenderer = body != null ? body.GetComponentsInChildren<Renderer>() : GetComponentsInChildren<Renderer>();
                foreach (Renderer r in meshRenderer)
                {
                    r.enabled = false;
                }
            }
            else
            {
                Renderer[] meshRenderer = body != null ? body.GetComponentsInChildren<Renderer>() : GetComponentsInChildren<Renderer>();
                foreach (Renderer r in meshRenderer)
                {
                    r.enabled = true;
                }
            }
        }
        #endregion

        // Other Methods
        #region
        private bool IsGrounded() // Ground check to see if the player is touching the ground using CheckBox.
        {
            Vector3 checkPosition = transform.position + Vector3.down * (characterController.height / 2);
            checkPosition.y -= groundBoxSize.y / 2;
            Quaternion[] rotations = // Sets the rotation for the 4 boxes used in the ground check.
            {
            transform.rotation,
            transform.rotation * Quaternion.Euler(0, 22.5f, 0),
            transform.rotation * Quaternion.Euler(0, 45, 0),
            transform.rotation * Quaternion.Euler(0, 67.5f, 0)
        };

            // Checks each box to see if it is in collision with the ground layer using Checkbox returning true if so.
            foreach (Quaternion rotation in rotations)
            {
                if (Physics.CheckBox(checkPosition, groundBoxSize / 2, rotation, groundLayerMask))
                {
                    return true;
                }
            }

            return false; // Returns false if no box is colliding with the layer mask.
        }

        private bool RoofAbove() // Checks the area above the player using a Raycast to determine if there is enough room to stand up.
        {
            Vector3 checkPosition = transform.position + Vector3.up * (characterController.height / 2);
            float checkDistance = originalHeight - characterCrouchHeight;
            checkDistance = isProne ? originalHeight - characterProneHeight : checkDistance;

            return Physics.Raycast(checkPosition, Vector3.up, checkDistance, groundLayerMask);
        }
        private bool RoofAboveProne() // Same as the RoofAbove method however is only used for checking if there is enough room to change to crouch from a prone state.
        {
            Vector3 checkPosition = transform.position + Vector3.up * (characterController.height / 2);
            float checkDistance = (characterCrouchHeight - characterProneHeight) * 2;

            return Physics.Raycast(checkPosition, Vector3.up, checkDistance, groundLayerMask);
        }

        private void Sprinting() // Reduces the players stamina whilst sprinting and if below 0 sets the run speed to the walk speed.
        {
            if (hasRegenerated)
            {
                isSprinting = true;
                playerStamina -= staminaDrain * Time.deltaTime;
                UpdateStamina();
                if (staminaCanvasGroup != null)
                {
                    staminaCanvasGroup.alpha = 1; // Shows the UI canvas group.
                }

                if (playerStamina <= 0)
                {
                    hasRegenerated = false;
                    SetRunSpeed(walkSpeed);
                    if (staminaCanvasGroup != null)
                    {
                        staminaCanvasGroup.alpha = 0; // Hides the UI canvas group.
                    }
                }
            }
        }
        private void UpdateStamina() // Updates the stamina UI bar.
        {
            if (staminaBar != null)
            {
                staminaBar.fillAmount = playerStamina / maxStamina;
            }
        }
        private void SetRunSpeed(float speed) // Sets the run speed using the inherited parameters from when the method is run.
        {
            runSpeed = speed;
        }

        private void SetStance(float newHeight, float newCameraHeight) // Sets the player height and camera height using the inherited parameters when going prone or crouching.
        {
            characterController.height = newHeight;
            StartCoroutine(LerpCamera(newCameraHeight));
        }
        private IEnumerator TryToUncrouch() // When crouch is set to not toggle it will attempt to uncrouch whenever there is no roof above the player and the key isnt being pressed.
        {
            while (RoofAbove())
            {
                if (Input.GetKeyDown(crouchButton))
                {
                    yield break;
                }
                yield return null;
            }

            SetStance(originalHeight, originalCameraHeight);
            isCrouching = false;
        }
        private IEnumerator LerpCamera(float targetHeight) // Smoothly lerps the camera to the crouch or prone height to reduce the sharp change.
        {
            float elapsedTime = 0f;
            float duration = 1f / cameraLerpSpeed;
            Vector3 startPosition = cam.transform.localPosition;
            Vector3 targetPosition = new(startPosition.x, targetHeight, startPosition.z);

            while (elapsedTime < duration)
            {
                cam.transform.localPosition = new(
                    startPosition.x,
                    Mathf.Lerp(startPosition.y, targetHeight, elapsedTime / duration),
                    startPosition.z
                );
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cam.transform.localPosition = targetPosition;
        }

        private void OnDrawGizmos() // Draws the gizmos for the ground check and roof check for debugging to see if it is inside an object.
        {
            if (characterController == null) return;
            Gizmos.color = Color.red;

            Vector3 checkPosition = Vector3.down * (characterController.height / 2);
            Vector3 checkPositionUp = Vector3.up * (characterController.height / 2);
            checkPosition.y -= groundBoxSize.y / 2;

            float checkDistance = originalHeight - characterCrouchHeight;
            checkDistance = isProne ? originalHeight - characterProneHeight : checkDistance;

            Matrix4x4 originalGizmo = Gizmos.matrix;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(checkPosition, groundBoxSize);
            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 22.5f, 0), Vector3.one);
            Gizmos.DrawWireCube(checkPosition, groundBoxSize);
            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 45, 0), Vector3.one);
            Gizmos.DrawWireCube(checkPosition, groundBoxSize);
            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 67.5f, 0), Vector3.one);
            Gizmos.DrawWireCube(checkPosition, groundBoxSize);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawRay(checkPositionUp, Vector3.up * checkDistance);

            Gizmos.matrix = originalGizmo;
        }
        #endregion
    }
}
