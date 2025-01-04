using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Graphics {
    public readonly struct ScreenConfig {
        public readonly Vector2Int resolution;
        public readonly FullScreenMode fullScreenMode;
        
        public ScreenConfig(Vector2Int resolution, FullScreenMode fullScreenMode) {
            this.resolution = resolution;
            this.fullScreenMode = fullScreenMode;
        }
    }
}