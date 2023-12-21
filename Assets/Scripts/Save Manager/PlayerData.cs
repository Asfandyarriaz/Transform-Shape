using UnityEngine;
/// <summary>
/// Stores All player data 
/// Any new data field entered must also be implemented in Save Manager
/// </summary>
[System.Serializable]
public class PlayerData : MonoBehaviour
{
    //Vehicles
    private int characterLevel; 
    private int carLevel;
    private int tankLevel;
    private int boatLevel;
    private int airplaneLevel;
    private int scooterLevel;

    //Cash
    private int totalCash;
    //Constructors
    PlayerData(int character,int car, int tank, int boat, int airplane, int scooter, int cash)
    {
        characterLevel = character;
        carLevel = car;
        tankLevel = tank;
        boatLevel = boat;
        airplaneLevel = airplane;
        scooterLevel = scooter;
        totalCash = cash;
    }
    PlayerData(PlayerData obj)
    {
        characterLevel = obj.characterLevel;
        carLevel = obj.carLevel;
        tankLevel = obj.tankLevel;
        boatLevel = obj.boatLevel;
        airplaneLevel = obj.airplaneLevel;
        scooterLevel = obj.scooterLevel;
        totalCash = obj.totalCash;
    }
    PlayerData() { }

    //Void Setters And Getters
    public void SetCharacter(int character)
    {
        characterLevel = character;
    }
    public int GetCharacterLevel()
    {
        return characterLevel;
    }

    public void SetCarLevel(int car)
    {
        carLevel = car;
    }
    public int GetCarLevel()
    {
        return carLevel;
    }

    public void SetTankLevel(int tank)
    {
        tankLevel = tank;
    }
    public int GetTankLevel()
    {
        return tankLevel;
    }

    public void SetBoatLevel(int boat)
    {
        boatLevel = boat;
    }
    public int GetBoatLevel()
    {
        return boatLevel;
    }

    public void SetAirplaneLevel(int airplane)
    {
        airplaneLevel = airplane;
    }
    public int GetAirplaneLevel()
    {
        return airplaneLevel;
    }

    public void SetScooterLevel(int scooter)
    {
        scooterLevel = scooter;
    }
    public int GetScooterLevel()
    {
        return scooterLevel;
    }

    public void SetTotalCash(int cash)
    {
        totalCash = cash;
    }
    public int GetTotalCash()
    {
        return totalCash;
    }
}
