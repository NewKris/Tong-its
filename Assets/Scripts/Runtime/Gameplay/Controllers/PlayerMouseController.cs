using System;
using NordicBibo.Runtime.Gameplay.Cards;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public class PlayerMouseController : MonoBehaviour {
        public MousePivot mousePivot;
        public LayerMask cardMask;
        
        private PlayerControls _controls;
        private PlayingCard _draggingCard;
        private Transform _draggingCardPreviousPivot;
        
        private void Awake() {
            _controls = new PlayerControls();

            _controls.MouseControls.Click.performed += _ => RayCastCard(ToggleCard);
            _controls.MouseControls.Hold.performed += _ => RayCastCard(DragCard);
            _controls.MouseControls.Hold.canceled += _ => ReleaseDraggingCard();
            
            _controls.Enable();
        }
        
        private void OnDestroy() {
            _controls.Dispose();
        }

        private void ToggleCard(PlayingCard card) {
            card.ToggleSelected();
        }

        private void DragCard(PlayingCard card) {
            _draggingCard = card;
            _draggingCardPreviousPivot = card.Pivot;
                
            card.Pivot = mousePivot.transform;
            card.PlayEffects = false;
        }

        private void ReleaseDraggingCard() {
            if (!_draggingCard) return;

            _draggingCard.Pivot = _draggingCardPreviousPivot;
            _draggingCard.PlayEffects = true;
            
            _draggingCard = null;
            _draggingCardPreviousPivot = null;
        }

        private void RayCastCard(Action<PlayingCard> onHitCallback) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool rayHit = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cardMask); 
            if (rayHit && hit.collider.TryGetComponent(out PlayingCard card)) {
                onHitCallback(card);
            }
        }
    }
}