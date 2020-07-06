using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControls : MonoBehaviour

{

    //guns
    [Header("Guns")]
    [SerializeField] GameObject[] guns;
    // variable speeds
    [Header("General")]
    [Tooltip("In Meters")] [SerializeField] float speed = 4f;
    [Tooltip("In Meters")] [SerializeField] float xRange = 5f;
    [Tooltip("In Meters")] [SerializeField] float yRange = 3f;

    

    [Header("Screen-Position Based")]
    [SerializeField] float controlPitchFactor = -5;
    [SerializeField] float positionYawFactor = 5;

    [Header("Control Throw Based")]

    [SerializeField] float positionPitchFactor = -5;
    [SerializeField] float controlRollFactor = 5;


    float xThrow, yThrow;
    bool isControlEnabled = true;


    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }

    }

    void OnPlayerDeath() // called by string reference
    {
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
        // pitch
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToThrow;


        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

    }
    private void ProcessTranslation()
    {

        // horizontal movement
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xOffset = xThrow * speed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXpos = Mathf.Clamp(rawXPos, -xRange, xRange);

        // vertical movement
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float yOffset = yThrow * speed * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXpos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else 
        {
            SetGunsActive(false);
        }
    }
    
    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns)

        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;  // care may affect death effect
        }
    }

}

