using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    public float lerpDur;
    public float minFov;
    public float maxFov;
    public float scrollSens;
    public float moveSens;
    private float lerpTarget = 60; //Starting FoV
    private Vector3 moveDir;
    public float xBorder;
    public float zBorder;

    Vector3 pointerPos;
    Vector3 pointerTarget;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        lerpTarget -= Input.GetAxis("Mouse ScrollWheel") * scrollSens;
        lerpTarget = Mathf.Clamp(lerpTarget, minFov, maxFov);

        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, lerpTarget, lerpDur * Time.deltaTime);

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveDir.x = Mathf.Clamp(moveDir.x, transform.position.x < -xBorder ? 0 : -1, transform.position.x > xBorder ? 0 : 1);
        moveDir.z = Mathf.Clamp(moveDir.z, transform.position.z < -zBorder ? 0 : -1, transform.position.z > zBorder ? 0 : 1);

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            pointerPos = MouseInWorldCoords();
            print(pointerPos);

        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            pointerTarget = MouseInWorldCoords();
            transform.position = new Vector3(pointerTarget.x, transform.position.y, pointerTarget.z);
        }
    }

    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distanceToPlane))
            return ray.GetPoint(distanceToPlane);
        else return Vector3.zero;
    }

    private void FixedUpdate()
    {
        float speedMultiplier = 1;
        speedMultiplier *= Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        speedMultiplier *= Input.GetKey(KeyCode.LeftControl) ? 0.5f : 1;
        transform.position += moveSens * speedMultiplier * moveDir;
    }
}