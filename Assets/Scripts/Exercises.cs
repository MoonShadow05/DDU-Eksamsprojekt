using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Exercises : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;

    [SerializeField] public TMP_Text FeedbackText;

    [HideInInspector] public string correctAnswer;

    [SerializeField] public Exercises.Difficulty questionDifficulty = Exercises.Difficulty.Medium;
    [HideInInspector] public int RightAnswers;
    [HideInInspector] public int WrongAnswers;

    [System.Serializable]
    public class Exercise
    {
        public string question;
        public string correctAnswer;
        public string[] options;

        public string FeedbackText;

        public int level; // 0 = Easy, 1 = Medium, 2 = Hard
    }

    void Update(){
        if(RightAnswers == 3 || WrongAnswers == 3){
            if(RightAnswers == 3 && questionDifficulty == Exercises.Difficulty.Easy || WrongAnswers == 3 && questionDifficulty == Exercises.Difficulty.Hard){
                questionDifficulty = Exercises.Difficulty.Medium;
                /* Debug.Log("Nyt niveau er mellem"); */

            }
            else if(RightAnswers == 3 && questionDifficulty == Exercises.Difficulty.Medium){
                questionDifficulty = Exercises.Difficulty.Hard;
                /* Debug.Log("Nyt niveau er svær"); */

            }
            else if(WrongAnswers == 3 && questionDifficulty == Exercises.Difficulty.Medium){
                questionDifficulty = Exercises.Difficulty.Easy;
                /* Debug.Log("Nyt niveau er nem"); */

            }
             RightAnswers = 0;
             WrongAnswers = 0;
        }

    }
    private void Awake()
{
    var HUD = GameObject.Find("HUD");

    if (HUD != null)
    {
        // Find question text
        if (questionText == null)
        {
            var questionObj = HUD.transform.Find("PopupMenu/Question");
            if (questionObj != null)
                questionText = questionObj.GetComponent<TMP_Text>();
        }

        // Find buttons under ButtonGroup
        var buttonGroup = HUD.transform.Find("PopupMenu/ButtonGroup");
        /* Debug.Log($"ButtonGroup found: {buttonGroup != null}"); */
        if (buttonGroup != null)
        {
            Button[] found = buttonGroup.GetComponentsInChildren<Button>();
            /* Debug.Log(buttonGroup.childCount); */
            answerButtons = found;
            /* Debug.Log($"✅ Found {answerButtons.Length} buttons under ButtonGroup."); */
        }
        else
        {
            /* Debug.LogWarning("❌ ButtonGroup not found under PopupMenu."); */
        }
    
    }
}


    public void GenerateQuestions()
    {
        // This method is for generating questions in the inspector.
        // You can add your own questions here or load them from a file.
        // For now, it's empty.

        // Example question:
        easyQuestions.Add(
            new Exercise { question = "Hvad er 2+2?", correctAnswer = "4", options = new string[] { "3", "4", "5" }, level = 0, FeedbackText = "Rigtig!" });
        easyQuestions.Add(
            new Exercise { question = "Hvad er 3+5?", correctAnswer = "8", options = new string[] { "7", "8", "9" }, level = 0, FeedbackText = "Rigtig!" });
        easyQuestions.Add(
            new Exercise { question = "Hvad er 5-2?", correctAnswer = "3", options = new string[] { "1", "2", "3" }, level = 0, FeedbackText = "Rigtig!" });

        mediumQuestions.Add(
            new Exercise { question = "Hvad er 10*2?", correctAnswer = "20", options = new string[] { "15", "20", "25" }, level = 1, FeedbackText = "Rigtig!" });
        mediumQuestions.Add(
            new Exercise { question = "Hvad er 15/3?", correctAnswer = "5", options = new string[] { "4", "5", "6" }, level = 1, FeedbackText = "Rigtig!" });
        mediumQuestions.Add(
            new Exercise { question = "Hvad er 12-4?", correctAnswer = "8", options = new string[] { "6", "7", "8" }, level = 1, FeedbackText = "Rigtig!" });
        
        hardQuestions.Add(
            new Exercise { question = "Hvad er 2^3?", correctAnswer = "8", options = new string[] { "6", "7", "8" }, level = 2, FeedbackText = "Rigtig!" });
        hardQuestions.Add(
            new Exercise { question = "Hvad er kvadratroden af 16?", correctAnswer = "4", options = new string[] { "3", "4", "5" }, level = 2, FeedbackText = "Rigtig!" });
        hardQuestions.Add(
            new Exercise { question = "Hvad er 5*5?", correctAnswer = "25", options = new string[] { "20", "25", "30" }, level = 2, FeedbackText = "Rigtig!" }); 

    }

    [Header("Difficulty Lists")]
    public List<Exercise> easyQuestions;
    public List<Exercise> mediumQuestions;
    public List<Exercise> hardQuestions;

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
            /* Debug.LogWarning("No questions found for selected difficulty."); */
            return;
        }

        Exercise exercise = selectedList[Random.Range(0, selectedList.Count)];
        correctAnswer = exercise.correctAnswer;
        questionText.text = exercise.question;

        List<string> shuffled = new List<string>(exercise.options);
        ShuffleList(shuffled);

        if (answerButtons.Length < shuffled.Count)
        {
            /* Debug.LogError("Number of answer buttons does not match number of options."); */
            return;
        }

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

    public void Start()
    {
        GenerateQuestions();
        /* Debug.Log("Questions generated."); */
    }
}