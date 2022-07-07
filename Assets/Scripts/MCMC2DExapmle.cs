using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MCMC {
    public class MCMC2DExapmle : MonoBehaviour {
        [SerializeField] private float stddev = 0.01f;
        [SerializeField] private int nInitials = 100; // burn-in
        [SerializeField] private int nSamples = 100;

        [SerializeField] private GameObject prefab;
        [SerializeField] private Texture2D texture;

        [SerializeField] private float sleepDuration = 0.1f;

        private MCMCTextureSampler mcmc;

        void Start() {
            mcmc = new MCMCTextureSampler(texture, stddev);
            StartCoroutine(StartChaining());
        }
        private IEnumerator StartChaining() {
            while (true) {
                if (sleepDuration <= 0f)
                    yield return null;
                else
                    yield return new WaitForSeconds(sleepDuration);

                foreach (var uv in mcmc.Chain(nInitials, nSamples)) {
                    var worldPos = Camera.main.ViewportToWorldPoint(uv);
                    worldPos.z = 0f;

                    Instantiate(prefab, worldPos, Quaternion.identity);
                }
            }
        }
    }
}