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

    [SerializeField] public static Exercises.Difficulty questionDifficulty = Exercises.Difficulty.Medium;
    [HideInInspector] public static int RightAnswers;
    [HideInInspector] public static int WrongAnswers;

     public Exercise currentExercise { get; set; }

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
        if(RightAnswers == 3){
            if(questionDifficulty == Exercises.Difficulty.Easy){
                questionDifficulty = Exercises.Difficulty.Medium;
                Debug.Log("Nyt niveau er mellem");
            }
            else{
                questionDifficulty = Exercises.Difficulty.Hard;
               Debug.Log("Nyt niveau er svær");
            }
            RightAnswers = 0;
        }
        if(WrongAnswers == 3){
            if(questionDifficulty == Exercises.Difficulty.Hard){
                questionDifficulty = Exercises.Difficulty.Medium;
                Debug.Log("Nyt niveau er mellem");
            }
            else{
                questionDifficulty = Exercises.Difficulty.Easy;
                Debug.Log("Nyt niveau er nem");
            }
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
            new Exercise { question = "Hvad bliver værdien af funktionen f(x) = x - 2, hvor x = 1?", correctAnswer = "-1", options = new string[] { "1", "-2", "-1" }, level = 0, FeedbackText = "Rigtig!" });
        easyQuestions.Add(
            new Exercise { question = "Hvad bliver værdien af funktionen f(x) = 2x + 1, hvor x = 2?", correctAnswer = "5", options = new string[] { "12", "5", "2" }, level = 0, FeedbackText = "Rigtig!" });
        easyQuestions.Add(
            new Exercise { question = "Hvad bliver værdien af funktionen f(x) = x + (-3), hvor x = 7?", correctAnswer = "4", options = new string[] { "7", "2", "4" }, level = 0, FeedbackText = "Rigtig!" });
        easyQuestions.Add(
           new Exercise { question = "Hvad bliver værdien af funktionen f(x) = 3x + (-4), hvor x = 9?", correctAnswer = "23", options = new string[] { "23", "13", "20" }, level = 0, FeedbackText = "Rigtig!" });
        easyQuestions.Add(
           new Exercise { question = "Hvad bliver værdien af funktionen f(x) = 5x + 3, hvor x = 0?", correctAnswer = "3", options = new string[] { "0", "3", "5" }, level = 0, FeedbackText = "Rigtig!" });

        mediumQuestions.Add(
            new Exercise { question = "Hvad er hældningen af denne rette linje gennem punkterne A(3, 7) og B(9, 19)?", correctAnswer = "2", options = new string[] { "2", "5", "10" }, level = 1, FeedbackText = "Rigtig!" });
        mediumQuestions.Add(
            new Exercise { question = "Hvad er hældningen af denne rette linje gennem punkterne A(4, 7) og B(7, 10)?", correctAnswer = "1", options = new string[] { "4", "1", "6" }, level = 1, FeedbackText = "Rigtig!" });
        mediumQuestions.Add(
            new Exercise { question = "Hvad er hældningen af denne rette linje gennem punkterne A(4, 21) og B(6, 29)", correctAnswer = "4", options = new string[] { "6", "7", "4" }, level = 1, FeedbackText = "Rigtig!" });
        mediumQuestions.Add(
            new Exercise { question = "Hvad er hældningen af denne rette linje gennem punkterne A(0, 5) og (10, 15)", correctAnswer = "1", options = new string[] { "1", "7", "2" }, level = 1, FeedbackText = "Rigtig!" });
        mediumQuestions.Add(
            new Exercise { question = "Hvad er hældningen af denne rette linje gennem punkterne A(0, 3) og (9, 48)", correctAnswer = "5", options = new string[] { "5", "6", "9" }, level = 1, FeedbackText = "Rigtig!" });


        hardQuestions.Add(
            new Exercise { question = "Du skal løse denne ligning og finde x: 5x - 7 = 3x + 9?", correctAnswer = "8", options = new string[] { "-5", "9", "8" }, level = 2, FeedbackText = "Rigtig!" });
        hardQuestions.Add(
            new Exercise { question = "Du skal løse denne ligning og finde x: 4x + 6 = 2x + 12?", correctAnswer = "3", options = new string[] { "3", "4", "5" }, level = 2, FeedbackText = "Rigtig!" });
        hardQuestions.Add(
            new Exercise { question = "Du skal løse denne ligning og finde x: 7x - 4 = 5x + 10?", correctAnswer = "7", options = new string[] { "7", "-3", "5" }, level = 2, FeedbackText = "Rigtig!" });
        hardQuestions.Add(
            new Exercise { question = "Du skal løse denne ligning og finde x: 8x + 5 = 6x + 15?", correctAnswer = "5", options = new string[] { "9", "12", "5" }, level = 2, FeedbackText = "Rigtig!" });
        hardQuestions.Add(
            new Exercise { question = "Du skal løse denne ligning og finde x: 10x - 12 = 3x + 9?", correctAnswer = "3", options = new string[] { "4", "1", "3" }, level = 2, FeedbackText = "Rigtig!" });


    }

    [Header("Difficulty Lists")]
    public List<Exercise> easyQuestions;
    public List<Exercise> mediumQuestions;
    public List<Exercise> hardQuestions;

    public enum Difficulty { Easy, Medium, Hard }

    public void LoadRandomQuestion(QuestionPopupTrigger triggerScript, Difficulty difficulty = Difficulty.Easy)
    {
        Debug.Log($"vælger spørgsmål med sværhedsgrad: {difficulty.ToString()}");
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