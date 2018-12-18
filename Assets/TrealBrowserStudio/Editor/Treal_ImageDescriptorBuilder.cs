using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using Treal.Browser.AR;
using Treal.Browser.Core;

public class Treal_ImageDescriptorBuilder : EditorWindow
{
    [DllImport("ImageDescriptorBuilderUnityPlugin")]
    private extern static int BuildDescriptor(int targetCount, string[] targets, string output, string descriptorName, string[] targetImageName);


    [MenuItem("T real/Descriptor/T real AR", false, 1)]
    public static void ShowWindow()
    {
        var win = GetWindow(typeof(Treal_ImageDescriptorBuilder));
        win.titleContent = new GUIContent("T real Image AR");
        win.minSize = new Vector2(500, 500);
        //win.maxSize = new Vector2(500, 500);
        win.Focus();
    }

    private Vector2 mScrollPos;

    #region Variables for List Page

    private MtaFormat[] descriptorSet;
    private MtaFormat selectedDescriptorSet;
    private int descriptorCount = 0;
    private string originDescName;

    //private Treal_Form_ImageDescriptor addForm;
    //private Treal_Form_ImageDescriptor editForm;
    private Treal_Form_ImageDescriptor form;
    private int selectedTargetIdx;

    private enum PageStatus
    {
        List,
        Add,
        Edit,
        Contents
    }
    private PageStatus page = PageStatus.List;

    private GUIStyle guiStyleButton;

    #endregion


    // added
    private Texture2D trealImageARLogo;
    private Texture2D trealImageARLogoExtra;
    private Dictionary<MtaFormat, MappingDataList> descriptorDic;

    void OnEnable()
    {
        Init();
        CollectDescriptors();
    }

    private void Init()
    {
        //addForm = new Treal_Form_ImageDescriptor();
        //editForm = new Treal_Form_ImageDescriptor();
        form = new Treal_Form_ImageDescriptor();

        trealImageARLogo = Resources.Load<Texture2D>("trealimagearlogo");
        trealImageARLogoExtra = Resources.Load<Texture2D>("logoextra");
    }

    private void CollectDescriptors()
    {
        string path = PathUtil.GetARFolderPath();

        // obtain folder list
        DirectoryInfo root = new DirectoryInfo(path);
        DirectoryInfo[] descriptorDir = root.GetDirectories();

        List<MtaFormat> descriptorList = new List<MtaFormat>();

        if (descriptorDic == null)
        {
            descriptorDic = new Dictionary<MtaFormat, MappingDataList>();
        }
        else
        {
            descriptorDic.Clear();
        }

        // check inside folder
        foreach (var folder in descriptorDir)
        {
            MtaFormat mta = null;
            MappingDataList map = null;

            FileInfo[] fileInfo = folder.GetFiles();

            foreach (var file in fileInfo)
            {
                if (file.Name.Contains(".mta"))
                {
                    // parse mta
                    mta = ReadMta(file.FullName);
                }
                else if (file.Name.Contains(".map"))
                {
                    // parse map
                    map = ReadMappingFile(file.FullName);
                }
            }

            if (mta != null && map != null)
            {
                descriptorDic.Add(mta, map);
            }
        }
        
        descriptorSet = descriptorDic.Keys.ToArray(); // descriptorList.ToArray();
        descriptorCount = descriptorSet.Length;
    }

    void OnGUI()
    {
        Logo();

        guiStyleButton = new GUIStyle("button");

        // start scroll view
        mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos);

        if (page == PageStatus.List)
        {
            ListPage();
        }
        else if (page == PageStatus.Add)
        {
            DescriptorInfoPage();
        }
        else if (page == PageStatus.Edit)
        {
            DescriptorInfoPage();
        }
        else if (page == PageStatus.Contents)
        {
            ContentsInfoPage();
        }

