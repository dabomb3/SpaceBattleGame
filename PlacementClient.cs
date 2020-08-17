/**/ /* BEGIN PlacementClient */ /**/
/**/
/*
Class PlacementClient

NAME

        class PlacementClient - Holds the functionality for setting up the board

SYNOPSIS

        placingInstance --> the instance for the PlacementClient to be accessed from outside, PlacementClient
        placingMode     --> true if the game is in the setup state, bool
        placemntAllowed --> true if the ship can be placed at a certain location, bool
        board           --> the board in which the ships are being placed, PlayerBoardGenerator
        placedLayer     --> the layer that the ships are being placed on, LayerMask
        cBtn            --> Button to choose Carrier model for placing, button
        bsBtn           --> Button to choose Battleship model for placing, button
        subBtn          --> Button to choose Submarine model for placing, button
        desBtn          --> Button to choose Destroyer model for placing, button
        crBtn           --> Button to choose Cruiser model for placing, button
        rdyBtn          --> Button to finish setting up board, button
        ghostList       --> List of ships that will be placed, List<PlacementShips>
        current         --> Index for the ship currently being put down
        pointedTile     --> The tile pointed to by the player's cursor
        pointedPos      --> Position for the ship to be placed in

DESCRIPTION

        The PlacementClient class provides the functionality for setting up the players' boards.
        The class takes the location being pointed at by the player and checks if the user can place a ship there and displays the ghost.
        The player can move the ship around based on mouse location.
        Once the location is checked, the player can place the ship, and the class takes care of instantiating the ship GameObjects
AUTHOR

        Thomas Piacentini

DATE

        6/16/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementClient : MonoBehaviour
{
    public static PlacementClient placingInstance;
    bool placingMode;
    bool placementAllowed;
    PlayerBoardGenerator board;
    public LayerMask placedLayer;
    public UnityEngine.UI.Button cBtn;
    public UnityEngine.UI.Button bsBtn;
    public UnityEngine.UI.Button subBtn;
    public UnityEngine.UI.Button desBtn;
    public UnityEngine.UI.Button crBtn;
    public UnityEngine.UI.Button rdyBtn;
    public List<PlacementShips> ghostList = new List<PlacementShips>();
    int current = 0;
    RaycastHit pointedTile;
    Vector3 pointedPos;

    /**/
    /*
    Class PlacementClient::PlacementShips

    NAME

            class PlacementClient::PlacementShips - Holds the ship's ghost and prefab (model)

    SYNOPSIS

            ghost       --> the specific ship's ghost model, GameObject
            prefab      --> the specific ship's prefab model, GameObject
            leftToPlace --> how many of the ship need to be placed, int

    DESCRIPTION

            The PlacementShips class holds the ghost and prefab for each ship that needs to be placed.
            The class needs to be serialized so that the objects can be set in the inspector
    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    [System.Serializable]
    public class PlacementShips
    {
        public GameObject ghost;
        public GameObject prefab;
        public int leftToPlace = 1;
        public int alreadyPlaced = 0;

    }

    /**/
    /*
    PlacementClient::Awake()

    NAME

            PlacementClient::Awake - Called every frame of the game

    SYNOPSIS

            

    DESCRIPTION

            Awake() is called every frame of the game.
            Awake is used to initialize the placingInstance to the current instance to be accessed from outside.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void Awake()
    {
        placingInstance = this;
    }/*void Awake();*/

    /**/
    /*
    PlacementClient::Start()

    NAME

            PlacementClient::Start - Houses the code to be run at the start of the game.

    SYNOPSIS

            

    DESCRIPTION

            Start() is called at the beginning of the game and makes sure none of the ghosts are currently showing.

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
        DisplayGhost(-1);
    }/*void Start();*/

    /**/
    /*
    PlacementClient::SetBoard(PlayerBoardGenerator inBoard, string pType)

    NAME

            PlacementClient::SetBoard - Takes a player's board and gets it ready to be set up

    SYNOPSIS

            inBoard --> the board that is currently being set up, PlayerBoardGenerator
            pType   --> string representation of the PlayerType attributed to the player who is setting up the board

    DESCRIPTION

            SetBoard() takes in a player's board as an argument and sets the board for this instance of the PlacementClient to that player's board.
            The ready button is deactivated so that the player cannot say they are ready before all of the ships have been placed.
            The board is cleared so that there is definitely nothing already placed.
            If the current player is a computer, the ships are placed automatically and the player is marked ready in the GameManager.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public void SetBoard(PlayerBoardGenerator inBoard, string pType)
    {
        board = inBoard;
        rdyBtn.interactable = false;
        ClearBoard();
        if(pType == "COMPUTER")
        {
            AutoFillBoard();
            GameManager.currentInstance.ReadyFun();
        }
    }/*public void SetBoard(PlayerBoardGenerator inBoard, string pType);*/

    /**/
    /*
    PlacementClient::Update()

    NAME

            PlacementClient::Update - Houses the code to be run every frame of the game.

    SYNOPSIS

            r   --> the Ray that is formed by the mouse hovering over a location on the board, Ray

    DESCRIPTION

            Creates a Ray based on the cursor location in order to check if the ship can be placed ther
            If the tile being hovered over belongs the the current player's board, the position is recorded in pointedPos
            If the left mouse button is clicked and the current player is allowed to place a ship, PlaceShip() is called.
            If the right mouse button is clicked and the current player is allowed to place a ship, the ship ghost is rotated.
            ShowGhost is called at the end after all of the information is acquired, happening every frame so that the ghost is always responsive.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void Update()
    {
        if(placingMode)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r, out pointedTile, Mathf.Infinity, placedLayer))
            {
                if(!board.CheckTile(pointedTile.collider.GetComponent<TileBehavior>()))
                {
                    return;
                }
                pointedPos = pointedTile.point;
            }
            if(Input.GetMouseButtonDown(0) && placementAllowed)
            {
                PlaceShip();
            }
            if(Input.GetMouseButtonDown(1) && placementAllowed)
            {
                Rotate();
            }
            ShowGhost();
        }
        
    }/*void Update();*/

    /**/
    /*
    PlacementClient::Rotate()

    NAME

            PlacementClient::Rotate - Rotates the ship ghost.

    SYNOPSIS

            

    DESCRIPTION

            Rotate() rotates the ship ghost when called from Update() when the right mouse button is clicked.
            The ghost is rotated 90 degrees

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void Rotate()
    {
        ghostList[current].ghost.transform.localEulerAngles += new Vector3(0, 90f, 0);
    }/*void Rotate();*/

    /**/
    /*
    PlacementClient::DisplayGhost(int ghostNum)

    NAME

            PlacementClient::DisplayGhost - Changes visibility of ghosts.

    SYNOPSIS

            ghostNum    --> index of ship ghost to display, int

    DESCRIPTION

            DisplayGhost() makes ghosts visibile depending on the current ship being placed.
            Passing -1 makes all of the ghosts invisible
            The ghost in ghostList corresponding to the index is set to active in the Heirarchy.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void DisplayGhost(int ghostNum)
    {
        if(ghostNum != -1)
        {
            //make sure current ghost is not already shown
            if(ghostList[ghostNum].ghost.activeInHierarchy)
            {
                return;
            }
        }
        //make all ghosts invisible
        for(int i = 0; i < ghostList.Count; i++)
        {
            ghostList[i].ghost.SetActive(false);
        }
        if(ghostNum == -1)
        {
            return;
        }
        ghostList[ghostNum].ghost.SetActive(true);
    }/*void DisplayGhost();*/

    /**/
    /*
    PlacementClient::ShowGhost()

    NAME

            PlacementClient::ShowGhost - Shows the current ship ghost that is being placed.

    SYNOPSIS

            

    DESCRIPTION

            ShowGhost() checks if a ship can be placed in a location by calling CheckPlacement() which checks if the whole ship can be placed.
            The ghost is placed over the tiles that the player is hovering over with the cursor

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    void ShowGhost()
    {
        if(placingMode)
        {
            placementAllowed = CheckPlacement();
            ghostList[current].ghost.transform.position = new Vector3(Mathf.Round(pointedPos.x), 0, Mathf.Round(pointedPos.z));
        }
        else
        {
            DisplayGhost(-1);
        }
    }/*void ShowGhost();*/

    /**/
    /*
    PlacementClient::CheckPlacement()

    NAME

            PlacementClient::CheckPlacement() - Changes colors of the ship ghosts depending on if the ship can be placed there or not.

    SYNOPSIS

            

    DESCRIPTION

            CheckPlacement() iterates through the transforms that make up the ship ghosts and check if there is anything under them by calling IsOccupied().
            For all of the spaces that are occupied, the ghost turns red.
            For all of the spaces that are not occupied, the ghost turns green.
            Returns true if the ship can be placed in that position

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    bool CheckPlacement()
    {
        foreach(Transform i in ghostList[current].ghost.transform)
        {
            PlaceLocationBehavior currentGhost = i.GetComponent<PlaceLocationBehavior>();
            if(!currentGhost.IsOccupied())
            {
                currentGhost.GetComponent<MeshRenderer>().material.color = new Color32(255,0,0,127);
                return false;
            }
            currentGhost.GetComponent<MeshRenderer>().material.color = new Color32(166, 254, 0,100);
        }
        return true;
    }/*bool CheckPlacement();*/

    /**/
    /*
    PlacementClient::CheckAutoPlacement(Transform t)

    NAME

            PlacementClient::CheckAutoPlacement - Used when automatically filling the boar

    SYNOPSIS

            t   --> the ship ghost that is being checked to see if a ship can be placed, Transform

    DESCRIPTION

            CheckAutoPlacement() is called when the board is being set up automatically.
            Since no ghosts need to be shown, it just checks if a ship can be placed in a location.
            Returns true if it can be placed.

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            7/6/20

    */
    /**/
    bool CheckAutoPlacement(Transform t)
    {
        foreach(Transform i in t)
        {
            PlaceLocationBehavior currentGhost = i.GetComponent<PlaceLocationBehavior>();
            if(!currentGhost.IsOccupied())
            {
                return false;
            }
            
        }
        return true;
    }/*bool CheckAutoPlacement(Transform t);*/

    /**/
    /*
    PlacementClient::PlaceShip()

    NAME

            PlacementClient::PlaceShip - Places the ship at the location the player requests.

    SYNOPSIS

            pos     --> Position where the ship is to be placed, Vector3
            rotn    --> Rotation of the ship to be placed, Quaternion
            newShip --> The ship model to be placed, GameObject;

    DESCRIPTION

            PlaceShip() instantiates the ship that is being placed using the prefab (model) for that ship.
            Since positions are defined by Vector3 in Unity, the pointedPos is converted to a Vector3.
            Rotations are defined by Quaternions in Unity, so the rotation of the ghost is saved to be used for the model.
            Since the ghost model rotation was off by 90 degrees when I made them, I had to add 270 degrees of rotation to the model in the Y-axis.
            Once a ship has been placed, the button is deactivated so only one can be placed.
            All of the ghosts are hidden.
            CheckForAllShips() is called to see if the ready button needs to be activated.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            7/5/20

    */
    /**/
    void PlaceShip()
    {
        Vector3 pos = new Vector3(Mathf.Round(pointedPos.x), 0, Mathf.Round(pointedPos.z));
        Quaternion rotn = ghostList[current].ghost.transform.rotation;
        //fvisual rotation is off by 90 due to the default rotation of the ghost models i made not matching the ship models
        rotn *= Quaternion.Euler(0,270f,0);
        GameObject newShip = Instantiate(ghostList[current].prefab, pos, rotn);
        newShip.transform.SetParent((board.transform));
        GameManager.currentInstance.addToGrid(ghostList[current].ghost.transform, newShip.GetComponent<ModelBehavior>(), newShip);
        ghostList[current].alreadyPlaced++;
        switch (current)
        {
            case 4:
                cBtn.interactable = false;
                break;
            case 3:
                bsBtn.interactable = false;
                break;
            case 2:
                subBtn.interactable = false;
                break;
            case 1:
                desBtn.interactable = false;
                break;
            case 0:
                crBtn.interactable = false;
                break;
            default:
                break;
        }
        placingMode = false;
        DisplayGhost(-1);
        CheckForAllShips();
    }/*void PlaceShip();*/

    /**/
    /*
    PlacementClient::ShipAlreadyPlaced(int index)

    NAME

            PlacementClient::ShipAlreadyPlaced - Checks if ship at index in ghostList has already been placed.

    SYNOPSIS

            index   --> the index of the ship that is being checked, int

    DESCRIPTION

            ShipAlreadyPlaced() is called when the ship buttons are pressed, to make sure they are not placed multiple times if the buttons arent deactivated for some reason
            Returns true if ship has already been placed

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            6/17/20

    */
    /**/
    bool ShipAlreadyPlaced(int index)
    {
        if(ghostList[index].leftToPlace == ghostList[index].alreadyPlaced)
        {
            return true;
        }
        return false;
    }/*bool ShipAlreadyPlaced(int index);*/

    /**/
    /*
    PlacementClient::ShipButtons

    NAME

            PlacementClient::ShipButtons - Sets the current ship to be placed based on button press

    SYNOPSIS

            index   --> the index of the ship that is being selected, int

    DESCRIPTION

            Checks if selected ship has already been selected.
            Sets current to the index of the ship selected.
            Displays the ghost of the current ship
            Sets playing mode to true.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/6/20

    */
    /**/
    public void ShipButtons(int index)
    {
        if(ShipAlreadyPlaced(index))
        {
            return;
        }

        current = index;
        DisplayGhost(current);
        placingMode = true;
    }/*ShipButtons(int index);*/

    /**/
    /*
    PlacementClient::ClearBoard()

    NAME

            PlacementClient::ClearBoard - Clears all the ships from the board

    SYNOPSIS

            

    DESCRIPTION

            ClearBoard() clears all of the ships from the board as well as calls GameManager.clearList() to make sure that the GameManager knows the board has been cleared.
            Makes sure the buttons to select a ship are active again.
            Makes sure ready button is not active.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            7/18/20

    */
    /**/
    public void ClearBoard()
    {
        //clears ships from board and resets stats
        GameManager.currentInstance.clearList();
        foreach(PlacementShips ship in ghostList)
        {
            ship.alreadyPlaced = 0;
        }
        cBtn.interactable = true;
        bsBtn.interactable = true;
        subBtn.interactable = true;
        desBtn.interactable = true;
        crBtn.interactable = true;
        rdyBtn.interactable = false;
    }/*void ClearBoard();*/

    /**/
    /*
    PlacementClient::CheckForAllShips()

    NAME

            PlacementClient::CheckForAllShips - Checks if all of the ships have been placed

    SYNOPSIS

            

    DESCRIPTION

            CheckForAllShips() iterates through the ghostList and checks how many are left to place
            If any still have a ship left to place, returns false.
            If all the ships have been placed, the ready button is activated and returns true.

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            7/5/20

    */
    /**/
    bool CheckForAllShips()
    {
        foreach(PlacementShips ship in ghostList)
        {
            if(ship.alreadyPlaced != ship.leftToPlace)
            {
                return false;
            }
        }
        rdyBtn.interactable = true;
        return true;
    }

    /**/
    /*
    PlacementClient::AutoFillBoard()

    NAME

            PlacementClient::AutoFillBoard - Automatically places all of the ships

    SYNOPSIS

            decisionMade        --> true if position has been decided for the given ship, false if not, bool
            x                   --> x coordinate of location chosen, int
            y                   --> y coordinate of location chosen, int
            checkRandPos        --> ghost to check the chosen location, GameObject
            possibleRotations   --> Array of rotation values, Vector3[]
            randomIndex         --> List of indexes to be randomly chosen for rotation, List<int>
            randomNum           --> Randomly chosen index to choose rotation, int


    DESCRIPTION

            AutoFillBoard() automattically places all of the ships on the board. Used for when you are lazy and for the Computer player.
            Iterates through the ghostList and for each one picks a random x,y.
            Chooses a random rotation and then uses a ghost to check if the ship can be placed there.
            Calls CheckAutoPlacement and if it can be placed there and calls InstantiateAutoShip().
            Sets all of the ship buttons to inactive and acitvates ready button.

    RETURNS

            void

    AUTHOR

            Thomas Piacentini

    DATE

            6/16/20

    */
    /**/
    public void AutoFillBoard()
    {
        ClearBoard();
        //GameManager.currentInstance.clearList();
        bool decisionMade = false;
        for(int i = 0; i < ghostList.Count; i++)
        {
            for(int j = 0; j < ghostList[i].leftToPlace; j++)
            {
                if(ghostList[i].leftToPlace <= 0)
                {
                    return;
                }
                decisionMade = false;
                while(!decisionMade)
                {
                    current = i;
                    int x = Random.Range(0,10);
                    int y = Random.Range(0,10);
                    //use a ghost to check if a ship can be placed there
                    GameObject checkRandPos = Instantiate(ghostList[current].ghost);
                    checkRandPos.SetActive(true);
                    checkRandPos.transform.position = new Vector3(board.transform.position.x + x, 0, board.transform.position.z + y);
                    Vector3[] possibleRotations = {new Vector3(0,0,0), new Vector3(0,90,0), new Vector3(0,180,0), new Vector3(0,270,0)};
                    for(int k = 0; k < possibleRotations.Length; k++)
                    {
                        List<int> randomIndex = new List<int> {0,1,2,3};
                        int randomNum = randomIndex[Random.Range(0,4)];
                        checkRandPos.transform.rotation = Quaternion.Euler(possibleRotations[randomNum]);
                        if(CheckAutoPlacement(checkRandPos.transform))
                        {
                            InstantiateAutoShip(checkRandPos);
                            decisionMade = true;
                        }
                        else
                        {
                            Destroy(checkRandPos);
                            randomIndex.Remove(k);
                        }
                    }
                }
            }
        }
        rdyBtn.interactable = true;
        cBtn.interactable = false;
        bsBtn.interactable = false;
        subBtn.interactable = false;
        desBtn.interactable = false;
        crBtn.interactable = false;
    }

    /**/
    /*
    PlacementClient::InstantiateAutoShip(GameObject inShip)

    NAME

            PlacementClient::InstantiateAutoShip - Instantiates the ship from AutoFillShips()

    SYNOPSIS

            inShip  --> the ship to be instantiated with position already set

    DESCRIPTION

            InstantiateShip() takes a ship produced in AutoFillShips() and instantiates it using the randomly created position stored in the transform.
            Adds the ship to the player's grid

    RETURNS

            bool

    AUTHOR

            Thomas Piacentini

    DATE

            7/30/20

    */
    /**/
    public void InstantiateAutoShip(GameObject inShip)
    {
        GameObject newShip = Instantiate(ghostList[current].prefab, inShip.transform.position, inShip.transform.rotation);
        newShip.transform.rotation *= Quaternion.Euler(0,270f,0);
        //set parent of ship to be the board so that when the board moves the ships do too.
        newShip.transform.SetParent(board.transform);
        GameManager.currentInstance.addToGrid(inShip.transform, newShip.GetComponent<ModelBehavior>(), newShip);
        ghostList[current].alreadyPlaced++;
        Destroy(inShip);
    }/*public void InstantiateAutoShip(GameObject Inship);*/
}
/**/ /* END PlacementClient */ /**/