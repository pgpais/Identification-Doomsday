using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODPChanger : MonoBehaviour
{
	
	private float playerRatio;
    // Start is called before the first frame update
    void Start()
    {
		GameController.inst.GetComponent<SoundtrackFMOD>().soundtrack.setParameterValue("inorout", 0);
		
		playerRatio = GameController.inst.GetPlayerHouseRatio();
		GameController.inst.GetComponent<SoundtrackFMOD>().soundtrack.setParameterValue("decor", playerRatio);
		Debug.Log("Player ratio: "+playerRatio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	
	void OnDestroy()
	{
		GameController.inst.GetComponent<SoundtrackFMOD>().soundtrack.setParameterValue("inorout", 1);
	}
}
