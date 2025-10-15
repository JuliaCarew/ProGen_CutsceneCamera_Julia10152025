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

    void Start()
    {
        
    }
    
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
        transform.eulerAngles = new Vector3(currentRot, currentTilt, 0); // should rotate camera ??
    }

    void Pan()
    {
        Debug.Log("Pan running");
        Vector3 panVector = new Vector3(5, 5, 5);
        MoveCamera(startPoint, endPoint, 5f);
    }

    // will move camera from current pos to new point(s) at a specified time
    IEnumerator MoveCamera(Transform pos, Transform newPos, float moveTime)
    {
        float currentTime = 0f;

        currentPos = pos.position;       

        while (currentTime < 1)
        {

            transform.position = Vector3.Lerp(currentPos, newPos.position, currentTime / moveTime);

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
// Transform.Translate
// Transform.LookAt

// can manually set start/end points for now, but they would need to dynamically move with each different camera shot.
// pan would need a start / end point transforms