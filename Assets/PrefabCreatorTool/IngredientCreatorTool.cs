using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class IngredientCreatorTool : EditorWindow
{
    static IngredientCreatorTool window;
    [MenuItem("Tools/IngredientCreator")]
    public static void ShowWindow()
    {
        window = GetWindow<IngredientCreatorTool>("IngredientCreator");
    }

    static IngredientCreationInfoStorage info;

    static string objectName;

    Mesh defaultMesh;
    Material defaultMaterial;

    bool canFry;
    bool fryFoldout;

    float fryTime; 
    float burnTime;
    Material friedMaterial;
    Material burntMaterial;

    bool canChop;
    bool chopFoldout;
    int requiredChops;
    Mesh choppedMesh;

    private void OnGUI()
    {
        GUILayout.Label("Create new Ingredient", EditorStyles.boldLabel);

        objectName = EditorGUILayout.TextField("Name", objectName);

        GUILayout.Space(10);

        defaultMesh = (Mesh)EditorGUILayout.ObjectField("Default Mesh", defaultMesh, typeof(Mesh), false);
        defaultMaterial = (Material)EditorGUILayout.ObjectField("Default Material", defaultMaterial, typeof(Material), false);

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        fryFoldout = EditorGUILayout.Foldout(fryFoldout, "Can Fry", true);
        canFry = EditorGUILayout.Toggle(canFry);
        EditorGUILayout.EndHorizontal();

        GUI.enabled = canFry;

        if (fryFoldout)
        {
            EditorGUI.indentLevel++;

            fryTime = EditorGUILayout.FloatField("Fry Time", fryTime);
            burnTime = EditorGUILayout.FloatField("Burn Time", burnTime);
            friedMaterial = (Material)EditorGUILayout.ObjectField("Fried Material", friedMaterial, typeof(Material), false);
            burntMaterial = (Material)EditorGUILayout.ObjectField("Burnt Material", burntMaterial, typeof(Material), false);

            EditorGUI.indentLevel--;
        }

        GUI.enabled = true;

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        chopFoldout = EditorGUILayout.Foldout(chopFoldout, "Can Chop", true);
        canChop = EditorGUILayout.Toggle(canChop);
        EditorGUILayout.EndHorizontal();

        GUI.enabled = canChop;

        if (chopFoldout)
        {
            EditorGUI.indentLevel++;

            requiredChops = EditorGUILayout.IntField("Required Chops", requiredChops);
            choppedMesh = (Mesh)EditorGUILayout.ObjectField("Chopped Mesh", choppedMesh, typeof(Mesh), false);

            EditorGUI.indentLevel--;
        }

        GUI.enabled = true;

        GUILayout.Space(10);

        info = (IngredientCreationInfoStorage)EditorGUILayout.ObjectField(info, typeof(IngredientCreationInfoStorage), false);

        if(GUILayout.Button("Create Ingredient"))
        {
            CreatePrefab();
        }
    }

    static GameObject gameObject;
    void CreatePrefab()
    {
        gameObject = new GameObject(objectName);

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = defaultMesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = defaultMaterial;

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = defaultMesh;

        if (canFry)
        {
            Fryable fryable = gameObject.AddComponent<Fryable>();
            fryable.fryTime = fryTime;
            fryable.burnTime = burnTime;
            fryable.friedMaterial = friedMaterial;
            fryable.burntMaterial = burntMaterial;
        }

        if (canChop)
        {
            Choppable choppable = gameObject.AddComponent<Choppable>();
            choppable.requiredChops = requiredChops;
            choppable.choppedMesh = choppedMesh;
        }

        string prefabPath = $"Assets/Prefabs/Ingredients/{objectName}.prefab";
        string directoryPath = "Assets/Prefabs/Ingredients";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
        DestroyImmediate(gameObject);

        info.objectName = objectName;
        info.gameObject = prefab;
        info.used = false;

        CreateScript();
    }

    void CreateScript()
    {
        if (string.IsNullOrWhiteSpace(objectName))
        {
            Debug.LogError("Ingredient name cannot be empty");
            return;
        }

        string scriptContent = $@"
using UnityEngine;

public class {objectName} : Ingredient
{{

}}
";
        string scriptPath = $"Assets/Scripts/Ingredients/{objectName}.cs";

        if (File.Exists(scriptPath))
        {
            Debug.LogError($"A file with the name {objectName} already exists at {scriptPath}");
            return;
        }

        File.WriteAllText(scriptPath, scriptContent);

        AssetDatabase.Refresh();

        info.createdScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);

        Debug.Log($"Script {objectName} created at {scriptPath}");

        return;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    static void CompletePrefabCreation()
    {
        info = AssetDatabase.LoadAssetAtPath<IngredientCreationInfoStorage>("Assets/PrefabCreatorTool/InfoStorage.asset");

        if (info.used) return;
        if (info.gameObject == null) return;
        if (info.createdScript == null) return;

        GameObject gameObject = Instantiate(info.gameObject);

        Ingredient oldIngredientComponent = gameObject.GetComponent<Ingredient>();

        Type type = info.createdScript.GetClass();
        gameObject.AddComponent(type);

        DestroyImmediate(oldIngredientComponent);

        string prefabPath = $"Assets/Prefabs/Ingredients/{info.objectName}.prefab";

        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, prefabPath, InteractionMode.UserAction);

        Debug.Log($"Prefab {info.objectName} created at {prefabPath}");

        DestroyImmediate(gameObject);

        info.used = true;
    }
}