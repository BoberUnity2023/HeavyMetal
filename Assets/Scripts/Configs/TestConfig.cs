using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfig : MonoBehaviour
{
    [SerializeField] private ConfigLevels _levels;
    private void Start()
    {
        TestCount();
        TestStarsForOpen();
        Count(LevelType.Table);
        Count(LevelType.Chairs);
        Count(LevelType.MushroomRed);
        Count(LevelType.Books);
        Count(LevelType.Boards);
    }

    private void TestCount()
    {
        for (int i = 0; i < _levels.Levels.Length; i++)
        {
            if (_levels.Levels[i].Type == LevelType.Pumpkin ||
                _levels.Levels[i].Type == LevelType.MushroomBrown ||
                _levels.Levels[i].Type == LevelType.MushroomRed ||
                _levels.Levels[i].Type == LevelType.Table ||
                _levels.Levels[i].Type == LevelType.Waterlemon)
            {
                if (_levels.Levels[i].NumOfCakesOnStart != 1)
                    Debug.LogError("Config Error: NumOfCakesOnStart! Level: " + i);
            }
        }
    }

    private void TestStarsForOpen()
    {
        int stars = 0;
        for (int i = 0; i < _levels.Levels.Length; i++)
        {
            if (_levels.Levels[i].StarsForOpen < stars ||
                _levels.Levels[i].StarsForOpen > i * 3)
            {
                Debug.LogError("Config Error: StarsForOpen! Level: " + i);
            }
            else
            {
                stars = _levels.Levels[i].StarsForOpen;
            }
        }
    }

    private void Count(LevelType levelType)
    {
        int output = 0;
        for (int i = 0; i < _levels.Levels.Length; i++)
        {
            if (_levels.Levels[i].Type == levelType)
            {
                output += _levels.Levels[i].NumOfCakesOnStart;
            }            
        }
        Debug.Log("Config Info: Count : " + levelType.ToString() + ": " + output.ToString());
    }
}
