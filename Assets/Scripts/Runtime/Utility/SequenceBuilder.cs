using System;
using System.Collections;
using System.Collections.Generic;

namespace NordicBibo.Runtime.Utility {
    public class SequenceBuilder {
        private readonly List<object> _sequence = new List<object>();

        public IEnumerator Build() {
            foreach (object step in _sequence) {
                switch (step) {
                    case IEnumerator coroutine:
                        yield return coroutine;
                        break;
                    case Action action:
                        action();
                        break;
                }
            }
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