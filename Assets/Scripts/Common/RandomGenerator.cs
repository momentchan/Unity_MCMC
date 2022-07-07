using UnityEngine;

namespace mj.gist.math {
    public static class RandomGenerator {
        // exponential distribution
        public static float ExponentialDistribution(float lambda) {
            return -Mathf.Log(Random.value) / lambda;
        }

        // gaussian distribution
        public static float RandGaussian(float mu, float sigma) {
            float z = Mathf.Sqrt(-2.0f * Mathf.Log(Random.value)) * Mathf.Sin(2.0f * Mathf.PI * Random.value);
            return mu + sigma * z;
        }

        public static Vector2 RandGaussian() {
            var u1 = Random.value;
            var u2 = Random.value;

            var sqrtLuU1 = Mathf.Sqrt(-2.0f * Mathf.Log(u1));

            return new Vector2(Mathf.Cos(2.0f * Mathf.PI * u2), Mathf.Sin(2.0f * Mathf.PI * u2)) * sqrtLuU1;
        }

        // cauchy distribution
        public static float RandCauchy(float mu, float gamma) {
            return mu + gamma * Mathf.Tan(Mathf.PI * (Random.value - 0.5f));
        }

        // chi-squared distribution
        public static float Randchi(int k) {
            float w = 0;

            for (int i = 0; i < k; i++) {
                float z = Mathf.Sqrt(-2.0f * Mathf.Log(Random.value)) * Mathf.Sin(2.0f * Mathf.PI * Random.value);
                w += z * z;
            }

            return w;
        }

        // lognormal distribution
        public static float RandLNormal(float mu, float sigma) {
            float z = mu + sigma * Mathf.Sqrt(-2.0f * Mathf.Log(Random.value)) * Mathf.Sin(2.0f * Mathf.PI * Random.value);
            return Mathf.Exp(z);
        }

        // prepared random distribution
        public static float RandPrepared(float[] arr) {
            int segments = arr.Length - 1;
            float x0 = 1.0f / segments;
            float r = Random.value;
            int i = 0;
            while (arr[i] < r) {
                i++;
                if (i > arr.Length) break;
            }
            return (r - arr[i - 1]) / (arr[i] - arr[i - 1]) * x0 + (i - 1) * x0;
        }
    }
}