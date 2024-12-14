using System;
using System.Collections;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay {
    public class GameplayManager : MonoBehaviour {
        public bool playOnStart;
        public TongItsEngine engine;

        private void Start() {
            if (playOnStart) {
                engine.StartNewGame();
            }
        }
    }
}
