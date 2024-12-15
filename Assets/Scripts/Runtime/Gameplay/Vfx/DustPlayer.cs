using System;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.VFX;

namespace NordicBibo.Runtime.Gameplay.Vfx {
    public class DustPlayer : MonoBehaviour {
        public VisualEffect dustVfx;

        public void Play(Vector3 position) {
            dustVfx.SetVector3("Position", position);
            dustVfx.Play();
        }
    }
}
