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

    private void Start()
    {
        InputService.SetFocus();
        locationText.text = _game.Player.CurrentRoom.Name;
    }

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Run(InputService, OutputService);
    }

    private void Player_LocationChanged(object sender, Room location)
    {
        locationText.text = location.Name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if (_game.IsRunning == false)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    private Game _game;
}
