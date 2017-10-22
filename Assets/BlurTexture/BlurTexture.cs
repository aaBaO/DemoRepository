using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BlurTexture : MonoBehaviour {

	private RawImage texture;
	/// <summary>
	/// texture所在的Root Canvas
	/// </summary>
	public Canvas rootCanvas;

	private void Awake(){
		texture = GetComponent<RawImage>();		
	}

	private void Update()
	{
		var screenWidth = rootCanvas.pixelRect.xMax / rootCanvas.scaleFactor;
		var screenHeight = rootCanvas.pixelRect.yMax / rootCanvas.scaleFactor;

		//得到一个从屏幕中心点到texture左下角的点的向量
		var bottomLeft = texture.rectTransform.anchoredPosition + texture.rectTransform.rect.min;

		var normalWidth = texture.rectTransform.rect.width / screenWidth;
		var normalHeight = texture.rectTransform.rect.height  / screenHeight;

		//uv的xy起点为(0,0)，我们在计算坐下角的点时是从屏幕中心点开始，因此要加上0.5
		texture.uvRect = new Rect(0.5f + bottomLeft.x / screenWidth, 0.5f + bottomLeft.y / screenHeight, normalWidth, normalHeight);
	}
}
