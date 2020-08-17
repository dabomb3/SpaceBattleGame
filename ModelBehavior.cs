/**/ /*** BEGIN ModelBehavior ***/ /**/
/**/
/*
Class ModelBehvior

NAME

        class ModelBehavior - Defines the behavior of the ships of the game

SYNOPSIS

        length  --> the length of the ship, int
        hitNum  --> the number of times the ship must be hit before it will sink, int
        sType   --> the type of ship, ShipType

DESCRIPTION

        This class defines the behavior of the ships of the game such as taking damage, reporting if it has been hit, and
        reporting if it has been destroyed.
AUTHOR

        Thomas Piacentini

DATE

        6/15/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBehavior : MonoBehaviour
{
    public int length;
    int hitNum;
    public ShipType sType;

    /**/
    /*
    ModelBehavior::Start()

    NAME

            ModelBehavior::Start - Houses the code to be run at the start of the game.

    SYNOPSIS

            

    DESCRIPTION

            Start() is called at the beginning of the game and sets the number of hits required to destroy the ship to the length

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/15/20

    */
    /**/
    void Start()
    {
        hitNum = length;
    }/*void Start();*/

    /**/
    /*
    ModelBehavior::Sunk()

    NAME

            ModelBehavior::Sunk - Reports true if sunk, false if not.

    SYNOPSIS

            

    DESCRIPTION

            Sunk() reports whether the ship has been sunk/destroyed.
            If the hitNum (number of times to be hit to be sunk) is 0 or less, the ship reports that it has been sunk

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/15/20

    */
    /**/
    bool Sunk()
    {
        if(hitNum <= 0)
        {
            return true;
        }
        return false;
    }/*bool Sunk();*/

    /**/
    /*
    ModelBehavior::hit()

    NAME

            ModelBehavior::hit - Reports true if hit, false if not.

    SYNOPSIS

            

    DESCRIPTION

            hit() reports whether the ship has been hit.
            If the hitNum (number of times to be hit to be sunk) is less than length and greater than 0, the ship reports that it has been hit.

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/15/20

    */
    /**/
    public bool hit()
    {
        if(hitNum < length && hitNum > 0)
        {
            return true;
        }
        return false;
    }/*public bool hit();*/

    /**/
    /*
    ModelBehavior::DecreaseHealth()

    NAME

            ModelBehavior::DecreaseHealth - Decreases hitNum, checks if ship sunk, and makes it visible if it is.

    SYNOPSIS

            

    DESCRIPTION

            DecreaseHealth() is called when a shot hits a ship. The health of the ship is decreased by 1.
            If the ship has been destroyed, it reveals the ship on the board for both players to see.
            Returns true if the ship has been sunk, false if not

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/15/20

    */
    /**/
    public bool DecreaseHealth()
    {
        hitNum--;
        if(Sunk())
        {
            //Because some of the 3D Models have their MeshRenders tied to their submodels, the component must be searched for specfically
            if(sType == ShipType.CRUISER)
            {
                Transform mesh = transform.Find("current2");
                mesh.GetComponent<MeshRenderer>().enabled = true;
                return true;
            }
            if(sType == ShipType.DESTROYER)
            {
                Transform mesh = transform.Find("ship");
                mesh.GetComponent<MeshRenderer>().enabled = true;
                return true;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
                return true;
            }
        }
        return false;
    }/*public bool DecreaseHealth();*/
}
/**/ /*** END ModelBehavior ***/ /**/