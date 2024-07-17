using System.IO;
using UnityEditor;
using UnityEngine;
namespace Editor.EntitiesTools
{
    public class EntitiesTool : EditorWindow
    {
        [MenuItem("EntitiesTools/GenEntitiesPath")]
        private static void Open()
        {
            //打开窗口
            GetWindow<EntitiesTool>().Show();
        }
        private string path;
        private string GenFolderName;
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("路径", GUILayout.Width(50f));
                path = GUILayout.TextField(path);
                if (GUILayout.Button("浏览", GUILayout.Width(50f)))
                {
                    path = EditorUtility.OpenFolderPanel("窗口标题", Application.dataPath, "123");
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                //行头
                GUILayout.Label("文件夹名称", GUILayout.Width(50f));
                //文本输入框 用于编辑并展示路径
                GenFolderName = GUILayout.TextField(GenFolderName);
            }
            GUILayout.EndHorizontal();
            
            //水平布局
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("生成路径", GUILayout.Width(200)))
            {
                path = $"{path}/{GenFolderName}";
                Directory.CreateDirectory(path + "/Scenes");
                Directory.CreateDirectory(path + "/Scripts");
                Directory.CreateDirectory(path + "/Scripts/Aspects");
                Directory.CreateDirectory(path + "/Scripts/Authoring");
                Directory.CreateDirectory(path + "/Scripts/Components");
                Directory.CreateDirectory(path + "/Scripts/Jobs");
                Directory.CreateDirectory(path + "/Scripts/SystemGroups");
                Directory.CreateDirectory(path + "/Scripts/Systems");
            }
            GUILayout.EndHorizontal();
        }
    }
}
