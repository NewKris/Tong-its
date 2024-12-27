using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NordicBibo.Runtime.Utility {
    public class SequenceBuilder {
        private float _actionPadding = 0;
        private readonly List<object> _sequence = new List<object>();

        public IEnumerator Build() {
            WaitForSeconds padding = new WaitForSeconds(_actionPadding);
            
            foreach (object step in _sequence) {
                switch (step) {
                    case IEnumerator coroutine:
                        yield return coroutine;
                        break;
                    case Action action:
                        action();
                        break;
                    case WaitForSeconds wait:
                        yield return wait;
                        break;
                }

                if (_actionPadding != 0) {
                    yield return padding;
                }
            }
        }

        public SequenceBuilder SetActionPadding(float padding) {
            _actionPadding = padding;
            return this;
        }
        
        public SequenceBuilder Wait(WaitForSeconds wait) {
            _sequence.Add(wait);
            return this;
        }
        
        public SequenceBuilder RunAsync(IEnumerator coroutine) {
            _sequence.Add(coroutine);
            return this;
        }

        public SequenceBuilder Run(Action callback) {
            _sequence.Add(callback);
            return this;
        }
    }
}