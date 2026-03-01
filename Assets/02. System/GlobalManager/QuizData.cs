using System;
using System.Collections.Generic;
using UnityEngine;

namespace FUTUREVISION
{
    [Serializable]
    public class AnswerGroup
    {
        public List<string> Answers = new List<string>();
    }

    [CreateAssetMenu(fileName = "QuizData", menuName = "FUTUREVISION/QuizData")]
    public class QuizData : ScriptableObject
    {
        [Header("설문 질문")]
        public List<string> Questions = new List<string>();

        [Header("설문 답변 (질문 순서와 동일)")]
        public List<AnswerGroup> AnswerGroups = new List<AnswerGroup>();

        [Header("Gemini 추천 코스 설명")]
        public List<string> Recommendations = new List<string>();
    }
}
