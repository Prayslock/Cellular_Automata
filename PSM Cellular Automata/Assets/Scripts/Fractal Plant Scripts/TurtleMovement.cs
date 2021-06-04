using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurtleMovement : MonoBehaviour
{

    private Stack<Vector3> turtleStack;
    private Stack<Quaternion> turtleStackRotation;
    private Quaternion originalRotation;

    [Header("Controls")]
    public KeyCode key;

    [Space]

    [Header("Movement Parameters")]
    public float forwardLength;
    public float degreesForRight;
    public float degreesForLeft;

    

    public enum Instruction
    {
        FORWARD,
        RIGHT,
        LEFT, 
        PUSH,
        POP,
        IGNORE
    }


    public void MoveForward()
    {
        transform.position = Vector3.Lerp(transform.position, transform.up  * forwardLength, 5);
        
    }

    public void MoveRight()
    {
        transform.Rotate(new Vector3(0, 0, 1), degreesForRight);
    }

    public void MoveLeft()
    {
        transform.Rotate(new Vector3(0, 0, 1), degreesForLeft);
    }

    public void PushPosition()
    {
        turtleStack.Push(transform.position);
        turtleStackRotation.Push(transform.rotation);
    }

    public void PopPosition()
    {
        
        transform.position = turtleStack.Pop();
        transform.rotation = turtleStackRotation.Pop();
    }


    public void ExecuteInstruction(Instruction inst)
    {
        switch(inst)
        {
            case Instruction.FORWARD:
                MoveForward();
                break;

            case Instruction.RIGHT:
                MoveRight();
                break;

            case Instruction.LEFT:
                MoveLeft();
                break;

            case Instruction.PUSH:
                PushPosition();
                break;

            case Instruction.POP:
                PopPosition();
                break;

            case Instruction.IGNORE:
                //Ignore This instruction
                break;

            default:
                Debug.LogWarning("Instruction can't be read!");
                break;
              
        }
    }


    public void PerformSetOfInstructions(List<Instruction> instructions)
    {
        foreach (Instruction i in instructions)
        {
            ExecuteInstruction(i);
        }
    }


    private void Awake()
    {
        turtleStack = new Stack<Vector3>();
        turtleStackRotation = new Stack<Quaternion>();
}

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    
}
