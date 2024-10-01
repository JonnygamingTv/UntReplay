using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay.Patches
{
    public class MyTransform
    {
        public Vector3 position;
        public Quaternion rotation;

        public MyTransform(Transform tf)
        {
            position = tf.position;
            rotation = tf.rotation;
        }
        public MyTransform(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}
