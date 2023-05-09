using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList;
    private int index;

    // Start is called before the first frame update
    private void Start()
    {
        return;
        index = PlayerPrefs.GetInt("CharacterSelected");
        characterList = new GameObject[transform.childCount];
        // Fill the array with our models
        for (int i = 0; i < transform.childCount; i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;
        }
        // We toggle off their renderer
        foreach (GameObject go in characterList)
        {
            go.SetActive(false);
        }
        // We toggle on the selected index
        if (characterList[index])
        {
            characterList[index].SetActive(true);
        }
    }

    public void ToggleLeft()
    {
        // Toggle off the currect model
        characterList[index].SetActive(false);

        index--;
        if (index < 0)
        {
            index = characterList.Length - 1;
        }

        //Toggle on the new model
        characterList[index].SetActive(true);
    }

    public void ToggleRight()
    {
        // Toggle off the currect model
        characterList[index].SetActive(false);

        index++;
        if (index == characterList.Length)
        {
            index = 0;
        }

        //Toggle on the new model
        characterList[index].SetActive(true);
    }
    private void Update()
    {
        // It just for test. you can debug to see what happened.
        if (Input.GetKeyUp(KeyCode.S))
        {
            ConfirmButton();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            Demo_1_onload();
        }
    }
    public void ConfirmButton()
    {
        PlayerData data = new PlayerData();
        //set your variable(s)
        data.color = Color.red;
        data.data1 = 1;
        data.data2 = "something";
        data.data3 = 10.5f;
        data.Save(index);
        PlayerPrefs.SetInt("CharacterSelected", index);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Demo_1");
    }
    public void Demo_1_onload()
    {
        if (PlayerPrefs.HasKey("CharacterSelected"))
        {
            int index = PlayerPrefs.GetInt("CharacterSelected");
            PlayerData data = new PlayerData();
            if (data.Load(index))
            {
                //you have all data(s) now.
            }
        }
        else
        {
            //return to charachter select scene
        }
    }
}
