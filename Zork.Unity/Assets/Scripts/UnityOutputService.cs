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

    [SerializeField]
    [Range(0, 20)]
    private int MaxEntries;

    public void Write(object obj) => ParseAndWriteLine(obj.ToString());

    public void Write(string message) => ParseAndWriteLine(message);

    public void WriteLine(object obj) => ParseAndWriteLine(obj.ToString());

    public void WriteLine(string message) => ParseAndWriteLine(message);

    private void ParseAndWriteLine(string message)
    {
        var textLine = Instantiate(TextLinePrefab, ContentTransform);
        textLine.text = message;
        _entries.Add(textLine.gameObject);
        var newLine = Instantiate(TextLinePrefab, ContentTransform);
        newLine.text = "";
        if (_entries.Count >= MaxEntries)
        {
            _entries.RemoveAt(0);
        }
    }

    private List<GameObject> _entries = new List<GameObject>();
}
