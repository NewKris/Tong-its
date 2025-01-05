using System.Collections;
using System.Collections.Generic;

namespace NordicBibo.Runtime.Common {
    public class CoroutineSequenceBuilder {
        private readonly List<IEnumerator> _coroutines = new List<IEnumerator>();
        
        public IEnumerator Build() {
            foreach (IEnumerator enumerator in _coroutines) {
                yield return enumerator;
            }
        }

        public CoroutineSequenceBuilder AddCoroutine(IEnumerator coroutine) {
            _coroutines.Add(coroutine);
            return this;
        }
    }
}