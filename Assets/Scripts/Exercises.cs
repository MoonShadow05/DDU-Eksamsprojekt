using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Exercises : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TMP_Text equationText;
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
        public string equation;
        public string correctAnswer;
        public string[] options;
        public string FeedbackText;

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

        // Find equation text
        if (equationText == null)
        {
            var equationObj = HUD.transform.Find("PopupMenu/Equation");
            if (equationObj != null)
                equationText = equationObj.GetComponent<TMP_Text>();
        }

        if (FeedbackText == null)
        {
            var feedbackObj = HUD.transform.Find("FeedbackMenu/FeedbackText");
            if (feedbackObj != null)
                FeedbackText = feedbackObj.GetComponent<TMP_Text>();
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

    // Easy questions
    easyQuestions.Add(
    new Exercise {
        question = "Hvad bliver værdien, hvor x = 1?",
        equation = "f(x) = x - 2",
        correctAnswer = "-1",
        options = new string[] { "1", "-2", "-1" },
        FeedbackText = "Trin 1: f(x) = x - 2;\nTrin 2: Vi ved at x = 1;\nTrin 3: Indsætter i formlen f(1) -> 1 * 1 - 2 = -1"
    });

    easyQuestions.Add(
        new Exercise {
            question = "Hvad bliver værdien, hvor x = 2?",
            equation = "f(x) = 2x + 1",
            correctAnswer = "5",
            options = new string[] { "12", "5", "2" },
            FeedbackText = "Trin 1: f(x) = 2x + 1;\nTrin 2: Vi ved at x = 2;\nTrin 3: Indsætter i formlen f(2) -> 2 * 2 + 1 = 5"
        });

    easyQuestions.Add(
        new Exercise {
            question = "Hvad bliver værdien, hvor x = 7?",
            equation = "f(x) = x + (-3)",
            correctAnswer = "4",
            options = new string[] { "7", "2", "4" },
            FeedbackText = "Trin 1: f(x) = x + (-3);\nTrin 2: Vi ved at x = 7;\nTrin 3: Indsætter i formlen f(7) -> 7 + (-3) = 4"
        });

    easyQuestions.Add(
        new Exercise {
            question = "Hvad bliver værdien, hvor x = 9?",
            equation = "f(x) = 3x + (-4)",
            correctAnswer = "23",
            options = new string[] { "23", "13", "20" },
            FeedbackText = "Trin 1: f(x) = 3x + (-4);\nTrin 2: Vi ved at x = 9;\nTrin 3: Indsætter i formlen f(9) -> 3 * 9 + (-4) = 23"
        });

    easyQuestions.Add(
        new Exercise {
            question = "Hvad bliver værdien, hvor x = 0?",
            equation = "f(x) = 5x + 3",
            correctAnswer = "3",
            options = new string[] { "0", "3", "5" },
            FeedbackText = "Trin 1: f(x) = 5x + 3;\nTrin 2: Vi ved at x = 0;\nTrin 3: Indsætter i formlen f(0) -> 5 * 0 + 3 = 3"
        });

    mediumQuestions.Add(
        new Exercise {
            question = "Hvad er hældningen af denne rette linje gennem punkterne?",
            equation = "A(3, 7) og B(9, 19)",
            correctAnswer = "2",
            options = new string[] { "2", "5", "10" },
            FeedbackText = "Trin 1: Brug formlen = (y2 - y1 / x2 - x1);\nTrin 2: Indsæt værdierne i formlen: (19 - 7) / (9 - 3);\nTrin 3: Forenkle: 12 / 6 = 2"
        });

    mediumQuestions.Add(
        new Exercise {
            question = "Hvad er hældningen af denne rette linje gennem punkterne?",
            equation = "A(4, 7) og B(7, 10)",
            correctAnswer = "1",
            options = new string[] { "4", "1", "6" },
            FeedbackText = "Trin 1: Brug formlen = (y2 - y1 / x2 - x1);\nTrin 2: Indsæt værdierne i formlen: (10 - 7) / (7 - 4);\nTrin 3: Forenkle: 3 / 3 = 1"
        });

    mediumQuestions.Add(
        new Exercise {
            question = "Hvad er hældningen af denne rette linje gennem punkterne?",
            equation = "A(4, 21) og B(6, 29)",
            correctAnswer = "4",
            options = new string[] { "6", "7", "4" },
            FeedbackText = "Trin 1: Brug formlen = (y2 - y1 / x2 - x1);\nTrin 2: Indsæt værdierne i formlen: (29 - 21) / (6 - 4);\nTrin 3: Forenkle: 8 / 2 = 4"
        });

    mediumQuestions.Add(
        new Exercise {
            question = "Hvad er hældningen af denne rette linje gennem punkterne?",
            equation = "A(0, 5) og B(10, 15)",
            correctAnswer = "1",
            options = new string[] { "1", "7", "2" },
            FeedbackText = "Trin 1: Brug formlen = (y2 - y1 / x2 - x1);\nTrin 2: Indsæt værdierne i formlen: (15 - 5) / (10 - 0);\nTrin 3: Forenkle: 10 / 10 = 1"
        });

    mediumQuestions.Add(
        new Exercise {
            question = "Hvad er hældningen af denne rette linje gennem punkterne?",
            equation = "A(0, 3) og B(9, 48)",
            correctAnswer = "5",
            options = new string[] { "5", "6", "9" },
            FeedbackText = "Trin 1: Brug formlen = (y2 - y1 / x2 - x1);\nTrin 2: Indsæt værdierne i formlen: (48 - 3) / (9 - 0);\nTrin 3: Forenkle: 45 / 9 = 5"
        });

    hardQuestions.Add(
        new Exercise {
            question = "Du skal løse denne ligning og finde x:",
            equation = "5x - 7 = 3x + 9",
            correctAnswer = "8",
            options = new string[] { "-5", "9", "8" },
            FeedbackText = "Trin 1: Træk 3x -> 2x - 7 = 9;\nTrin 2: Læg 7 til begge sider -> 2x = 16;\nTrin 3: Del med 2 -> x = 8"
        });

    hardQuestions.Add(
        new Exercise {
            question = "Du skal løse denne ligning og finde x:",
            equation = "4x + 6 = 2x + 12",
            correctAnswer = "3",
            options = new string[] { "3", "4", "5" },
            FeedbackText = "Trin 1: Træk 2x -> 4x - 2x + 6 = 12;\nTrin 2: Læg 6 til begge sider -> 2x = 6;\nTrin 3: Del med 2 -> x = 3"
        });

    hardQuestions.Add(
        new Exercise {
            question = "Du skal løse denne ligning og finde x:",
            equation = "7x - 4 = 5x + 10",
            correctAnswer = "7",
            options = new string[] { "7", "-3", "5" },
            FeedbackText = "Trin 1: Træk 5x -> 7x - 5x - 4 = 10;\nTrin 2: Læg 4 til begge sider -> 2x = 14;\nTrin 3: Del med 2 -> x = 7"
        });

    hardQuestions.Add(
        new Exercise {
            question = "Du skal løse denne ligning og finde x:",
            equation = "8x + 5 = 6x + 15",
            correctAnswer = "5",
            options = new string[] { "9", "12", "5" },
            FeedbackText = "Trin 1: Træk 6x -> 8x - 6x + 5 = 15;\nTrin 2: Læg 5 til begge sider -> 2x = 10;\nTrin 3: Del med 2 -> x = 5"
        });

    hardQuestions.Add(
        new Exercise {
            question = "Du skal løse denne ligning og finde x:",
            equation = "10x - 12 = 3x + 9",
            correctAnswer = "3",
            options = new string[] { "4", "1", "3" },
            FeedbackText = "Trin 1: Træk 3x -> 10x - 3x - 12 = 9;\nTrin 2: Læg 12 til begge sider -> 7x = 21;\nTrin 3: Del med 7 -> x = 3"
        });



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
        equationText.text = exercise.equation;
        currentExercise = exercise;

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