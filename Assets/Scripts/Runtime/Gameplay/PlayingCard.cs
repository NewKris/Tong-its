using System;
using System.Collections;
using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.CardEffect;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NordicBibo.Runtime.Gameplay {
    public class PlayingCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
        private static readonly int CardIndex = Shader.PropertyToID("_Card_Index");
        private const string HOVER_KEY = "Hover";
        private const string SELECT_KEY = "Select";
        private const string DRAG_KEY = "Drag";

        [Header("Damping")]
        public float pivotMoveDamping;
        public float pivotRotationMaxDelta;
        
        [Header("Audio")] 
        public float pitchStep;
        public float maxPitch;

        [Header("Animations")] 
        public float effectPlaySpeed = 0.2f;
        public TransformEffect hoverEffect;
        public TransformEffect selectEffect;
        public TransformEffect dragEffect;

        private bool _selected;
        private EffectTransform _transformOffset;
        private Vector3 _positionPivot;
        private Vector3 _positionPivotVel;
        private Quaternion _rotationPivot;
        private AudioSource _drawSound;
        
        private readonly Dictionary<string, RunningEffect> _activeEffects = new Dictionary<string, RunningEffect>();
        private readonly List<string> _expiredEffects = new List<string>(3);

        public int Tally { get; private set; }
        public Vector3 TargetPivotPosition { get; set; }
        public Quaternion TargetPivotRotation { get; set; }

        public bool Interactable {
            set => GetComponent<Collider>().enabled = value;
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            RunningEffect hover = CreateNewEffect(hoverEffect, false);
            _activeEffects[HOVER_KEY] = hover;
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            RunningEffect hover = CreateNewEffect(hoverEffect, true);
            _activeEffects[HOVER_KEY] = hover;
        }
        
        public void OnPointerDown(PointerEventData eventData) {
            _selected = !_selected;
            
            RunningEffect select = CreateNewEffect(selectEffect, !_selected);
            _activeEffects[SELECT_KEY] = select;

            PlayDrawSound(_selected ? 1.2f : 0.8f);
        }

        public void SetPivot(Pivot pivot) {
            TargetPivotPosition = pivot.position;
            TargetPivotRotation = pivot.rotation;
        }
        
        public void SetPivotImmediate(Pivot pivot) {
            TargetPivotPosition = pivot.position;
            _positionPivot = pivot.position;
            _positionPivotVel = Vector3.zero;
            
            TargetPivotRotation = pivot.rotation;
            _rotationPivot = pivot.rotation;
        }
        
        public void Initialize(int cardIndex) {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetInt(CardIndex, cardIndex);
            GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);

            TargetPivotPosition = transform.position;
            _positionPivot = TargetPivotPosition;

            TargetPivotRotation = transform.rotation;
            _rotationPivot = TargetPivotRotation;

            Tally = PointCalculator.IndexToPoint(cardIndex);
        }
        
        public void PlayDrawSound(float pitch = 1) {
            _drawSound.pitch = 1;
            _drawSound.Play();
        }

        public void PlayBatchDrawSound(int timesPlayed) {
            _drawSound.pitch = Mathf.Min(1 + pitchStep * timesPlayed, maxPitch);
            _drawSound.Play();
        }

        private void Awake() {
            _drawSound = GetComponent<AudioSource>();
        }

        private void Update() {
            _positionPivot = Vector3.SmoothDamp(
                _positionPivot, 
                TargetPivotPosition, 
                ref _positionPivotVel, 
                pivotMoveDamping
            );
            
            _rotationPivot = Quaternion.RotateTowards(
                _rotationPivot, 
                TargetPivotRotation, 
                pivotRotationMaxDelta
            );
            
            StepEffects();

            transform.SetPositionAndRotation(
                _positionPivot + _transformOffset.position,
                _rotationPivot * Quaternion.Euler(_transformOffset.rotation)
            );
            
            transform.localScale = Vector3.one + _transformOffset.scale;
        }

        private void StepEffects() {
            _expiredEffects.Clear();
            _transformOffset = new EffectTransform();
            
            foreach ((string key, RunningEffect value) in _activeEffects) {
                if (value.HasElapsed) {
                    if (value.ExpireWhenDone) {
                        _expiredEffects.Add(key);
                        continue;
                    }
                    else {
                        _transformOffset += value.GetFinalTransform();
                    }
                }
                else {
                    _transformOffset += value.EvaluateCurrentTransform();
                }
            }
            
            foreach (string expiredEffect in _expiredEffects) {
                _activeEffects.Remove(expiredEffect);
            }
        }

        private RunningEffect CreateNewEffect(TransformEffect effect, bool playReverse) {
            return new RunningEffect(
                effect, 
                Time.time, 
                Time.time + effectPlaySpeed, 
                playReverse
            );
        }
    }
}