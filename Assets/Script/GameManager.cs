using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private UnityBoard board;
    private LogicManager logicManager;

    // Start is called before the first frame update
    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        board = GetComponent<UnityBoard>();
        logicManager = GetComponent<LogicManager>();
        logicManager.Board = board;
        InitGame();
    }

    void InitGame()
    {
        logicManager.InitializeTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
