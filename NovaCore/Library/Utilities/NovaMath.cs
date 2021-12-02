namespace NovaCore.Utilities
{
    public static class NovaMath
    {
        public static int WrappedClamp(int n, int min, int max)
        {
            if (n > max) return 0;
            return n < min ? max : n;
        }

        public static float AngleDifference(float angle1, float angle2)
        {
            float diff = (angle2 - angle1 + 180) % 360 - 180;
            return diff < -180 ? diff + 360 : diff;
        }
        
        public static float Diff(float a, float b)
        {
            return System.Math.Abs(a - b);
        }
        
        public static bool Between(float value, float lowerBound, float upperBound, bool inclusive = false)
        {
            return inclusive ? value >= lowerBound && value <= upperBound : value > lowerBound && value < upperBound;
        }
        
        public static bool Between(float value, float bounds, bool inclusive = false)
        {
            return inclusive ? value >= -bounds && value <= bounds : value > -bounds && value < bounds;
        }
    }
}