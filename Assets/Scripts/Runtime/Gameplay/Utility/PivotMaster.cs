using System;
using System.Collections.Generic;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Utility {
    public class PivotMaster : MonoBehaviour {
        private static Mesh GizmoMesh;
        
        public bool cardsFaceUp;
        
        [Header("Collision")]
        public bool pivotsHaveCollision;
        public float collisionRadius;
        
        [Header("Spread")]
        public float rotationOffset = 5;
        public Vector3 positionOffset;
        public float heightScaling;
        
        [Header("Debug")] 
        public int drawCount;
        
        public List<GameObject> Pivots { get; private set; }
        public GameObject this[int i] => Pivots[i];
        public int Count => Pivots.Count;

        public void DestroyPivot(GameObject pivot) {
            Pivots.Remove(pivot);
            Destroy(pivot);
            
            ForEachPivotTransform(Pivots.Count, (pos, rot, i) => {
                Pivots[i].transform.SetLocalPositionAndRotation(pos, rot);
            });
        }
        
        public GameObject CreatePivot() {
            GameObject newPivot = new GameObject {
                transform = {
                    parent = transform
                }
            };

            if (pivotsHaveCollision) {
                SphereCollider col = newPivot.AddComponent<SphereCollider>();
                col.radius = collisionRadius;
                col.isTrigger = true;
                newPivot.layer = LayerMask.NameToLayer("Pivot");

                col.enabled = false;
            }
                
            Pivots.Add(newPivot);
            
            ForEachPivotTransform(Pivots.Count, (pos, rot, i) => {
                Pivots[i].transform.SetLocalPositionAndRotation(pos, rot);
            });

            return newPivot;
        }

        public void EnableCollisions() {
            if (!pivotsHaveCollision) return;

            foreach (GameObject pivot in Pivots) {
                pivot.GetComponent<Collider>().enabled = true;
            }
        }

        public void DisableCollisions() {
            if (!pivotsHaveCollision) return;
            
            foreach (GameObject pivot in Pivots) {
                pivot.GetComponent<Collider>().enabled = false;
            }
        }

        private void Awake() {
            Pivots = new List<GameObject>(16);
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
            
            
            ForEachPivotTransform(drawCount, (pos, rot, _) => {
                Gizmos.color = Color.red;
                Gizmos.DrawWireMesh(
                    GizmoMesh, 
                    transform.TransformPoint(pos), 
                    transform.rotation * rot
                );
                
                Gizmos.color = Color.yellow;
                if (pivotsHaveCollision) {
                    Gizmos.DrawWireSphere(transform.TransformPoint(pos), collisionRadius);
                }
            });

            
        }
    }
}