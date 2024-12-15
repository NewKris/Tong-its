using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class PivotMaster : MonoBehaviour {
        private static Mesh GizmoMesh;
        
        public bool cardsFaceUp;
        
        [Header("Spread")]
        public float rotationOffset = 5;
        public Vector3 positionOffset;
        public float heightScaling;
        
        [Header("Debug")] 
        public int drawCount;
        
        private readonly List<GameObject> _pivots = new List<GameObject>(16);

        public void DestroyPivot(GameObject pivot) {
            _pivots.Remove(pivot);
            Destroy(pivot);
            
            ForEachPivotTransform(_pivots.Count, (pos, rot, i) => {
                _pivots[i].transform.SetLocalPositionAndRotation(pos, rot);
            });
        }
        
        public GameObject CreatePivot() {
            GameObject newPivot = new GameObject {
                transform = {
                    parent = transform
                }
            };
                
            _pivots.Add(newPivot);
            
            ForEachPivotTransform(_pivots.Count, (pos, rot, i) => {
                _pivots[i].transform.SetLocalPositionAndRotation(pos, rot);
            });

            return newPivot;
        }
        
        private void ForEachPivotTransform(int pivotCount, Action<Vector3, Quaternion, int> callback) {
            int halfCount = Mathf.FloorToInt(pivotCount / 2f);
            bool evenPivotCount = pivotCount % 2 == 0;
            
            Vector3 halfPosOffset = evenPivotCount ? positionOffset * 0.5f : Vector3.zero;
            Vector3 startPos = -positionOffset * halfCount + halfPosOffset;


            float invertRotOffset = cardsFaceUp ? -1 : 1;
            float offset = rotationOffset * invertRotOffset;
            float halfRotOffset = evenPivotCount ? offset * 0.5f : 0;
            float startRot = offset * halfCount - halfRotOffset;
            float pivotYRot = cardsFaceUp ? 180 : 0;

            for (int i = 0; i < pivotCount; i++) {
                Vector3 localPos = startPos + positionOffset * i;
                localPos.y *= heightScaling * Mathf.Abs(halfCount - i);

                if (i > halfCount) {
                    localPos.y *= -1;
                }
                
                float localZRot = startRot - offset * i;
                Quaternion localRot = Quaternion.Euler(0, pivotYRot, localZRot);
                
                callback(localPos, localRot, i);
            }
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
            
            ForEachPivotTransform(drawCount, (pos, rot, _) => {
                Gizmos.DrawWireMesh(
                    GizmoMesh, 
                    transform.TransformPoint(pos), 
                    transform.rotation * rot
                );
            });
        }
    }
}