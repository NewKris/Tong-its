using System;
using System.Collections;
using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.CardEffect;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NordicBibo.Runtime.Gameplay {
    public class PlayingCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
        public static event Action<PlayingCard> OnCardSelected;
        public static event Action<PlayingCard> OnCardDeSelected; 
        
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

        public int Index { get; private set; }
        public int Tally { get; private set; }
        public CardStack ParentStack { get; set; }
        
        private Vector3 TargetPivotPosition { get; set; }
        private Quaternion TargetPivotRotation { get; set; }

        public void MoveCardToStack(CardStack toStack, int audioPlays = 0) {
            ResetSelection();
            ParentStack.RemoveCard(this);
            toStack.AddCard(this);
            PlayBatchDrawSound(audioPlays);
        }
        
        public void SetInteractable(bool interactable) {
            GetComponent<Collider>().enabled = interactable;
            ResetSelection();
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            _activeEffects[HOVER_KEY] = CreateNewEffect(hoverEffect, false);
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            _activeEffects[HOVER_KEY] = CreateNewEffect(hoverEffect, true);
        }
        
        public void OnPointerDown(PointerEventData eventData) {
            ToggleSelection();
            
            _activeEffects[SELECT_KEY] = CreateNewEffect(selectEffect, !_selected);

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

            Index = cardIndex;
            Tally = PointCalculator.IndexToPoint(cardIndex);
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
        
        private void PlayDrawSound(float pitch = 1) {
            _drawSound.pitch = pitch;
            _drawSound.Play();
        }

        private void PlayBatchDrawSound(int timesPlayed) {
            _drawSound.pitch = Mathf.Min(1 + pitchStep * timesPlayed, maxPitch);
            _drawSound.Play();
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
        
        private void ResetSelection() {
            _selected = false;
            DisableAllEffects();
        }

        private void DisableAllEffects() {
            if (_activeEffects.ContainsKey(HOVER_KEY) && !_activeEffects[HOVER_KEY].ExpireWhenDone) {
                _activeEffects[HOVER_KEY] = CreateNewEffect(hoverEffect, true);
            }

            if (_activeEffects.ContainsKey(SELECT_KEY) && !_activeEffects[SELECT_KEY].ExpireWhenDone) {
                _activeEffects[SELECT_KEY] = CreateNewEffect(selectEffect, true);
            }
        }

        private void ToggleSelection() {
            _selected = !_selected;
            
            if (_selected) {
                OnCardSelected?.Invoke(this);
            }
            else {
                OnCardDeSelected?.Invoke(this);
            }
        }
    }
}