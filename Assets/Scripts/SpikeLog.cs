using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLog : MonoBehaviour
{
    private List<Color> colorList = new List<Color>();
    void Start()
    {
        foreach (var materials in gameObject.GetComponentInChildren<MeshRenderer>().materials)
        {
            colorList.Add(materials.color);

            if(materials.name == "Spike (Instance)")
            {
                materials.color = Color.black;
            }
            else if(materials.name == "Cylinder (Instance)")
            {
                materials.color = Color.red;
            }
           
        }
    }

    void Update()
    {
        

    }
}
