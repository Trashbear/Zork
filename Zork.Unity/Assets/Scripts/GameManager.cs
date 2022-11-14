using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UnityInputService Input;
    [SerializeField]
    private UnityOutputService Output;

    public TMPro.TextMeshProUGUI locationText;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI moveText;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Run(Input, Output);
    }

    private Game _game;
}
