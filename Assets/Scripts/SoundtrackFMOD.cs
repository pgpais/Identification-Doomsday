using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackFMOD : MonoBehaviour
{
	
	//FMOD Event Emitter
	[FMODUnity.EventRef]
    public string soundtrackEvent;
	public FMOD.Studio.EventInstance soundtrack;
	
    void Start()
    {
        //FMOD Event Emitter
		soundtrack = FMODUnity.RuntimeManager.CreateInstance(soundtrackEvent);
        soundtrack.start();
		
		soundtrack.setParameterValue("inorout", 1);
		soundtrack.setParameterValue("time", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
