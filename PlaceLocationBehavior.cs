/**/ /*** BEGIN PlaceLocationBehavior ***/ /**/
/**/
/*
Class PlaceLocationBehavior

NAME

        class PlaceLocationBehaviorr - Helps the Placement Client to determine whether a tile is occupied during placing

SYNOPSIS

        placedLayer --> the layer that the ships are being placed on, LayerMask
        occupied    --> the RaycastHit object that checks if the tile is there, RacastHit
        info        --> the information provided by the tile, TileBehavior
        board       --> the board on which the ships are being placed, PlayerBoardGenerator

DESCRIPTION

        This class provides the Placement Client with the functionality to check if a ship has been placed on a specific tile.
        A RaycastHit object is used to check the specified tile to see if a ship is already on that tile, and to also check if there
        is a tile in that location.
        The tile outline ghosts models are PlaceLocationBehavior objects, and are what check the location of where the user is hovering while placing ships
AUTHOR

        Thomas Piacentini

DATE

        6/16/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceLocationBehavior : MonoBehaviour
{
    public LayerMask placedLayer;
    RaycastHit occupied;
    TileBehavior info;
    PlayerBoardGenerator board;

    /**/
    /*
    PlaceLocationBehavior::GetTileBehavior()

    NAME

            PlaceLocationBehavior::GetTileBehavior - Shoots a ray down onto a tile and returns that specific tile's TileBehavior.

    SYNOPSIS

            r   --> the ray being shot down onto the board at the given point, Ray

    DESCRIPTION

            GetTileBehavior() shoots a ray down on the point being hovered over by the user when placing a ship.
            The RaycastHit object occupied grabs the TileBehavior component from the tile which it collides with
            Returns null if there is no tile under the ghost

    RETURNS

            TileBehavior

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public TileBehavior GetTileBehavior()
    {
        Ray r = new Ray(transform.position, -transform.up);
        if(Physics.Raycast(r, out occupied, 0.9f, placedLayer))
        {
            Debug.DrawRay(r.origin, r.direction, Color.red);
            return occupied.collider.GetComponent<TileBehavior>();
        }
        return null;
    }/*public TileBehavior GetTileBehavior();*/

    /**/
    /*
    PlaceLocationBehavior::SetBoard(PlayerBoardGenerator brd)

    NAME

            PlaceLocationBehavior::SetBoard - Takes in a PlayerBoardGenerator object and sets it to the board object.

    SYNOPSIS

            brd   --> the board which corresponds to the current player who is putting down their ships, PlayerBoardGenerator

    DESCRIPTION

            SetBoard() sets the board to the correct player's board to be checking against in this instance. 

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public void SetBoard(PlayerBoardGenerator brd)
    {
        board = brd;
    }/*public void SetBoard(PlayerBoardGenerator brd);*/

    /**/
    /*
    PlaceLocationBehavior::IsOccupied()

    NAME

            PlaceLocationBehavior::IsOccupied - Checks if a ship is already placed in the location being hovered over

    SYNOPSIS

            

    DESCRIPTION

            IsOccupied() calls GetTileBehavior() to check if there is a tile below the ghost and checks if there is a ship there
            by requesting the GameManager to report if there is a ship at that location.

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public bool IsOccupied()
    {
        info = GetTileBehavior();
        if(info != null && !GameManager.currentInstance.CheckOccupation(info.xPos, info.zPos))
        {
            return true;
        }
        info = null;
        return false;
    }/*public bool IsOccupied();*/

}
/**/ /*** END PlaceLocationBehavior ***/ /**/