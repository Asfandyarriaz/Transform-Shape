﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {
	public int PlayerGold = 2000;
	public int PlayerCash = 2000;
	public int CurrentPlayerLevel=1;
	public int CurrentMode=1;
	public int TotalLevels=20;

	public List<DataContainerCar> CarsList;
	public List<PlayerEnvironmentDefaultData> envioronmentList;


}
