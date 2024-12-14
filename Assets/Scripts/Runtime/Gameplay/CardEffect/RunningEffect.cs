using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.CardEffect {
    public readonly struct RunningEffect {
        private readonly TransformEffect _effect;
        private readonly float _startTime;
        private readonly float _endTime;
        private readonly bool _reverse;

        public bool HasElapsed => Time.time > _endTime;
        public bool ExpireWhenDone => _reverse;
        
        public RunningEffect(TransformEffect effect, float startTime, float endTime, bool reverse) {
            _effect = effect;
            _startTime = startTime;
            _endTime = endTime;
            _reverse = reverse;
        }

        public EffectTransform GetFinalTransform() {
            return new EffectTransform(
                position: _effect.EvaluatePosition(1),
                rotation: _effect.EvaluateRotation(1),
                scale: _effect.EvaluateScale(1)
            );
        }

        public EffectTransform EvaluateCurrentTransform() {
            float t = Mathf.Clamp01(CalculateElapsedPercentage());
            t = _reverse ? 1 - t : t;

            return new EffectTransform(
                position: _effect.EvaluatePosition(t),
                rotation: _effect.EvaluateRotation(t),
                scale: _effect.EvaluateScale(t)
            );
        }
        
        private float CalculateElapsedPercentage() {
            float d = _endTime - _startTime;
            float d2 = Time.time - _startTime;

            return d2 / d;
        }
    }
}