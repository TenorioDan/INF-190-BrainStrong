using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class CharacterInputHandler : MonoBehaviour {
	public float movementSpeed = 20.0f;
	public float boostSpeed = 100.0f;
	public float boostTime = 0.75f;
	public float boostCurrentTime = 0.0f;
	public float boostCoolDownTime = 5.0f;
	public float boostCurrentCoolDownTime = 0.0f;
	public float rotateSpeed = 90.0f;
	public float movementSpeed_Rotation = 360.0f;
	public float weaponDamage = 10.0f;
	public float fireRate = 0.15f;
	public float shieldCharge = 200.0f;
	public float maxShieldCharge = 200.0f;
	public float maxHealth = 200.0f;
	public float health = 200.0f;
	public float boostDamage = 500.0f;
	public GameObject laser;
	public GameObject weapon;
	public GameObject triShot1;
	public GameObject triShot2;
	public GameObject triShot3;
	public Texture boostIcon;

	public Image shieldImage;
	public Image jumpImage;
	public Image spreadImage;
	
	private float nextFire = 0.0F;
	private float previousRotAngle;
	private float currentRotAngle;
	private bool boosting = false;
	private bool boostOnCoolDown = false;
	private bool usingTriShot = false;

	private AudioSource audio;
	
	public bool hasFragment;
	public bool hasDamagingBoost;
	private float fragReadTimer;
	private int startCounter = 0;

	private bool jumpUpgrade;
	private bool spreadUpgrade;
	private bool shieldUpgrade;

	private string filepath;
	XmlDocument xmlDoc;
	
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();		
		boostCurrentTime = Time.time;
		boostCurrentCoolDownTime = Time.time;
		Cursor.lockState = CursorLockMode.Locked;
		System.Drawing.Point mousePoint = new System.Drawing.Point(Screen.width/2, 250);
		System.Windows.Forms.Cursor.Position = mousePoint;
		float x = Input.mousePosition.x - Screen.width / 2;
		float y = 250;
		currentRotAngle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;
		LoadXML();
		if(!shieldUpgrade) {
			shieldImage.sprite = Resources.Load<Sprite>("Sprites/lockedshield");
		}
		else {
			shieldImage.fillAmount = 0f;			
		}
		if(!jumpUpgrade) {
			jumpImage.sprite = Resources.Load<Sprite>("Sprites/lockedjump");
		}
		if(!spreadUpgrade) {
			spreadImage.sprite = Resources.Load<Sprite>("Sprites/lockedspread");
		}
		else {
			spreadImage.sprite = Resources.Load<Sprite>("Sprites/spreadsingle");
		}
	}
	
	void Awake(){

		hasFragment = false;
		fragReadTimer = 5.0f;
	}
	
	void FixedUpdate()
	{
		if(hasFragment)
		{
			if(fragReadTimer >= 0)
				fragReadTimer -= Time.deltaTime;
			else 
			{
				fragReadTimer = 5.0f;
				movementSpeed += 10.0f;
				hasFragment = false;
				GameObject.Find ("Cell").GetComponent<GameManager>().numFragments++;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		startCounter++;

		if (GameManager.gameStarted) {
			Cursor.lockState = CursorLockMode.None;
			Attacks ();
			Move ();
			CoolDowns ();
			if(shieldUpgrade) {
				shieldImage.color = Color.gray;
				shieldImage.fillAmount = 1 - shieldCharge/200f;
			}
		} 
		Aim ();
	}
	
	public void Aim(){
		
		if (!boosting) {
			previousRotAngle = currentRotAngle;
			float x = Input.mousePosition.x - Screen.width / 2;
			float y = Input.mousePosition.y - Screen.height / 2;
			currentRotAngle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;

			if (GameManager.gameStarted)
				weapon.transform.RotateAround (transform.position, -this.transform.up, currentRotAngle - previousRotAngle);
		}
	}
	
	public void Move(){
		
		if (!boosting) {
			Vector3 perpedicular = Vector3.Cross (transform.forward, transform.up);
			transform.RotateAround (GameManager.sphereCenter, -perpedicular, Input.GetAxis ("Vertical") * movementSpeed * Time.deltaTime);
			Camera.main.transform.RotateAround (GameManager.sphereCenter, -perpedicular, Input.GetAxis ("Vertical") * movementSpeed * Time.deltaTime);
			
			perpedicular = Vector3.Cross (transform.right, transform.up);
			transform.RotateAround (GameManager.sphereCenter, -perpedicular, Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime);
			Camera.main.transform.RotateAround (GameManager.sphereCenter, -perpedicular, Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime);
		}
		else {
		
			Vector3 perpedicular = Vector3.Cross (weapon.transform.forward, transform.up);
			transform.RotateAround (GameManager.sphereCenter, -perpedicular, boostSpeed * Time.deltaTime);
			Camera.main.transform.RotateAround (GameManager.sphereCenter, -perpedicular, boostSpeed * Time.deltaTime);
			
			if (Time.time - boostCurrentTime > boostTime){
				boosting = false;
				boostOnCoolDown = true;
				boostCurrentCoolDownTime = Time.time;
			}
		}
	}
	
	public void Attacks(){
		
		if (Input.GetButton("Fire1") && !boosting && Time.time > nextFire) {
			if(!audio.isPlaying) {
				audio.Play();
			}
			if (!usingTriShot){
				Instantiate(laser, transform.position, weapon.transform.rotation);
			}
			else{
				Instantiate(laser, transform.position, triShot1.transform.rotation);
				Instantiate(laser, transform.position, triShot2.transform.rotation);
				Instantiate(laser, transform.position, triShot3.transform.rotation);
			}
			//Instantiate(laser, transform.position, weapon.transform.rotation);
			nextFire = Time.time + fireRate;
		} 
		else if(Input.GetKeyUp(KeyCode.Mouse0)) {
			if(audio.isPlaying) {
				audio.Stop();
			}
		}

		if (Input.GetKey (KeyCode.R) && spreadUpgrade) {
			if(usingTriShot) {
				spreadImage.sprite = Resources.Load<Sprite>("Sprites/spreadsingle");
			}
			else {
				spreadImage.sprite = Resources.Load<Sprite>("Sprites/spreadshot");
			}
			usingTriShot = !usingTriShot;
		}
		
		if (Input.GetButton ("Jump") && !boosting && !boostOnCoolDown && jumpUpgrade) {
			boosting = true;
			boostCurrentTime = Time.time;
		}
		
		if (Input.GetKey (KeyCode.LeftShift) && !boosting && shieldCharge > 0 && shieldUpgrade) {
			shieldCharge -= 1.5f;
			
			//add the shield visual and the rigid body to push enemies away
			GameObject shield = GameObject.Find("Shield");
			shield.GetComponent<MeshRenderer>().enabled = true;
			shield.GetComponent<SphereCollider>().enabled = true;
		} 
		else if ((!Input.GetKey (KeyCode.LeftShift) && shieldCharge < maxShieldCharge) || shieldCharge <= 0  && shieldUpgrade) {
			GameObject shield = GameObject.Find("Shield");
			shield.GetComponent<MeshRenderer>().enabled = false;
			shield.GetComponent<SphereCollider>().enabled = false;
			
			shieldCharge += 1.0f;
			if (shieldCharge > maxShieldCharge){
				shieldCharge = maxShieldCharge;
			}
		}


	}
	
	//check what abilities are on cooldown
	//if any are on cooldown, check if they have exceeded their cooldown time and reset
	public void CoolDowns(){
		if (boostOnCoolDown) {
			if (Time.time - boostCurrentCoolDownTime >= boostCoolDownTime){
				boostOnCoolDown = false;
				jumpImage.color = Color.white;
				jumpImage.fillAmount = 1f;
			}
			else {
				jumpImage.color = Color.gray;
				jumpImage.fillAmount = (Time.time - boostCurrentCoolDownTime)/boostCoolDownTime;
			}
		}
	}
	
	private void TakeDamage (float damage){
		health -= damage;
		
		if (health > maxHealth) {
			health = maxHealth;
		}
		else if (health < 0) {
			health = 0;
		}
	}
	
	//gui associated with the player
	void OnGUI(){
		/*
		if (!boostOnCoolDown) {
			GUI.DrawTexture(new Rect(10, 100, 50, 50), boostIcon, ScaleMode.StretchToFill, true, 10.0f);
		}

		GUI.Box(new Rect(10, 10, health, 20), health / 2 + "/100");
		GUI.Box(new Rect(10, 75, shieldCharge, 20), "SHIELD");
		*/
	}
	
	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Enemy") {

			if (hasDamagingBoost && boosting){
				collision.gameObject.GetComponent<BasicEnemyBehavior>().TakeDamage(boostDamage);
			}
			else{
				TakeDamage(10);
			}
		}
	}
	
	
	public bool PickUpFragment()
	{
		if(!hasFragment)
		{
			hasFragment = true;
			movementSpeed -= 10.0f;
			return true;
		}
		return false;
	}


	void LoadXML()
	{
		filepath = Application.streamingAssetsPath + @"/XML/PlayerInfo.xml";
		//Debug.Log("Loading XML for levels");
		if (File.Exists (filepath)) {
			//Debug.Log("Found XML file");
			xmlDoc = new XmlDocument ();
			try {
				xmlDoc.Load (filepath);
			} catch (FileNotFoundException) {
				Debug.Log ("The file for loading the XML was not found");
				return;
			}

			XmlNodeList currentNode = xmlDoc.GetElementsByTagName ("Upgrade");
			
			foreach (XmlNode upgrade in currentNode) 
			{
				if(upgrade.Attributes["Name"].Value == "Jump Forward") {
					if(int.Parse(upgrade.Attributes["unlocked"].Value) == 1) {
						jumpUpgrade = true;
					}
				}
				else if(upgrade.Attributes["Name"].Value == "Shield") {
					if(int.Parse(upgrade.Attributes["unlocked"].Value) == 1) {
						shieldUpgrade = true;
					}
				}
				else if(upgrade.Attributes["Name"].Value == "Spread Shot") {
					if(int.Parse(upgrade.Attributes["unlocked"].Value) == 1) {
						spreadUpgrade = true;						
					}
				}
			}
		}
	}

}
