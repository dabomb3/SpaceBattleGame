/**/ /*** BEGIN Menu ***/ /**/
/**/
/*
Class Menu

NAME

        class Menu

SYNOPSIS

        HelpDialog   --> The Help dialog popup on the menue screen, GameObject object

DESCRIPTION

        This class provides the main menu functionality. The member functions are called by OnClick events attached to buttons
        in the MainMenu scene as well as on the end of game menus.
        Functionality for getting help, restarting the round, going back to the main menu, starting a new game, and quitting the game is provided.

AUTHOR

        Thomas Piacentini

DATE

        8/10/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject HelpDialog;

    /**/
    /*
    Menu::Start()

    NAME

            Menu::Start - Houses the code to be run at the start of the game.

    SYNOPSIS

            

    DESCRIPTION

            Start() is called at the beginning of the game and makes sure the help dialog is not visible until it is requested.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/10/20

    */
    /**/
    public void Start()
    {
        HelpDialog.SetActive(false);
    }/*void Start();*/

    /**/
    /*
    Menu::RestartRound()

    NAME

            Menu::RestartRound - Restarts the current round.

    SYNOPSIS

            

    DESCRIPTION

            RestartRound() is called when the Restart Round button is pressed on the game over screen. 
            The current Active Scene is loaded, thus restarting the scene.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/10/20

    */
    /**/
    public void RestartRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }/*void RestartRound()*/

    /**/
    /*
    Menu::BackButton()

    NAME

            Menu::BackButton - Brings the player back to the main menu.

    SYNOPSIS

            

    DESCRIPTION

            BackButton() Brings the player back to the main menu.
            The MainMenu scene is loaded upon the BackButton being pressed

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/10/20

    */
    /**/
    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }/*void BackButton()*/

    /**/
    /*
    Menu::StartGame(string gameMode)

    NAME

            Menu::StartGame - Starts the given game mode corresponding to the game mode button pressed.

    SYNOPSIS

            void Menu::StartGame(string gameMode)
                gameMode    --> the gameMode to be started, string

    DESCRIPTION

            StartGame() is called when the SinglePlayer, Local 1v1, and Online 1v1 buttons are pressed in the Main Menu scene.
            The functions are triggered by the OnClick Events attached to the corresponding buttons, and the game mode is specified by a string
            which is sent by the OnClick event.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/10/20

    */
    /**/
    public void StartGame(string gameMode)
    {
        SceneManager.LoadScene(gameMode);
    }/*public void SartGame(string gameMode);*/

    /**/
    /*
    Menu::HelpButton()

    NAME

            Menu::HelpButton - Activates the help dialog on the main menu

    SYNOPSIS

            

    DESCRIPTION

            HelpButton() is called by the HelpButton OnClick event.
            The HelpDialog GameObject is set to active which makes it appear on the screen.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/10/20

    */
    /**/
    public void HelpButton()
    {
        HelpDialog.SetActive(true);
    }/*public void HelpButton();*/

    /**/
    /*
    Menu::HideHelp()

    NAME

            Menu::HideHelp - Hides the Help dialog on the main menu

    SYNOPSIS

            

    DESCRIPTION

            HideHelp() is called by the X button on the help dialog. It closes the window so that the player can choose a game mode.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/10/20

    */
    /**/
    public void HideHelp()
    {
        HelpDialog.SetActive(false);
    }/*public void HideHelp();*/

    /**/
    /*
    Menu::QuitGame()

    NAME

            Menu::QuitGame - Terminates the application

    SYNOPSIS

            

    DESCRIPTION

            QuitGame() is called when the Quit button is pressed in the main menu. It calls the Application.Quit function.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            8/14/20

    */
    /**/
    public void QuitGame()
    {
        Application.Quit();
    }/*public void QuitGame();*/
}
/**/ /*** END Menu ***/ /**/