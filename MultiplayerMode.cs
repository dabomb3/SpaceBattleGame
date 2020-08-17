/**/ /*** BEGIN MultiplayerMode ***/ /**/
/**/
/*
Class MultiplayerMode

NAME

        class MultiplayerMode - Extension of GameManager class for Multiplayer Mode

SYNOPSIS

        

DESCRIPTION

        The MultiplayerMode class adds different behaviors for Multiplayer to GameManager class
AUTHOR

        Thomas Piacentini

DATE

        8/6/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class MultiplayerMode : GameManager
{
    OnlineCode onlineCode;
    int thisPlayer = 0;

    /**/
    /*
    MultiplayerMode::getCPlayer()

    NAME

            MultiplayerMode::getCPlayer - Matches the Nickname to the players of the game

    SYNOPSIS

            

    DESCRIPTION

            getCPlayer() determines which Multiplayer players are which
            Returns the index of playerList that matches the current players ID to the ID given to the players in playerList

    RETURNS

            int

    AUTHOR

            Thomas Piacentini

    DATE

            8/6/20

    */
    /**/
    int getCPlayer()
    {
        if(playerList[0].playerID.Equals(NetworkClient.Instance.PlayerId))
        {
            return 0;
        }
        return 1;
    }/*int getCPlayer();*/

    /**/
    /*
    MultiplayerMode::Awake()

    NAME

            MultiplayerMode::Awake - Overides Awake() function from GameManager class

    SYNOPSIS

            

    DESCRIPTION

            Calls the awake method from GameManager
            Gets the players in the online game
            Goes thru the online players and if the current players online id matches the host then it sets playerList[0].playerID to the online ID
            If the ids dont match, playerList[1].playerID is set

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            8/6/20

    */
    /**/
    protected new void Awake()
    {
        base.Awake();
        NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) =>
        {
            onlineCode = FindObjectOfType<OnlineCode>();
            if(successful)
            {
                foreach(SWPlayer swp in reply.players)
                {
                    //string pName = swp.GetCustomDataString();
                    string pID = swp.id;
                    if(pID.Equals(NetworkClient.Instance.PlayerId))
                    {
                        playerList[0].playerID = pID;
                    }
                    else
                    {
                        playerList[1].playerID = pID;
                    }
                }
                
            }
        });
    }/*void Awake();*/


    /**/
    /*
    MultiplayerMode::SendShipPos()

    NAME

            MultiplayerMode::SendShipPos - sends the current players ship locations to the server

    SYNOPSIS

            message --> carries the data to the server, SWNetworkMessage

    DESCRIPTION

            Creates a Byte array containing the positions of the ships placed by the playes to be pulled back to each other

    RETURNS

            Byte[]

    AUTHOR

            Thomas Piacentini

    DATE

            8/8/20

    */
    /**/
    public Byte[] SendShipPos()
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push((Byte)playerList[0].shipList.Count);
        message.Push((Byte)playerList[0].shipList);
        message.Push((Byte)playerList[1].shipList.Count);
        message.Push((Byte)playerList[1].shipList);
        return message.ToArray();
    }/*public Byte[] SendShipPos();*/

}
/**/ /*** END MultiplayerMode ***/ /**/