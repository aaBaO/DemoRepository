using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

 [RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour {

	private Button backButton;

	void Start () {
		backButton = gameObject.GetComponent<Button>();			
		backButton.onClick.AddListener(()=> SceneManager.LoadSceneAsync("MainMenu"));
	}
	
}
