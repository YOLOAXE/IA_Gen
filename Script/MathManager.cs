using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Question
{
    public float[] entrer = new float[50];
    public float[] Reponce = new float[50];
}

public class MathManager : MonoBehaviour
{
    [SerializeField] private int Generation = 0;
    [SerializeField] private float TauxMutation = 0.1f;
    [SerializeField] private Question[] LesQuestions = new Question[1];
    [SerializeField] private float[,,,] HidenNeuronePoids = new float[123, 10, 50, 50];
    [SerializeField] private float[,,] HidenNeuronePerceptron = new float[123, 10, 50];
    [SerializeField] private float[,,] MeilleurePoids = new float[10, 50, 50];
    [SerializeField] private float[,] MeilleurePerceptron = new float[10, 50];
    [SerializeField] private float ScoreZeroMeilleur = 3000f;
    [SerializeField] private float Mult = 100f;
    [SerializeField] private float[] ScoreZero = new float[123];
    [SerializeField] private TextMeshProUGUI TxtGen = null;
    [SerializeField] private TextMeshProUGUI TauxDeReussite = null;
    [SerializeField] private TextMeshProUGUI[] textEnterValue = null;
    [SerializeField] private TextMeshProUGUI[] textSortieValue = null;
    [SerializeField] private InputField NumQuestion = null;
    [SerializeField] private InputField Set = null;
    [SerializeField] private InputField[] EnterQuestion = null;
    [SerializeField] private InputField[] reponceQuestion = null;
    public int Ne = 50;// neurone par couche
    public int NBNe = 100;// nombre d'IA
    public int NBC = 10;// nombre de couche
    public bool mode = true;

    private int i = 0, j = 0, k = 0, s = 0, de = 0,a = 0,b = 0,c = 0;
    private float result = 0;
    private int tchange = 0;

    void Start()
    {
        HidenNeuronePoids = new float[NBNe, NBC, Ne, Ne];
        HidenNeuronePerceptron = new float[NBNe, NBC, Ne];
        MeilleurePoids = new float[NBC, Ne, Ne];
        MeilleurePerceptron = new float[NBC, Ne];
        ScoreZero = new float[NBNe];
        for (s = 0; s < NBNe; s++)
        {
            RandomizeAllPoids(s);
        }
    }

    void FixedUpdate()
    {
        if (mode)
        {
            for (j = 0; j < NBNe; j++)
            {
                ScoreZero[j] = 0;
            }
            for (i = 0; i < LesQuestions.Length; i++)
            {
                for (j = 0; j < NBNe; j++)
                {
                    for (k = 0; k < Ne; k++)
                    {
                        HidenNeuronePerceptron[j, 0 ,k] = LesQuestions[i].entrer[k];
                    }
                    Propagation(j);
                    for (k = 0; k < Ne; k++)
                    {
                         ScoreZero[j] += Mathf.Abs(HidenNeuronePerceptron[j, NBC - 1, k] - (LesQuestions[i].Reponce[k] / Mult));
                    }
                }
            }
            for (j = 0, s = 0, result = ScoreZero[0]; j < NBNe; j++)
            {
                if (ScoreZero[j] < result)
                {
                    result = ScoreZero[j];/////trouver le meilleur
                    s = j;
                }
            }
            TauxDeReussite.text = "Taux Zero: " + ScoreZero[s].ToString();
            if (ScoreZeroMeilleur > ScoreZero[s])
            {
                for (i = 0; i < NBC - 1; i++)
                {
                    for (j = 0; j < Ne; j++)
                    {
                        for (k = 0; k < Ne; k++)
                        {
                            MeilleurePoids[i, j, k] = HidenNeuronePoids[s, i, j, k];//attribution des poids
                        }
                    }
                }
                ScoreZeroMeilleur = ScoreZero[s];
                tchange = 0;
            }
            for (de = 0; de < NBNe; de++)
            {
                for (i = 0; i < NBC - 1; i++)
                {
                    for (j = 0; j < Ne; j++)
                    {
                        for (k = 0; k < Ne; k++)
                        {
                            HidenNeuronePoids[de, i, j, k] = MeilleurePoids[i, j, k] + Random.Range(-TauxMutation, TauxMutation);
                        }
                    }
                }
            }
            Generation++;
            tchange++;
            TxtGen.text = "Generation: " + Generation.ToString();
            if(tchange > 20)
            {
                TauxMutation = TauxMutation * 0.95f;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mode = !mode;
            for (k = 0; k < Ne; k++)
            {
                MeilleurePerceptron[0, k] = LesQuestions[0].entrer[k];
                if (k < textEnterValue.Length)
                {
                    textEnterValue[k].text = MeilleurePerceptron[0, k].ToString();
                }
            }
            PropagationMeilleur();
            for (k = 0; k < Ne; k++)
            {
                if (k < textSortieValue.Length)
                {
                    textSortieValue[k].text = (MeilleurePerceptron[NBC - 1, k] * Mult).ToString("F5");
                }
            }
        }
    }

    void Propagation(int num)
    {
        for (a = 0; a < NBC - 1; a++)
        {
            for (b = 0; b < Ne; b++)
            {
                result = 0;
                for (c = 0; c < Ne; c++)
                {
                    result += HidenNeuronePoids[num, a, b, c] * HidenNeuronePerceptron[num, a, c];
                }
                HidenNeuronePerceptron[num, a + 1, b] = (1 / (1 + Mathf.Exp(result)));
            }
        }
    }

    void PropagationMeilleur()
    {
        for (a = 0; a < NBC - 1; a++)
        {
            for (b = 0; b < Ne; b++)
            {
                result = 0;
                for (c = 0; c < Ne; c++)
                {
                    result += MeilleurePoids[a, b, c] * MeilleurePerceptron[a, c];
                }
                MeilleurePerceptron[a + 1, b] = (1 / (1 + Mathf.Exp(result)));
            }
        }
    }

    void RandomizeAllPoids(int num)
    {
        for (i = 0; i < NBC - 1; i++)
        {
            for (j = 0; j < Ne; j++)
            {
                for (k = 0; k < Ne; k++)
                {
                    HidenNeuronePoids[num, i, j, k] = Random.Range(-1.0f, 1.0f);
                }
            }
        }
    }
}