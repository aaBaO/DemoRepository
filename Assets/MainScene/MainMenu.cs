using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	public ScrollRect scrollRect;
	public GameObject buttonTemplete;

	private static Dictionary<string,string> buttonToSceneDictionary = new Dictionary<string, string>(){
		{"BlurTexture", "BlurTexture"},
		{"BlurTexture_性能测试", "BlurTexture_PerformanceTest"},
		{"SRenameDemo", "SRenameDemo"},
		{"iOS原生插件", "iOSNativePlugin"},
	};

	void Start () {
		scrollRect.gameObject.SetActive(false);
		buttonTemplete.SetActive(false);
		foreach(var button in buttonToSceneDictionary){
			GameObject buttonObj = Instantiate(buttonTemplete, buttonTemplete.transform.parent) as GameObject;	
			buttonObj.name = button.Value;
			buttonObj.GetComponentInChildren<Text>().text = button.Key;
			buttonObj.SetActive(true);
			var copy = button;
			buttonObj.GetComponent<Button>().onClick.AddListener( ()=> {
				Debug.Log(string.Format("click [{0}] go to sence [{1}]", copy.Key, copy.Value));	
				SceneManager.LoadScene(copy.Value);
			} ); 
		}	
		scrollRect.gameObject.SetActive(true);
	}

	/// <summary>
	/// 这里的结果并非预期的，因为Mono编译的差别导致 
	/// </summary>
	/// <returns>The button to scene.</returns>
	IEnumerator InitButtonToScene(){
		buttonTemplete.SetActive(false);
		foreach(var button in buttonToSceneDictionary){
			GameObject buttonObj = Instantiate(buttonTemplete, buttonTemplete.transform.parent) as GameObject;	
			buttonObj.name = button.Key;
			buttonObj.GetComponentInChildren<Text>().text = button.Key;
			buttonObj.SetActive(true);
			var copy = button;
			buttonObj.GetComponent<Button>().onClick.AddListener( ()=> {
				Debug.Log(copy);	
			} ); 
			yield return 0;
		}	
	}
}
