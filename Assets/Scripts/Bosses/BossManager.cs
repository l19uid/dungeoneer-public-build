using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Dungeoneer
{
    public class BossManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> bossPhases;
        [SerializeField] private int currentPhase = 0;
        [SerializeField] private GameObject currentBoss;
        [SerializeField] private bool isStarted = false;
        [SerializeField] private bool isFinished = false;
        [SerializeField] private bool isSwitchingPhases = false;
        [SerializeField] private float _phaseWait = 2.5f;
        private Vector2 _spawnPosition;
        [SerializeField] private Image _healthBar;
        [SerializeField] private GameObject _exitPortal;
        [SerializeField] private GameObject[] _rewardChests;
        private GradingSystem _gradingSystem;
        public AnimationClip _spawnAnimation;

        private void Start()
        {
            _gradingSystem = GameObject.Find("Generator").GetComponent<GradingSystem>();
        }

        public IEnumerator StartBoss(Vector2 spawnPosition)
        {
            EnemyManager.KillAllEnemies();
            isSwitchingPhases = true;
            _spawnPosition = spawnPosition;
            currentBoss = Instantiate(bossPhases[0], _spawnPosition, Quaternion.identity);
            currentBoss.GetComponent<BossPhase>().CreateBoss(this);
            currentBoss.GetComponent<BossPhase>().SetCanMove(false);
            CameraMovement.SetFocus(currentBoss);
            if (GetComponentInChildren<Health>() != null)
                currentBoss.GetComponentInChildren<Health>().healthBar = _healthBar;
            currentBoss.GetComponentInChildren<Animator>().Play(_spawnAnimation.name);
            yield return new WaitForSeconds(3f);
            CameraMovement.SetFocus(null);
            currentBoss.GetComponent<BossPhase>().SetCanMove(true);
            isStarted = true;
            isSwitchingPhases = false;
        }

        private void Update()
        {
            if (isFinished)
                return;
            if (isSwitchingPhases)
                currentBoss.transform.position =
                    Vector2.Lerp(currentBoss.transform.position, _spawnPosition, Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.N))
                EndPhase();
        }

        public void EndPhase()
        {
            currentBoss.GetComponent<BossPhase>().SetCanMove(false);
            currentPhase++;

            StartCoroutine(SwitchPhase(5f));
        }

        private IEnumerator SwitchPhase(float waitTime = 2.5f)
        {
            isSwitchingPhases = true;
            yield return new WaitForSeconds(waitTime);
            NextPhase();
        }

        private void NextPhase()
        {
            isSwitchingPhases = false;

            if (currentBoss != null)
                Destroy(currentBoss);
            if (currentPhase < bossPhases.Count)
            {
                currentBoss = Instantiate(bossPhases[currentPhase], _spawnPosition, Quaternion.identity);
                currentBoss.GetComponent<BossPhase>().CreateBoss(this);
                if (GetComponentInChildren<Health>() != null)
                    currentBoss.GetComponentInChildren<Health>().healthBar = _healthBar;
            }
            else
                FinishBoss();
        }

        private void FinishBoss()
        {
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(0))
                _rewardChests[0].gameObject.SetActive(true);
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(1))
                _rewardChests[1].gameObject.SetActive(true);
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(2))
                _rewardChests[2].gameObject.SetActive(true);
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(3))
                _rewardChests[3].gameObject.SetActive(true);
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(4))
                _rewardChests[4].gameObject.SetActive(true);
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(5))
                _rewardChests[5].gameObject.SetActive(true);
            if(_gradingSystem.GetScore() >= _gradingSystem.GetGradeThreshold(6))
                _rewardChests[6].gameObject.SetActive(true);
            isFinished = true;

            _exitPortal.SetActive(true);
            _gradingSystem.FinishDungeon();
            GameObject.FindWithTag("Canvas").GetComponent<UIManager>().FinishFloor();
        }
    }
}
