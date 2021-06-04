using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    private string[] alphabet = {"F", "-", "+", "[", "]", "X" };
    private List<TurtleMovement.Instruction> setOfInstructions;
    private string currentWord;
    private int actualNumberOfWords;

    public GameObject turtle;

    [Header("Rules")]
    public string[] keysToRules;
    public string[] valuesToRules;

    public Dictionary<string, string> rules;
    public Dictionary<string, TurtleMovement.Instruction> stringsToInstructions;

    [Space]

    [Header("Parameters")]
    public string initialWord;
    public int numberOfWords;
    

    private void Awake()
    {
        setOfInstructions = new List<TurtleMovement.Instruction>();
        rules = new Dictionary<string, string>();
        stringsToInstructions = new Dictionary<string, TurtleMovement.Instruction>();
        currentWord = initialWord;
        actualNumberOfWords = numberOfWords;

        if(keysToRules.Length == valuesToRules.Length)
        {
            for (int i = 0; i < keysToRules.Length; i++)
            {
                rules.Add(keysToRules[i], valuesToRules[i]);
            }
        }
        else
        {
            Debug.LogError("Keys and Values to Rules are not the same size!");
        }


        stringsToInstructions.Add(alphabet[0], TurtleMovement.Instruction.FORWARD);
        stringsToInstructions.Add(alphabet[1], TurtleMovement.Instruction.LEFT);
        stringsToInstructions.Add(alphabet[2], TurtleMovement.Instruction.RIGHT);
        stringsToInstructions.Add(alphabet[3], TurtleMovement.Instruction.PUSH);
        stringsToInstructions.Add(alphabet[4], TurtleMovement.Instruction.POP);
        stringsToInstructions.Add(alphabet[5], TurtleMovement.Instruction.IGNORE);
    }

    private void FixedUpdate()
    {
        /*
        if (actualNumberOfWords > 0)
        {
            FractalCreationManagement();
            actualNumberOfWords--;
            Debug.Log(currentWord);
        }
        else
        {
            Debug.Log("Words ended!");
        }
        */
    }

    public void FractalCreationManagement()
    {
        PerformWord();
        TransferToNewWord();
    }

    public void PerformWord()
    {
        setOfInstructions.Clear();
        foreach(char c in currentWord)
        {
            setOfInstructions.Add(stringsToInstructions[c.ToString()]);
        }

        turtle.GetComponent<TurtleMovement>().PerformSetOfInstructions(setOfInstructions);
    }

    public void TransferToNewWord()
    {
        string newWord = null;
        string letter = null;

        foreach(char c in currentWord)
        {
            if(newWord != null)
            {
                if (rules.TryGetValue(c.ToString(), out letter))
                {
                    newWord += letter;
                }
                else
                {
                    newWord += c.ToString();
                }
            }
            else
            {
                newWord = rules[c.ToString()];
            }
        }

        currentWord = newWord;

    }



}
