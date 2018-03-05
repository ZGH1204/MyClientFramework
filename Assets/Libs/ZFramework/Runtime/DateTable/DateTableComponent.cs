using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFramework.Runtime
{
    /// <summary>
    /// 配置表组件。
    /// </summary>
    [DisallowMultipleComponent]
    public class DateTableComponent : ZFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 保存所有的csv数据
        /// </summary>
        private Hashtable csvHash = new Hashtable();

        private void OnEnable()
        {
            LoadAllCSV();
        }

        /// <summary>
        ///加载全部的CSV文件
        /// </summary>
        private void LoadAllCSV()
        {
            AddDictValue<UIFormCSV>(DataTableName.UIForm);
            AddDictValue<DropItemCSV>(DataTableName.DropItem);
            AddDictValue<TreasureBoxCSV>(DataTableName.TreasureBox);
            AddDictValue<EquipCSV>(DataTableName.Equip);
        }

        /// <summary>
        /// 读取到缓存
        /// </summary>
        /// <param name="csvName">Csv name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private void AddDictValue<T>(string csvName)
        {
            Dictionary<int, T> dictTmp = ReadCSVTable<T>(csvName);
            csvHash.Add(csvName, dictTmp);
        }

        /// <summary>
        /// CSV 表映射到对象
        /// </summary>
        private Dictionary<int, T> ReadCSVTable<T>(string csvName)
        {
            Dictionary<int, T> csvTable = new Dictionary<int, T>();
            CSVUtil.ReadCSV(csvName, csvTable);
            return csvTable;
        }

        /// <summary>
        /// 获取CSV对象
        /// </summary>
        public Dictionary<int, T> GetCSVByName<T>(string strName)
        {
            if (!csvHash.ContainsKey(strName))
            {
                AddDictValue<T>(strName);
            }
            return (Dictionary<int, T>)csvHash[strName];
        }

        public T GetTypeById<T>(string strName, int id) where T : new()
        {
            T info = default(T);

            Dictionary<int, T> dic = (Dictionary<int, T>)GetCSVByName<T>(strName);
            if (!dic.TryGetValue(id, out info))
            {
                Debug.Log(id.ToString() + " Id字段不存在，请检查！ " + strName);
            }

            return info;
        }

        private void OnDestroy()
        {
            //清理资源
        }
    }
}