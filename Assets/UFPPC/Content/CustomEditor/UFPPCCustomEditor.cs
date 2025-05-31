using UnityEditor;

namespace UFPPC
{
    [CustomEditor(typeof(UniversalFirstPersonPlayerController))]
    public class UFPPCCustomEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty walkSpeed;
        SerializedProperty jumpHeight;
        SerializedProperty runSpeed;
        SerializedProperty slowWalkSpeed;

        SerializedProperty gravity;

        SerializedProperty enableJumping;
        SerializedProperty enableRunning;
        SerializedProperty doubleJump;
        SerializedProperty enableSlowWalk;

        SerializedProperty enableCrouch;
        SerializedProperty toggleCrouch;
        SerializedProperty enableProne;
        SerializedProperty characterCrouchHeight;
        SerializedProperty characterProneHeight;
        SerializedProperty proneCameraOffset;
        SerializedProperty cameraLerpSpeed;

        SerializedProperty sprintButton;
        SerializedProperty crouchButton;
        SerializedProperty proneButton;
        SerializedProperty slowWalkButton;

        SerializedProperty useStamina;
        SerializedProperty maxStamina;
        SerializedProperty staminaDrain;
        SerializedProperty staminaRegen;

        SerializedProperty staminaBar;
        SerializedProperty staminaCanvasGroup;

        SerializedProperty useCameraSystem;
        SerializedProperty mouseSensitivity;

        SerializedProperty renderPlayerMesh;
        SerializedProperty body;

        SerializedProperty lockCursorToScreen;
        SerializedProperty useCinemachine;

        SerializedProperty antiBump;
        SerializedProperty groundBoxSize;
        SerializedProperty groundLayerMask;

        bool mainAttributesGroup = true, crouchSystemGroup = false, keybindsGroup = false, staminaSystemGroup = false, simpleCameraSystemGroup = true, otherSettingsGroup = true;
        #endregion

        private void OnEnable()
        {
            walkSpeed = serializedObject.FindProperty("walkSpeed");
            jumpHeight = serializedObject.FindProperty("jumpHeight");
            runSpeed = serializedObject.FindProperty("runSpeed");
            slowWalkSpeed = serializedObject.FindProperty("slowWalkSpeed");

            gravity = serializedObject.FindProperty("gravity");

            enableJumping = serializedObject.FindProperty("enableJumping");
            enableRunning = serializedObject.FindProperty("enableRunning");
            doubleJump = serializedObject.FindProperty("doubleJump");
            enableSlowWalk = serializedObject.FindProperty("enableSlowWalk");

            enableCrouch = serializedObject.FindProperty("enableCrouch");
            toggleCrouch = serializedObject.FindProperty("toggleCrouch");
            enableProne = serializedObject.FindProperty("enableProne");
            characterCrouchHeight = serializedObject.FindProperty("characterCrouchHeight");
            characterProneHeight = serializedObject.FindProperty("characterProneHeight");
            proneCameraOffset = serializedObject.FindProperty("proneCameraOffset");
            cameraLerpSpeed = serializedObject.FindProperty("cameraLerpSpeed");

            sprintButton = serializedObject.FindProperty("sprintButton");
            crouchButton = serializedObject.FindProperty("crouchButton");
            proneButton = serializedObject.FindProperty("proneButton");
            slowWalkButton = serializedObject.FindProperty("slowWalkButton");

            useStamina = serializedObject.FindProperty("useStamina");
            maxStamina = serializedObject.FindProperty("maxStamina");
            staminaDrain = serializedObject.FindProperty("staminaDrain");
            staminaRegen = serializedObject.FindProperty("staminaRegen");

            staminaBar = serializedObject.FindProperty("staminaBar");
            staminaCanvasGroup = serializedObject.FindProperty("staminaCanvasGroup");

            useCameraSystem = serializedObject.FindProperty("useCameraSystem");
            mouseSensitivity = serializedObject.FindProperty("mouseSensitivity");

            renderPlayerMesh = serializedObject.FindProperty("renderPlayerMesh");
            body = serializedObject.FindProperty("body");

            lockCursorToScreen = serializedObject.FindProperty("lockCursorToScreen");
            useCinemachine = serializedObject.FindProperty("useCinemachine");

            antiBump = serializedObject.FindProperty("antiBump");
            groundBoxSize = serializedObject.FindProperty("groundBoxSize");
            groundLayerMask = serializedObject.FindProperty("groundLayerMask");
        }

