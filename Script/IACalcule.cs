using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuronesCoucheD
{
    public float[] Perceptron = new float[10];
    public float[,] Poids = new float[10, 10];
    public float[] Delta = new float[10];
}

[System.Serializable]
public class Question
{
    public float[] entrer = new float[10];
    public float[] Reponce = new float[10];
}

public class IACalcule : MonoBehaviour
{
    [SerializeField] private NeuronesCoucheD[] HidenNeurone = new NeuronesCoucheD[10];
    [SerializeField] private Question[] QestValide;
    [SerializeField] private Question[] Qest;
    [SerializeField] private float TauxDapprentissage = 0.1f;
    [SerializeField] private float Divisseur = 100;
    public const int Ne = 10;

    private int i = 0, j = 0, k = 0;
    private int indice = 0;
    private float result = 0;

    void Start()
    {
        for (i = 0; i < HidenNeurone.Length; i++)
        {
            HidenNeurone[i].Perceptron = new float[Ne];
            HidenNeurone[i].Poids = new float[Ne, Ne];
            HidenNeurone[i].Delta = new float[Ne];
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GetValueExo();
            Propagation();
            RetroPropagation();
            MiseAjour();
            AplyValue();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            indice = Random.Range(0, QestValide.Length);
            Propagation();
            Debug.Log(QestValide[indice].entrer[0]);
            Debug.Log(QestValide[indice].entrer[1]);
            Debug.Log(QestValide[indice].Reponce[0]);
            Debug.Log(HidenNeurone[HidenNeurone.Length - 1].Perceptron[0] * Divisseur);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomizeAllPoids();
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

    void Propagation()
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
                HidenNeurone[i + 1].Perceptron[j] = 1 / (1 + Mathf.Exp(result));// logistique
            }
        }
    }

    void RetroPropagation()
    {
        for(i = 0; i < Ne; i++)
        {
            HidenNeurone[HidenNeurone.Length - 1].Delta[i] = (QestValide[indice].Reponce[i] / Divisseur) - HidenNeurone[HidenNeurone.Length - 1].Perceptron[i];
        }
        for (i = HidenNeurone.Length - 2; i >= 0; i--)
        {
            for (j = 0; j < Ne; j++)
            {
                result = 0;
                for (k = 0; k < Ne; k++)
                {
                    result += HidenNeurone[i].Poids[k, j] * HidenNeurone[i + 1].Delta[j];
                }
                HidenNeurone[i].Delta[j] = result * (HidenNeurone[i].Perceptron[j] * (1 - HidenNeurone[i].Perceptron[j]));
            }
        }
        indice++;
        if(indice == QestValide.Length)
        {
            indice = 0;
        }
    }

    void MiseAjour()
    {
        for (i = 0; i < HidenNeurone.Length - 1; i++)
        {
            for (j = 0; j < Ne; j++)
            {
                HidenNeurone[i].Poids[j, i] = HidenNeurone[i].Poids[j, i] + TauxDapprentissage * HidenNeurone[i].Perceptron[j] + HidenNeurone[i + 1].Delta[j];
            }
        }
    }

    void GetValueExo()
    {
        for (i = 0; i < Ne; i++)
        {
            HidenNeurone[0].Perceptron[i] = QestValide[indice].entrer[i] / Divisseur;
        }
    }

    void AplyValue()
    {

    }
}