using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace FUTUREVISION
{
    public class DataModel : BaseModel
    {
        [Header("Data Model")]
        [Header("Data Model/Parameter")]
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        [Header("Data Model/Data")]
        public string mode;

        [Header("Data Model/Reference")]
        public List<AssetReference> ObjectReferences = new List<AssetReference>();

        public List<int> AnsweredQuestionIndices = new List<int>();

        [Header("Data Model/Quiz")]
        public QuizData QuizData;

        public override void Initialize()
        {
            // Parameter
            InitializeParameters();

            // Data
            string mode = Parameters.ContainsKey("mode") ? Parameters["mode"] : "";

            // QuizData 누락 경고
            if (QuizData == null)
            {
                Debug.LogError("[DataModel] QuizData가 연결되지 않았습니다. Inspector에서 QuizData 에셋을 연결해주세요.", this);
            }

            // Reference
            // 최초 실행시 어드레서블 오브젝트 로드
            foreach (var objRef in ObjectReferences)
            {
                objRef.LoadAssetAsync<GameObject>();
            }
        }

        private void InitializeParameters()
        {
            // URL 파라미터에서 값 가져오기
            var url = Application.absoluteURL;

            if (url.Contains("?"))
            {
                var param = url.Split('?')[1];  // ? 뒤의 파라미터 부분만 가져오기
                var paramList = param.Split('&');
                Parameters = new Dictionary<string, string>();

                foreach (var p in paramList)
                {
                    var keyValue = p.Split('=');
                    if (keyValue.Length == 2)
                    {
                        // URL 디코딩 적용 (특수문자 및 한글 처리)
                        string key = WWW.UnEscapeURL(keyValue[0]);
                        string value = WWW.UnEscapeURL(keyValue[1]);
                        Parameters[key] = value;
                    }
                }
            }
        }
        #region Data Management
        public void SaveJsonData<T>(string key, T data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public T LoadJsonData<T>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string jsonData = PlayerPrefs.GetString(key);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            // Simplify 'default' expression to remove IDE0034 warning
            return default;
        }

        public void DeleteData(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
        #endregion
    }
}
