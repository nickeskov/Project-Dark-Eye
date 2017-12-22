using System;
using System.IO;
using UnityEngine;


class ErrorLogging : MonoBehaviour
{
    private StreamWriter _sw;
    public string LogName = "ErrorLogs.txt";

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        _sw = new StreamWriter(Application.persistentDataPath + "/" + LogName);
        Debug.Log(Application.persistentDataPath + "/" + LogName);
    }

    private void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }

    private void OnDisable()
    {
        Application.RegisterLogCallback(HandleLog);
    }

    void HandleLog(string errorName, string errorTrace, LogType errorType)
    {
        _sw.WriteLine("The error was in" + System.DateTime.Now.ToString() + " . The name of error is: " + errorName + " the trace is: " + errorTrace + " and the type of error is" + errorType.ToString());
    }

    private void OnDestroy()
    {
        _sw.Close();

    }
}

