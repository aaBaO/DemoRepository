using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class IOSNativePluginTest : MonoBehaviour {

	[DllImport("__Internal")]
	private static extern void CalliOSNativeFunction();

	public Button calliOSNativeFunction;

	void Start () {
		calliOSNativeFunction.onClick.AddListener(CalliOS_CallNativeFunction);
	}

	private void CalliOS_CallNativeFunction(){
		Debug.Log("[Unity call iOS]");
		CalliOSNativeFunction();
	}

	/// <summary>
	/// Callbacks from iOS .
	/// </summary>
	/// <param name="message">Message.</param>
	private void Callback_iOSReturnMessage(string message){
		Debug.Log("[Unity callback] Message is :" + message);
	}
}
