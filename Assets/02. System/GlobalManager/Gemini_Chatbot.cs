/*
 * 작성자: Claude
 * 작성일: 2026.03.01
 */

using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace FUTUREVISION
{
    public class Gemini_Chatbot : MonoBehaviour
    {
        [Header("Gemini Chatbot")]
        [SerializeField] private string apiKey;
        [SerializeField] private string modelName = "gemini-2.0-flash";

        public string LatestResponse { get; private set; }
        public UnityEvent OnReceiveChatbot = new UnityEvent();

        private const string API_URL = "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}";

        public void SendText(string text)
        {
            StartCoroutine(SendRequest(text));
        }

        private IEnumerator SendRequest(string text)
        {
            string url = string.Format(API_URL, modelName, apiKey);

            var requestBody = new GeminiRequest
            {
                contents = new[]
                {
                    new Content
                    {
                        parts = new[] { new Part { text = text } }
                    }
                }
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            using UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[Gemini_Chatbot] 요청 실패: {request.responseCode} - {request.error}\n{request.downloadHandler.text}", this);
                yield break;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<GeminiResponse>(request.downloadHandler.text);
                LatestResponse = response.candidates[0].content.parts[0].text;
                Debug.Log($"[Gemini_Chatbot] 응답: {LatestResponse}", this);
                OnReceiveChatbot.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"[Gemini_Chatbot] 응답 파싱 실패: {e.Message}\n{request.downloadHandler.text}", this);
            }
        }

        #region JSON Data Classes
        [Serializable]
        private class GeminiRequest
        {
            public Content[] contents;
        }

        [Serializable]
        private class Content
        {
            public Part[] parts;
        }

        [Serializable]
        private class Part
        {
            public string text;
        }

        [Serializable]
        private class GeminiResponse
        {
            public Candidate[] candidates;
        }

        [Serializable]
        private class Candidate
        {
            public Content content;
        }
        #endregion
    }
}
