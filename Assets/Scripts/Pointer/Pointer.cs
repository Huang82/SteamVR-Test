using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float m_DefaultLength = 5.0f;
    public GameObject m_Dot;
    public VRInputModule m_inputModule;

    private LineRenderer m_LineRenderer = null;


    void Start()
    {
        m_LineRenderer = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        // Use default or distance
        float targetLength = m_DefaultLength;

        // Raycast
        RaycastHit hit = CreateRatcast(targetLength);

        // Default
        Vector3 endPosition = this.transform.position + (transform.forward * targetLength);

        // Or base on hit
        if (hit.collider != null)
            endPosition = hit.point;

        // Set position of the dot
        m_Dot.transform.position = endPosition;

        // Set linerenderer
        m_LineRenderer.SetPosition(0, this.transform.position);
        m_LineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRatcast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);

        return hit;
    }
}
