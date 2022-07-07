using mj.gist.math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MCMC {
    public class MCMCTextureSampler {
        public static readonly int LIMIT_RESET_LOOP_COUNT = 100;
        public Texture2D Texture { get; private set; }
        public float StdDev { get; private set; }
        private float aspect => (float)Texture.width / Texture.height;

        private Vector2 stddevAspect => new Vector2(StdDev, StdDev/aspect);
            
        private Vector2 curr;
        private float currDensity;

        public MCMCTextureSampler(Texture2D texture, float stddev) {
            this.Texture = texture;
            this.StdDev = stddev;
        }

        public void Reset() {
            for (var i = 0; currDensity <= 0 && i < LIMIT_RESET_LOOP_COUNT; i++) {
                curr = new Vector2(Random.value, Random.value);
                currDensity = ComputeDensity(curr);
            }
        }

        public IEnumerable<Vector2> Chain(int nInitials, int nSamples) {
            Reset();

            for (var i = 0; i < nInitials; i++)
                Next();

            for(var i = 0; i < nSamples; i++) {
                yield return curr;
                Next();
            }
        }


        private void Next() {
            var next = curr + Vector2.Scale(stddevAspect, RandomGenerator.RandGaussian());
            next.x -= Mathf.Floor(next.x);
            next.y -= Mathf.Floor(next.y);

            var nextDensity = ComputeDensity(next);
            if (currDensity <= 0f || Mathf.Min(1f, nextDensity / currDensity) >= Random.value) {
                curr = next;
                currDensity = nextDensity;
            }
        }

        private float ComputeDensity(Vector2 curr) {
            return Texture.GetPixelBilinear(curr.x, curr.y).r;
        }
    }
}