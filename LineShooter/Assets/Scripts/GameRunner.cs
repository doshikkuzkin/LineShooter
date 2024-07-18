using UnityEngine;

public class GameRunner : MonoBehaviour
{
	[SerializeField]
	private EnemiesSettingsScriptableObject _enemiesSettings;

	[SerializeField]
	private Transform[] _enemiesSpawnPoints;

    [SerializeField]
    private Transform _finishLine;

	private EnemiesController _enemiesController;

    private bool _isGameRunning;

    private void Awake()
    {
		_enemiesController = new EnemiesController(_enemiesSettings.EnemiesSettings, _enemiesSpawnPoints, _finishLine.position);
    }

    private void Start()
    {
		StartGame();

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // bottom-left corner
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); // top-right corner

        Debug.Log(min);
        Debug.Log(max);
    }

    private void StartGame()
	{
		_isGameRunning = true;
	}

    private void Update()
    {
        if (!_isGameRunning)
        {
            return;
        }

        _enemiesController.Update();
    }
}
