using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerType : ScriptableObject 
{
	public string playerTypeName = "Aylmao";
	
	public int maxHealth = 100;
	public int maxHeat = 100;
	public int maxAmmunitionTurret = 100;
	public int turretDamage = 20;
	public int heatRegen = 2;

	
}
