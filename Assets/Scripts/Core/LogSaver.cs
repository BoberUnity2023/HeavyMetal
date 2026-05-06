using System.Collections.Generic;
using System;
using UnityEngine;

public class LogSaver : MonoBehaviour
{
    [SerializeField] private List<string> ignories = null;

    [Serializable]
    public struct LogParams
    {
        public string condition;
        public string stackTrace;
        public LogType type;
        public string dateTime;

        public LogParams(string condition, string stackTrace, LogType type, string dateTime)
        {
            this.condition = condition;
            this.stackTrace = stackTrace;
            this.type = type;
            this.dateTime = dateTime;
        }
    }

    [Serializable]
    public class LogInfo
    {
        public List<LogParams> logParamsList = new List<LogParams>();
    }

    LogInfo logInfo = new LogInfo();
    private void Awake()
    { 
        Application.logMessageReceived += LogCallback;
        Debug.Log("LogSaver inited");
    }

    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        Debug.Log("Log: " + condition + "TYPE: " + type);
        if (type == LogType.Log || type == LogType.Warning)
            stackTrace = "";

        LogParams logInfo = new LogParams(condition, stackTrace, type, DateTime.Now.ToString("d MMMM yyyy H:mm:ss"));

        if (condition.Contains("Some objects were not cleaned"))
        {
            Debug.Log("sTOP gAME");
            Debug.Break(); 
        }

        //this.logInfo.logParamsList.Add(logInfo);
        //logFull.text = "";

        //for (int _i = Mathf.Max(0, this.logInfo.logParamsList.Count - 32); _i < this.logInfo.logParamsList.Count; _i++)
        //{
        //    LogParams _log = this.logInfo.logParamsList[_i];
        //    string _shortenCondition = _log.condition.Substring(0, Mathf.Min(_log.condition.Length, 200));

        //    if (_log.type == LogType.Warning)
        //    {
        //        logFull.text = "\n " + "<color=#00ff00>" + _i + ". " + _shortenCondition + "</color>" + logFull.text;
        //    }
        //    if (_log.type == LogType.Error)
        //    {
        //        logFull.text = "\n " + "<color=#ffaa00>" + _i + ". " + _shortenCondition + "</color>" + logFull.text;
        //    }
        //    if (_log.type == LogType.Exception)
        //    {
        //        logFull.text = "\n " + "<color=#ff0000>" + _i + ". " + _log.condition + "\n" + _log.stackTrace + "</color>" + logFull.text;
        //        if (PlayerPrefs.GetInt("ShowErrors") == 1)
        //        {
        //            if (logRegime == LogRegime.Off)//Автовключение лога
        //                ChangeLog();
        //        }
        //    }
        //    if (_log.type == LogType.Log)
        //    {
        //        logFull.text = "\n " + _i + ". " + _shortenCondition + logFull.text;
        //    }
        //}


        //logGame.text = "";
        //int _count = 0;
        //int _maxLogs = 40;

        //for (int _i = Mathf.Max(0, this.logInfo.logParamsList.Count - 200); _i < this.logInfo.logParamsList.Count; _i++)
        //{
        //    LogParams _log = this.logInfo.logParamsList[_i];
        //    string _shortenCondition = _log.condition.Substring(0, Mathf.Min(_log.condition.Length, 200));

        //    if (_log.type == LogType.Warning)
        //    {
        //        _count++;
        //        string _color = "<color=#00ff00>";
        //        if (_shortenCondition.Contains("Ответ сервера") || _shortenCondition.Contains("Поиск карты"))//серым выделим логи сервера
        //            _color = "<color=#888888>";

        //        if (_count <= _maxLogs)
        //        {
        //            string _text = logGame.text;
        //            logGame.text = "\n " + _color + _i + ". " + _shortenCondition + "</color>" + _text;
        //        }
        //    }

        //    if (showErrorsInGameLog)
        //    {
        //        if (_log.type == LogType.Error)
        //        {
        //            string _text = logGame.text;
        //            logGame.text = "\n " + "<color=#ffaa00>" + _i + ". " + _shortenCondition + "</color>" + _text;
        //        }
        //        if (_log.type == LogType.Exception)
        //        {
        //            string _text = logGame.text;
        //            logGame.text = "\n " + "<color=#ff0000>" + _i + ". " + _log.condition + "\n" + _log.stackTrace + "</color>" + _text;
        //        }
        //    }
        //}
    }
}
