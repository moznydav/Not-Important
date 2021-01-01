
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScreen : MonoBehaviour {

    [SerializeField] GameObject[] options;
    [SerializeField] GameObject skipButton;

    [SerializeField] GameObject placeholder1;
    [SerializeField] GameObject placeholder2;
    [SerializeField] GameObject placeholder4;

    private GameObject option1;
    private GameObject option2;
    private GameObject option3;
    private GameObject option4;

    //GameManager gameManager;

    public void Init() {

        int randomIndex = Mathf.RoundToInt(Random.Range(0, options.Length));
        option1 = Instantiate(options[randomIndex], placeholder1.transform);

        randomIndex = Mathf.RoundToInt(Random.Range(0, options.Length));
        option2 = Instantiate(options[randomIndex], placeholder2.transform);

        option4 = Instantiate(skipButton, placeholder4.transform);

    }

    public void Close() {
        Destroy(option1);
        Destroy(option2);
        Destroy(option3);
        Destroy(option4);
    }
}
