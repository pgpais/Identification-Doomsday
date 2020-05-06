using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] houses, insideBackgrounds, spawnPoints;
    public int[][] houseStats;
    public int houseInteriorNum = 4;
    public static GameController inst;
    public GameObject canvas;
    public Animator canvasAnim;
    public bool isInHouse = false;
    public int timeLimit = 150;
    public Image timer;

    private bool gameEnded = false;
    private bool win = false;
    private float time;
    [SerializeField]
    private GameObject[] spawnedHouses;
    private int[] houseNumber;
    private int houseNum, interiorNum;
    [SerializeField]
    private int selectedBackground = -1; // position in spawned houses that belongs to player (can be -1)

    void Awake()
    {
        DontDestroyOnLoad(canvas);
        DontDestroyOnLoad(gameObject);
        if (inst != null)
        {
            Destroy(gameObject);
        }
        else
        {
            inst = this;
        }

        

        spawnedHouses = new GameObject[spawnPoints.Length];
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    }

    // Start is called before the first frame update
    void Start()
    {
                     // N,  F,  A,  J
        int[][] temp = {
            new int[] {00, 75, 25, 00}, //LGBT
            new int[] {00, 20, 80, 00}, //CatWoman
            new int[] {00, 00, 80, 20}, //Junkey
            new int[] {00, 80, 10, 10}, //Princess
            new int[] {30, 00, 70, 00}, //Gamer
            new int[] {20, 00, 60, 20}, //Messy
            new int[] {00, 00, 30, 70}, //Skater
            new int[] {10, 00, 80, 10}, //Racist
            new int[] {00, 30, 70, 00}, //Rich
            new int[] {00, 00, 20, 80}, //Medieval
            new int[] {20, 00, 50, 30}, //Party
            new int[] {80, 20, 00, 00}, //Nerd
            new int[] {00, 00, 00, 00}  //Scientist
        };
        houseStats = temp;

        time = timeLimit;
        

        houseNumber = new int[spawnPoints.Length];
        canvasAnim = canvas.GetComponentInChildren<Animator>();

       

        //for (int i = 0; i < spawnPoints.Length; i++)
        //{
        //    GameObject house = Instantiate(houses[Random.Range(0, houses.Length)], spawnPoints[i].transform.position, Quaternion.identity);
        //    spawnedHouses[i] = house;
        //    houseNumber[i] = house.GetComponent<House>().houseNum;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timer.fillAmount = time / (float)timeLimit;

        if(time <= 0 && !win)
        {
            win = true;
            if (SceneManager.GetActiveScene().buildIndex == 2 && GetPlayerHouseRatio() == 1)
            {
                canvasAnim.SetTrigger("FadeIn");
                //Close door
                StartCoroutine(WinScreen());
            }
            else
            {
                GetComponent<SoundtrackFMOD>().soundtrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GetComponent<SoundtrackFMOD>().soundtrack.release();
                SceneManager.LoadScene(3);
                Destroy(gameObject);
                Destroy(canvas);
                foreach (GameObject house in spawnedHouses)
                {
                    Destroy(house);
                }
            }
        }

        if (isInHouse)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                StartCoroutine(ExitHouse());
            }
        }
        if (gameEnded)
        {
            if (Input.anyKeyDown)
                Application.Quit();
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (canvasAnim != null)
            canvasAnim.SetTrigger("FadeOut");
        int level = scene.buildIndex;
        if (level == 1)
        {
            Canvas[] tooManyCanvases = FindObjectsOfType<Canvas>();
            foreach (Canvas can in tooManyCanvases)
            {
                Debug.Log(canvas.name);
                if (can.gameObject != canvas.gameObject)
                    Destroy(can.gameObject);
            }
            Debug.Log("Scene 0 was loaded");
            isInHouse = false;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnedHouses[i] == null)
                {
                    GameObject house = Instantiate(houses[Random.Range(0, houses.Length)], spawnPoints[i].transform.position, Quaternion.identity, gameObject.transform);
                    spawnedHouses[i] = house;
                }
                else
                {
                    spawnedHouses[i].SetActive(true);
                }
            }
            if (selectedBackground == -1)
            {
                int selectedHouse = Random.Range(0, spawnPoints.Length - 1);
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    House h = spawnedHouses[selectedHouse].GetComponent<House>();
                    Debug.Log("Select House with name " + h.gameObject.name);
                    if (h.houseNum == 4)
                    {
                        selectedHouse = (selectedHouse + 1) % spawnPoints.Length;
                    }
                    else
                    {
                        selectedBackground = h.houseNum * 4 + h.houseInteriorNum;
                        break;
                    }
                }
                if(selectedBackground > 12)
                {
                    selectedBackground = 12;
                }
            }
        }
        else if (level == 2)
        {
            foreach(GameObject h in spawnedHouses)
            {
                h.SetActive(false);
            }
            Debug.Log("Scene 1 was loaded");
            Instantiate(insideBackgrounds[houseNum * 4 + interiorNum], Vector3.zero, Quaternion.identity);
            if(GetPlayerHouseRatio() == 1)
            {
                Debug.Log("You win");
            }
        }
    }

    public float GetPlayerHouseRatio()
    {
        float result = 0;
        //foreach(int attribute in houseStats[selectedBackground])
        //{
        //    result += attribute;
        //}
        for (int i = 0; i < houseStats[selectedBackground].Length; i++)
        {
            int background = Mathf.Clamp(houseNum * 4 + interiorNum, 0, 12);
            Debug.Log(houseNum * 4 + interiorNum);
            Debug.Log(i);
            if (houseStats[background][i] == 0)
                continue;
            else
                result += ((float)houseStats[selectedBackground][i]/100) / ((float)houseStats[background][i]/100);
        }
        result = result / 2;
        return (Mathf.Abs(result - 1) * -1) + 1;
    }

    public void LoadHouseInfo(int houseNum, int interiorNum)
    {
        this.houseNum = houseNum;
        this.interiorNum = interiorNum;
    }

    public IEnumerator ExitHouse()
    {
        isInHouse = false;
        canvasAnim.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    public void EnterHouse()
    {
        isInHouse = true;
    }

    public IEnumerator WinScreen()
    {
        Destroy(timer);
        GameObject door = GameObject.Find("WinDoor");
        yield return new WaitForSecondsRealtime(1);

        canvasAnim.SetTrigger("FadeOut");
        //Start door animation
        door.GetComponent<Animation>().Play();
		FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/doorClose", transform.position);
        gameEnded = true;
    }
}