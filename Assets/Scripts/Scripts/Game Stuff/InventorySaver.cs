using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Leguar.TotalJSON;
using static SerializableListString;

public enum SaveSwitch
{
    json,
    binary
}
public class InventorySaver : MonoBehaviour
{
    [SerializeField] private Inventory myInventory;
    public ItemDatabase ItemDB;
    private SerializableListString SL = new SerializableListString();
    public SaveSwitch saveType = SaveSwitch.json;
    private void OnEnable()
    {
        myInventory.items.Clear();

        SL.serializableList.Clear();
        LoadScriptables();
        ImportSaveData();
    }

    private void OnDisable()
    {
        SL.serializableList.Clear();
        BuildSaveData();
        SaveScriptables();
    }

    private void BuildSaveData()
    {
        for (int i = 0; i < myInventory.items.Count; i++)
        {
            SerializableListString.SerialItem SI = new SerializableListString.SerialItem();
            SI.name = myInventory.items[i].itemName;
            SI.count = myInventory.items[i].numberHeld;
            SL.serializableList.Add(SI);
        }

    }

    public void SaveScriptables()
    {
        string filepath = Application.persistentDataPath + "/newsave.json";
        StreamWriter sw = new StreamWriter(filepath);
        JSON jsonObject = JSON.Serialize(SL);
        string json = jsonObject.CreatePrettyString();
        sw.WriteLine(json);
        sw.Close();
    }

    public void LoadScriptables()
    {
        switch (saveType)
        {
            case SaveSwitch.json:
                JSONLoad();
                break;

            case SaveSwitch.binary:
                BinaryLoad();
                break;

            default:
                break;
        }
    }

    private void BinarySave()
    {
        BinarySaver.Save(SL.serializableList, "Inventory");
    }

    private void BinaryLoad()
    {
        SL.serializableList = BinarySaver.Load<List<SerialItem>>("Inventory");
    }


    public void JSONLoad()
    {

        //filepath
        string filepath = Application.persistentDataPath + "/newsave.json";

        if (File.Exists(filepath))
        {
            //read in the file to a string
            string json = File.ReadAllText(filepath);
            //use the JSON library to parse the string
            JSON jsonObject = JSON.ParseString(json);
            //deserialize the JSON object back into our Serializable class
            SL = jsonObject.Deserialize<SerializableListString>();
        }

    }

    private void ImportSaveData()
    {
        for (int i = 0; i < SL.serializableList.Count; i++)
        {
            string name = SL.serializableList[i].name;
            int count = SL.serializableList[i].count;

            Item obj = ItemDB.GetItem(name);
            if (obj)
            {
                obj.numberHeld = count;
                myInventory.items.Add(obj);
            }
        }
    }

}