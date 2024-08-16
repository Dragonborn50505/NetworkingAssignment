using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraMovment : NetworkBehaviour
{
    private Camera m_Camera;
    private float cameraPositionZ;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        cameraPositionZ = m_Camera.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (!m_Camera)
            {
                m_Camera = Camera.main;
            }
            Vector3 PlayerPosition = this.transform.position;

            m_Camera.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y, cameraPositionZ);
        }
    }
}
