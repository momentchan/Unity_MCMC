using mj.gist.math;
using mj.gist.sampling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MCMC {
    public class MCMC3DExample : MonoBehaviour {
        [SerializeField] private int lEdge = 30;
        [SerializeField] private int nInitials = 100; // burn-in
        [SerializeField] private int nSamples = 100;
        [SerializeField] private int loop = 400;
        [SerializeField] private float threshold = -100f;
        [SerializeField] private GameObject prefab;

        [SerializeField] private float sleepDuration = 0.1f;

        private Vector4[] data;
        private MCMC3DSampler mcmc;

        void Start() {
            data = GenerateDistribution();

            mcmc = new MCMC3DSampler(data, lEdge * Vector3.one, threshold);
            StartCoroutine(StartChaining());
        }


        private Vector4[] GenerateDistribution() {
            var sn = new SimplexNoiseGenerator();
            var data = new List<Vector4>();

            for (var x = 0; x < lEdge; x++)
                for (var y = 0; y < lEdge; y++)
                    for (var z = 0; z < lEdge; z++) {
                        var n = sn.noise(x, y, z);
                        data.Add(new Vector4(x, y, z, n));
                    }
            return data.ToArray();
        }

        private IEnumerator StartChaining() {
            for(var i = 0; i < loop; i++) {
                yield return new WaitForSeconds(sleepDuration);

                foreach(var pos in mcmc.Chain(nInitials, nSamples)) {
                    var go = Instantiate(prefab, pos, Quaternion.identity);
                    go.transform.parent = transform;
                }
            }
        }
    }
}