using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

namespace Quiz
{
    public class QuizManager : MonoBehaviour
    {
        public Question[] questions; // Array of your questions
        private int currentQuestionIndex;
        public int correctAnswers { get; private set; }

        public CurrentPlayerUI currentPlayerUI;

        public float totalTimer = 60; // Quiz total timer
        private float timer; // Quiz current timer

        public TMP_Text questionText;
        public Button[] answerButtons;
        public GameObject quizUI; // The UI element containing the quiz

        public GameObject menuUI;
        public GameObject CurrentPlayerCanvas; // The UI element of the current player
        public GameObject MainMenuManager; // The manager for maiun menu UI
        public MainMenuManager mainMenuManager;

        private bool isQuizRunning = false;
        [HideInInspector] public bool isQuizComplete { get; set; } = false;

        void Start()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "questions.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                QuestionContainer questionContainer = JsonUtility.FromJson<QuestionContainer>(json);
                questions = questionContainer.questions;
            }
            else
            {
                Debug.LogError("Cannot find questions file!");
            }

            currentPlayerUI.NextPlayerUI();
            quizUI.SetActive(false);
        }

        void Update()
        {
            // Check if the Escape key is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Toggle the main menu on/off
                mainMenuManager.SetMainMenuActive(true);
                mainMenuManager.isStartMenuActive = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isQuizRunning && !mainMenuManager.isStartMenuActive)
            {
                // Ensure that there are at least 6 questions
                if (questions.Length >= 6)
                {
                    StartCoroutine(StartQuiz());
                }
                else
                {
                    Debug.LogError("Not enough questions for the quiz!");
                }
            }
        }

        void LoadQuestion(int questionIndex)
        {
            Question question = questions[questionIndex];
            // var newOrder = new List<string>();
            // question.correctAnswerIndex=-1;
            // for (int r =0; r<4; r++)
            // {
            //     var randomItem = Random.Range(0,question.answers.Count);
            //     newOrder.Add(question.answers[randomItem]);
            //     if (randomItem == 0 && question.correctAnswerIndex==-1)
            //     {
            //         question.correctAnswerIndex = newOrder.Count-1;
            //     }
            //     question.answers.RemoveAt(randomItem);
            // }
            // question.answers = newOrder;
            // Debug.Log(question.correctAnswerIndex);

            questionText.text = question.questionText;
            for (int i = 0; i < question.answers.Count; i++)
            {
                Debug.Log( question.answers[i]);
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = question.answers[i];
            }
        }

        IEnumerator StartQuiz()
        {
            // Mark the quiz as running
            isQuizRunning = true;

            // Set up quiz
            quizUI.SetActive(true);
            correctAnswers = 0;
            timer = totalTimer;

            // Number of questions asked in this quiz
            int questionsAsked = 0;

            // Player's turn
            while (questionsAsked < 6 && currentQuestionIndex < questions.Length && timer > 0)
            {
                LoadQuestion(currentQuestionIndex);
                int playerAnswer = -1;

                for (int i = 0; i < answerButtons.Length; i++)
                {
                    int index = i;
                    answerButtons[i].onClick.AddListener(() => playerAnswer = index);
                }

                // Wait for player to press a button or time to run out
                yield return new WaitUntil(() => playerAnswer != -1 || timer <= 0);

                // If time ran out, break the loop
                if (timer <= 0)
                    break;

                for (int i = 0; i < answerButtons.Length; i++)
                {
                    answerButtons[i].onClick.RemoveAllListeners();
                }

                // Check the player's answer
                if (playerAnswer == questions[currentQuestionIndex].correctAnswerIndex)
                {
                    correctAnswers++;
                }

                currentQuestionIndex++;
                questionsAsked++;

                // If all questions are done, stop the quiz
                if (currentQuestionIndex == questions.Length)
                {
                    break;
                }
            }

            Debug.Log("Quiz done. Correct answers: " + correctAnswers);

            // Hide the quiz UI after a player's turn
            quizUI.SetActive(false);

            // Set next player
            currentPlayerUI.NextPlayerUI();
            // Mark the quiz as done, so we can trigger player movement. 
            isQuizComplete = true;
            // Mark the quiz as not running
            isQuizRunning = false;
        }

        public void ResetCorrectAnswers()
        {
            correctAnswers = 0;
        }

        private void FixedUpdate()
        {
            if (quizUI.activeSelf)
            {
                // Decrease the timer by the time since the last frame
                timer -= Time.fixedDeltaTime;
            }
        }

        [System.Serializable]
        public class Question
        {
            public string questionText; // The question text
            public List<string> answers; // The possible answers
            public int correctAnswerIndex; // The index of the correct answer in the array
        }

        [System.Serializable]
        public class QuestionContainer
        {
            public Question[] questions;
        }
    }
}