        public override void OnInspectorGUI()
        {
            UniversalFirstPersonPlayerController ufppc = (UniversalFirstPersonPlayerController)target;

            serializedObject.Update();

            EditorGUILayout.LabelField("Camera Mode: First Person | Made By: Jake Brotherton", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            mainAttributesGroup = EditorGUILayout.BeginFoldoutHeaderGroup(mainAttributesGroup, "Main Attributes");
            if (mainAttributesGroup)
            {
                EditorGUILayout.PropertyField(walkSpeed);
                EditorGUILayout.PropertyField(jumpHeight);
                EditorGUILayout.PropertyField(runSpeed);
                EditorGUILayout.PropertyField(slowWalkSpeed);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(gravity);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(enableJumping);
                EditorGUILayout.PropertyField(enableRunning);
                EditorGUILayout.PropertyField(doubleJump);
                EditorGUILayout.PropertyField(enableSlowWalk);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            crouchSystemGroup = EditorGUILayout.BeginFoldoutHeaderGroup(crouchSystemGroup, "Crouch System");
            if (crouchSystemGroup)
            {
                EditorGUILayout.PropertyField(enableCrouch);
                if (ufppc.enableCrouch)
                {
                    EditorGUILayout.PropertyField(toggleCrouch);
                    if (ufppc.toggleCrouch)
                    {
                        EditorGUILayout.PropertyField(enableProne);
                    }
                    EditorGUILayout.PropertyField(characterCrouchHeight);
                    if (ufppc.enableProne)
                    {
                        EditorGUILayout.PropertyField(characterProneHeight);
                        EditorGUILayout.PropertyField(proneCameraOffset);
                    }
                    EditorGUILayout.PropertyField(cameraLerpSpeed);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            keybindsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(keybindsGroup, "Keybinds");
            if (keybindsGroup)
            {
                if (ufppc.enableRunning)
                {
                    EditorGUILayout.PropertyField(sprintButton);
                }
                if (ufppc.enableCrouch)
                {
                    EditorGUILayout.PropertyField(crouchButton);
                }
                if (ufppc.enableProne)
                {
                    EditorGUILayout.PropertyField(proneButton);
                }
                if (ufppc.enableSlowWalk)
                {
                    EditorGUILayout.PropertyField(slowWalkButton);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            staminaSystemGroup = EditorGUILayout.BeginFoldoutHeaderGroup(staminaSystemGroup, "Stamina System");
            if (staminaSystemGroup)
            {
                EditorGUILayout.PropertyField(useStamina);
                if (ufppc.useStamina)
                {
                    EditorGUILayout.PropertyField(maxStamina);
                    EditorGUILayout.PropertyField(staminaDrain);
                    EditorGUILayout.PropertyField(staminaRegen);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(staminaBar);
                    EditorGUILayout.PropertyField(staminaCanvasGroup);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            simpleCameraSystemGroup = EditorGUILayout.BeginFoldoutHeaderGroup(simpleCameraSystemGroup, "Simple Camera System");
            if (simpleCameraSystemGroup)
            {
                EditorGUILayout.PropertyField(useCameraSystem);
                if (ufppc.useCameraSystem)
                {
                    EditorGUILayout.PropertyField(mouseSensitivity);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            otherSettingsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(otherSettingsGroup, "Other Settings");
            if (otherSettingsGroup)
            {
                EditorGUILayout.PropertyField(renderPlayerMesh);
                EditorGUILayout.PropertyField(body);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(lockCursorToScreen);
                EditorGUILayout.PropertyField(useCinemachine);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(antiBump);
                EditorGUILayout.PropertyField(groundBoxSize);
                EditorGUILayout.PropertyField(groundLayerMask);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
