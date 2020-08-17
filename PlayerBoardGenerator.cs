/**/ /* BEGIN PlayerBoardGenerator */ /**/
/**/
/*
Class PlayerBoardGenerator

NAME

        class PlayerBoardGenerator - Holds the functionality for the player boards

SYNOPSIS

        fill        --> Used in Editor to create board, bool 
        tilePrefab  --> the tile model that makes up the board, GameObject
        tileList    --> list that holds all of the tiles, List<GameObject>();
        infoList    --> list that holds all of the tile behaviors, List<TileBehavior>();

DESCRIPTION

        The PlayerBoardGenerator class is the board class.
        Creates the player's boards, defines the behaviors of the boards
AUTHOR

        Thomas Piacentini

DATE

        6/16/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardGenerator : MonoBehaviour
{
    public bool fill;
    public GameObject tilePrefab;
    List<GameObject> tileList = new List<GameObject>();
    List<TileBehavior> infoList = new List<TileBehavior>();

    /**/
    /*
    PlayerBoardGenerator::Start()

    NAME

            PlayerBoardGenerator::Start - Houses the code to be run at the start of the game.

    SYNOPSIS

            

    DESCRIPTION

            Start() is called at the beginning of the game and takes the already created boards and adds them to the tileList and infoList.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void Start()
    {
        tileList.Clear();
        infoList.Clear();
        foreach(Transform t in transform)
        {
            if(t != transform)
            {
                tileList.Add(t.gameObject);
            }
        }
        foreach(GameObject obj in tileList)
        {
            infoList.Add(obj.GetComponent<TileBehavior>());
        }
    }

    /**/
    /*
    PlayerBoardGenerator::CheckTile(TileBehavior info)

    NAME

            PlayerBoardGenerator::CheckTile - Checks if this board contains the tile that is passed in

    SYNOPSIS

            info    --> the TileBehavior object that is being looked for

    DESCRIPTION

            The list of TileBehaviors is checked to see if it contains the same object that is being passed to it.
            If infoList does contain the passed in TileBehavior, return true.
            Used to check if the tile being clicked on belongs to the player clicking on it.

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public bool CheckTile(TileBehavior info)
    {
        if(infoList.Contains(info))
        {
            return true;
        }
        return false;
    }/*public bool CheckTile(TileBehavior info);*/

    /**/
    /*
    PlayerBoardGenerator::GetTile(int x,int y)

    NAME

            PlayerBoardGenerator::GetTile - Returns the tile at given x,y position of player board

    SYNOPSIS

            x   --> the x coordinate of the tile being requested, int
            y   --> the y coordinate of the tile being requested, int

    DESCRIPTION

            GetTile() searches through the tiles on the board for the tile with the coordinates being requested.
            Returns tile if foound.

    RETURNS

            TileBehavior

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public TileBehavior GetTile(int x, int y)
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            if(infoList[i].xPos == x && infoList[i].zPos == y)
            {
                return infoList[i];
            }
        }
        return null;
    }/*public TileBehavior GetTile(int x, int y);*/

    /**/
    /*
    PlayerBoardGenerator::OnDrawGizmos()

    NAME

            PlayerBoardGenerator::OnDrawGizmos - Creates the visual board

    SYNOPSIS

            

    DESCRIPTION

            OnDrawGizmos is a built in Unity function that is implemnted when an object is to always be created.
            This implementation just creates a 10x10 board when called in the editor.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void OnDrawGizmos()
    {
        if(tilePrefab != null && fill)
        {
            //Clear out any tiles already made
            for(int i = 0; i < tileList.Count; i++)
            {
                DestroyImmediate(tileList[i]);
            }
            tileList.Clear();
            
            //create new tiles
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    Vector3 pos = new Vector3(transform.position.x + i,0,transform.position.z + j);
                    GameObject t = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                    t.GetComponent<TileBehavior>().SetTileBehavior(i, j);
                    tileList.Add(t);
                }
            }
        }
    }/*void OnDrawGizmos()*/
}
/**/ /* END PlayerBoardGenerator */ /**/