namespace NordicBibo.Runtime.Gameplay.Utility {
    public static class CardMatrix {
        private const int COLUMN_COUNT = 13;
        private const int ROW_COUNT = 4;
        
        public static int GetRow(int index) {
            return index / COLUMN_COUNT;
        }

        public static int GetColumn(int index) {
            return index % COLUMN_COUNT;
        }
    }
}