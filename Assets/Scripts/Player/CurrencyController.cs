using System;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController Instance;

    [SerializeField] private int startingMoney;
    private int playerMoney = 100;
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            playerMoney = startingMoney;
        }

    }

    public int GetMoney() => playerMoney;

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            OnMoneyChanged?.Invoke(playerMoney);
            return true;  //money has been spent
        }
        return false;  //not enough money
    }   

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        OnMoneyChanged?.Invoke(playerMoney);
    }

    public void SetMoney(int amount)
    {
        playerMoney = amount;
        OnMoneyChanged?.Invoke(playerMoney);
    }



}
