using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public static int NHouseInteriors = 4;

    public int houseNum;
    public int houseInteriorNum;
    public Cinemachine.CinemachineVirtualCamera vCam;

    // Start is called before the first frame update
    void Awake()
    {
        houseInteriorNum = Random.Range(0, NHouseInteriors - 1);
        vCam = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        vCam.Priority = 10;
    }
}
