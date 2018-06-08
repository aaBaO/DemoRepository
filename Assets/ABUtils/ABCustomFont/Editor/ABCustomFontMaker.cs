using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ABUtils
{
    /*
    Usage: Apply sprite sheet metadata to generate CustomFont.fontsettings
     */
    public class ABCustomFontMaker : EditorWindow{

        private string m_fontName;
        private Texture2D m_fontTexture;
        private Material m_fontMaterial;
        private TextureImporter m_fontTextureImporter;
        public int perSpriteWidth;
        public int perSpriteHeight;

        [MenuItem("Tools/ABCustomFontMaker")]
        public static void Init()
        {
            EditorWindow window = EditorWindow.GetWindow<ABCustomFontMaker>();
            window.Show();
            window.minSize = new Vector2(400, 300);
        }

        public void OnGUI()
        {
            m_fontName = EditorGUILayout.TextField("FontName", m_fontName);
            m_fontTexture = EditorGUILayout.ObjectField(new GUIContent("FontTexture", "Multi Sprite"), m_fontTexture, typeof(Texture2D), false) as Texture2D;
            m_fontMaterial = EditorGUILayout.ObjectField(new GUIContent("Font Mat", ""), m_fontMaterial, typeof(Material), false) as Material;

            if (GUILayout.Button(new GUIContent("Done", "Make font with FontTexture")))
            {
                CreateCustomFont(m_fontName);
            }
        }

        private void CreateCustomFont(string fileName)
        {
            string savePath = EditorUtility.SaveFilePanelInProject("Where to Save CustomFont", fileName, "fontsettings", "awasome font!");
            Font font = AssetDatabase.LoadAssetAtPath<Font>(savePath);
            if (font == null)
                font = new Font();

            string fontTexturePath = AssetDatabase.GetAssetPath(m_fontTexture.GetInstanceID());
            //Get font texture importer
            m_fontTextureImporter = AssetImporter.GetAtPath(fontTexturePath) as TextureImporter;
            int spritesheetCount = m_fontTextureImporter.spritesheet.Length;
            if (spritesheetCount <= 1)
            {
                Debug.LogError("Need texture set to sprite and has multiple sprites sliced !");
                return;
            }
            CharacterInfo[] characterInfo = new CharacterInfo[spritesheetCount];
            font.characterInfo = null;
            for(int i = 0; i < characterInfo.Length; i++)
            {
                SpriteMetaData spriteMetaData = m_fontTextureImporter.spritesheet[i];
                int width = Mathf.CeilToInt(spriteMetaData.rect.width);
                int height = Mathf.CeilToInt(spriteMetaData.rect.height);
                //uv
                float uvx = spriteMetaData.rect.min.x / m_fontTexture.width;
                float uvy = spriteMetaData.rect.min.y / m_fontTexture.height;
                float uvw = spriteMetaData.rect.size.x / m_fontTexture.width;
                float uvh = spriteMetaData.rect.size.y / m_fontTexture.height;
                characterInfo[i].uvTopLeft = new Vector2(uvx, uvy + uvh);
                characterInfo[i].uvTopRight = new Vector2(uvx + uvw, uvy + uvh);
                characterInfo[i].uvBottomRight = new Vector2(uvx + uvw, uvy);
                characterInfo[i].uvBottomLeft = new Vector2(uvx, uvy);

                //vert
                characterInfo[i].glyphWidth = width;
                characterInfo[i].glyphHeight = height;
                characterInfo[i].minX = 0;
                characterInfo[i].maxX = width;
                characterInfo[i].minY = -height;
                characterInfo[i].maxY = 0;

                //advance
                characterInfo[i].advance = width;

                //index
                characterInfo[i].index = StringToUnicodeIndex(spriteMetaData.name);

                //size
                characterInfo[i].size = 0;
            }
            font.characterInfo = characterInfo;
            font.material = m_fontMaterial;

            //save asset 
            if(!AssetDatabase.Contains(font.GetInstanceID()))
            {
                AssetDatabase.CreateAsset(font, savePath);
            }

            //Save and refresh Assets
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(font);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(font);
        }

        /// <summary>
        /// Only convert first string to UnicodeIndex
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int StringToUnicodeIndex(string s)
        {
            byte[] sBytes = System.Text.Encoding.Unicode.GetBytes(s.Substring(0, 1).ToCharArray());
            int result = 0;
            result = sBytes[0] | sBytes[1] << 8;
            return result;
        }
    }
}
