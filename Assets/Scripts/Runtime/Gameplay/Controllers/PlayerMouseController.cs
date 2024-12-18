using System;
using NordicBibo.Runtime.Gameplay.Cards;
using NordicBibo.Runtime.Gameplay.Utility;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Controllers {
    public class PlayerMouseController : MonoBehaviour {
        // TODO Bug: You can peek the stock by dragging the top card
        
        public static event Action<CardStack> OnCardStackClicked; 
        
        public MousePivot mousePivot;
        public LayerMask cardMask;
        public LayerMask pivotMask;
        public LayerMask stackMask;

        [Header("References")] 
        public PivotMaster pivotMaster;
        public CardStack playerHand;

        private bool _isDraggingCard;
        private PlayerControls _controls;
        private PlayingCard _draggingCard;
        
        private void Awake() {
            _controls = new PlayerControls();

            _controls.MouseControls.Click.performed += _ => {
                RayCast<PlayingCard>(cardMask, ToggleCard);
                RayCast<CardStack>(stackMask, ClickOnStack);
            };
            
            _controls.MouseControls.Hold.performed += _ => RayCast<PlayingCard>(cardMask, DragCard);
            _controls.MouseControls.Hold.canceled += _ => ReleaseDraggingCard();
            
            _controls.Enable();
        }
        
        private void OnDestroy() {
            _controls.Dispose();
        }

        private void Update() {
            if (_isDraggingCard) {
                TryFindNewPivot();
            }
        }

        private void TryFindNewPivot() {
            RayCast<Collider>(pivotMask, pivot => {
                if (!pivotMaster.Pivots.Contains(pivot.gameObject)) return;
                
                int pivotIndex = pivotMaster.Pivots.IndexOf(pivot.gameObject);

                playerHand.MoveCardToIndex(_draggingCard, pivotIndex);
            });
        }

        private void ClickOnStack(CardStack stack) {
            OnCardStackClicked?.Invoke(stack);
        }

        private void ToggleCard(PlayingCard card) {
            card.ToggleSelected();
        }

        private void DragCard(PlayingCard card) {
            _draggingCard = card;
                
            card.TempPivot = mousePivot.transform;
            pivotMaster.EnableCollisions();

            _isDraggingCard = true;
        }

        private void ReleaseDraggingCard() {
            if (!_draggingCard) return;

            _draggingCard.TempPivot = null;
            pivotMaster.DisableCollisions();
            
            _draggingCard = null;
            _isDraggingCard = false;
        }

        private void RayCast<T>(LayerMask mask, Action<T> callback) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool rayHit = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask); 
            
            if (rayHit && hit.collider.TryGetComponent(out T hitComponent)) {
                callback(hitComponent);
            }
        }
    }
}