using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace TS
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoyPath = "";
        public string saveFileName = "";

        public bool FileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoyPath, saveFileName)))
            {
                return true;
            }

            return false;
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoyPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            string savePath = Path.Combine(saveDataDirectoyPath, saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log($"CREATING SAVE FILE, AT PATH: {savePath}");

                string dataToStore = JsonUtility.ToJson(characterData, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED {savePath}\n{e}");
            }
        }

        // Used to Load a save file upon loading a previous game
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            string loadPath = Path.Combine(saveDataDirectoyPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // Deserialize the data from json back to unity
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ERROR WHILST TRYING TO LOAD CHARACTER DATA, GAME NOT LOADED {loadPath}\n{e}");
                }
            }

            return characterData;
        }
    }
}
