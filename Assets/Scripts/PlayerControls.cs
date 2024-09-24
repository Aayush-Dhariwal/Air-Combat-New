using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //[SerializeField] InputAction movement;      ---------- New Input System ---------
    [Header("General Setup Settings")]
    [Tooltip("How fast the ship moves up and down based upon player input")] [SerializeField] float controlSpeed = 10f;
    [Tooltip("How far player moves horizontally")][SerializeField] float xRange = 10f;
    [Tooltip("How far player moves vertically")][SerializeField] float yRange = 7f;

    [Header("Laser gun Array")]
    [Tooltip("Add all player lasers here")]
    [SerializeField] GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -20f;
    //[SerializeField] float rotationFactor = 1f;

    float xThrow, yThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        //Quaternion tragetRotation = Quaternion.Euler(pitch, yaw, roll);
        //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, tragetRotation, rotationFactor);    //---- To make the rotation less choppy
    }

    private void ProcessTranslation()
    {
        //float horizontalThrow = movement.ReadValue<Vector2>().x;
        //Debug.Log(horizontalThrow);

        //float verticalThrow = movement.ReadValue<Vector2>().y;
        //Debug.Log(verticalThrow);

        xThrow = Input.GetAxis("Horizontal");     //----- To use A and D for Horizontal Movement (-1 to 1)
        yThrow = Input.GetAxis("Vertical");       //----- W and S for Vertical Movement

        float xOffset = xThrow * Time.deltaTime * controlSpeed;  // To achieve frame rate independence and to get desired speed
        float yoffset = yThrow * Time.deltaTime * controlSpeed;

        float rawXPos = transform.localPosition.x + xOffset;     // new local position of player 
        float rawYPos = transform.localPosition.y + yoffset;

        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);    // to clamp the position of player within desired range
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);   // to move the player
    }

    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }

    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

    //private void OnEnable()      ----------- For New Input System -------
    //{
    //    movement.Enable();
    //}

    //private void OnDisable()
    //{
    //    movement.Disable();
    //}
}
