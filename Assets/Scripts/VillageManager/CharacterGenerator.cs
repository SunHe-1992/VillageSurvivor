using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator
{

    public static CharacterGenerator Inst;

    List<string> firstNameList = new List<string>();
    List<string> lastNameList = new List<string>();
    public CharacterGenerator()
    {
        this.ReadConfig();
    }
    public void ReadConfig()
    {
        foreach (var data in ConfigManager.table.CharacterName.DataList)
        {
            firstNameList.Add(data.FirstName);
            lastNameList.Add(data.LastName);
        }
    }
    public string GenerateFullName()
    {
        return $"{RandomFirstName()} {RandomLastName()}";
    }
    string RandomFirstName()
    {
        int idx = Random.Range(0, firstNameList.Count);
        return firstNameList[idx];

    }
    string RandomLastName()
    {
        int idx = Random.Range(0, lastNameList.Count);
        return lastNameList[idx];

    }
}