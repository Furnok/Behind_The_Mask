using UnityEngine;
using System;

[Serializable]
public struct S_StructPlayerSettings
{
    [Header("Player Movement Settings")]
    public float WalkSpeed;
    public float SprintSpeed;

    [Header("Player Stamina Settings")]
    public float StaminaMax;
    public float StaminaDrainRate;
    public float StaminaRecoveryRate;
    public float StaminaRecoveryDelay;

    [Header("Player Rotation Settings")]
    public float MaxPitchAngle;
    public float MinPitchAngle;
    public float Sensitivity;

    [Header("Camera FOV Settings")]
    public float NormalFOV;
    public float SprintFOV;
    public float SmoothTimeFOV;

    [Header("Masks Settings")]
    public float MaskDelayToEquipped;

}
