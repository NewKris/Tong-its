using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.CardEffect {
    [CreateAssetMenu(menuName = "Transform Effect")]
    public class TransformEffect : ScriptableObject {
        [Header("Position")] 
        public AnimationCurve xPos;
        public AnimationCurve yPos;
        public AnimationCurve zPos;
        
        [Header("Rotation")] 
        public AnimationCurve xRot;
        public AnimationCurve yRot;
        public AnimationCurve zRot;
        
        [Header("Scale")] 
        public AnimationCurve xScale;
        public AnimationCurve yScale;
        public AnimationCurve zScale;

        public Vector3 EvaluatePosition(float t) {
            return new Vector3() {
                x = xPos.Evaluate(t),
                y = yPos.Evaluate(t),
                z = zPos.Evaluate(t)
            };
        }
        
        public Vector3 EvaluateRotation(float t) {
            return new Vector3() {
                x = xRot.Evaluate(t),
                y = yRot.Evaluate(t),
                z = zRot.Evaluate(t)
            };
        }
        
        public Vector3 EvaluateScale(float t) {
            return new Vector3() {
                x = xScale.Evaluate(t),
                y = yScale.Evaluate(t),
                z = zScale.Evaluate(t)
            };
        }
    }
}