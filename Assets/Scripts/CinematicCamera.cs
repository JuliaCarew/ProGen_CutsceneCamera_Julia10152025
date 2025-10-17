using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    // point of interest ("Target")
    [SerializeField] private Transform poi;

    // list of possible points to move to
    public List<Transform> points = new List<Transform>();
    public Transform testPoint;
    public Transform startPoint;
    public Transform endPoint;

    // position
    [Header("Current Variables")]
    [SerializeField]  private Vector3 currentPos;
    [SerializeField] private float currentDist;
    [SerializeField] private float currentTilt;
    [SerializeField] private float currentRot;
    // initial variables

    [Header("Initial Variables")]
    [SerializeField]  private Vector3 initialPos;
    [SerializeField] private float initialDist;
    [SerializeField] private float initialTilt;
    [SerializeField] private float initialRot;


    // variables for distance, vertical rot, horizontal rot
    [Header("Distance")]
    [SerializeField] private float minDist; // make these into circle around target
    [SerializeField] private float maxDist;

    [Header("Tilt (vertical rotation)")]
    [SerializeField] private float mintilt;
    [SerializeField] private float maxTilt;

    [Header("Angle (horizontal rotation")]
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;

    // enums of types of shots    
    public enum CameraShot 
    {
        Pan,                 // rotate from 1 set angle to another horizontally or vertically
        Orbit,               // move in a circular motion around target horizontally
        Dolly,               // move on a straight 'track' from one point to another
        Zoom,                // move in closer to target
        Wide,                // show all activity in one shot
        OverTheShoulder,     // over target's shoulder at an offset
        CloseUp,             // show details of target
        CutIn,               // abrupt close shot
        BirdsEye,            // scene shown from above
        TwoShot,             // show two targets at medium dist
        None                 
    }

    [Header("Camera Shots")]
    public CameraShot shots;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Escape is Initial shot
        {
            MoveToPosition(initialPos, initialTilt, initialRot, initialDist);
            Debug.Log("Moved to initial position variables");

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) // 1 is Pan
        {
            SetCameraShot(CameraShot.Pan);
        }
    }

    void SetCameraShot(CameraShot shot)
    {
        switch(shots)
        {
            case CameraShot.Pan:
                Pan();
                break;




        }

    }

    void MoveToPosition(Vector3 pos, float tilt, float rotation, float dist)
    {
        currentPos = pos;
        currentTilt = tilt;
        currentRot = rotation;
        currentDist = dist;

        transform.position = currentPos;
        transform.eulerAngles = new Vector3(currentRot, currentTilt, 0);
    }

    void Pan()
    {
        Debug.Log("Pan running");
        StartCoroutine(MoveCamera(startPoint, endPoint, 5f));
    }

    // will move camera from current pos to new point(s) at a specified time
    IEnumerator MoveCamera(Transform pos, Transform newPos, float moveTime)
    {
        float currentTime = 0f;

        // position
        Vector3 startPos = pos.position;
        Vector3 targetPos = newPos.position;

        // rotation
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(poi.position - targetPos);

        while (currentTime < moveTime)
        {
            float t = currentTime / moveTime;

            // lerp to end using position and rotation
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            currentTime += Time.deltaTime;
            yield return null;
        }

        // set final transform / rot
        transform.position = targetPos;
        transform.rotation = endRot;
    }
}
