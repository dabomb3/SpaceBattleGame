/**/ /*** BEGIN GameManager ***/ /**/
/**/
/*
Class GameManager

NAME

        class GameManager

SYNOPSIS

        currentInstance     --> The instance of the GameManager so that it can be accessed from outside
        currentState        --> The current state of the game, State enum
        shootCam            --> The camera position that is used while shooting, GameObject
        CameraInMotion      --> True when the camera is moving to its next position, bool
        BoardInMotion       --> True when the board is in motion, bool
        Firing              --> True when the laser is moving to its target, bool
        setupCanvas         --> The UI for setting up the board, GameObject
        p1BoardOGPos        --> The flat position of the 1st players board, GameObject
        p2BoardOGPos        --> The flat position of the 2nd players board, GameObject
        p1BoardTiltPos      --> The tilted position of the 1st players board, GameObject
        p2BoardTiltPos      --> The tilted position of the 2nd players board, GameObject
        HitDialog           --> The "Hit" message that displays when the player hits a ship, GameObject
        MissDialog          --> The "Miss" message that displays when the player misses a ship, GameObject
        CPUFireDialog       --> The "Computer Firing" message that displays when the computer player is shooting, GameObject
        LaserPrefab         --> The model for the laser, GameObject
        P1LaserCannon       --> The position of the Player1 laser origin, GameObject
        P2LaserCannon       --> The position of the Player2 laser origin, GameObject
        P2LaserCannonCOMP   --> The position of the Player2 laser origin when player 2 is computer, GameObject
        P1Win               --> The Win UI for player1, GameObject
        P2Win               --> The Win UI for player2, GameObject
        P2WinComp           --> The UI that displays when the computer wins, GameObject
        BtnOvrlay           --> The UI element that holds the quit and restart buttons during gameplay,GameObject
        movedB              --> True when the board has been moved for the current player, GameObject
        movedC              --> True when the camera has been moved for the current player, GameObject
        mTime               --> The time value used by the laser animation for movement, float
        currentPlayer       --> The current player of the game
        playerList          --> The list of players for the game, List<Players>

DESCRIPTION

        The GameManager class holds all of the functions for the actual gameplay of the SpaceBattle game.
        Unity handles these classes in a specific way and encourages the dev to name it the GameManager, however the functionality could be described as a Round class.
        The GameManager holds the players, manages the setup of the boards, turns, and camera and board movements of the game.
        Specifies the current state that the game is in
        Holds the Computer player's behavior/ AI

AUTHOR

        Thomas Piacentini

DATE

        7/10/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager currentInstance;
    public State currentState;
    public GameObject shootCam;
    bool CameraInMotion;
    bool BoardInMotion;
    bool Firing;
    public GameObject setupCanvas;
    public GameObject p1BoardOGPos;
    public GameObject p2BoardOGPos;
    public GameObject p1BoardTiltPos;
    public GameObject p2BoardTiltPos;
    public GameObject HitDialog;
    public GameObject MissDialog;
    public GameObject CPUFireDialog;
    public GameObject LaserPrefab;
    public GameObject P1LaserCannon;
    public GameObject P2LaserCannon;
    public GameObject P2LaserCannonCOMP;
    public GameObject P1Win;
    public GameObject P2Win;
    public GameObject P2WinComp;
    public GameObject BtnOvrlay;
    bool movedB = false;
    bool movedC = false;
    float mTime;
    
    /**/
    /*
    GameManager::Awake()

    NAME

            GameManager::Awake - Houses the code to be run when the script is run for the first time

    SYNOPSIS

            

    DESCRIPTION

            Awake() is called when the GameManager script is run for the first time, which is when a new game starts
            It sets the currentInstance to the current running GameManager so that it can be accessed by other scripts

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public void Awake()
    {
        currentInstance = this;
    }/*public void Awake();*/

    /**/
    /*
    GameManager::State (enum)

    NAME

            GameManager::State - Defines the states of the game

    SYNOPSIS

            

    DESCRIPTION

            P1_SETUP is the state the game is in while Player1 sets up their board
            P2_SETUP is the state the game is in while Player2 sets up their board
            FIRE is the state the game is in when a player is making a shot
            IDLE is the state the game is in when it is just waiting for the player to do something

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public enum State
    {
        P1_SETUP,
        P2_SETUP,
        FIRE,
        IDLE
    }
    
    /**/
    /*
    Class Player

    NAME

            class Player

    SYNOPSIS

            PlayerType      --> enum that describes what kind of player the player is -- HUMAN / COMPUTER
            ptype           --> the type of this player, PLayerType
            grid            --> internal representation of the player's board, Tile[]
            shotGrid        --> internal representation of the shots made on a player's board, bool[]
            board           --> the player's board, PlayerBoardGenerator
            setupMenu       --> the screen that shows before the player sets up their board, GameObject
            fireMenu        --> the screen that shows before the player takes a shot, GameObject
            cameraPos       --> the position of the camera that is used by the player, GameObject
            altCamPos       --> the alternative position of the camera that is used by the player, GameObject
            playerID        --> the ID of the player used in Multiplayer, string
            shipLocations   --> list of the locations of the ships on player's board, List<GameObject>

    DESCRIPTION

            The Player class holds the information for each player to be used by the GameManager

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    [System.Serializable]
    public class Player
    {
        public enum PlayerType
        {
            HUMAN,
            COMPUTER
        }
        public PlayerType ptype;
        public Tile[,] grid = new Tile[10,10];
        public bool[,] shotGrid = new bool[10,10];
        public PlayerBoardGenerator board;
        public GameObject setupMenu;
        public GameObject fireMenu;
        public GameObject cameraPos;
        public GameObject altCamPos;
        public string playerID;
        public List<GameObject> shipLocations = new List<GameObject>();

        /**/
        /*
        Player::Player()

        NAME

                Player::Player - Constructor for the Player class

        SYNOPSIS

                

        DESCRIPTION

                Initializes the internal representation of the board

        RETURNS

               

        AUTHOR

                Thomas Piacentini

        DATE

                7/10/20

        */
        /**/
        public Player()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    ShipType st = ShipType.EMPTY;
                    grid[i,j] = new Tile(st, null);
                    shotGrid[i,j] = false;
                }
            }
        }
        
    }

    int currentPlayer;
    public Player[] playerList = new Player[2];

    /**/
    /*
    GameManager::PickFirstTurn()

    NAME

            GameManager::PickFirstTurn - Randomly selects the first player of the game

    SYNOPSIS

            first    --> random number 0 or 1, int

    DESCRIPTION

            PickFirstTurn is called after both players set up their boards in order to decide which player shoots first

    RETURNS

            int

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    int PickFirstTurn()
    {
        int first = Random.Range(0,2);
        return first;
    }/*int PickFirstTurn();*/

    /**/
    /*
    GameManager::Start()

    NAME

            GameManager::Start - The code that is run when the game starts

    SYNOPSIS

            

    DESCRIPTION

            Sets the target framerate
            Sets currentState to IDLE
            Shows player1's setup screen
            hides all other screens

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    void Start()
    {
        Application.targetFrameRate = 60;
        HideMenus();
        playerList[currentPlayer].setupMenu.SetActive(true);
        currentState = State.IDLE;
        HitDialog.SetActive(false);
        MissDialog.SetActive(false);
        P1Win.SetActive(false);
        P2Win.SetActive(false);
        P2WinComp.SetActive(false);
        BtnOvrlay.SetActive(false);
        CPUFireDialog.SetActive(false);
    }/*void Start();*/

    /**/
    /*
    GameManager::receiveShip(GameObject ship)

    NAME

            GameManager:: - Receives the ship being added to the board

    SYNOPSIS

            ship    --> the ship being sent to the board, GameObject

    DESCRIPTION

            Adds ship to the current player's shipLocations

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public void receiveShip(GameObject ship)
    {
        playerList[currentPlayer].shipLocations.Add(ship);
    }/*public void receiveShip();*/

    /**/
    /*
    GameManager::addToGrid(Transform shipTrans, ModelBehavior shipModel, GameObject placedShip)

    NAME

            GameManager::addToGrid - Adds ship to grid from PlacementClient

    SYNOPSIS

            shipTrans   --> The ship location as set in PlacementClient, Transform
            shipModel   --> The ship ModelBehavior as set in PLacementClient, ModelBehavior
            placedShip  --> The actual 3D model of the ship

    DESCRIPTION

            Takes in a ship and adds it to the current player's grid
            Sends it to receiveShip to add it to the list of ships 
            Outputs a string to the debug window that represents the grid for testing purposes

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public void addToGrid(Transform shipTrans, ModelBehavior shipModel, GameObject placedShip)
    {
        foreach(Transform t in shipTrans)
        {
            TileBehavior tileBehavior = t.GetComponent<PlaceLocationBehavior>().GetTileBehavior();
            playerList[currentPlayer].grid[tileBehavior.xPos, tileBehavior.zPos] = new Tile(shipModel.sType, shipModel);
        }
        receiveShip(placedShip);
        TestGrid();
    }/*public void addToGrid(Transform shipTrans, ModelBehavior shipModel, GameObject placedShip);*/

    /**/
    /*
    GameManager::CheckOccupation(int xPos, int yPos)

    NAME

            GameManager::CheckOccupation - Checks a given position in the grid to see if it is occupied

    SYNOPSIS

            xPos    --> the x coordinate of the position to be checked
            yPos    --> the y coordinate of the position to be checked

    DESCRIPTION

            Takes in x and y coordinates and checks the current player's grid to see if that position is occupied
            Returns true if occupied by ship

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public bool CheckOccupation(int xPos, int yPos)
    {
        if(playerList[currentPlayer].grid[xPos, yPos].Occupied())
        {
            return true;
        }
        return false;
    }/*bool CheckOccupation(int xPos, int yPos);*/

    /**/
    /*
    GameManager::TestGrid()

    NAME

            GameManager::TestGrid - Outputs a string representation of the grid for debugging

    SYNOPSIS

            s   --> the string that represents the grid, string
            sep --> used to determine when to split string with a "|" to make it look correct, int

    DESCRIPTION

            Iterates through the grid and adds "C" to s if there is a cruiser on that tile, "D" for destroyer, "S" for submarine, "B" for battleship, "K" for carrier.
            Used when debugging to see if the ships are being placed correctly

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    void TestGrid()
    {
        string s = "";
        int sep = 0;
        for(int i = 0; i < 10; i++)
        {
            s += "|";
            for(int j = 0; j < 10; j++)
            {
                string t = "0";
                if(playerList[currentPlayer].grid[i,j].type == ShipType.CRUISER)
                {
                    t = "C";
                }
                if(playerList[currentPlayer].grid[i,j].type == ShipType.DESTROYER)
                {
                    t = "D";
                }
                if(playerList[currentPlayer].grid[i,j].type == ShipType.SUB)
                {
                    t = "S";
                }
                if(playerList[currentPlayer].grid[i,j].type == ShipType.BS)
                {
                    t = "B";
                }
                if(playerList[currentPlayer].grid[i,j].type == ShipType.CARRIER)
                {
                    t = "K";
                }
                s += t;
                sep = j%10;
                if(sep == 9)
                {
                    s += "|";
                }
            }
            s += "\n";
        }
        print(s);
    }/*void TestGrid();*/

    /**/
    /*
    GameManager::clearList()

    NAME

            GameManager::clearList - Clears the shipLocations list

    SYNOPSIS

            

    DESCRIPTION

            Iterates through shipList and destroys all of the GameObjects
            Clears the list
            Creates a new grid

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public void clearList()
    {
        foreach(GameObject playedShip in playerList[currentPlayer].shipLocations)
        {
            Destroy(playedShip);
        }
        //Garbage collection
        playerList[currentPlayer].shipLocations.Clear();
        CreateGrid();
        
    }/*void clearList();*/

    /**/
    /*
    GameManager::CreateGrid()

    NAME

            GameManager::CreateGrid - Creates a new grid

    SYNOPSIS

           

    DESCRIPTION

            Creaes a 10x10 grid of empty tiles for the current player

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    public void CreateGrid()
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                ShipType st = ShipType.EMPTY;
                playerList[currentPlayer].grid[i,j] = new Tile(st, null);
                playerList[currentPlayer].shotGrid[i,j] = false;
            }
        }
    }/*public void CreateGrid();*/

    /**/
    /*
    GameManager::Update()

    NAME

            GameManager::Update - Called every frame of the game

    SYNOPSIS

            s   --> the string that represents the grid, string
            sep --> used to determine when to split string with a "|" to make it look correct, int

    DESCRIPTION

            Uses a switch statement to determine the current game state.
            When the currentState is IDLE, the buttons are hidden
            When the currentState is FIRE, if the current player is human it shows the quit and restart buttons
                Moves the boards and cameras to correct location for the current player
            When the currentState is P1_SETUP, it allows player 1 to setup their board
            When the currentState is P2_SETUP, it allows player 2 to setup their board

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                BtnOvrlay.SetActive(false);
                
                break;
            case State.FIRE:
                if(playerList[currentPlayer].ptype == Player.PlayerType.HUMAN)
                {
                    BtnOvrlay.SetActive(true);
                }
                if(currentPlayer == 0 && !movedB && !movedC)
                {
                    StartCoroutine(BoardTilt(p2BoardTiltPos,1));
                    StartCoroutine(CameraChange(playerList[currentPlayer].altCamPos));
                }
                if(currentPlayer == 1 && !movedB && !movedC)
                {
                    StartCoroutine(BoardTilt(p1BoardTiltPos,0));
                    StartCoroutine(CameraChange(playerList[currentPlayer].altCamPos));
                }
                break;
            case State.P1_SETUP:
                playerList[currentPlayer].setupMenu.SetActive(false);
                PlacementClient.placingInstance.SetBoard(playerList[currentPlayer].board, playerList[currentPlayer].ptype.ToString());
                StartCoroutine(CameraChange(playerList[currentPlayer].cameraPos));
                currentState = State.IDLE;
                break;
            case State.P2_SETUP:
                //ResetBoardLocations();
                playerList[currentPlayer].setupMenu.SetActive(false);
                PlacementClient.placingInstance.SetBoard(playerList[currentPlayer].board, playerList[currentPlayer].ptype.ToString());
                StartCoroutine(CameraChange(playerList[currentPlayer].cameraPos));
                
                currentState = State.IDLE;
                break;
        }
    }/*void Update();*/

    /**/
    /*
    GameManager::HideMenus()

    NAME

            GameManager::HideMenus - Hides all of the UI menu objects

    SYNOPSIS

            

    DESCRIPTION

            Sets all menus to inactive

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/10/20

    */
    /**/
    void HideMenus()
    {
        playerList[0].setupMenu.SetActive(false);
        playerList[0].fireMenu.SetActive(false);
        playerList[1].setupMenu.SetActive(false);
        playerList[1].fireMenu.SetActive(false);
    }/*void HideMenus();*/

    /**/
    /*
    GameManager::P1Setup()

    NAME

            GameManager::P1Setup - sets the currentState to P1_SETUP

    SYNOPSIS

            

    DESCRIPTION

            P1Setup is called by player 1 clicking the setup button

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    public void P1Setup()
    {
        currentState = State.P1_SETUP;
    }/*public void P1Setup();*/

    /**/
    /*
    GameManager::P2Setup()

    NAME

            GameManager::P2Setup - sets the currentState to P2_SETUP

    SYNOPSIS

            

    DESCRIPTION

            P2Setup is called by player 2 clicking the setup button

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    public void P2Setup()
    {
        currentState = State.P2_SETUP;
    }/*public void P2Setup();*/

    /**/
    /*
    GameManager::ReadyFun()

    NAME

            GameManager::ReadyFun - holds the functionality for the ready button being pressed

    SYNOPSIS

            

    DESCRIPTION

            ReadyFun() is called when the player presses the ready button.
            If the currentPlayer is player1, the ships are hidden, and the currentPlayer is increased to 1
                After increasing, if currentPlayer (now player2) is a computer, the state is set to P2_SETUP and the camera stays on player1s board.
                If player2 is a human then the setup panel is shown
            If the currentPlayer is player2, the ships are hidden and the first turn is calculated
            After calculating the next turn, if the current player is a computer the computer takes a shot
            If the player that is going first is a human, the firing menu is shown 
            The currentState is set to FIRE.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    public void ReadyFun()
    {
        if(currentPlayer == 0)
        {
            HideShips();
            NextPlayer();
            if(playerList[currentPlayer].ptype == Player.PlayerType.COMPUTER)
            {
                currentState = State.P2_SETUP;
                StartCoroutine(CameraChange(playerList[0].altCamPos));
                StartCoroutine(BoardTilt(p2BoardTiltPos, 1));
                return;
            }
            playerList[currentPlayer].setupMenu.SetActive(true);
            return;
        }
        if(currentPlayer == 1)
        {
            HideShips();
            currentPlayer = PickFirstTurn();
            if(playerList[currentPlayer].ptype == Player.PlayerType.COMPUTER)
            {
                FixBoard(1);
                ComputerFire();
                setupCanvas.SetActive(false);
                currentState = State.FIRE;
                return;
            }
            StartCoroutine(CameraChange(playerList[currentPlayer].altCamPos));
            if(currentPlayer == 0)
            {
                StartCoroutine(BoardTilt(p2BoardTiltPos,1));
            }
            else
            {
                StartCoroutine(BoardTilt(p1BoardTiltPos,0));
            }
            playerList[currentPlayer].fireMenu.SetActive(true);
            setupCanvas.SetActive(false);
        }
    }/*void ReadyFun();*/

    /**/
    /*
    GameManager::HideShips()

    NAME

            GameManager::HideShips - hides all of the currentPlayer's ships

    SYNOPSIS

            cruiserMesh --> the Mesh of the cruiser that needs to be found specifically
            desMesh     --> the Mesh of the destroyer that needs to be found specifically

    DESCRIPTION

            Called when the opposite player is playing so they cannot see the placement of ships on the board
            Goes through the shipList of the current player and disables the mesh renderers on the models.
            Since the Cruiser and Destroyer models have sublayers, the MeshRenderer needs to be found in the child of the root GameObject
            The other ships have their mesh renderers attached to the root

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    void HideShips()
    {
        //Cruiser and Destroyer models do not have MeshRenderer in prefab root, have to grab MeshRenderer from child
        foreach (GameObject ship in playerList[currentPlayer].shipLocations)
        {
            Transform cruiserMesh = ship.transform.Find("current2");
            Transform desMesh = ship.transform.Find("ship");
            if(cruiserMesh != null)
            {
                cruiserMesh.GetComponent<MeshRenderer>().enabled = false;
            }
            else if(desMesh != null)
            {
                desMesh.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                ship.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }/*void UnHideShips();*/

    /**/
    /*
    GameManager::UnHideShips()

    NAME

            GameManager::UnHideShips - Shows all of the ships

    SYNOPSIS

            cruiserMesh --> the Mesh of the cruiser that needs to be found specifically
            desMesh     --> the Mesh of the destroyer that needs to be found specifically

    DESCRIPTION

            Shows current player's ships while they are firing on their opponent
            Goes through the shipList of the current player and enables the mesh renderers on the models.
            Since the Cruiser and Destroyer models have sublayers, the MeshRenderer needs to be found in the child of the root GameObject
            The other ships have their mesh renderers attached to the root

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    void UnHideShips()
    {
        foreach (GameObject ship in playerList[currentPlayer].shipLocations)
        {
            Transform cruiserMesh = ship.transform.Find("current2");
            Transform desMesh = ship.transform.Find("ship");
            if(cruiserMesh != null)
            {
                cruiserMesh.GetComponent<MeshRenderer>().enabled = true;
            }
            else if(desMesh != null)
            {
                desMesh.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                ship.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }/*void UnHideShips();*/

    /**/
    /*
    GameManager::NextPlayer()

    NAME

            GameManager::NextPlayer - switches to the next player

    SYNOPSIS

            

    DESCRIPTION

            Increments currentPlayer, then uses % to make the player go back to zero after it reaches 1

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    void NextPlayer()
    {
        currentPlayer++;
        currentPlayer %= 2;
        movedC = false;
        movedB = false;
    }/*void NextPlayer();*/

    /**/
    /*
    GameManager::CameraChange(GameObject camPos)

    NAME

            GameManager::CameraChange - moves the Camera from the current position to given position

    SYNOPSIS

            camPos      --> the position the camera is moving to, GameObject
            time        --> time in which the camera is moving, float
            duratiion   --> the duration of the movement, float
            startPos    --> the starting position of the camera, Vector3
            startRot    --> the starting rotation of the camera, Quaternion
            endPos      --> the end position of the camera, Vector3
            endRot      --> the end rotation of the camera, Quaternion

    DESCRIPTION

            CameraChange() is called whenever the camera location is being switched from a Coroutine.
            Using an IEnumerator and Coroutine allows the camera to move smoothly and be seen by the players.
            The current camera position is used for start position and start rotation.
            The passed in camPos is used as the end position and rotation.
            camPos is specified when calling the function, usually it is the current player's camera position.
            Linear Interpolation (Lerp) is used to get a smooth movement from one position to another.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    IEnumerator CameraChange(GameObject camPos)
    {
        if(CameraInMotion)
        {
            yield break;
        }
        CameraInMotion = true;
        float time = 0;
        float duration = 0.5f;
        Vector3 startPos = Camera.main.transform.position;
        Quaternion startRot = Camera.main.transform.rotation;
        Vector3 endPos = camPos.transform.position;
        Quaternion endRot = camPos.transform.rotation;
        while(time < duration)
        {
            time += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(startPos, endPos, time/duration);
            Camera.main.transform.rotation = Quaternion.Lerp(startRot, endRot, time/duration);
            yield return null;
        }
        movedC = true;
        CameraInMotion = false;
    }/*IEnumerator CameraChange(GameObject camPos);*/

    /**/
    /*
    GameManager::BoadTilt(GameObject cBoard, int p)

    NAME

            GameManager::BoardTilt - moves the enemy of the current player's board to the correct tilt location

    SYNOPSIS

            cBoard      --> the position the board is moving to, GameObject
            p           --> the player whos board needs to be moved to the location, int
            time        --> time in which the board is moving, float
            duratiion   --> the duration of the movement, float
            startPos    --> the starting position of the board, Vector3
            startRot    --> the starting rotation of the board, Quaternion
            endPos      --> the end position of the board, Vector3
            endRot      --> the end rotation of the board, Quaternion

    DESCRIPTION

            BoardTilt() is called whenever the next player is going to be taking a turn
            It puts the boards in the correct positions to look like real life battleship
            IEnumerator is called by Startcoroutine so that the players can see the boards move
            The current player's board position is used for startPos and startRot
            The passed in location is used for end position and rotation
            p is used to tell which board needs to be moved
            Lerp is used for smooth movement
            FixBoard is called to make sure all of the ships are positioned correctly

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/11/20

    */
    /**/
    IEnumerator BoardTilt(GameObject cBoard, int p)
    {
        if(BoardInMotion)
        {
            yield break;
        }
        ResetBoardLocations();
        BoardInMotion = true;
        float time = 0;
        float duration = 0.2f;
        Vector3 startPos = playerList[currentPlayer].board.transform.position;
        Quaternion startRot = playerList[currentPlayer].board.transform.rotation;
        Vector3 endPos = cBoard.transform.position;
        Quaternion endRot = cBoard.transform.rotation;
        while(time < duration)
        {
            time += Time.deltaTime;
            playerList[p].board.transform.position = Vector3.Lerp(startPos, endPos, time/duration);
            playerList[p].board.transform.rotation = Quaternion.Lerp(startRot, endRot, time/duration);
            
            yield return null;
        }
        FixBoard(p);
        movedB = true;
        BoardInMotion = false;
    }/*IEnumerator BoardTilt(GameObject cBoard, int p);*/

    /**/
    /*
    GameManager::FixBoard(int p)

    NAME

            GameManager::FixBoard - makes sure the ship models are placed in the correct places on the board after moving

    SYNOPSIS

            p           --> the player whos board needs to be fixed, int
            tempShip    --> temporary transform of the ship to be fixed, Transform
            tmpT        --> hold the original transform of the ship to preserve its location on the board, Vector3
            eulers      --> holds the original rotation of the ship to preserve its rotation, Vector3

    DESCRIPTION

            Grabs the transfrm of each of the ships on the board and preserves its original x and z positions, as well as rotations
            Sets the local y position of the ship to 0 so that it is sitting on top of the board
            Sets the local x rotation of the ship to 0 so that it sits correctly on the board
            Cannot modify individual axes directly, so a roundabout method was required using temporary objects

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/20/20

    */
    /**/
    void FixBoard(int p)
    {
       Transform tempShip;
        tempShip = playerList[p].board.transform.Find("battleshipJ(Clone)");
        Vector3 tmpT = tempShip.transform.localPosition;
        tmpT.y = 0.0f;
        Vector3 eulers = tempShip.transform.localEulerAngles;
        tempShip.localRotation = Quaternion.Euler(new Vector3(0,eulers.y,eulers.z));
        tempShip.localPosition = tmpT;
        tempShip = playerList[p].board.transform.Find("carrierJ(Clone)");
        tmpT = tempShip.transform.localPosition;
        tmpT.y = 0.0f;
        eulers = tempShip.transform.localEulerAngles;
        tempShip.localRotation = Quaternion.Euler(new Vector3(0,eulers.y,eulers.z));
        tempShip.localPosition = tmpT;
        tempShip = playerList[p].board.transform.Find("destroyerj(Clone)");
        tmpT = tempShip.transform.localPosition;
        tmpT.y = 0.0f;
        eulers = tempShip.transform.localEulerAngles;
        tempShip.localRotation = Quaternion.Euler(new Vector3(0,eulers.y,eulers.z));
        tempShip.localPosition = tmpT;
        tempShip = playerList[p].board.transform.Find("submarineJ(Clone)");
        tmpT = tempShip.transform.localPosition;
        tmpT.y = 0.0f;
        eulers = tempShip.transform.localEulerAngles;
        tempShip.localRotation = Quaternion.Euler(new Vector3(0,eulers.y,eulers.z));
        tempShip.localPosition = tmpT;
        tempShip = playerList[p].board.transform.Find("cruiserJ(Clone)");
        tmpT = tempShip.transform.localPosition;
        tmpT.y = 0.0f;
        eulers = tempShip.transform.localEulerAngles;
        tempShip.localRotation = Quaternion.Euler(new Vector3(0,eulers.y,eulers.z));
        tempShip.localPosition = tmpT;
    }/*void FixBoard();*/
    
    /**/
    /*
    GameManager::ResetBoardLocations()

    NAME

            GameManager::ResetBoardLocations - moves the boards back to their original positions

    SYNOPSIS

            

    DESCRIPTION

            Sets both player's boards to their original positions so that they can be moved correctly

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/12/20

    */
    /**/
    void ResetBoardLocations()
    {
        playerList[0].board.transform.position = p1BoardOGPos.transform.position;
        playerList[0].board.transform.rotation = p1BoardOGPos.transform.rotation;
        playerList[1].board.transform.position = p2BoardOGPos.transform.position;
        playerList[1].board.transform.rotation = p2BoardOGPos.transform.rotation;
    }/*ResetBoardLocations();*/

    /**/
    /*
    GameManager::SelectingFireButton()

    NAME

            GameManager::SelectingFireButton - called when the player is ready to fire

    SYNOPSIS

            

    DESCRIPTION

            Unhides the current player's ships, hides all of the menus
            Sets currentState to FIRE

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/12/20

    */
    /**/
    public void SelectingFireButton()
    {
        UnHideShips();
        playerList[currentPlayer].fireMenu.SetActive(false);
        movedB = false;
        movedC = false;
        currentState = State.FIRE;
    }/*void SelectingFireButton();*/

    /**/
    /*
    GameManager::GetEnemy()

    NAME

            GameManager::GetEnemy - returns the opposite player index

    SYNOPSIS

            enemy   --> the enemy index, int

    DESCRIPTION

            If currentPlayer is 0 (player 1), enemy is 1 (player 2)
            If currentPlayer is 1 (player 2), enemy is 0 (player1 )

    RETURNS

            int

    AUTHOR

            Thomas Piacentini

    DATE

            7/12/20

    */
    /**/
    int GetEnemy()
    {
        int enemy = 0;
        if(currentPlayer == 0)
        {
            enemy = 1;
        }
        if(currentPlayer == 1)
        {
            enemy = 0;
        }
        return enemy;
    }/*int GetEnemy();*/

    /**/
    /*
    GameManager::TakeShot(int x, int y, TileBehavior tLoc)

    NAME

            GameManager::TakeShot - Called when tile is clicked on

    SYNOPSIS

            x       --> x coordinate of tile being shot, int
            y       --> y coordinate of tile being shot, int
            tLoc    --> the tile information of the tile, TileBehavior

    DESCRIPTION

            Called by clicking on enemy board tile when firing is allowed
            Calls CheckShotLocation() using StartCoroutine

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/15/20

    */
    /**/
    public void TakeShot(int x, int y, TileBehavior tLoc)
    {
        StartCoroutine(CheckShotLocation(x,y,tLoc));
    }/*public void TakeShot(int x, int y, TileBehavior tLoc);*/

    /**/
    /*
    GameManager::CheckShotLocation(int x, int y, TileBehavior tLoc)

    NAME

            GameManager::CheckShotLocation - Performs firing behavior

    SYNOPSIS

            x       --> x coordinate of tile being shot, int
            y       --> y coordinate of tile being shot, int
            tLoc    --> the tile information of the tile, TileBehavior
            startP  --> the start location of the laser, Vector3
            goalP   --> the end location of the laser, Vector3
            laser   --> the laser that gets shot, GameObject

    DESCRIPTION

            CheckShotLocation takes the location that the player has fired at and checks to see if it is a valid shot.
            Shoots a laser from the current player's board to the enemy players board.
            If a shot hits a ship, "Hit" is displayed, the grid is updated, the tile icon is changed, and damage is done to the ship using ModelBehavior::DecreaseHealth().
            If a shot misses, "Miss" is displayed
            If a shot hits a ship, the ship is checked on if it is destroyed
                If it is sunk, it is removed from the shipList
            If shipList has no more ships in it, the player who sank the ship wins
            After the shot is made, if the next player is a computer, the ComputerFire() function is called
            IEnumerator is used so that the laser can be animated

    RETURNS

            IEnumerator

    AUTHOR

            Thomas Piacentini

    DATE

            7/15/20

    */
    /**/
    IEnumerator CheckShotLocation(int x, int y, TileBehavior tLoc)
    {
        if(Firing)
        {
            yield break;
        }
        Firing = true;
        int enemy = GetEnemy();
        if(!playerList[enemy].board.CheckTile(tLoc))
        {
            Firing = false;
            yield break;
        }
        if(playerList[enemy].shotGrid[x,y])
        {
            Firing = false;
            yield break;
        }
        Vector3 startP = Vector3.zero;
        Vector3 goalP = tLoc.gameObject.transform.position;
        if(playerList[currentPlayer].ptype == Player.PlayerType.COMPUTER)
        {
            startP = P2LaserCannonCOMP.transform.position;
            CPUFireDialog.SetActive(true);
        }
        if(playerList[currentPlayer].ptype == Player.PlayerType.HUMAN)
        {
            if(currentPlayer == 0)
            {
                startP = P1LaserCannon.transform.position;
            }
            if(currentPlayer == 1)
            {
                startP = P2LaserCannon.transform.position;
            }
        }
        
        GameObject laser = Instantiate(LaserPrefab, startP, Quaternion.identity);
        while(!ShootLaser(startP, goalP, 0.8f, laser))
        {
            yield return null;
        }
        Destroy(laser);
        mTime = 0;
        CPUFireDialog.SetActive(false);
        if(playerList[enemy].grid[x,y].Occupied())
        {
            bool sunk = playerList[enemy].grid[x,y].placedShip.DecreaseHealth();
            if(sunk)
            {
                playerList[enemy].shipLocations.Remove(playerList[enemy].grid[x,y].placedShip.gameObject);
            }
            HitDialog.SetActive(true);
            tLoc.ActivateIcon(3, true);
        }
        else
        {
            MissDialog.SetActive(true);
            tLoc.ActivateIcon(2, true);
        }
        playerList[enemy].shotGrid[x,y] = true;
        if(playerList[enemy].shipLocations.Count == 0)
        {
            if(currentPlayer == 0)
            {
                P1Win.SetActive(true);
            }
            if(currentPlayer == 1)
            {
                if(playerList[currentPlayer].ptype == Player.PlayerType.COMPUTER)
                {
                    P2WinComp.SetActive(true);
                }
                else
                {
                    P2Win.SetActive(true);
                }
            }
            yield break;
        }
        yield return new WaitForSeconds(2f);
        HitDialog.SetActive(false);
        MissDialog.SetActive(false);
        HideShips();
        NextPlayer();
        if(playerList[currentPlayer].ptype == Player.PlayerType.COMPUTER)
        {
            Firing = false;
            currentState = State.IDLE;
            ComputerFire();
            yield break;
        }
        Firing = false;
        playerList[currentPlayer].fireMenu.SetActive(true);
        currentState = State.IDLE;
    }/*IEnumerator CheckShotLocation(int x, int y, TileBehavior tLoc);*/

    /**/
    /*
    GameManager::ComputerFire()

    NAME

            GameManager::ComputerFire - AI for Computer Player

    SYNOPSIS

            index           --> random index chosen to place shot on board, int
            xP              --> x coord of randomly chosen location, int
            yP              --> y coord of randmoly chosen location, int
            enemy           --> the enemy, int
            hitLocations    --> list of locations on the board that have already been shot at and have a ship on them, List<int[2]>
            possibleShots   --> list of locations on the board based on already made hits, List<int[2]
            commonLocs      --> list of locations on the board where people commonly places ships, List<int[2]>
            randomShots     --> list of locations on the board where a shot has not been placed yet, List<int[2]>


    DESCRIPTION

            ComputerFire() is the brain of the Computer player.
            First it populates a list of 2 dimensional integer arrays that resemble the locations of all of the tiles that have already shown they are a hit.
            After populating the list of already hit tiles, shots around the hit locations are calculated with GetPossShots
            With the list of possible locations a ship could be based on the already hit tiles, the computer chooses one randomly
            If there are not any already partially hit ships, the computer randomly chooses a common location
                If that location has not already been shot at, it shoots at that location
            If the common location it chose has already been shot at, it chooses a random location from the available not already shot locations and fires at it.


    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/15/20

    */
    /**/
    void ComputerFire()
    {
        
        int index;
        int xP = 0;
        int yP = 0;
        int enemy = GetEnemy();
        List<int[]> hitLocations = new List<int[]>();
        //check for shots already made
        for(int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(playerList[enemy].shotGrid[i,j])
                {
                    if(playerList[enemy].grid[i,j].Occupied())
                    {
                        if(playerList[enemy].grid[i,j].placedShip.hit())
                        {
                            hitLocations.Add(new int[2] {i,j});
                        }
                    }
                }
            }
        }
        //list to hold possible shots to take based on already taken shots
        List<int[]> possibleShots = new List<int[]>();
        //list holds common locations
        List<int[]> commonLocs = new List<int[]>();
        commonLocs.Add(new int [] {0,0});
        commonLocs.Add(new int [] {0,9});
        commonLocs.Add(new int [] {9,0});
        commonLocs.Add(new int [] {9,9});
        commonLocs.Add(new int [] {4,4});
        commonLocs.Add(new int [] {5,5});
        commonLocs.Add(new int [] {4,5});
        commonLocs.Add(new int [] {5,4});
        if(hitLocations.Count > 0)
        {
            for (int i = 0; i < hitLocations.Count; i++)
            {
                possibleShots.AddRange(GetPossShots(hitLocations[i]));
            }
            bool shotGood = false;
            while(!shotGood)
            {
                index = Random.Range(0,possibleShots.Count);
                xP = possibleShots[index][0];
                yP = possibleShots[index][1];
                if(!playerList[enemy].shotGrid[xP,yP])
                {
                    shotGood = true;
                    TileBehavior tB = playerList[enemy].board.GetTile(xP,yP);
                    TakeShot(xP,yP,tB);
                }
            }
            
            
            return;
        }
        index = Random.Range(0,commonLocs.Count);
        xP = commonLocs[index][0];
        yP = commonLocs[index][0];
        //check if common locations are shot already and if not shoot at one
        if(!playerList[enemy].shotGrid[xP, yP])
        {
            TileBehavior tB = playerList[enemy].board.GetTile(xP,yP);
            TakeShot(xP,yP,tB);
            return;
        }
        //anywhere else that hasnt been shot
        List<int[]> randomShots = new List<int[]>();
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if(!playerList[enemy].shotGrid[i,j])
                {
                    randomShots.Add(new int[2] {i,j});
                }
            }
        }
        index = Random.Range(0,randomShots.Count);
        xP = randomShots[index][0];
        yP = randomShots[index][1];
        TileBehavior tileB = playerList[enemy].board.GetTile(xP,yP);
        TakeShot(xP,yP,tileB);
        
    }/*void ComputerFire();*/

    /**/
    /*
    GameManager::GetPossShots(int[] inList)

    NAME

            GameManager::GetPossShots - Gets the locations of shots to take around an aready hit tile

    SYNOPSIS

            inList      --> the location of the hit tile, int[]
            possShots   --> the list of the possible shots calculated, List<int[]>

    DESCRIPTION

            A square is made around the hit tile, and the locations of the vertically and horizontally adjacent tiles are put in possShots

    RETURNS

            List<int[]>

    AUTHOR

            Thomas Piacentini

    DATE

            7/15/20

    */
    /**/
    List<int[]> GetPossShots(int[] inList)
    {
        List<int[]> possShots = new List<int[]>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                //original point is in middle of square, disregard
                if(i == 0 && j == 0)
                {
                    continue;
                }
                //diagonal top left of square - disregard 
                if(i == -1 && j == 1)
                {
                    continue;
                }
                //diagonal top right of square - disregard
                if(i == 1 && j == 1)
                {
                    continue;
                }
                //diagonal bottom left of square - disregard
                if(i == -1 && j == -1)
                {
                    continue;
                }
                //diagonal bottom right of square - disregard
                if(i == 1 && j == -1)
                {
                    continue;
                }
                int possX = inList[0] + i;
                int possY = inList[1] + j;
                if(possX >= 0 && possX < 10 && possY >= 0 && possY < 10)
                {
                    possShots.Add(new int[2] {possX, possY});
                }
            }
        }
        return possShots;
    }/*List<int[]> GetPossShots(int[] inList);*/

    /**/
    /*
    GameManager::ShootLaser(Vector3 st, Vector3 g, float sp, GameObject l)

    NAME

            GameManager::ShootLaser - Called when tile is clicked on

    SYNOPSIS

            st  --> start position of laser, Vector3
            g   --> goal position of laser, Vector3
            sp  --> speed of movement, float
            l   --> laser prefab, GameObject

    DESCRIPTION

            Used by CheckShotLocation when it fires the laser. Returns false until it hits the target tile
            Uses Lerp to smoothly move the laser

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            7/15/20

    */
    /**/
    bool ShootLaser(Vector3 st, Vector3 g, float sp, GameObject l)
    {
        mTime += sp * Time.deltaTime;
        Vector3 pos = Vector3.Lerp(st,g,mTime);
        l.transform.LookAt(pos);
        if(g != (r.transform.position = Vector3.Lerp(r.transform.position, pos ,mTime)))
        {
            return false;
        }
        return true;
    }/*bool ShootLaser(Vector3 st, Vector3 g, float sp, GameObject l);*/
    
    /**/
    /*
    GameManager::CheatButton()

    NAME

            GameManager::CheatButton - Called when hidden cheat button is pressed

    SYNOPSIS

            enemy   --> current player's enemy

    DESCRIPTION

            Called by pressing the hidden cheat button.
            Causes the current player to win on this round no matter what
            Used for testing win functionality without having to play a full game

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/30/20

    */
    /**/
    public void CheatButton()
    {
        int enemy = GetEnemy();
        playerList[enemy].shipLocations.Clear();
    }/*public void CheatButton();*/
}
/**/ /*** END GameManager ***/ /**/