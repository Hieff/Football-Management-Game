using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class titleScreenUI : MonoBehaviour
{

    public UIDocument UI;
    public gameManager gm;
    VisualElement root;
    Button newGameButton, loadGameButton, quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
      UI = GetComponent<UIDocument>();
      root = UI.rootVisualElement;
      gm = GameObject.Find("gameManager").GetComponent<gameManager>();
      newGameButton = root.Q<Button>("newGameButton");
      loadGameButton = root.Q<Button>("loadGameButton");
      quitGameButton = root.Q<Button>("quitButton");
      loadGameButton.style.display = DisplayStyle.Flex;

      // Add listeners to UI buttons
      newGameButton.clicked += () => newGame();
      loadGameButton.clicked += () => loadGame();
      quitGameButton.clicked += () => quitGame();

      // Check if player has no save data
      if(PlayerPrefs.GetInt("StadiumCapacity") == 0){
        loadGameButton.style.display = DisplayStyle.None;
      }


    }

    void Awake(){
      Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Start a new game
    public void newGame(){
      gm.setLoadType("newGame");
      SceneManager.LoadScene("SampleScene");
    }

    // Load a game
    public void loadGame(){
      gm.setLoadType("loadGame");
      SceneManager.LoadScene("SampleScene");
    }

    // Close game
    public void quitGame(){
      Application.Quit();
    }

}
