﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelParserStarter : MonoBehaviour
{
    public string filename;

    public GameObject Rock;

    public GameObject Brick;

    public GameObject QuestionBox;

    public GameObject Stone;

    public GameObject Water;

    public GameObject Pill;

    public GameObject Coin;

    public Transform parentTransform;

    //Raycast Stuff
    public float length = 300f;
    public LayerMask mask;

    public Text timer;
    // Start is called before the first frame update
    void Start()
    {
        RefreshParse();
    }


    private void FileParser()
    {
        string fileToParse = string.Format("{0}{1}{2}.txt", Application.dataPath, "/Resources/", filename);

        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            int row = 0;

            while ((line = sr.ReadLine()) != null)
            {
                int column = 0;
                char[] letters = line.ToCharArray();
                //Debug.Log(row + "," + column);
                Vector3 myvector = new Vector3(row, column, 0);
                foreach (var letter in letters)
                {
                    //Call SpawnPrefab
                    SpawnPrefab(letter, new Vector3(column,-row,0));
                    column++;
                }
                row++;

            }

            sr.Close();
        }
    }

    private void SpawnPrefab(char spot, Vector3 positionToSpawn)
    {
        GameObject ToSpawn;

        switch (spot)
        {
            case 'b': 
                ToSpawn = Brick;
                break;
            case 'g': 
                ToSpawn = Coin;
                break;
            case 'x': 
                ToSpawn = Rock;
                break;
            case 's': 
                ToSpawn = Stone;
                break;
            case 'a': 
                ToSpawn = Water;
                break;
            case 'p': 
                ToSpawn = Pill;
                break;
            default: return;
        }

        ToSpawn = GameObject.Instantiate(ToSpawn, parentTransform);
        ToSpawn.transform.localPosition = positionToSpawn;
    }

    public void RefreshParse()
    {
        GameObject newParent = new GameObject();
        newParent.name = "Environment";
        newParent.transform.position = parentTransform.position;
        newParent.transform.parent = this.transform;
        
        if (parentTransform) Destroy(parentTransform.gameObject);

        parentTransform = newParent.transform;
        FileParser();
    }

    
}
