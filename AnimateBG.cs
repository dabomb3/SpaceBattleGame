/**/ /*** BEGIN AnimateBG ***/ /**/
/**/
/*
Class AnimateBG

NAME

        class AnimateBG

SYNOPSIS

        speed   --> The speed of the animation, floating point var
        offset  --> The offset of the next image in the animation, floating point var
        U       --> Whether the animation is going to move on X-Axis (Required by Unity to automate animation), bool var
        V       --> Whether the animation is going to move on Y-Axis (Required by Unity to automate animation), bool var
        mat     --> Material to be animated, Material object

DESCRIPTION

        This class handles the background animation of the Tiles on the players' boards. The "starfield" material is animated to
        look like the ships are moving through space.

AUTHOR

        Thomas Piacentini

DATE

        6/10/20

*/
/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimateBG : MonoBehaviour
{

    public float speed = 0.5f;
    float offset;

    //U is X cord
    public bool U = false;
    //V is Y cord
    public bool V = false;
    Material mat;
    
    /**/
    /*
    AnimateBG::Start()

    NAME

            AnimateBG::Start - Houses the code to be run when the game starts.

    SYNOPSIS

            

    DESCRIPTION

            Start() is called automatically when the game starts.
            This function gets the Material component inserted in the inspector to be used for the animation.

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    void Start()
    {
        mat= GetComponent<MeshRenderer>().material;
    }/*void Start();*/
    
    /**/
    /*
    AnimateBG::Update()

    NAME

            AnimateBG::Update - Houses the code to be run every frame of the game.

    SYNOPSIS

            

    DESCRIPTION

            Update() is called automatically for every frame of the game that is rendered.
            offset is found by taking the current time and multiplying it by the predetermined speed, then modulo'ed by 1 to get a decimal
            If U and V are set to true in the inspector, the animation will move diagonally
            If U is true and V is false, the animation will move vertically.
            If U is false and V is true, the animation will move horizontally

    RETURNS

            Void

    AUTHOR

            Thomas Piacentini

    DATE

            6/10/20

    */
    /**/
    void Update()
    {
        offset = Time.time * speed % 1;
        if(U && V)
        {
            mat.mainTextureOffset = new Vector2(offset, offset);
        }
        else if(U)
        {
            mat.mainTextureOffset = new Vector2(offset, 0);
        }
        else if(V)
        {
            mat.mainTextureOffset = new Vector2(0, offset);
        }
    }/*void Update();*/
}
/**/ /*** END AnimateBG ***/ /**/