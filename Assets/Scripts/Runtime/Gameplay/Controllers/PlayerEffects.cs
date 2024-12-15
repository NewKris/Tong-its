using NordicBibo.Runtime.Gameplay.Vfx;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public class PlayerEffects : MonoBehaviour {
        [Header("Juice")] 
        public LayerMask dustSurface;
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                TryPlayDust();
            }
        }

        private void TryPlayDust() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSurface = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, dustSurface); 
            
            if (hitSurface && hit.collider.TryGetComponent(out DustPlayer dustPlayer)) {
                dustPlayer.Play(hit.point);
            }
        }
    }
}