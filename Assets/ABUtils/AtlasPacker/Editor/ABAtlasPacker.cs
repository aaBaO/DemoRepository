using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ABUtils
{
    public class ABAtlasPacker : EditorWindow{

        static Texture2D[] selected;
        static TextureImporterSettings[] selectedTextureSettings;
        private Vector2 m_previewScrollViewValue;
        private string m_outputName = "atlas";
        public static string projectPath;

        [MenuItem("Tools/AtlasPacker")]
        public static void Init(){

            projectPath = Application.dataPath + "/../";
            selected = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
            EditorWindow window = EditorWindow.GetWindow<ABAtlasPacker>();
            window.Show();
            window.minSize = new Vector2(400, 300);

        }

        void OnGUI()
        {
            DrawControlButtons(new Rect(0, 0, EditorGUIUtility.currentViewWidth * 0.5f, GetWindow<ABAtlasPacker>().minSize.y));
            DrawPreview(new Rect(EditorGUIUtility.currentViewWidth * 0.5f, 0, EditorGUIUtility.currentViewWidth * 0.5f, GetWindow<ABAtlasPacker>().minSize.y));
        }

        public void DrawControlButtons(Rect rect){

            GUILayout.BeginArea(rect);
            m_outputName = EditorGUILayout.TextField(new GUIContent("Output name", "Atlas out put name"), m_outputName);
            if(GUILayout.Button("Pack Atlas")){
                PackTextureToAtlas(m_outputName);
            }
            GUILayout.EndArea();

        }

        public void DrawPreview(Rect rect){
            
            if(selected.Length < 0){
                return;
            }

            var selectedEditor = Editor.CreateEditor(selected);
            if(selectedEditor != null)
                selectedEditor.DrawPreview(rect);

        }

        void PackTextureToAtlas(string outputName){
            if(selected.Length <= 0)
                return;

            //Get selected Sprite Original Settings
            selectedTextureSettings = new TextureImporterSettings[selected.Length];
            for(int i = 0; i < selectedTextureSettings.Length; i++){
                string spriteAssetPath = AssetDatabase.GetAssetPath(selected[i]);
                TextureImporter spriteAsset = AssetImporter.GetAtPath(spriteAssetPath) as TextureImporter;
                //Only if the texture is readable can be packed
                spriteAsset.isReadable = true;
                spriteAsset.SaveAndReimport();
                //get original border, pivot setting
                selectedTextureSettings[i] = new TextureImporterSettings();
                selectedTextureSettings[i].spriteBorder = spriteAsset.spriteBorder;
                selectedTextureSettings[i].spritePivot = spriteAsset.spritePivot;
            }

            Texture2D resultAtlas = new Texture2D(1, 1);
            Rect[] rects = resultAtlas.PackTextures(selected, 1, 2048);
            string currentDirPath = AssetDatabase.GetAssetPath(selected[0]).TrimEnd((selected[0].name+".png").ToCharArray());
            string assetPath = currentDirPath + outputName + ".png";
            string outputPath = Path.Combine(projectPath, assetPath);
            // Debug.Log(outputPath);
            File.WriteAllBytes(outputPath, resultAtlas.EncodeToPNG());
            AssetDatabase.Refresh();
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture));
            EditorGUIUtility.PingObject(asset);

            //Apply Import Setting
            StringBuilder outputLog = new StringBuilder();
            outputLog.Append("Packed : ");
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.mipmapEnabled = false;
            SpriteMetaData[] sheet = new SpriteMetaData[rects.Length];
            for(int i = 0; i < sheet.Length; i++){
                sheet[i].name = selected[i].name;
                sheet[i].rect.Set(rects[i].x * resultAtlas.width,
                    rects[i].y * resultAtlas.height,
                    rects[i].width * resultAtlas.width,
                    rects[i].height * resultAtlas.height);

                sheet[i].border = selectedTextureSettings[i].spriteBorder;
                sheet[i].pivot = selectedTextureSettings[i].spritePivot;

                //Append log info
                outputLog.Append(selected[i].name);
                if(i < sheet.Length - 1)
                    outputLog.Append(" ; ");
            }
            importer.spritesheet = sheet;
            importer.SaveAndReimport();
            Debug.Log(outputLog);

        }
    }
}

