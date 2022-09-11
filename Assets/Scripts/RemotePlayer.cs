using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

namespace ITRI
{
    public class RemotePlayer : MonoBehaviour
    {
        [SerializeField]
        public VRIK VRIK;
        [SerializeField]
        protected Transform Scaler;
        [SerializeField]
        protected Transform HeadTarget;
        [SerializeField]
        protected Transform LeftHandTarget;
        [SerializeField]
        protected Transform RightHandTarget;
        [SerializeField]
        protected Transform LeftHandBone;
        [SerializeField]
        protected Transform RightHandBone;
        [SerializeField]
        protected Transform LeftElbow;
        [SerializeField]
        protected Transform RightElbow;
        [SerializeField]
        protected Transform LeftFootTarget;
        [SerializeField]
        protected Transform RightFootTarget;
        [SerializeField]
        protected Canvas Canvas;
        [SerializeField]
        public Text Name;

        protected Dictionary<Material, Material> Materials = new Dictionary<Material, Material>();

        protected void Awake()
        {
            foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>(true))
            {
                CreateMaterials(renderer);
            }

            foreach(SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                CreateMaterials(renderer);
            }

            SetData("536447", "­ËµÛ©À¤]¬O", "­Û©s­Û");
        }

        protected void CreateMaterials(Renderer renderer)
        {
            List<Material> materials = new List<Material>();

            foreach(Material m in renderer.sharedMaterials)
            {
                Material clone;
                if(Materials.TryGetValue(m, out clone) == false)
                {
                    clone = new Material(m);
                    Materials.Add(m, clone);
                }

                materials.Add(clone);
            }

            renderer.sharedMaterials = materials.ToArray();
        }

        protected virtual void Update()
        {
            if(Canvas != null)
            {
                Vector3 forward = -transform.forward;
                forward.y = 0.0f;
                Canvas.transform.position = HeadTarget.position + Vector3.up * 0.2f;
                Canvas.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }

            if(LeftHandBone != null)
            {
                LeftHandBone.localPosition = VRIK.references.leftHand.localPosition;
            }

            if(RightHandBone != null)
            {
                RightHandBone.localPosition = VRIK.references.rightHand.localPosition;
            }

            if(LeftElbow != null)
            {
                LeftElbow.transform.position = VRIK.references.chest.TransformPoint(new Vector3(-0.36f, -0.12f, 0.16f));
            }

            if(RightElbow != null)
            {
                RightElbow.transform.position = VRIK.references.chest.TransformPoint(new Vector3(0.36f, -0.12f, 0.16f));
            }
        }

        public void SetData(string id, string role, string name)
        {
            //ID = id;
            Name.text = string.Format("<color=#FFC000FF><size=24>{0}</size></color> {1}", role, name);
        }

        public partial class C2S_PlayerPose
        {
            public float y;
            public Vector3 head_position;
            public Quaternion head_rotation;

            public bool left_hand_tracked;
            public Vector3 left_hand_position;
            public Quaternion left_hand_rotation;

            public bool right_hand_tracked;
            public Vector3 right_hand_position;
            public Quaternion right_hand_rotation;
        }

        public C2S_PlayerPose Pose = null;

        public void SetData(C2S_PlayerPose data)
        {
            Pose = data;
            transform.position = new Vector3(transform.position.x, data.y, transform.position.z);
            HeadTarget.position = data.head_position;
            HeadTarget.rotation = data.head_rotation;

            VRIK.solver.leftArm.positionWeight = data.left_hand_tracked ? 1.0f : 0.0f;
            VRIK.solver.leftArm.rotationWeight = data.left_hand_tracked ? 1.0f : 0.0f;
            LeftHandTarget.position = data.left_hand_position;
            LeftHandTarget.rotation = data.left_hand_rotation;

            if(data.left_hand_tracked)
            {
                LeftHandBone.rotation = Quaternion.LookRotation(data.left_hand_rotation * Vector3.right, data.left_hand_rotation * Vector3.back);
            }
            else
            {
                LeftHandBone.localRotation = Quaternion.identity;
            }

            VRIK.solver.rightArm.positionWeight = data.right_hand_tracked ? 1.0f : 0.0f;
            VRIK.solver.rightArm.rotationWeight = data.right_hand_tracked ? 1.0f : 0.0f;
            RightHandTarget.position = data.right_hand_position;
            RightHandTarget.rotation = data.right_hand_rotation;

            if(data.right_hand_tracked)
            {
                RightHandBone.rotation = Quaternion.LookRotation(data.right_hand_rotation * Vector3.left, data.right_hand_rotation * Vector3.back);
            }
            else
            {
                RightHandBone.localRotation = Quaternion.identity;
            }
        }

        public IEnumerator FadeIn()
        {
            foreach(Material m in Materials.Values)
            {
                float y = transform.position.y;
                m.EnableKeyword("_CLIPPING");
                m.SetVector("_ClipPlane", new Vector4(0.0f, -1.0f, 0.0f, y - 0.2f));
                m.SetColor("_ClipColor", Color.cyan);
                m.SetFloat("_ClipGap", 0.1f);
            }

            float now = Time.time;
            while(Time.time - now < 0.6f)
            {
                float y = transform.position.y;
                float t = (Time.time - now) / 0.6f;
                Vector4 plane = new Vector4(0.0f, -1.0f, 0.0f, Mathf.Lerp(y - 0.2f, y + 2.0f, t));

                foreach(Material m in Materials.Values)
                {
                    m.SetVector("_ClipPlane", plane);
                }

                yield return null;
            }

            foreach(Material m in Materials.Values)
            {
                m.DisableKeyword("_CLIPPING");
            }
        }

        public IEnumerator FadeOut(bool destroy)
        {
            if(destroy)
            {
                VRIK.enabled = false;
            }

            float y = transform.position.y;

            foreach(Material m in Materials.Values)
            {
                m.EnableKeyword("_CLIPPING");
                m.SetVector("_ClipPlane", new Vector4(0.0f, 1.0f, 0.0f, 0.2f - y));
                m.SetColor("_ClipColor", Color.cyan);
                m.SetFloat("_ClipGap", 0.1f);
            }

            float now = Time.time;
            while(Time.time - now < 0.6f)
            {
                float t = (Time.time - now) / 0.6f;
                Vector4 plane = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.2f - y, -2.0f - y, t));

                foreach(Material m in Materials.Values)
                {
                    m.SetVector("_ClipPlane", plane);
                }

                yield return null;
            }

            foreach(Material m in Materials.Values)
            {
                m.DisableKeyword("_CLIPPING");
            }

            if(destroy)
            {
                Destroy(gameObject);
            }
        }

        public void Calibrate()
        {
            if(Scaler != null && HeadTarget.position.y - transform.position.y >= 1.4f)
            {
                float scale = (HeadTarget.position.y - transform.position.y) / 1.64f;
                Scaler.transform.localScale = Vector3.one * scale;
            }
        }

        protected virtual void OnDestroy()
        {
            foreach(Material m in Materials.Values)
            {
                Destroy(m);
            }
        }
    }
}
