using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UnityInputService InputService;
    [SerializeField]
    private UnityOutputService OutputService;

    [SerializeField]
    private TMPro.TextMeshProUGUI locationText;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    private TMPro.TextMeshProUGUI moveText;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChanged += Player_LocationChanged;
        _game.Run(InputService, OutputService);
    }
    private void Start()
    {
        InputService.SetFocus();
        locationText.text = _game.Player.CurrentRoom.Name;
        scoreText.text = $"Score: {_game.Player.Score}";
        moveText.text = $"Moves: {_game.Player.MovesMade}";
    }

    private void Player_LocationChanged(object sender, Room location)
    {
        locationText.text = location.Name;
        moveText.text = $"Moves: {_game.Player.MovesMade}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
            scoreText.text = $"Score: {_game.Player.Score}";
        }

        if (_game.IsRunning == false)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    private Game _game;
}
