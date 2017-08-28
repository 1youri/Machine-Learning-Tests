using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public MazeManager maze;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.UpArrow))
	        maze.DoAction("up");
        if (Input.GetKeyDown(KeyCode.DownArrow))
            maze.DoAction("down");
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            maze.DoAction("left");
        if (Input.GetKeyDown(KeyCode.RightArrow))
            maze.DoAction("right");
    }
}
