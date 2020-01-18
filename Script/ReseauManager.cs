using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class BettyReseau
{
    public float[,,,] Poids = new float[50,6,25,25];
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
    [SerializeField] private float[,,,] BRPoids = new float[50,6,25,25];
    [SerializeField] private TextMeshProUGUI TxtGen = null;

    private float[] Dista = new float[100]; //Constante
    private GameObject Objectif = null;
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
            for(i = 0; i < Dista.Length; i++)
            {
                Dista[i] = Vector3.Distance(IAR[i].transform.position,Objectif.transform.position);// get position
            }
            for(i = 0;i < 50;i++)
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
                helpDestroy = IAR[de];
                IAR.Remove(helpDestroy); 
                Destroy(helpDestroy); 
            }

            for(i = 0;i < IAR.Count;i++)
            {
                for(j = 0;j < IAR[i].GetComponent<Reseau>().HidenNeurone.Length;j++)
                {
                    for (k = 0; k < 25; k++)
                    {
                        for(s = 0; s < 25; s++)
                        {
                            BRPoids[i,j,k,s] = IAR[i].GetComponent<Reseau>().HidenNeurone[j].Poids[k,s];
                        }
                    }
                }
                Destroy(IAR[i]);// take Info
            }
            IAR.Clear();
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
        }
    }
}
