using System;
using System.IO;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public event Action OnShowTutorial;

    [SerializeField]
    private GameData _gameData = new();
    public GameData CurrentGameData => _gameData;

    [SerializeField] bool wipe = false;

    private string permanentDataFilePath;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        permanentDataFilePath = Application.persistentDataPath + "/GameData.save";

    }

    private void Start()
    {
        if (HasSaveData())
            LoadPermanentData();
        else
        {
            _gameData.isFirstTime = false;
            SavePermanentData();
        }

        if(_gameData.isFirstTime)
            ShowTutorial();
    }

    private void ShowTutorial()
    {
        OnShowTutorial?.Invoke();
    }

    public void DoneTutorial()
    {
        _gameData.isFirstTime = false;
        SavePermanentData();
    }

    private void Update()
    {
        if (wipe)
        {
           _gameData.isFirstTime = true;
            SavePermanentData();
            Debug.Log("resetGame");
            wipe = false;
        }
    }

    #region Save/Load/Delete
    public void SavePermanentData()
    {
        try
        {
            string json = JsonUtility.ToJson(_gameData, true);
            File.WriteAllText(permanentDataFilePath, json);
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save data to {permanentDataFilePath}: {ex.Message}");
        }
    }

    public void LoadPermanentData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            try
            {
                string json = File.ReadAllText(permanentDataFilePath);
                JsonUtility.FromJsonOverwrite(json, _gameData);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to load data from {permanentDataFilePath}: {ex.Message}");
            }
        }
    }

    public void DeleteAllSaveData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            File.Delete(permanentDataFilePath);
        }
    }

    public bool HasSaveData()
    {
        return File.Exists(permanentDataFilePath);
    }
    #endregion
}

[Serializable]
public class GameData
{
    public bool isFirstTime;
}
