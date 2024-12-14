using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.CardEffect {
    public readonly struct EffectTransform {
        public readonly Vector3 position;
        public readonly Vector3 rotation;
        public readonly Vector3 scale;
        
        public EffectTransform(Vector3 position, Vector3 rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public static EffectTransform operator +(EffectTransform a, EffectTransform b) => new EffectTransform(
            a.position + b.position,
            a.rotation + b.rotation,
            a.scale + b.scale
        );
        
        public static EffectTransform operator -(EffectTransform a, EffectTransform b) => new EffectTransform(
            a.position - b.position,
            a.rotation - b.rotation,
            a.scale - b.scale
        );
    }
}