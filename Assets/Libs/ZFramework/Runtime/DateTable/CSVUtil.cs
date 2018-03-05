using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine; 

public class CSVUtil
{
    public static Dictionary<int, T> ReadCSV<T>(string filePath, Dictionary<int, T> entity)
    {
        ZFramework.Runtime.GameEntry.GetComponent<ZFramework.Runtime.ResourceComponent>().LoadDataTable(filePath, (CSVObject) =>
        {
            if (null != CSVObject)
            {
                OnCSVLoad<T>(CSVObject, entity);
            }
        });
        return entity;
    }

    private static void OnCSVLoad<T>(object CSVObject, Dictionary<int, T> entity)
    {
        List<string> tempTable = new List<string>();
        ByteReader reader = new ByteReader(CSVObject as TextAsset);
        //表头
        List<string> temp = reader.ReadCSV();
        for (int i = 0; i < temp.Count; i++)
        {
            tempTable.Add(temp[i]);
        }
        Type type = typeof(T);
        PropertyInfo[] piList = type.GetProperties();
        //表的第二行数据类型
        List<string> tempType = reader.ReadCSV();
        //表的第三行数据说明
        List<string> tempInfo = reader.ReadCSV();
        //行内容
        temp = reader.ReadCSV();
        int nIndex = 0;

        while (null != temp)
        {
            Type t = typeof(T);
            T entityValue = (T)Assembly.GetExecutingAssembly().CreateInstance(t.FullName, false);
            if (temp[0] == null)
            {
                Debug.LogError("csv表格中有空行");
            }

            int key = 0;
            int.TryParse(temp[0], out key);
            if (key == 0)
            {
                Debug.Log("CSV id:" + "csv表格:" + t.FullName + " 中有空行.");
            }

            entity.Add(key, entityValue);
            int colIndex = 0;
            for (int b = 0; b < piList.Length; b++)
            {
                try
                {
                    if (tempTable.Contains(piList[b].Name) && temp[colIndex] != "")
                    {
                        switch (piList[b].PropertyType.ToString())
                        {
                            case "System.String":
                                piList[b].SetValue(entityValue, Convert.ToString(temp[colIndex]), null);
                                break;

                            case "System.Int32":
                                piList[b].SetValue(entityValue, int.Parse(temp[colIndex]), null);
                                break;

                            case "System.Boolean":
                                piList[b].SetValue(entityValue, bool.Parse(temp[colIndex]), null);
                                break;

                            case "System.Single":
                                piList[b].SetValue(entityValue, float.Parse(temp[colIndex]), null);
                                break;

                            case "UnityEngine.Vector2":
                                string value = temp[colIndex];
                                string[] vecs = value.Split(';');
                                Vector2 vec = new Vector2(float.Parse(vecs[0]), float.Parse(vecs[1]));
                                piList[b].SetValue(entityValue, vec, null);
                                break;

                            case "System.Collections.Generic.List`1[System.Int32]":
                                string[] values = temp[colIndex].Split(';');
                                List<int> valueList = new List<int>();
                                for (int j = 0; j < values.Length; j++)
                                {
                                    if (values[j] != null && values[j] != "")
                                    {
                                        valueList.Add(Convert.ToInt32(values[j]));
                                    }
                                }
                                piList[b].SetValue(entityValue, valueList, null);
                                break;

                            case "System.Collections.Generic.List`1[System.String]":
                                string[] strvalues = temp[colIndex].Split(';');
                                List<string> strvalueList = new List<string>();

                                for (int j = 0; j < strvalues.Length; j++)
                                {
                                    strvalueList.Add(strvalues[j]);
                                }
                                piList[b].SetValue(entityValue, strvalueList, null);
                                break;

                            case "System.Collections.Generic.List`1[System.Single]":
                                string[] flvalues = temp[colIndex].Split(';');
                                List<float> flvalueList = new List<float>();

                                for (int j = 0; j < flvalues.Length; j++)
                                {
                                    flvalueList.Add(float.Parse(flvalues[j]));
                                }
                                piList[b].SetValue(entityValue, flvalueList, null);
                                break;

                            case "System.Collections.Generic.List`1[UnityEngine.Vector2]":
                                string[] vecvalues = temp[colIndex].Split(';');
                                List<Vector2> vecvalueList = new List<Vector2>();
                                for (int j = 0; j < vecvalues.Length; j++)
                                {
                                    string[] xy = vecvalues[j].Split(':');
                                    vecvalueList.Add(new Vector2(float.Parse(xy[0]), float.Parse(xy[1])));
                                }
                                piList[b].SetValue(entityValue, vecvalueList, null);
                                break;

                            default:
                                Debug.Log(t.FullName.ToString() + "表中" + temp[colIndex] + " 转换失败 类型：" + piList[b].PropertyType.ToString());
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("**************" + "csv表格:" + t.FullName + "**************" + piList[b].Name + "**************" + colIndex + "==" + temp[colIndex]);
                }

                colIndex++;
            }
            nIndex++;
            temp = reader.ReadCSV();
        }
    }
}