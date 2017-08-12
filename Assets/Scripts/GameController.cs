using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private int maxSingleDice = 41;
    private int numberOfRedDice = 1;
    private int numberOfBlackDice = 1;
    public InputField numberOfRedInput;
    public InputField numberOfBlackInput;
    public Button rollBtn;
    bool rolled = false;
    // Dice prefab as GameObject to instanciate them dynamically
    public GameObject[] diceModels;

    // To keep the generated dice at launch in object pool then re-use 
    public Stack<GameObject> redDice = new Stack<GameObject>();
    public Stack<GameObject> blackDice = new Stack<GameObject>();

    // Co0rdinates for red dice generation
    Dictionary<string, string> rightSideCoOrdinates = new Dictionary<string, string>(){
            {"xMin", "5.5"},
            {"xMax", "6"},
            {"yMax", "1"},
            {"yMin", "1"},
            {"zMax", "5"},
            {"zMin", "-2.5"}
    };

    // Co0rdinates for black dice generation
    Dictionary<string, string> leftSideCoOrdinates = new Dictionary<string, string>(){
            {"xMin", "-10.5"},
            {"xMax", "-10"},
            {"yMax", "1.5"},
            {"yMin", "0.5"},
            {"zMax", "5"},
            {"zMin", "-2.5"}
    };


    // Use this for initialization
    void Start()
    {

        // Generate the maximum number of dice necessary
        generateDice(maxSingleDice);

    }

    public void generateDice(int num)
    {
        // Generate dice clones max possible amount
        for (int i = 0; i <= num; i++)
        {

            // Pushing the first dice prefab instance the red dice in stack
            // Instanciat the gameobject
            redDice.Push(Instantiate(diceModels[0]));
            // Change the clone name for future use
            redDice.Peek().name = "redDice";
            // Set initial state to in-active
            redDice.Peek().SetActive(false);

            // Pushing the first dice prefab instance the red dice in stack
            // Instanciat the gameobject
            blackDice.Push(Instantiate(diceModels[1]));
            // Change the clone name for future use
            blackDice.Peek().name = "blackDice";
            // Set initial state to in-active
            blackDice.Peek().SetActive(false);
        }


    }

    void Update()
    {
        // Detect Key Press 'A' after Roll
        if( Input.GetKeyDown(KeyCode.A) && rolled == true)
        {
            numberOfBlackInput.gameObject.SetActive(true);
            numberOfRedInput.gameObject.SetActive(true);
            rollBtn.gameObject.SetActive(true);
            rolled = false;
        }

    }

    // Roll button click event
    public void throughDice()
    {
        // Hide the text fields and button on roll
        if (numberOfRedInput.text != "" || numberOfBlackInput.text != "")
        {
            numberOfBlackInput.gameObject.SetActive(false);
            numberOfRedInput.gameObject.SetActive(false);
            rollBtn.gameObject.SetActive(false);
            rolled = true;
        }
        
        // Grabbing all the active dice before re-roll
        var activeRed = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "redDice");
        var activeBlack = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "blackDice");

        // Destroy old dice on reroll and add new dice to object pool
        foreach(GameObject o in activeRed)
        {
            if (o.activeInHierarchy)
            {
                Destroy(o);

                // Pushing the first dice prefab instance the red dice in stack
                // Instanciat the gameobject
                redDice.Push(Instantiate(diceModels[0]));
                // Change the clone name for future use
                redDice.Peek().name = "redDice";
                // Set initial state to in-active
                redDice.Peek().SetActive(false);
            }
        }

        // Destroy old dice on reroll and add new dice to object pool
        foreach (GameObject o in activeBlack)
        {
            if (o.activeInHierarchy)
            {
                Destroy(o);

                // Pushing the first dice prefab instance the red dice in stack
                // Instanciat the gameobject
                blackDice.Push(Instantiate(diceModels[1]));
                // Change the clone name for future use
                blackDice.Peek().name = "blackDice";
                // Set initial state to in-active
                blackDice.Peek().SetActive(false);
            }
        }

    
        // Check if the text field is empty to handle error
        if (numberOfRedInput.text != "")
        {
            // Checking if the number given is higher the max value
            if (int.Parse(numberOfRedInput.text) < maxSingleDice)
            {
                // Grab user input from text field
                numberOfRedDice = int.Parse(numberOfRedInput.text);

                // Spawning dice as asked 
                for (int i = 0; i < numberOfRedDice; i++)
                {

                    GameObject temp = redDice.Pop();
                    temp.SetActive(true);
                    temp.transform.position = new Vector3(Random.Range(float.Parse(leftSideCoOrdinates["xMax"]), float.Parse(leftSideCoOrdinates["xMin"])),
                        Random.Range(float.Parse(leftSideCoOrdinates["yMax"]), float.Parse(leftSideCoOrdinates["yMin"])),
                        Random.Range(float.Parse(leftSideCoOrdinates["zMax"]), float.Parse(leftSideCoOrdinates["zMin"]))
                    );
                    // Add force and torque to dice
                    temp.GetComponent<Rigidbody>().AddForce(new Vector3((Random.Range(9.5f, 3.5f)), 0, 0), ForceMode.Impulse);
                    temp.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * Random.Range(18, 8), ForceMode.Impulse);

                }
            }

        }

        // Check if the text field is empty to handle error
        if (numberOfBlackInput.text != "")
        {
            // Check if the number given is higher the max value
            if (int.Parse(numberOfBlackInput.text) < maxSingleDice)
            {
                // Grab user input from text field
                numberOfBlackDice = int.Parse(numberOfBlackInput.text);
                
                // Spawning dice as asked 
                for (int i = 0; i < numberOfBlackDice; i++)
                {

                    GameObject temp = blackDice.Pop();
                    temp.SetActive(true);
                    temp.transform.position = new Vector3(Random.Range(float.Parse(rightSideCoOrdinates["xMax"]), float.Parse(rightSideCoOrdinates["xMin"])),
                    Random.Range(float.Parse(rightSideCoOrdinates["yMax"]), float.Parse(rightSideCoOrdinates["yMin"])),
                    Random.Range(float.Parse(rightSideCoOrdinates["zMax"]), float.Parse(rightSideCoOrdinates["zMin"]))
                    );
                    // Add force and torque to dice
                    temp.GetComponent<Rigidbody>().AddForce(new Vector3(-(Random.Range(9.5f, 3.5f)), 0, 0), ForceMode.Impulse);
                    temp.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * Random.Range(18, 8), ForceMode.Impulse);

                }
            }

        }

    }

}


