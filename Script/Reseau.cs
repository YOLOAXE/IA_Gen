using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuronesCouche
{
    public float[] Perceptron = new float[50];
    public float[,] Poids = new float[50,50];
}

public class Reseau : MonoBehaviour
{
    public NeuronesCouche[] HidenNeurone = new NeuronesCouche[5];
    [SerializeField] private RaycastHit[] hit = new RaycastHit[6];
    [SerializeField] private GameObject Objectif = null;
    private Vector3[] VD = {Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back};

    private Rigidbody rb = null;
    private int iDist =  0;

    public bool activation = false;

    void Start()
    {
        for (int i = 0; i < HidenNeurone.Length; i++)
        {
            HidenNeurone[i].Perceptron = new float[50];
        }
        Objectif = GameObject.FindWithTag("Drapeau");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(activation)
        {
            GetValue();
            Propagation();
        }
    }

    void FixedUpdate()
    {
        if(activation)
        {
            AplyValue();
        }
    }


    public void RandomizeAllPoids()
    {
        int i = 0,j = 0,k = 0;
        for(i = 0; i < HidenNeurone.Length-1;i++)
        {
            for(j = 0; j < HidenNeurone[i].Perceptron.Length;j++)
            {
                for(k = 0; k < HidenNeurone[i].Perceptron.Length;k++)
                {
                    HidenNeurone[i].Poids[j,k] = Random.Range(-1.0f,1.0f);
                }
            }
        }
    }

    void Propagation()
    {
        int i = 0,j = 0,k = 0;
        float result = 0;
        for(i = 0; i < HidenNeurone.Length-1;i++)
        {
            for(j = 0; j < 50;j++)
            {
                result = 0;
                for(k = 0; k < 50;k++)
                {
                   result += HidenNeurone[i].Poids[k,j] * HidenNeurone[i].Perceptron[k];  
                }
                HidenNeurone[i+1].Perceptron[j] = CalculeLogistique(result);
            }
        }
    }
    
    void GetValue()
    {
        for(iDist = 0; iDist < hit.Length;iDist++)
        {
           Physics.Raycast(transform.position, transform.TransformDirection(VD[iDist]), out hit[iDist]);
           HidenNeurone[0].Perceptron[iDist] = hit[iDist].distance;
        }
        HidenNeurone[0].Perceptron[6] = transform.eulerAngles.y/180;
        HidenNeurone[0].Perceptron[7] = transform.position.x;
        HidenNeurone[0].Perceptron[8] = transform.position.y;
        HidenNeurone[0].Perceptron[9] = transform.position.z;
        HidenNeurone[0].Perceptron[10] = Objectif.transform.position.x;
        HidenNeurone[0].Perceptron[11] = Objectif.transform.position.y;
        HidenNeurone[0].Perceptron[12] = Objectif.transform.position.z;
    }

    void AplyValue()
    {
        rb.AddForce(transform.forward * (400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[0]) * Time.deltaTime);
        rb.AddForce(transform.forward * (-400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[1]) * Time.deltaTime);
        rb.AddForce(transform.right * (400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[2]) * Time.deltaTime);
        rb.AddForce(transform.right * (-400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[3]) * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,HidenNeurone[HidenNeurone.Length-1].Perceptron[4] * 180,transform.eulerAngles.z);
    }

    float CalculeLogistique(float result)
    {
        return 1/(1+Mathf.Exp(result));
    }
}