        //end scroll view
        EditorGUILayout.EndScrollView();
    }

    private void Logo()
    {
        GUI.DrawTexture(new Rect(0, 0, 271, 63), trealImageARLogo);
        GUI.DrawTexture(new Rect(271, 0, position.width, 63), trealImageARLogoExtra);
        GUILayout.Space(63);
    }

    private void ListPage()
    {

        GUILayout.Space(20);

        //show descriptor list

        foreach (var mta in descriptorDic.Keys.ToList())
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            guiStyleButton.fontSize = 15;
            guiStyleButton.fontStyle = FontStyle.Bold;

            if (GUILayout.Button(mta.name, guiStyleButton, GUILayout.Height(35)))
            {
                OnSelectItem(mta);

                // change page
                page = PageStatus.Edit;
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Delete", GUILayout.Width(50), GUILayout.Height(35)))
            {
                if (EditorUtility.DisplayDialog("Delete", "Are you sure you want to delete?", "Yes", "No"))
                {
                    string deletePath = PathUtil.GetARFolderPath() + "/" + mta.name;

                    if (Directory.Exists(deletePath))
                    {
                        DirectoryInfo dir = new DirectoryInfo(deletePath);
                        Empty(dir);
                        CollectDescriptors();
                        continue;
                    }
                }
            }

            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        GUILayout.Space(50);
        GUILayout.BeginHorizontal();
        GUILayout.Space(150);

        if (GUILayout.Button("New", GUILayout.Height(25)))
        {
            form.targetImageList.Clear();
            form.descriptorName = string.Empty;

            // change page
            page = PageStatus.Add;
        }

        GUILayout.Space(150);
        GUILayout.EndHorizontal();
    }

    private void DescriptorInfoPage()
    {
        if (GUILayout.Button("<-", GUILayout.Width(100), GUILayout.Height(20)))
        {
            page = PageStatus.List;
        }

        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        // Add Target
        if (GUILayout.Button("Add", GUILayout.Width(100), GUILayout.Height(20)))
        {
            form.targetImageList.Add(new Treal_Form_TargetImage());
        }

        GUILayout.Space(80);

        // Build
        if (GUILayout.Button("Build", GUILayout.Width(100), GUILayout.Height(20)))
        {
            Build();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(25);

        // Descriptor Name Field
        form.descriptorName = EditorGUILayout.TextField("Descriptor Name :", form.descriptorName, GUILayout.Width(300));

        GUILayout.Space(50);
        GUILayout.BeginHorizontal(GUILayout.Width(300));

        GUILayout.Space(30);
        GUILayout.Label("Target Image");
        GUILayout.Space(100);
        GUILayout.Label("Target Name");

        GUILayout.EndHorizontal();

        // Texture2d field for images
        for (int i = 0; i < form.targetImageList.Count; i++)
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal(GUILayout.Width(350));
            GUILayout.Space(15);

            // Target Image
            form.targetImageList[i].targetTexture = EditorGUILayout.ObjectField(form.targetImageList[i].targetTexture, typeof(Texture2D), false, GUILayout.Width(120), GUILayout.Height(120)) as Texture2D;
            if (GUI.changed)
            {
                if (form.targetImageList[i].targetTexture != null)
                {
                    form.targetImageList[i].thumbnailPath = AssetDatabase.GetAssetPath(form.targetImageList[i].targetTexture);
                }
            }

            GUILayout.Space(25);
            GUILayout.BeginVertical();

            // Target Name
            form.targetImageList[i].targetName = EditorGUILayout.TextField(form.targetImageList[i].targetName);

            GUILayout.Space(15);

            // contents button
            if (GUILayout.Button("Contents Setting", GUILayout.Width(150), GUILayout.Height(30)))
            {
                selectedTargetIdx = i;
                page = PageStatus.Contents;
            }

            GUILayout.Space(15);

            // contents button
            if (GUILayout.Button("Delete", GUILayout.Width(150), GUILayout.Height(30)))
            {
                form.targetImageList.RemoveAt(i);
                continue;
            }

            GUILayout.EndVertical();
            GUILayout.Space(15);
            GUILayout.EndHorizontal();
            GUILayout.Space(35);
        }
    }

    private void ContentsInfoPage()
    {
        if (GUILayout.Button("<-", GUILayout.Width(100), GUILayout.Height(20)))
        {
            page = PageStatus.Add;
        }

        GUILayout.Space(30);

        if (GUILayout.Button("Add", GUILayout.Width(100), GUILayout.Height(20)))
        {
            var newContents = new Treal_ContentsForm();
            newContents.scale = Vector3.one;
            form.targetImageList[selectedTargetIdx].contentsList.Add(newContents);
            page = PageStatus.Contents;
        }

        GUILayout.Space(20);

        for (int i = 0; i < form.targetImageList[selectedTargetIdx].contentsList.Count; i++)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal(GUILayout.Width(300));

            GUILayout.Space(10);

            form.targetImageList[selectedTargetIdx].contentsList[i].contents = EditorGUILayout.ObjectField(form.targetImageList[selectedTargetIdx].contentsList[i].contents, typeof(UnityEngine.Object), false) as UnityEngine.Object;

            if (GUI.changed)
            {
                if (form.targetImageList[selectedTargetIdx].contentsList[i].contents != null)
                {
                    var contentsPath = AssetDatabase.GetAssetPath(form.targetImageList[selectedTargetIdx].contentsList[i].contents);
                    var contentsPathGUID = AssetDatabase.AssetPathToGUID(contentsPath);

                    form.targetImageList[selectedTargetIdx].contentsList[i].contentsPathGUID = contentsPathGUID;
                }
            }

            if (GUILayout.Button("Delete", GUILayout.Width(50), GUILayout.Height(20)))
            {
                form.targetImageList[selectedTargetIdx].contentsList.RemoveAt(i);
                continue;
            }

            GUILayout.EndHorizontal();
            
            // transform 
            GUILayout.BeginHorizontal(GUILayout.Width(300));
            GUILayout.Space(10);
            form.targetImageList[selectedTargetIdx].contentsList[i].position = EditorGUILayout.Vector3Field("Position", form.targetImageList[selectedTargetIdx].contentsList[i].position);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(300));
            GUILayout.Space(10);
            form.targetImageList[selectedTargetIdx].contentsList[i].rotation = EditorGUILayout.Vector3Field("Rotation", form.targetImageList[selectedTargetIdx].contentsList[i].rotation);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(300));
            GUILayout.Space(10);
            form.targetImageList[selectedTargetIdx].contentsList[i].scale = EditorGUILayout.Vector3Field("Scale", form.targetImageList[selectedTargetIdx].contentsList[i].scale);
            GUILayout.EndHorizontal();

            GUILayout.Space(25);
        }
    }
    

    private void OnSelectItem(MtaFormat descriptorSet)
    {
        originDescName = descriptorSet.name;
        
        //selectedDescriptorSet = descriptorSet;

        form.targetImageList.Clear();
        form.descriptorName = descriptorSet.name;
        var map = descriptorDic[descriptorSet];

        // target image
        for (int i = 0; i < descriptorSet.data.Length; i++)
        {
            Treal_Form_TargetImage targetImage = new Treal_Form_TargetImage();
            targetImage.targetName = descriptorSet.data[i].name;
            targetImage.thumbnailPath = AssetDatabase.GUIDToAssetPath(descriptorSet.data[i].thumb);
            targetImage.targetTexture = AssetDatabase.LoadAssetAtPath(targetImage.thumbnailPath, typeof(Texture2D)) as Texture2D;

            // contents
            for (int j = 0; j < map.mappinglist[i].tro_list.Count; j++)
            {
                Treal_ContentsForm contentsForm = new Treal_ContentsForm();
                contentsForm.contentsPathGUID = map.mappinglist[i].tro_list[j].contents_tro_path;
                var assetPath = AssetDatabase.GUIDToAssetPath(contentsForm.contentsPathGUID);
                contentsForm.contents = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)) as Object;
                contentsForm.position = map.mappinglist[i].tro_list[j].position;
                contentsForm.rotation = map.mappinglist[i].tro_list[j].rotation;
                contentsForm.scale = map.mappinglist[i].tro_list[j].scale;

                targetImage.contentsList.Add(contentsForm);
            }

            form.targetImageList.Add(targetImage);
        }
    }

    private void Build()
    {
        if (string.IsNullOrEmpty(form.descriptorName))
        {
            EditorUtility.DisplayDialog("Error", "Type in name", "Ok");
            return;
        }

        if (form.targetImageList.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "At least 1 target image is required", "Ok");
            return;
        }

        for (int i = 0; i < form.targetImageList.Count; i++)
        {
            if (string.IsNullOrEmpty(form.targetImageList[i].thumbnailPath))
            {
                EditorUtility.DisplayDialog("Error", "Image is required", "Ok");
                return;
            }
            else
            {
                
                if (!Path.GetExtension(form.targetImageList[i].thumbnailPath).ToLower().Contains("jpg"))
                {
                    EditorUtility.DisplayDialog("Error", "Image must be JPG", "Ok");
                    return;
                }
            }

            if (string.IsNullOrEmpty(form.targetImageList[i].targetName))
            {
                EditorUtility.DisplayDialog("Error", "Image name is required", "Ok");
                return;
            }
        }


        string descriptorName = form.descriptorName;
        int targetImageCount = form.targetImageList.Count;
        string[] thumbnailPathArray = new string[targetImageCount];
        string[] targetNameArray = new string[targetImageCount];
        string[][] contentsPathGUID2DArray = new string[targetImageCount][];
        Vector3[][] contentsPosition = new Vector3[targetImageCount][];
        Vector3[][] contentsRotation = new Vector3[targetImageCount][];
        Vector3[][] contentsScale = new Vector3[targetImageCount][];


        for (int i = 0; i < targetImageCount; i++)
        {
            thumbnailPathArray[i] = form.targetImageList[i].thumbnailPath;
            targetNameArray[i] = form.targetImageList[i].targetName;

            contentsPathGUID2DArray[i] = new string[form.targetImageList[i].contentsList.Count];
            contentsPosition[i] = new Vector3[form.targetImageList[i].contentsList.Count];
            contentsRotation[i] = new Vector3[form.targetImageList[i].contentsList.Count];
            contentsScale[i] = new Vector3[form.targetImageList[i].contentsList.Count];

            for (int j = 0; j < form.targetImageList[i].contentsList.Count; j++)
            {
                var contentsPath = AssetDatabase.GUIDToAssetPath(form.targetImageList[i].contentsList[j].contentsPathGUID);
                contentsPathGUID2DArray[i][j] = form.targetImageList[i].contentsList[j].contentsPathGUID;

                contentsPosition[i][j] = form.targetImageList[i].contentsList[j].position;
                contentsRotation[i][j] = form.targetImageList[i].contentsList[j].rotation;
                contentsScale[i][j] = form.targetImageList[i].contentsList[j].scale;
            }
        }


        string binFilePath = PathUtil.GetARFolderPath() + "/" + descriptorName + "/" + descriptorName + ".dcs";

        // delete original path
        if (page == PageStatus.Edit)
        {
            string originalPath = PathUtil.GetARFolderPath()  + "/" + originDescName;
            Empty(new DirectoryInfo(originalPath));
        }

        // build
        BuildDescriptor(targetImageCount, thumbnailPathArray, binFilePath, descriptorName, targetNameArray);


        string mta_path = PathUtil.GetARFolderPath() + "/" + descriptorName + "/" + descriptorName + ".mta";
        string map_path = PathUtil.GetARFolderPath() + "/" + descriptorName + "/" + descriptorName + ".map";
        

        var mta = JsonUtility.FromJson<MtaFormat>(File.ReadAllText(mta_path));


        MappingDataList mapDataList = new MappingDataList();
        mapDataList.tro_id = mta.id;
        mapDataList.tro_name = mta.name;

        int descListCount = mta.data.Length;

        for (int i = 0; i < descListCount; i++)
        {

            MappingData mappingData = new MappingData();
            mappingData.desc_id = mta.data[i].id;
            mappingData.desc_name = mta.data[i].name;

            // rewrite thumbnail path
            mta.data[i].thumb = AssetDatabase.AssetPathToGUID(thumbnailPathArray[i]);


            for (int j = 0; j < form.targetImageList[i].contentsList.Count; j++)
            {
                // write mapping file
                TRO_ContentsInfo mappingTROContentsInfo = new TRO_ContentsInfo();
                mappingTROContentsInfo.contents_tro_id = AssetDatabase.AssetPathToGUID(contentsPathGUID2DArray[i][j]);
                mappingTROContentsInfo.contents_tro_path = contentsPathGUID2DArray[i][j];
                var contentsPath = AssetDatabase.GUIDToAssetPath(contentsPathGUID2DArray[i][j]);
                mappingTROContentsInfo.contents_tro_name = Path.GetFileNameWithoutExtension(contentsPath);
                mappingTROContentsInfo.position = contentsPosition[i][j];
                mappingTROContentsInfo.rotation = contentsRotation[i][j];
                mappingTROContentsInfo.scale = contentsScale[i][j];

                mappingData.tro_list.Add(mappingTROContentsInfo);
            }

            mapDataList.mappinglist.Add(mappingData);
        }

        // write mta file
        var data = JsonUtility.ToJson(mta);
        File.WriteAllText(mta_path, data);

        // write map file
        var mapData = JsonUtility.ToJson(mapDataList);
        File.WriteAllText(map_path, mapData);

        CollectDescriptors();
        page = PageStatus.List;

    }

    private static MtaFormat ReadMta(string path)
    {
        var jsonString = File.ReadAllText(path);
        var data = JsonUtility.FromJson<MtaFormat>(jsonString);

        return data;
    }

    private static MappingDataList ReadMappingFile(string path)
    {
        var jsonString = File.ReadAllText(path);
        var data = JsonUtility.FromJson<MappingDataList>(jsonString);

        return data;
    }

    private static void Empty(DirectoryInfo directory)
    {
        foreach (FileInfo file in directory.GetFiles())
            file.Delete();

        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            foreach (FileInfo file in subDirectory.GetFiles())
                file.Delete();

            subDirectory.Delete(true);
        }

        directory.Delete();
    }

    [System.Serializable]
    class MtaFormat
    {
        public string name;
        public string id;
        public MtaFormatData[] data;
    }

    [System.Serializable]
    class MtaFormatData
    {
        public string name;
        public string id;
        public string thumb;
    }
}