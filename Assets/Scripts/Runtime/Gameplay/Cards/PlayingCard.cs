using System;
using System.Collections.Generic;
using NordicBibo.Runtime.Gameplay.CardEffect;
using NordicBibo.Runtime.Gameplay.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NordicBibo.Runtime.Gameplay.Cards {
    public class PlayingCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
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
        private bool _hovered;
        private EffectTransform _transformOffset;
        private Vector3 _positionVel;
        private Vector3 _scaleVel;
        private AudioSource _drawSound;
        
        private readonly Dictionary<string, RunningEffect> _activeEffects = new Dictionary<string, RunningEffect>();
        private readonly List<string> _expiredEffects = new List<string>(3);

        public bool PlayEffects { get; set; }
        public int Index { get; private set; }
        public int Tally { get; private set; }
        public CardStack ParentStack { get; set; }
        public Transform Pivot { get; set; }

        public void MoveCardToStack(CardStack toStack, int audioPlays = 0) {
            ResetSelection();
            ParentStack.RemoveCard(this);
            toStack.AddCard(this);
            PlayBatchDrawSound(audioPlays);
        }

        public GameObject PopPivot() {
            GameObject pivotObject = Pivot.gameObject;
            Pivot = transform.root;
            
            return pivotObject;
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
        
        public void ToggleSelected() {
            _selected = !_selected;
            
            if (_selected) {
                OnCardSelected?.Invoke(this);
            }
            else {
                OnCardDeSelected?.Invoke(this);
            }
            
            _activeEffects[SELECT_KEY] = CreateNewEffect(selectEffect, !_selected);

            PlayDrawSound(_selected ? 1.2f : 0.8f);
        }
        
        public void Initialize(int cardIndex) {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetInt(CardIndex, cardIndex);
            GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
            
            Index = cardIndex;
            Tally = PointCalculator.IndexToPoint(cardIndex);
            PlayEffects = true;
        }
        
        private void Awake() {
            _drawSound = GetComponent<AudioSource>();
        }

        private void Update() {
            StepEffects();
            
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                CreateTargetPosition(), 
                ref _positionVel, 
                pivotMoveDamping
            );
            
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                CreateTargetRotation(),
                pivotRotationMaxDelta
            );

            transform.localScale = Vector3.SmoothDamp(
                transform.localScale,
                CreateTargetScale(),
                ref _scaleVel,
                pivotMoveDamping
            );
        }

        private Vector3 CreateTargetScale() {
            if (PlayEffects) return Pivot.parent.localScale;
            
            return Pivot.parent.localScale + _transformOffset.scale;
        }

        private Quaternion CreateTargetRotation() {
            if (!PlayEffects) return Pivot.rotation;

            return Pivot.rotation * Quaternion.Euler(_transformOffset.rotation);
        }

        private Vector3 CreateTargetPosition() {
            // Only x- and y-axis position offset will be local while z offset will be world
            // This prevents cards that are rotated 180d on the y-axis from having the opposite z-offset

            if (!PlayEffects) {
                return Pivot.position;
            }
            
            Vector3 targetPos = Pivot.position + Pivot.TransformDirection(new Vector3(
                _transformOffset.position.x,
                _transformOffset.position.y,
                0
            ));
            
            targetPos.z += _transformOffset.position.z;

            return targetPos;
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
    }
}