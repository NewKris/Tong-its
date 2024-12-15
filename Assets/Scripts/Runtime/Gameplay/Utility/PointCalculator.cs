using System;

namespace NordicBibo.Runtime.Gameplay.Utility {
    public static class PointCalculator {
        private const int COLUMN_COUNT = 13;
        private const int ROW_COUNT = 4;
        private const int CARD_COUNT = COLUMN_COUNT * ROW_COUNT;
        
        public static int IndexToPoint(int i) {
            if (i >= CARD_COUNT) {
                return 0;
            }
            
            int column = i % COLUMN_COUNT;
            return Math.Min(column + 1, 10);
        }
    }
}