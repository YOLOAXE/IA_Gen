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
    [SerializeField] private RaycastHit[] hit = new RaycastHit[5];
    [SerializeField] private GameObject Objectif = null;
    private Vector3[] VD = {Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back};

    private Rigidbody rb = null;
    private int iDist =  0;
    private int i = 0, j = 0, k = 0;
    private float result = 0;
    public bool activation = false;

    void Start()
    {
        for (i = 0; i < HidenNeurone.Length; i++)
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
        for(i = 0; i < HidenNeurone.Length-1;i++)
        {
            for(j = 0; j < 50;j++)
            {
                result = 0;
                for(k = 0; k < 50;k++)
                {
                   result += HidenNeurone[i].Poids[k,j] * HidenNeurone[i].Perceptron[k];  
                }
                HidenNeurone[i+1].Perceptron[j] = 1/(1+Mathf.Exp(result));// logistique
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
        HidenNeurone[0].Perceptron[6] = transform.rotation.x;
        HidenNeurone[0].Perceptron[7] = transform.rotation.y;
        HidenNeurone[0].Perceptron[8] = transform.rotation.z;
        HidenNeurone[0].Perceptron[9] = transform.rotation.w;
        HidenNeurone[0].Perceptron[10] = transform.position.x;
        HidenNeurone[0].Perceptron[11] = transform.position.y;
        HidenNeurone[0].Perceptron[12] = transform.position.z;
        HidenNeurone[0].Perceptron[13] = Objectif.transform.position.x;
        HidenNeurone[0].Perceptron[14] = Objectif.transform.position.y;
        HidenNeurone[0].Perceptron[15] = Objectif.transform.position.z;
        for(i = 1;i < 5;i++)
        {
            HidenNeurone[0].Perceptron[(16 + ((i-2) * 3))] = hit[i].normal.x;
            HidenNeurone[0].Perceptron[(17 + ((i - 2) * 3))] = hit[i].normal.y;
            HidenNeurone[0].Perceptron[(18 + ((i - 2) * 3))] = hit[i].normal.z;
        }
    }

    void AplyValue()
    {
        rb.AddForce(transform.forward * (400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[0]) * Time.deltaTime);
        rb.AddForce(transform.forward * (-400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[1]) * Time.deltaTime);
        rb.AddForce(transform.right * (400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[2]) * Time.deltaTime);
        rb.AddForce(transform.right * (-400 * HidenNeurone[HidenNeurone.Length-1].Perceptron[3]) * Time.deltaTime);
        transform.eulerAngles = new Vector3(0f,HidenNeurone[HidenNeurone.Length-1].Perceptron[4] * 180,0f);
    }
}
