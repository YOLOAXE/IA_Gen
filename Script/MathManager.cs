using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Question
{
    public float[] entrer = new float[50];
    public float[] Reponce = new float[50];
}

public class MathManager : MonoBehaviour
{
    [SerializeField] private GameObject IA = null;
    [SerializeField] private int Generation = 0;
    [SerializeField] private int nbIA = 100; //constante
    [SerializeField] private int nbIASelectionne = 2; //constante
    [SerializeField] private float TauxMutation = 0.1f;
    [SerializeField] private GameObject SpawnPoint = null;
    [SerializeField] private List<GameObject> IAR = new List<GameObject>();
    [SerializeField] private List<GameObject> IAM = new List<GameObject>();
    [SerializeField] private List<float> IAF = new List<float>();
    [SerializeField] private float[,,,] BRPoids = new float[400, 5, 50, 50];
    [SerializeField] private TextMeshProUGUI TxtGen = null;
    public const int Ne = 50;

    private int Indice = 0;
    private int i = 0, j = 0, k = 0, s = 0, de = 0;

    void Start()
    {
        for (i = 0; i < nbIA; i++)
        {
            IAR.Add(Instantiate(IA, SpawnPoint.transform.position, Quaternion.identity));
            IAR[i].name = "Conni_" + i.ToString();
            IAR[i].GetComponent<IACalcule>().RandomizeAllPoids();
        }
        Generation++;
        TxtGen.text = "Generation: " + Generation.ToString();
    }

    void Update()
    {

    }

    void FindMeilleur()
    {
        for(i = 0;i < IAR.Count;i++)
        {
            //IAF.Add = IAR.GetComponent<IACalcule>().H
            //IAM.Add = IAR[i];
        }
    }
}