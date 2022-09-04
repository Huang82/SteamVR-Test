using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRPortal : MonoBehaviour
{
    [SerializeField]
    private SnapTurn st;
    public Transform Portal;

    public void PortalEvent()
    {
        var player = Player.instance.gameObject.transform;

        Transform VRCam = null;

        foreach (Transform tf in player.GetComponentsInChildren<Transform>())
        {
            if (tf.name == "VRCamera")
            {
                VRCam = tf;
                break;
            }
        }

        // 先設定位置
        player.position = Portal.transform.position;
        // 在計算位置下去扣除
        player.position = new Vector3(Portal.transform.position.x - (VRCam.position.x - Portal.transform.position.x),
                                      Portal.transform.position.y,
                                      Portal.transform.position.z - (VRCam.position.z - Portal.transform.position.z));

        float angle = Portal.transform.localEulerAngles.y - player.rotation.eulerAngles.y + (Portal.transform.localEulerAngles.y - VRCam.localEulerAngles.y);

        st.RotatePlayer(angle);

    }
}
