using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OrbitAdjuster : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public Transform followTarget;
    public Vector3 desiredWorldPoint;

    public int orbitIndex = 2; // 0 = Top, 1 = Middle, 2 = Bottom

    void Start()
    {
        AdjustOrbit();
    }

    void AdjustOrbit()
    {
        Vector3 offset = desiredWorldPoint - followTarget.position;

        float height = offset.y;

        Vector2 horizontal = new Vector2(offset.x, offset.z);
        float radius = horizontal.magnitude;

        var orbit = freeLook.m_Orbits[orbitIndex];
        orbit.m_Height = height;
        orbit.m_Radius = radius;

        freeLook.m_Orbits[orbitIndex] = orbit;
    }
}
