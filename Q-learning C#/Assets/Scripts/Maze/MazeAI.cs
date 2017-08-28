using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.QLearning;
using UnityEngine;

public class MazeAI : MonoBehaviour
{

    public MazeManager maze;
    public Learner ai;
    public float waitTime;
    private float lastWaitTime;
    public WaitForSeconds wait;


    [Header("AI Stats")]
    public double gamma = 0;
    public double alpha = 0;
    public int GameCount = 0;
    public int Step = 0;
    public double Score = 0;
    public string RunTime = "";

    

	void Awake () {
        wait = new WaitForSeconds(waitTime);
        ai = new Learner(maze.actions,maze.PlayerStartTile.ToString(), maxIter:20000, gammaDecay:0.99, discount:0.5);
    }

	void Start ()
	{
	    StartCoroutine(AICoroutine());
        Application.runInBackground = true;
    }

    void Update()
    {
        gamma = ai.gamma;
        alpha = ai.alpha;
        GameCount = maze.games;
        Score = maze.Score;
        Step = maze.turns;

        int totSeconds = (int)UnityEngine.Time.timeSinceLevelLoad;
        int hours = Mathf.FloorToInt(totSeconds / 3600);
        int minutes = Mathf.FloorToInt((totSeconds - hours * 3600)/60);
        int seconds = Mathf.FloorToInt(totSeconds - hours * 3600 - minutes * 60);

        RunTime = hours > 0 ? hours + ":" : "" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    IEnumerator AICoroutine()
    {
        while (true)
        {
            if(maze.Restart) ai.EndEpisode();

            string decision = ai.MakeDecision();
            double reward = maze.Score;

            maze.DoAction(decision);

            reward = maze.Score - reward;
            if (ai.Learn(decision, maze.playerPos.ToString(), reward))
                maze.ResetMaze();


            if (lastWaitTime != waitTime)
            {
                lastWaitTime = waitTime;
                wait = new WaitForSeconds(waitTime);
            }
            yield return wait;
        }
    }
}
