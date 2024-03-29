﻿
using System.Collections.Generic;

[System.Serializable]
public class PlayerDataSerializeable  {
	public int PlayerGold = 0;

    public int characterLevel=1;
    public int carLevel=1;
    public int tankLevel=1;
    public int boatLevel=1;
    public int airplaneLevel=1;
    public int scooterLevel=1;
    public int currentLevel=0;

    //Vehicle Unlock Progression 
    public float boatUnlockProgress = 0;
    public float tankUnlockProgress = 0;
    public float planeUnlockProgress = 0;
    public float scooterUnlockProgress = 0;
    public float airplaneUnlockProgress = 0;
    public float gliderUnlockProgress = 0;

    //UI Setting
    public bool isSoundAllow = true;
    public bool isMusicAllow = true;
    public bool isVibrationAllow = true;
    public string playerName;

    //Transform Remake List End
    //------------------------------------------------
    public int PlayerCash = 100;
	public int Rank =1;
	public int xpoints =0;
	public List<PlayerCar> gunsList;
	public List<int> StarsList;



	public int CurrentSelectedPrimaryGun = 0;
    public int CurrentSelectedSecondaryGun = 1;
    public int SelectedVehicle_temp = 0;

    public int CurrentMode=1;
	public int CurrentEnvironment=1;
	public int SelectedControl=0;
	public bool isSoundOn= true;
	public bool isPromoClicked= false;

	public int currentSelectLevel_Mode2 = 1;
    public int currentSelectLevel_Mode3 = 1;
    public int currentSelectLevel_Mode1 = 1;
    public int currentSelectLevel_ExpertMode = 1;
    public int currentSelectLevel_FlyingMode = 1;
    public int currentSelectLevel_UnderWaterMode = 1;

    public int BestKill = 0;
    public bool isRateUSDone= false;
	public bool isControlTutorialDone= false;
	public bool buyShotgunTutorial;
	public bool secondModeUnlockCongrats;
	public bool firstTimeTutorial;
	
	public int LastUnlockedLevel_Mode2=1;
    public int LastUnlockedLevel_Mode3 = 1;
    public int LastUnlockedLevel_Mode1 = 1;
    public int LastUnlockedLevel_ExpertMode = 1;
    public int LastUnlockedLevel_FlyingMode = 1;
    public int LastUnlockedLevel_UnderWaterMode = 1;

    public int megaSaleCount = 0;

    public float BestTime = 0;
    public float SensivityValue=1;
    public bool isHighQuality;
    public bool isRemoveAds = false;
    public bool unlockedAllGuns;
    public bool unlockedAllLevels;
    public bool unlockEverything;

    public bool FirstTimeReward;
    public long lastChestOpened;
}
