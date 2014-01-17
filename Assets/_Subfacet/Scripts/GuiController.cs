using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour {

	public GameObject guiTemplate;
	public Font guiFont;

	private GUIText guiPlayerHealth;
	private GUIText guiEnemyInfo;
	private GUIText guiPlayerWeapon;
	private GUIText guiCrosshair;
	private GUIText guiScore;

	private GameObject player = null;

	void Start () {
		FindPlayer();

		if (! guiTemplate) {
			enabled = false;
			return;
		}

		guiPlayerHealth = makeGui("Health", TextAlignment.Left, TextAnchor.LowerLeft);
		guiEnemyInfo = makeGui("EnemyInfo", TextAlignment.Right, TextAnchor.UpperRight);
		guiPlayerWeapon = makeGui("Weapon", TextAlignment.Left, TextAnchor.UpperLeft);
		guiCrosshair = makeGui("Crosshair", TextAlignment.Center, TextAnchor.MiddleCenter);
		guiScore = makeGui("Score", TextAlignment.Right, TextAnchor.LowerRight);
	}

	void OnGUI () {
		if (player == null) {
			FindPlayer();
		}

		if (player) {
			WeaponInventory inventory = player.GetComponent<WeaponInventory>();
			CanShoot shooter = player.GetComponent<CanShoot>();
			SetWeaponText(inventory, shooter);

			HasHealth playerHealth = player.GetComponent<HasHealth>();
			guiPlayerHealth.text = "HP: " + playerHealth.health;

			guiCrosshair.text = "+";

			if (EnemySpawner.active) {
				guiEnemyInfo.text = "Enemies: " + EnemySpawner.active.enemyCount + "\n" + "Wave: " + EnemySpawner.active.wave;
			}

			if (ScoreController.active) {
				SetScoreText();
			}
		} else {
			guiPlayerWeapon.text = "Dead!";
			guiPlayerHealth.text = "";
			guiCrosshair.text = "Press [R] to respawn";
			guiEnemyInfo.text = "";
			guiScore.text = "";
		}
	}
    
    private void FindPlayer() {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void SetWeaponText(WeaponInventory inventory, CanShoot shooter) {
		string txt = "";
		txt += inventory.weapons[inventory.currentWeaponNumber].name;
		
		if (! shooter.canShoot) {
			txt += "\n";
			txt += "Reloading: ";
			
			float reloadTime = 1.0f / inventory.weapons[inventory.currentWeaponNumber].shotsPerSecond;
			float elapsedPercentTime = shooter.weapon.reloadTimeNeeded / reloadTime;
			
			int totalDots = 6;
			txt += "[";
			for (int i=1; i<=totalDots; i++) {
				if (elapsedPercentTime <= (1.0f * i) / (totalDots + 1)) {
					txt += "*";
				} else {
					txt += " ";
				}
			}
			txt += "]";
		}
		
		guiPlayerWeapon.text = txt;
	}

	private void SetScoreText() {
		string txt = "";
		txt += "Score: " + ScoreController.active.points;
		txt += "\n";
		txt += "Money: " + ScoreController.active.money;
		guiScore.text = txt;
	}

	private GUIText makeGui(string guiName, TextAlignment alignment, TextAnchor anchor) {
		GameObject guiObject = Instantiate(guiTemplate, Vector3.zero, Quaternion.identity) as GameObject;
		GUIText output = guiObject.GetComponent<GUIText>();
		
		output.text = guiName;
		output.alignment = alignment;
		output.anchor = anchor;
		output.font = guiFont;
		
		float margin = 0.02f;
		Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
		switch (anchor) {
		case TextAnchor.LowerLeft:
			pos.x = margin;
			pos.y = margin;
			break;
		case TextAnchor.LowerCenter:
			pos.x = 0.5f;
			pos.y = margin;
			break;
		case TextAnchor.LowerRight:
			pos.x = 1.0f - margin;
			pos.y = margin;
			break;
		case TextAnchor.MiddleLeft:
			pos.x = margin;
			pos.y = 0.5f;
			break;
		case TextAnchor.MiddleCenter:
			pos.x = 0.5f;
			pos.y = 0.5f;
			break;
		case TextAnchor.MiddleRight:
			pos.x = 1.0f - margin;
                pos.y = 0.5f;
                break;
            case TextAnchor.UpperLeft:
                pos.x = margin;
                pos.y = 1.0f - margin;
                break;
            case TextAnchor.UpperCenter:
                pos.x = 0.5f;
                pos.y = 1.0f - margin;
                break;
            case TextAnchor.UpperRight:
                pos.x = 1.0f - margin;
                pos.y = 1.0f - margin;
                break;
        }
		guiObject.transform.position = pos;
		guiObject.name = "gui" + guiName;
		guiObject.transform.parent = transform;
        
        return output;
    }
}
