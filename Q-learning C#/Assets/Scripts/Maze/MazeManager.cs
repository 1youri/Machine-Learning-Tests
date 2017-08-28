using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public Transform WorldParent;
    public Transform Player;
    public GameObject WallBlock;
    public Transform winTiletf;
    public Transform DebugTile;
    public MazeAI AI;
    //public GameObject GroundTile;


    [HideInInspector] public double Score = 1;
    public double WalkingCost = 0.04;
    public Vector2 MazeSize;
    public Vector2 WinTile;
    public Vector2 PlayerStartTile;
    public List<Vector2> wallList;
    private List<Vector2> allWalls;
    public Vector2 playerPos;
    public bool Restart;
    [HideInInspector] public int turns = 0;
    [HideInInspector] public int games = 0;

    public List<string> actions = new List<string>()
    {
        "left",
        "right",
        "up",
        "down"
    };

    void Awake()
    {
        allWalls = new List<Vector2>(wallList);
    }
    // Use this for initialization
    void Start()
    {
        StartMaze();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            DebugTile.position = new Vector3(Mathf.RoundToInt(hit.point.x),1.5f, Mathf.RoundToInt(hit.point.z));


            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.CompareTag("Wall"))
                {
                    Vector2 hitVec = new Vector2(Mathf.RoundToInt(hit.transform.position.x), Mathf.RoundToInt(hit.transform.position.z));
                    Destroy(hit.transform.gameObject);
                    wallList.Remove(hitVec);
                }
                else if (hit.transform.CompareTag("Ground"))
                {
                    Vector2 hitVec = new Vector2(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                    if (wallList.Contains(hitVec))
                    {
                        wallList.Remove(hitVec);

                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(hitVec.x, 0, hitVec.y), 0.2f);
                        int i = 0;
                        while (i < hitColliders.Length)
                        {
                            if (hitColliders[i].CompareTag("Wall"))
                                Destroy(hitColliders[i].gameObject);
                            i++;
                        }
                    }
                    else
                    {
                        SpawnWall(Mathf.RoundToInt(hitVec.x), Mathf.RoundToInt(hitVec.y));
                        wallList.Add(hitVec);
                    }

                }
            }
        }
    }

    public void StartMaze()
    {
        for (int x = -1; x < MazeSize.x + 2; x++)
        {
            for (int y = -1; y < MazeSize.y + 2; y++)
            {
                if (((x == -1 || x == MazeSize.x + 1) ||
                     (y == -1 || y == MazeSize.y + 1)))
                    allWalls.Add(new Vector2(x, y));
                if (allWalls.Contains(new Vector2(x, y)))
                {
                    SpawnWall(x, y);
                }
            }
        }

        winTiletf.position = new Vector3(WinTile.x, .1f, WinTile.y);
        playerPos = PlayerStartTile;
        Player.position = new Vector3(playerPos.x, 0, playerPos.y);
    }

    private void SpawnWall(int x, int y)
    {
        GameObject newObject = Instantiate(WallBlock, WorldParent);
        newObject.transform.position = new Vector3(x, 0, y);
    }

    public void ResetMaze()
    {
        Score = 1;
        turns = 0;
        playerPos = Vector2.zero;
        Player.position = Vector3.zero;
        Restart = false;
    }

    public void DoAction(string action)
    {
        turns++;
        //Debug.Log("Action: " + action);
        if (Restart) ResetMaze();

        if (!actions.Contains(action))
        {
            Debug.Log("Invalid Input: " + action);
            return;
        }
        Score -= WalkingCost;
        Vector2 startPoint = playerPos;
        if (action == "left")
        {
            Vector2 newPos = playerPos + new Vector2(-1, 0);
            if (!allWalls.Contains(newPos))
            {
                playerPos = newPos;
            }
        }
        if (action == "right")
        {
            Vector2 newPos = playerPos + new Vector2(1, 0);
            if (!allWalls.Contains(newPos))
            {
                playerPos = newPos;
            }
        }
        if (action == "up")
        {
            Vector2 newPos = playerPos + new Vector2(0, 1);
            if (!allWalls.Contains(newPos))
            {
                playerPos = newPos;
            }
        }
        if (action == "down")
        {
            Vector2 newPos = playerPos + new Vector2(0, -1);
            if (!allWalls.Contains(newPos))
            {
                playerPos = newPos;
            }
        }
        if (startPoint == playerPos)
            Score -= WalkingCost;

        Player.transform.position = new Vector3(playerPos.x, 0, playerPos.y);

        if (playerPos == WinTile)
        {
            Score += 10;
            Restart = true;
            Debug.Log("["+AI.RunTime +"] Win! Score: " + Score + " Turns: " + turns);
            games++;
        }
    }


    List<Vector3> recentHits = new List<Vector3>();
    void OnDrawGizmos()
    {
        while (recentHits.Count > 2)
        {
            recentHits.RemoveAt(0);
        }
        foreach (var recentHit in recentHits)
        {
            Gizmos.DrawSphere(recentHit, 0.2f);
        }
    }
}
