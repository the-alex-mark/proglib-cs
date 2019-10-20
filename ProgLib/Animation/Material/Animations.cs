using System;

namespace ProgLib.Animation.Material
{
    enum AnimationType
    {
        Linear,
        EaseInOut,
        EaseOut,
        CustomQuadratic
    }

    static class AnimationLinear
    {
        public static Double CalculateProgress(Double progress)
        {
            return progress;
        }
    }

    static class AnimationEaseInOut
    {
        public static Double PI = Math.PI;
        public static Double PI_HALF = Math.PI / 2;

        public static Double CalculateProgress(Double progress)
        {
            return EaseInOut(progress);
        }

        private static Double EaseInOut(Double s)
        {
            return s - Math.Sin(s * 2 * PI) / (2 * PI);
        }
    }

    public static class AnimationEaseOut
    {
        public static Double CalculateProgress(Double progress)
        {
            return -1 * progress * (progress - 2);
        }
    }

    public static class AnimationCustomQuadratic
    {
        public static Double CalculateProgress(Double progress)
        {
            var kickoff = 0.6;
            return 1 - Math.Cos((Math.Max(progress, kickoff) - kickoff) * Math.PI / (2 - (2 * kickoff)));
        }
    }
}
