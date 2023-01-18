using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameDataManager
{
    private Vector2Int posTower;
    private int money;
    private int crystals;
    private int stone;

    private event Action _onChangedGameData;
    public event Action OnChangedGameData {
        add
        {
            _onChangedGameData += value;
            _onChangedGameData?.Invoke();
        }

        remove
        {
            _onChangedGameData -= value;
        }
    }


    public Vector2Int PosTower { get => posTower; set => posTower = value; }
    public int Money { get => money; }
    public int Crystals { get => crystals;  }
    public int Stone { get => stone;  }

    public void SaveData()
    {
        GameData gameData = new GameData()
        {
            posTower =posTower,
            stone = Stone,
            crystals = Crystals,
            money=Money
        };
        GameManager.instance.xmlGameDataManager.SaveData(gameData);
    }

    public void LoadData()
    {
        GameData gameData = GameManager.instance.xmlGameDataManager.LoadData();
        posTower = gameData.posTower;
        ChangeCrystals(Operation.Equal, gameData.crystals);
        ChangeMoney(Operation.Equal, gameData.money);
        ChangeStone(Operation.Equal, gameData.stone);
    }

    public void LoadDefaultData()
    {
        GameData gameData = GameManager.instance.xmlDefaultGameDataManager.LoadData();
        posTower = gameData.posTower;
        ChangeCrystals(Operation.Equal, gameData.crystals);
        ChangeMoney(Operation.Equal, gameData.money);
        ChangeStone(Operation.Equal, gameData.stone);
    }


    public void ChangeMoney(Operation op,int value)
    {
        switch (op)
        {
            case Operation.Add:
                money += value;
                break;
            case Operation.Reduce:
                money -= value;
                break;
            case Operation.Equal:
                money = value;
                break;
        }
        _onChangedGameData?.Invoke();
    }

    public void ChangeCrystals(Operation op, int value)
    {
        switch (op)
        {
            case Operation.Add:
                crystals += value;
                break;
            case Operation.Reduce:
                crystals -= value;
                break;
            case Operation.Equal:
                crystals = value;
                break;
        }
        _onChangedGameData?.Invoke();
    }
    public void ChangeStone(Operation op, int value)
    {
        switch (op)
        {
            case Operation.Add:
                stone += value;
                break;
            case Operation.Reduce:
                stone -= value;
                break;
            case Operation.Equal:
                stone = value;
                break;
        }
        _onChangedGameData?.Invoke();
    }
}
