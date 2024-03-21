using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Zeng.Mdx
{
    public class NodeBillboarded : MonoBehaviour
    {
        public bool Billboarded = false;
        public bool BillboardedLockX = false;
        public bool BillboardedLockY = false;
        public bool BillboardedLockZ = false;
        private Camera Camera;
        public Vector3 localEulerAnglesOffset = new Vector3(0f, 90f, 0f);

        // Start is called before the first frame update
        private void Start()
        {
            Camera = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.LookAt(transform.position - Camera.transform.forward, Camera.transform.up);
            Vector3 angles = transform.localEulerAngles;
            //angles += localEulerAnglesOffset;
            transform.localEulerAngles = angles;

        }
    }
}
