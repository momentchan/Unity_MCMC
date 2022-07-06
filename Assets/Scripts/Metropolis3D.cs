using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj.gist.math {
    public class Metropolis3D {
        public static readonly int LimitResetLoopCount = 100;
        public static readonly int WeightReferenceLoopCount = 500;

        public Vector3 Scale { get; private set; }
        public Vector4[] Data { get; private set; }

        private float currDensity = 0f;
        private Vector3 curr;

        public Metropolis3D(Vector4[] data, Vector3 scale) {
            this.Data = data;
            this.Scale = scale;
        }

        public IEnumerable<Vector3> Chain(int nInitialize, int nLimit, float threshold) {
            Reset();

            // Burn-in
            for (var i = 0; i < nInitialize; i++)
                Next(threshold);

            for (var i = 0; i < nLimit; i++) {
                yield return curr;
                Next(threshold);
            }
        }

        private void Reset() {
            for (var i = 0; currDensity <= 0f && i < LimitResetLoopCount; i++) {
                curr = Vector3.Scale(Scale, new Vector3(Random.value, Random.value, Random.value));
                currDensity = ComputeDensity(curr);
            }
        }

        private float ComputeDensity(Vector3 pos) {
            var weight = 0f;

            for (var i = 0; i < WeightReferenceLoopCount; i++) {
                var id = Mathf.FloorToInt(Random.value * (Data.Length - 1));
                Vector3 posi = Data[id];
                var mag = Vector3.SqrMagnitude(posi - pos);
                weight += Mathf.Exp(-mag) * Data[id].w;
            }
            return weight;
        }

        private void Next(float threshold) {
            var next = curr + GaussianDistribution.GenerateRandomPointStandard();

            var nextDensity = ComputeDensity(next);
            var f1 = nextDensity > threshold;
            var f2 = currDensity <= 0f || Mathf.Min(1f, nextDensity / currDensity) >= Random.value;
            if (f1 && f2) {
                curr = next;
                currDensity = nextDensity;
            }
        }
    }
}