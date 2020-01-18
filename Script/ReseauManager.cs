using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class BettyReseau
{
    public float[,,,] Poids = new float[50,5,50,50];
}

public class ReseauManager : MonoBehaviour
{
    [SerializeField] private GameObject IA = null;
    [SerializeField] private int Generation = 0;
    [SerializeField] private int nbIA = 100; //constante
    [SerializeField] private float tempsGeneration = 15f;
    [SerializeField] private float TauxMutation = 0.1f;
    [SerializeField] private GameObject SpawnPoint = null;
    [SerializeField] private List<GameObject> IAR = new List<GameObject>();
    [SerializeField] private float[,,,] BRPoids = new float[400,5,50,50];
    [SerializeField] private TextMeshProUGUI TxtGen = null;
    [SerializeField] private GameObject Objectif = null;

    private List<float> Dista = new List<float>();
    private float Distance = 0;
    private GameObject helpDestroy = null;
    private int i = 0,j = 0,k = 0,s = 0,de = 0;

    void Start()
    {
        Objectif = GameObject.FindWithTag("Drapeau");
        StartCoroutine(PhaseGeneration());
    }

    IEnumerator PhaseGeneration()
    {     
        for(i = 0;i < nbIA;i++)
        {
            IAR.Add(Instantiate(IA,SpawnPoint.transform.position,Quaternion.identity));
            IAR[i].name = "Betty_" + Generation.ToString() + "_" + i.ToString();
            IAR[i].GetComponent<Reseau>().RandomizeAllPoids();
        }
        for(i = 0;i < nbIA;i++)
        {
            IAR[i].GetComponent<Reseau>().activation = true;
        }
        yield return new WaitForSeconds(tempsGeneration);
        Generation++;
        TxtGen.text = "Generation: " + Generation.ToString();
        /////////////////////////////////////////////////////////////////////
        while(true)
        {
            for(i = 0; i < IAR.Count; i++)
            {
                Dista.Add(Vector3.Distance(IAR[i].transform.position,Objectif.transform.position));// get position
            }
            for(i = 0;i < Mathf.RoundToInt(nbIA*0.8f);i++)
            {
                Distance = 0;
                for(j = 0; j < IAR.Count;j++)
                {
                    if(Dista[j] > Distance)
                    {
                        de = j;
                        Distance = Dista[j];
                    }
                }
                Dista.Remove(Dista[de]);
                helpDestroy = IAR[de];
                IAR.Remove(helpDestroy); 
                Destroy(helpDestroy); 
            }
            for(i = 0;i < IAR.Count;i++)
            {
                for(j = 0;j < IAR[i].GetComponent<Reseau>().HidenNeurone.Length;j++)
                {
                    for (k = 0; k < 50; k++)
                    {
                        for(s = 0; s < 50; s++)
                        {
                            BRPoids[i,j,k,s] = IAR[i].GetComponent<Reseau>().HidenNeurone[j].Poids[k,s];
                        }// take poids
                    }
                }
                Destroy(IAR[i]);
            }
            IAR.Clear();
            Dista.Clear();
            for(i = 0;i < nbIA;i++)
            {
                IAR.Add(Instantiate(IA,SpawnPoint.transform.position,Quaternion.identity));
                IAR[i].name = "Betty_" + Generation.ToString() + "_" + i.ToString();
                for(j = 0;j < IAR[i].GetComponent<Reseau>().HidenNeurone.Length;j++)
                {
                    for (k = 0; k < 50; k++)
                    {
                        for(s = 0; s < 50; s++)
                        {
                            IAR[i].GetComponent<Reseau>().HidenNeurone[j].Poids[k,s] = (((BRPoids[Random.Range(0,Mathf.RoundToInt(nbIA*0.2f)),j,k,s] + BRPoids[i%Mathf.RoundToInt(nbIA*0.2f),j,k,s])/2) + Random.Range(-TauxMutation,TauxMutation));
                        }// distribut poids
                    }
                }
            }
            for(i = 0;i < nbIA;i++)
            {
                IAR[i].GetComponent<Reseau>().activation = true;
            }
            yield return new WaitForSeconds(tempsGeneration);
            Generation++;
            TxtGen.text = "Generation: " + Generation.ToString();
        }
    }
}
