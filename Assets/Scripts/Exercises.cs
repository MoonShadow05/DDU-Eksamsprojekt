using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Exercises : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;

    [HideInInspector] public string correctAnswer;

    [SerializeField] private Exercises.Difficulty questionDifficulty = Exercises.Difficulty.Easy;


    [System.Serializable]
    public class Exercise
    {
        public string title;
        public string question;
        public string correctAnswer;
        public string[] options;
    }

    [Header("Difficulty Lists")]
    public List<Exercise> easyQuestions = new List<Exercise>();
    public List<Exercise> mediumQuestions = new List<Exercise>();
    public List<Exercise> hardQuestions = new List<Exercise>();

    public enum Difficulty { Easy, Medium, Hard }

    public void LoadRandomQuestion(QuestionPopupTrigger triggerScript, Difficulty difficulty = Difficulty.Easy)
    {
        List<Exercise> selectedList = difficulty switch
        {
            Difficulty.Medium => mediumQuestions,
            Difficulty.Hard => hardQuestions,
            _ => easyQuestions,
        };

        if (selectedList.Count == 0)
        {
            Debug.LogWarning("No questions found for selected difficulty.");
            return;
        }

        Exercise exercise = selectedList[Random.Range(0, selectedList.Count)];
        correctAnswer = exercise.correctAnswer;
        questionText.text = exercise.question;

        List<string> shuffled = new List<string>(exercise.options);
        ShuffleList(shuffled);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = shuffled[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() =>
            {
                triggerScript.CheckAnswer(shuffled[index]);
            });
        }
    }

    private void ShuffleList(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}