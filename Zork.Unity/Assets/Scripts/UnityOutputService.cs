using UnityEngine;
using UnityEngine.UI;
using Zork.Common;
using TMPro;
using System.Collections.Generic;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private TMPro.TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private Image NewLinePrefab;

    [SerializeField]
    private Transform ContentTransform;

    public void Write(object obj) => ParseAndWriteLine(obj.ToString());

    public void Write(string message) => ParseAndWriteLine(message);

    public void WriteLine(object obj) => ParseAndWriteLine(obj.ToString());

    public void WriteLine(string message) => ParseAndWriteLine(message);

    private void ParseAndWriteLine(string message)
    {
       var textLine = Instantiate(TextLinePrefab, ContentTransform);
        textLine.text = message;
    }

    private List<GameObject> _entries = new List<GameObject>();
}
