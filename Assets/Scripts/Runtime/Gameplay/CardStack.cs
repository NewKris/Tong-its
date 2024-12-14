using System;
using System.Collections.Generic;
using NordicBibo.Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace NordicBibo.Runtime.Gameplay {
    public struct Pivot {
        public Vector3 position;
        public Quaternion rotation;
    }
    
    public class CardStack : MonoBehaviour {
        private static Mesh GizmoMesh;
        
        public bool interactable;
        public bool cardsFaceUp;
        public UnityEvent<List<PlayingCard>> onStackUpdated;
        
        [Header("Spread")]
        public float rotationOffset = 5;
        public Vector3 positionOffset;
        public float heightScaling;

        [Header("Debug")] 
        public int drawCount;

        private readonly List<PlayingCard> _cardsInStack = new List<PlayingCard>(56);

        public int Count => _cardsInStack.Count;
        
        public void Shuffle() {
            _cardsInStack.Shuffle();
        }
        
        public void AddCard(PlayingCard card, bool snapToPivot = false) {
            card.Interactable = interactable;
            
            _cardsInStack.Add(card);
            UpdatePivots(snapToPivot);
            
            onStackUpdated.Invoke(_cardsInStack);
        }

        public PlayingCard Pop(bool snapToPivot = false) {
            PlayingCard topCard = _cardsInStack[0];
            
            _cardsInStack.Remove(topCard);
            UpdatePivots(snapToPivot);
            
            onStackUpdated.Invoke(_cardsInStack);

            return topCard;
        }

        private void UpdatePivots(bool snapToPivot = false) {
            ForEachPivot(_cardsInStack.Count, (pivot, i) => {
                if (snapToPivot) _cardsInStack[i].SetPivotImmediate(pivot);
                else _cardsInStack[i].SetPivot(pivot);
            });
        }
        
        private void OnDrawGizmos() {
            if (GizmoMesh == null) {
#if UNITY_EDITOR
                string guid = UnityEditor.AssetDatabase.FindAssets("card model")[0];

                if (string.IsNullOrEmpty(guid)) {
                    return;
                }
            
                GizmoMesh = (Mesh) UnityEditor.AssetDatabase.LoadAssetAtPath(
                    UnityEditor.AssetDatabase.GUIDToAssetPath(guid), 
                    typeof(Mesh)
                );
#endif
            }
            
            Gizmos.color = Color.red;
            
            ForEachPivot(drawCount, (pivot, i) => {
                Gizmos.DrawWireMesh(
                    GizmoMesh, 
                    pivot.position, 
                    pivot.rotation
                );
            });
        }

        private void ForEachPivot(int pivotCount, Action<Pivot, int> callback) {
            int halfCount = Mathf.FloorToInt(pivotCount / 2f);
            bool evenPivotCount = pivotCount % 2 == 0;
            
            float halfRotOffset = evenPivotCount ? rotationOffset * 0.5f : 0;
            Vector3 halfPosOffset = evenPivotCount ? positionOffset * 0.5f : Vector3.zero;
            
            float startRot = rotationOffset * halfCount - halfRotOffset;
            Vector3 startPos = -positionOffset * halfCount + halfPosOffset;
            float pivotYRot = cardsFaceUp ? 180 : 0;

            Vector3 parentPosition = transform.position;
            Quaternion parentRotation = transform.rotation;
            
            for (int i = 0; i < pivotCount; i++) {
                float localZRot = startRot - rotationOffset * i;
                Vector3 localPos = startPos + positionOffset * i;
                localPos.y *= heightScaling * Mathf.Abs(halfCount - i);

                if (i > halfCount) {
                    localPos.y *= -1;
                }
                
                Pivot pivot = new Pivot() {
                    position = parentPosition + transform.TransformDirection(localPos),
                    rotation = Quaternion.Euler(0, pivotYRot, parentRotation.eulerAngles.z + localZRot)
                };

                callback(pivot, i);
            }
        }
    }
}