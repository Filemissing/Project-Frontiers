using System;
using System.IO;
using System.Linq;
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
    static Sprite icon;

    Mesh defaultMesh;
    Material defaultMaterial;

    bool canFry;
    bool fryFoldout;
    float fryTime; 
    float fryBurnTime;
    Material friedMaterial;
    Material fryBurntMaterial;

    bool canChop;
    bool chopFoldout;
    int requiredChops;
    Mesh choppedMesh;

    bool canCook;
    bool cookFoldout;
    float cookTime;
    float cookBurnTime;
    Material cookedMaterial;
    Material cookBurntMaterial;

    private void OnGUI()
    {
        GUILayout.Label("Create new Ingredient", EditorStyles.boldLabel);

        objectName = EditorGUILayout.TextField("Name", objectName);
        icon = (Sprite)EditorGUILayout.ObjectField("Icon", icon, typeof(Sprite), false);

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
            fryBurnTime = EditorGUILayout.FloatField("Fry Burn Time", fryBurnTime);
            friedMaterial = (Material)EditorGUILayout.ObjectField("Fried Material", friedMaterial, typeof(Material), false);
            fryBurntMaterial = (Material)EditorGUILayout.ObjectField("Fry Burnt Material", fryBurntMaterial, typeof(Material), false);

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

        EditorGUILayout.BeginHorizontal();
        cookFoldout = EditorGUILayout.Foldout(cookFoldout, "Can Cook", true);
        canCook = EditorGUILayout.Toggle(canCook);
        EditorGUILayout.EndHorizontal();

        GUI.enabled = canCook;

        if (cookFoldout)
        {
            EditorGUI.indentLevel++;

            cookTime = EditorGUILayout.FloatField("Cook Time", cookTime);
            cookBurnTime = EditorGUILayout.FloatField("Cook Burn Time", cookBurnTime);
            cookedMaterial = (Material)EditorGUILayout.ObjectField("Fried Material", cookedMaterial, typeof(Material), false);
            cookBurntMaterial = (Material)EditorGUILayout.ObjectField("Cook Burnt Material", cookBurntMaterial, typeof(Material), false);

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
        if (string.IsNullOrWhiteSpace(objectName) || objectName.Any(char.IsWhiteSpace))
        {
            Debug.LogError("Ingredient name is Invalid");
            return;
        }

        gameObject = new GameObject(objectName); // create a new GameObject

        // set up normal components

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = defaultMesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = defaultMaterial;

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = defaultMesh;
        collider.convex = true;

        if (canFry)
        {
            Fryable fryable = gameObject.AddComponent<Fryable>();
            fryable.fryTime = fryTime;
            fryable.burnTime = fryBurnTime;
            fryable.friedMaterial = friedMaterial;
            fryable.burntMaterial = fryBurntMaterial;
        }

        if (canChop)
        {
            Choppable choppable = gameObject.AddComponent<Choppable>();
            choppable.requiredChops = requiredChops;
            choppable.choppedMesh = choppedMesh;
        }

        if (canChop)
        {
            Cookable cookable = gameObject.AddComponent<Cookable>();
            cookable.cookTime = cookTime;
            cookable.burnTime = cookBurnTime;
            cookable.cookedMaterial = cookedMaterial;
            cookable.burntMaterial = cookBurntMaterial;
        }

        // Save GameObject as Prefab
        string prefabPath = $"Assets/Prefabs/Ingredients/{objectName}.prefab";
        string directoryPath = "Assets/Prefabs/Ingredients";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
        DestroyImmediate(gameObject);

        // Store info for script reload
        info.objectName = objectName;
        info.icon = icon;
        info.gameObject = prefab;
        info.used = false;

        CreateScript();
    }

    void CreateScript()
    {
        string scriptContent = 
$@"using UnityEngine;

public class {objectName} : Ingredient
{{

}}
";
        string scriptPath = $"Assets/Scripts/Ingredients/{objectName}.cs";

        if (File.Exists(scriptPath))
        {
            Debug.LogError($"A file with the name {objectName} already exists at {scriptPath}");
            DestroyImmediate(gameObject);
            return;
        }

        File.WriteAllText(scriptPath, scriptContent); // write the new script

        AssetDatabase.Refresh();

        info.createdScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath); // recompile and store a reference to the newly created script

        Debug.Log($"Script {objectName} created at {scriptPath}");

        return;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    static void CompletePrefabCreation()
    {
        info = AssetDatabase.LoadAssetAtPath<IngredientCreationInfoStorage>("Assets/PrefabCreatorTool/InfoStorage.asset"); // load the stored data

        if (info.used) return; // return if it was already used (in case of normal scriptreloads for other reasons)

        if (info.gameObject == null) throw new NullReferenceException("the variable gameObject was not stored correctly");
        if (info.createdScript == null) throw new NullReferenceException("the new script was not correctly created or stored");

        GameObject gameObject = Instantiate(info.gameObject); //reinstantieate the gameObject to further edit it

        gameObject.TryGetComponent<Ingredient>(out Ingredient oldIngredientComponent); // get the plain Ingredient component created by dependencies

        Type type = info.createdScript.GetClass();
        Ingredient ingredient = (Ingredient)gameObject.AddComponent(type);
        ingredient.icon = info.icon;

        if(oldIngredientComponent != null) DestroyImmediate(oldIngredientComponent); // remove the plain ingredient component

        string prefabPath = $"Assets/Prefabs/Ingredients/{info.objectName}.prefab";

        PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath); // override the previously stored prefab with the newest changes

        Debug.Log($"Prefab {info.objectName} created at {prefabPath}");

        DestroyImmediate(gameObject);

        info.used = true;
    }
}