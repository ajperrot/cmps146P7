using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class LayeredNoise
    {
        public static float Noise(float x, float y, float amp, float freq, int octvs, float persistence)
        {
            float noiseVal = 0.0f;
            float maxVal = 0.0f;
            // Code from lecture 11
            for (int i = 0; i < octvs; ++i)
            {
                float pX = (x / 20.0f) * 3.0f * freq; // 20 * 0.15
                float pY = (y / 20.0f) * 3.0f * freq;

                noiseVal += Perlin.Noise(pX, 0.0f, pY);

                maxVal += amp;
                amp *= persistence;
                freq *= 2;
            }

            return (noiseVal / maxVal);
        }
    }
}