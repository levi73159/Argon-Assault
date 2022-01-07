using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private PlayerMovement input;
    private InputAction movement;

    [SerializeField] private float speed = 20f;

    // ranges are for clamp
    [SerializeField] private float xRange = 10f;
    [SerializeField] private float yRange = 13f;

    /// <summary>
    /// This is for the yRange - yOffset when in the min negtive
    /// 
    /// Example: 
    /// <code>
    /// Mathf.Clamp(newVector.y, -yRange + yOffset, yRange);
    /// </code>
    /// 
    /// </summary>
    /// <example>
    /// Mathf.Clamp(newVector.y, -yRange + yOffset, yRange);
    /// </example>
    [SerializeField] private float yOffset = 6f;

    [SerializeField] private float positonPitchFactor = -2f;
    [SerializeField] private float inputPitchFactor = -15f;
    [SerializeField] private float positonYawFactor = -5f;

    [SerializeField] private float inputRollFactor = 2f;

    private Vector2 move = Vector2.zero;

    #region Enable/Disable
    private void OnEnable()
    {
        input.Gameplay.Enable();
    }

    private void OnDisable()
    {
        input.Gameplay.Disable();
    }

    #endregion


    private void Awake()
    {
        input = new PlayerMovement();
        movement = input.Gameplay.Move;
    }


    private void Update()
    {
        Move();
        Rotation();
    }

    private void Rotation()
    {
        var pitch = transform.localPosition.y * positonPitchFactor // postion Pich
            + move.y * inputPitchFactor; // input pitch

        var yaw = transform.localPosition.x * positonYawFactor;
        var roll = move.x * inputRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void Move()
    {
        move = movement.ReadValue<Vector2>() * speed;
        var moveVector = move * Time.deltaTime;
        var newVector = transform.localPosition + (Vector3)moveVector;

        var clamp = GetClamp(newVector, xRange, yRange, yOffset);

        var clampVector = new Vector3(clamp.x, clamp.y, 0);

        transform.localPosition = clampVector;
    }

    private Vector2 GetClamp(Vector3 vector, float x, float y, float yOffset)
    {
        var clampX = Mathf.Clamp(vector.x, -x, x);
        var clampY = Mathf.Clamp(vector.y, -y + yOffset, y);
        return new Vector2(clampX, clampY);
    }
}