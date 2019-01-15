using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class PerlinNoise
{
    public const int OCTAVE_COUNT = 7;

    private System.Random random = new System.Random();

    public float[][] GeneratePerlinNoise(int width, int height)
    {
        float[][] baseNoise = this.generateWhiteNoise(width, height);

        return this.GeneratePerlinNoise(baseNoise);
    }

    public float[][] GeneratePerlinNoise(float[][] baseNoise)
    {
        int width = baseNoise.Length;
        int height = baseNoise[0].Length;

        float[][][] smoothNoise = new float[PerlinNoise.OCTAVE_COUNT][][];

        float persistence = 0.5f;

        for (int i = 0; i < PerlinNoise.OCTAVE_COUNT; i++) 
        {
            smoothNoise[i] = this.generateSmoothNoise(baseNoise, i);
        }

        float[][] perlinNoise = this.getEmptyArray(width, height);

        float amplitude = 1.0f;

        float totalAmplitude = 0.0f;

        for (int octave = PerlinNoise.OCTAVE_COUNT - 1; octave >= 0; octave--)
        {
            amplitude *= persistence;

            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                }
            }
        }

        return perlinNoise;
    }

    private float[][] generateSmoothNoise(float[][] baseNoise, int octave)
    {
        int width = baseNoise.Length;
        int height = baseNoise[0].Length;

        float[][] smoothNoise = this.getEmptyArray(width, height);

        int samplePeriod = 1 << octave;

        float sampleFrequency = 1.0f / samplePeriod;

        for (int i = 0; i < width; i++)
        {
            int sample_i0 = (i / samplePeriod) * samplePeriod;

            int sample_i1 = (sample_i0 + samplePeriod) % width;

            float horizontalBlend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; j++)
            {
                int sample_j0 = (j / samplePeriod) * samplePeriod;

                int sample_j1 = (sample_j0 + samplePeriod) % height;

                float verticalBlend = (j - sample_j0) * sampleFrequency;

                float top = this.interpolate(baseNoise[sample_i0][sample_j0], 
                                             baseNoise[sample_i1][sample_j0], 
                                             horizontalBlend);

                float bottom = this.interpolate(baseNoise[sample_i0][sample_j1],
                                                baseNoise[sample_i1][sample_j1],
                                                horizontalBlend);

                smoothNoise[i][j] = this.interpolate(top, bottom, verticalBlend);
            }
        }

        return smoothNoise;
    }

    private float[][] generateWhiteNoise(int width, int height)
    {
        float[][] whiteNoise = this.getEmptyArray(width, height);

        for (int i = 0; i < width; i++) 
        {
            for (int j = 0; j < height; j++) 
            {
                whiteNoise[i][j] = (float)this.random.NextDouble() % 1;
            }
        }

        return whiteNoise;
    }

    private float interpolate (float x, float y, float alpha)
    {
        return x * (1 - alpha) + alpha * y;
    }

    private float[][] getEmptyArray (int width, int height)
    {
        float[][] array = new float[width][];

        for (int i = 0; i < width; i++) 
        {
            array[i] = new float[height];
        }

        return array;
    }
}
