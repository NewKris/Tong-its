using System;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class MousePivot : MonoBehaviour {
        public float zPos;
        
        private void Update() {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = zPos;

            transform.position = pos;
        }
    }
}
