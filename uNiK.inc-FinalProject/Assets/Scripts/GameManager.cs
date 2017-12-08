using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using unikincTanks;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public int playerCount;
    public int stageIndex;
    public Dictionary<Teams, int> teamInfos;
    public Dictionary<int, string> stages;      // Unused

    private int firstStageIndex;

	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            instance = this;
        }

        teamInfos = new Dictionary<Teams, int>();
        stages = new Dictionary<int, string>();
        StoreStages();
        firstStageIndex = 2;
        stageIndex = firstStageIndex;

        playerCount = 0;
        teamInfos[Teams.RED] = 0;
        teamInfos[Teams.BLUE] = 0;
        teamInfos[Teams.GREEN] = 0;
        teamInfos[Teams.YELLOW] = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void StoreStages()
    {
        for (int x = 0; x < SceneManager.sceneCountInBuildSettings; x++)
        {
            stages[x] = SceneManager.GetSceneByBuildIndex(x).name;
            Debug.Log(x + " loading " + stages[x]);
        }
    }

    public void RedTeamPlayerCount(int value)
    {
        teamInfos[Teams.RED] = value;
    }

    public void BlueTeamPlayerCount(int value)
    {
        teamInfos[Teams.BLUE] = value;
    }

    public void GreenTeamPlayerCount(int value)
    {
        teamInfos[Teams.GREEN] = value;
    }

    public void YellowTeamPlayerCount(int value)
    {
        teamInfos[Teams.YELLOW] = value;
    }

    public void NextStage()
    {
        if (stageIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
        {
            stageIndex = firstStageIndex;
        }
        else
        {
            stageIndex++;
        }

    }

    public void PreviousStage()
    {
        if (stageIndex - 1 < firstStageIndex)
        {
            stageIndex = SceneManager.sceneCountInBuildSettings - 1;
        }
        else
        {
            stageIndex--;
        }
    }

    public void Play()
    {
        playerCount = teamInfos[Teams.RED] + teamInfos[Teams.BLUE] + teamInfos[Teams.GREEN] + teamInfos[Teams.YELLOW];
        SceneManager.LoadScene("_Main_v2");         // Only 1 stage at the moment
    }
}
