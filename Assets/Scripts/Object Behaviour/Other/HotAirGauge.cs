using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HOT AIR GAUGE
 * Handles rotating the needle of gauge when hot air is created and released 
*/
public class HotAirGauge : MonoBehaviour
{
    private GameObject needle;

    private float rotIncrement = 0.8f; 

    // Start is called before the first frame update
    void Start()
    {
        needle = GameObject.Find("Needle"); 
    }

    // rotate 
    public void RotateNeedlePositive()
    {
        needle.transform.Rotate(Vector3.back * rotIncrement);
    }

    public void RotateNeedleNegative()
    {
        needle.transform.Rotate(Vector3.forward * rotIncrement);
    }
}
