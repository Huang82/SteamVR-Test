using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrorControlModel : MonoBehaviour
{
    private GameObject NowTarget = null;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (NowTarget == null)
                    NowTarget = hit.transform.gameObject;
                else
                {
                    NowTarget.transform.position = new Vector3(hit.point.x, NowTarget.transform.position.y, hit.point.z);
                }
            }
        }
        else
            NowTarget = null;
    }
}
