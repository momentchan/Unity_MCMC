using mj.gist.math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MCMC {
    public class Metropolis3DSample : MonoBehaviour {
        [SerializeField] private int lEdge = 30;
        [SerializeField] private int nInitial = 100; // burn-in
        [SerializeField] private int nLimit = 100;
        [SerializeField] private int loop = 400;
        [SerializeField] private float threshold = -100f;
        [SerializeField] private GameObject prefab;

        private Vector4[] data;
        private Metropolis3D metropolis;

        void Start() {
            data = GenerateDistribution();

            metropolis = new Metropolis3D(data, lEdge * Vector3.one);
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
                yield return new WaitForSeconds(0.1f);

                foreach(var pos in metropolis.Chain(nInitial, nLimit, threshold)) {
                    Instantiate(prefab, pos, Quaternion.identity);
                }
            }
        }
    }
}