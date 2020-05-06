    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
    public IEnumerator OpenDoor(House house)
    {
        //Play Animation
        GameController.inst.LoadHouseInfo(house.houseNum, house.houseInteriorNum);
        
        house.vCam.Priority = 11;
        Animator houseAnim = house.GetComponentInChildren<Animator>();
        houseAnim.SetTrigger("openDoor");
        yield return new WaitForSecondsRealtime(1.5f);

       
        

		//FMOD door sound
		FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/door", transform.position);
		
        GameController.inst.canvasAnim.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(0.7f);
        SceneManager.LoadScene(2);
        GameController.inst.isInHouse = true;
    }
}
