/**/ /*** BEGIN Tile ***/ /**/
/**/
/*
Class Tile

NAME

        class Tile - Holds Tile basics

SYNOPSIS

        ShipType    --> the state of the tile, enum
        type        --> the ShipType of the tile, ShipType
        placedShip  --> the ship placed on the tile, ModelBehavior

DESCRIPTION

        The Tile class holds the basics of the Tile object, including what ship is placed on it.
        State of the tile refers to whether its empty, occupied by a specific type of ship, hit, or missed
AUTHOR

        Thomas Piacentini

DATE

        6/10/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipType
{
    EMPTY,
    CRUISER,
    DESTROYER,
    SUB,
    BS,
    CARRIER,
    HIT,
    MISS
}

public class Tile
{
    public ShipType type;
    public ModelBehavior placedShip;

    /**/
    /*
    Tile::Tile(ShipType sType, ModelBehavior ps)

    NAME

            Tile::Tile - Constructor for Tile class

    SYNOPSIS

            stype   --> the state of the tile being created, ShipType
            ps      --> the ship that is placed on the tile being created, ModelBehavior

    DESCRIPTION

            Initializes the member variables; type and placed ship

    RETURNS

            

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    public Tile(ShipType stype, ModelBehavior ps)
    {
        type = stype;
        placedShip = ps;
    }/*public Tile(ShipType stype, ModelBehavior ps);*/

    /**/
    /*
    Tile::Occupied()

    NAME

            Tile::Occupied - Reports whether the tile is occupied

    SYNOPSIS

           

    DESCRIPTION

            returns true if the type matches any of the ships

    RETURNS

        bool    

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    public bool Occupied()
    {
        if(type != ShipType.EMPTY && type != ShipType.HIT && type != ShipType.MISS)
        {
            return true;
        }
        return false;
    }/*public bool Occupied();*/
}
/**/ /*** END Tile ***/ /**/