using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] options;
    public int correctAnswerIndex; // Índice de la respuesta correcta en el array de opciones
}

public class QuizLogic : MonoBehaviour
{
    public List<Question> questions; // Lista de preguntas
    public TMP_Text questionText; // Referencia al texto de la pregunta
    public TMP_Text failedphrase; 
    public TMP_Text[] optionTexts; 
    SingletonPattern singletonPattern;
    public GameObject[] checkboxes; 
    public GameObject[] first_planks; 
    public GameObject[] second_planks; 
    public GameObject[] failed_planks; 
    public GameObject correct; 
    public GameObject incorrect; 
    public GameObject questionPanel; 
    public GameObject canvasScore; 
    private List<int> availableIndices;
    private int currentQuestionIndex = -1;
    private int selectedAnswer = -1;
    private int continueGLoop = 0;
    private int randomIndex = 0;
    private bool hasFirstPlanks = false;
    private bool hasSecondPlanks = false;
    
    // Start is called before the first frame update
    void Start()
    {
        singletonPattern = SingletonPattern.Instance;
        // Inicializamos la lista de índices disponibles con todos los índices de las preguntas
        availableIndices = new List<int>();
        for (int i = 0; i < questions.Count; i++)
        {
            availableIndices.Add(i);
        }
        // Mostramos la primera pregunta
        ShowRandomQuestion();
    }

    public void ShowRandomQuestion()
    {
        UpdateCheck(-1);
        if (availableIndices.Count > 0)
        {
            // Seleccionamos un índice aleatorio de los disponibles
            randomIndex = Random.Range(0, availableIndices.Count);
            currentQuestionIndex = availableIndices[randomIndex];

            Question currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;

            for (int i = 0; i < optionTexts.Length; i++)
            {
                if (i < currentQuestion.options.Length)
                {
                    optionTexts[i].text = currentQuestion.options[i];
                }
            }
        }
        else
        {
            Debug.Log("No hay más preguntas disponibles");
        }
    }

    public void UpdateSelectedIndex (int selectedIndex)
    {
        selectedAnswer = selectedIndex;
        UpdateCheck(selectedIndex);
    }

    public void UpdateCheck(int selectedIndex)
    {
        // Desactivar todos los checkboxes primero
        for (int i = 0; i < checkboxes.Length; i++)
        {
            checkboxes[i].SetActive(false);
        }
        if (selectedAnswer >= 0)
        {
            // Luego activar solo el seleccionado
            checkboxes[selectedIndex].SetActive(true);
        }
    }

    public void CheckAnswer()
    {
        if (selectedAnswer == -1)
        {
            return;
        }
        if (selectedAnswer == questions[currentQuestionIndex].correctAnswerIndex)
        {
            // Se activa interfaz de correcto y se desactiva el panel de preguntas
            canvasScore.SetActive(false);
            correct.SetActive(true);
            questionPanel.SetActive(false);
            availableIndices.RemoveAt(randomIndex);  // Eliminamos el índice para que no se repita
        }
        else
        {
            // Llamar a la función ContinueGameFailed para ejecutar 
            // las respectivas acciones
            continueGLoop++;
            ContinueGameFailed();
        }
    }

    public void ActivatePlanks(GameObject[] listPlanks, bool option = true)
    {
        for (int i=0; i<listPlanks.Length; i++)
        {
            listPlanks[i].SetActive(option);
        }
    }

    public void DeactivatePlanks(int from, int to)
    {
        for (int i=from; i<to; i++)
        {
            failed_planks[i].SetActive(false);
        }
    }

    public void ContinueGameFailed()
    {
        if (continueGLoop < 2)
        {
            failedphrase.text = "Tienes otro intento ¡Puedes hacerlo!";
            canvasScore.SetActive(false);
            incorrect.SetActive(true);
            questionPanel.SetActive(false);
            selectedAnswer = -1;
            UpdateCheck(selectedAnswer);
        }
        else
        {
            canvasScore.SetActive(false);
            failedphrase.text = "Lo harás mejor la próxima vez";
            incorrect.SetActive(true);
            questionPanel.SetActive(false);
        }
    }

    public void ButtonContinueFailed()
    {
        if (continueGLoop < 2)
        {
            // Otro intento
            continueGameActions();
        }
        else
        {
            // Failed Actions
            EndGameActions();
        }
    }

    private void continueGameActions()
    {
        incorrect.SetActive(false);
        questionPanel.SetActive(true);
        UpdateCheck(-1);
        ShowRandomQuestion();
    }

    private void EndGameActions()
    {
        canvasScore.SetActive(true);
        incorrect.SetActive(false);
        Time.timeScale = 1f;
        continueGLoop = 0;
        DeactivatePlanks(0, 6);
    }

    public void ContinueGame(GameObject[] listPlanks)
    {
        canvasScore.SetActive(true);
        Time.timeScale = 1f;
        questionPanel.SetActive(false);
        correct.SetActive(false);
        incorrect.SetActive(false);
        ActivatePlanks(listPlanks);
        // singletonPattern.GetDatabase().UpdateData(singletonPattern.GetCollisions().lastIsland);
    }

    public void ResumeGameWin()
    {
        if (!hasFirstPlanks)
        {
            // singletonPattern.SetHasFirstPlanks(true);
            hasFirstPlanks = true;
            ContinueGame(first_planks);
        }
        // else if (!singletonPattern.GetHasSecondPlanks())
        else if (!hasSecondPlanks)
        {
            // singletonPattern.SetHasSecondPlanks(true);
            hasSecondPlanks = true;
            ContinueGame(second_planks);
        }
        else
        {
            Debug.Log("Ambos conjuntos de tablones ya están activos.");
        }
    }

}
