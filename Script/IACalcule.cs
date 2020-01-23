using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuronesCoucheD
{
    public float[] Perceptron = new float[50];
    public float[,] Poids = new float[50, 50];
}

public class IACalcule : MonoBehaviour
{
    [SerializeField] private NeuronesCoucheD[] HidenNeurone = new NeuronesCoucheD[10];
    [SerializeField] private float TauxDapprentissage = 0.1f;
    public const int Ne = 50;

    private int i = 0, j = 0, k = 0;
    private float result = 0;

    void Start()
    {
        for (i = 0; i < HidenNeurone.Length; i++)
        {
            HidenNeurone[i].Perceptron = new float[Ne];
            HidenNeurone[i].Poids = new float[Ne, Ne];
          //  HidenNeurone[i].Delta = new float[Ne];
        }
    }

    public void RandomizeAllPoids()
    {
        for (i = 0; i < HidenNeurone.Length - 1; i++)
        {
            for (j = 0; j < HidenNeurone[i].Perceptron.Length; j++)
            {
                for (k = 0; k < HidenNeurone[i].Perceptron.Length; k++)
                {
                    HidenNeurone[i].Poids[j, k] = Random.Range(-1.0f, 1.0f);
                }
            }
        }
    }

    public void Propagation()
    {
        for (i = 0; i < HidenNeurone.Length - 1; i++)
        {
            for (j = 0; j < Ne; j++)
            {
                result = 0;
                for (k = 0; k < Ne; k++)
                {
                    result += HidenNeurone[i].Poids[k, j] * HidenNeurone[i].Perceptron[k];
                }
                HidenNeurone[i + 1].Perceptron[j] = 1 / (1 + Mathf.Exp(result));
            }
        }
    }
}