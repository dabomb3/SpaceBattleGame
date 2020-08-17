/**/ /*** BEGIN TileBehavior ***/ /**/
/**/
/*
Class TileBehavior

NAME

        class TileBehavior - Holds Tile model info

SYNOPSIS

        xPos        --> the x coordinate of the tile, int
        yPod        --> the y coordinate of the tile, int
        shot        --> whether the tile has been shot at, bool
        sprite      --> SpriteRenderer of the Tile, SpriteRenderer
        tileIcons   --> Array of tile icons that show the tile state, Sprite[]

DESCRIPTION

        The TileBehavior class holds the visual behavior of the Tile object.
        Provides functioality for showing the state of the icon.
AUTHOR

        Thomas Piacentini

DATE

        6/10/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public int xPos;
    public int zPos;
    public bool shot;
    public SpriteRenderer sprite;
    public Sprite[] tileIcons;
    //[0]-Empty, [1]-Aim, [2]-Miss, [3]-Hit 

    /**/
    /*
    TileBehavior::ActivateIcon(int index, bool inShot)

    NAME

            Tile::ActivateIcon - Changes the appearance of the tile based on index of sprite array and whether its been shot

    SYNOPSIS

           index    --> index of sprite to be activated, int
           inShot   --> whether the tile has been shot at, bool

    DESCRIPTION

            ActivateIcon() is called when icons are hovered over with the mouse and when shots are made.
            The appearance of the tile changes based on the index

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    public void ActivateIcon(int index, bool inShot)
    {
        if(index == 2)
        {
            sprite.color = Color.red;
        }
        sprite.sprite = tileIcons[index];

        shot = inShot;
    }/*public void ActivateIcon(int index, bool inShot);*/

    /**/
    /*
    TileBehavior::SetTileBehavior(int x, int y)

    NAME

            Tile::SetTileBehavior - sets xPos and yPos of Tile

    SYNOPSIS

           x    --> x coordinate
           y    --> y coordinate

    DESCRIPTION

            SetTileBeahvior() just sets the xPos and yPos of the tile.
            Used by PlayerBoardGenerator.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    public void SetTileBehavior(int x, int y)
    {
        xPos = x;
        zPos = y;
    }/*public void SetTileBehavior(int x, int y);*/

    /**/
    /*
    TileBehavior::OnMouseOver()

    NAME

            Tile::OnMouseOver - Called when the mouse travels over the tile

    SYNOPSIS

           

    DESCRIPTION

            If the game is in the FIRE state, the aim icon shows up if it has not been shot at
            If the tile is clicked, the TakeShot() function in the GameManager is called

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    void OnMouseOver()
    {
        if(GameManager.currentInstance.currentState == GameManager.State.FIRE)
        {
            if(!shot)
            {
                ActivateIcon(1, false);
            }
            if(Input.GetMouseButtonDown(0))
            {
                GameManager.currentInstance.TakeShot(xPos, zPos, this);
            }
        }
        
    }/*void OnMouseOver()*/

    /**/
    /*
    TileBehavior::OnMouseExit()

    NAME

            Tile::OnMouseExit - Called when the mouse leaves the tile

    SYNOPSIS

           

    DESCRIPTION

            If the tile has not been shot at, the icon is set back to empty

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    void OnMouseExit()
    {
        if(!shot)
        {
            ActivateIcon(0, false);
        }
        
    }/*void OnMouseExit()*/
}
/**/ /*** END TileBehavior ***/ /**/