using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private float m_DefaultLength = 5.0f;
    [SerializeField] private GameObject m_Dot;
    [SerializeField] private LineRenderer m_LineRenderer = null;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private PointerCallEvent m_PointerCallEvent;



    void Start()
    {
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



        // Or base on hit, 先別碰到東西在顯示(常開)
        if (hit.collider != null)
        { 
            endPosition = hit.point;
            m_PointerCallEvent.IsHit = true;
        //    m_Dot.SetActive(true);
        //    m_LineRenderer.gameObject.SetActive(true);
        }
        else
        {
            m_PointerCallEvent.IsHit = false;
            //m_Dot.SetActive(false);
            //m_LineRenderer.gameObject.SetActive(false);
        }

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
        Physics.Raycast(ray, out hit, m_DefaultLength, layerMask);

        return hit;
    }
}
