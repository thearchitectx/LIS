using System;
using System.Linq;
using TheArchitect.Core;
using TheArchitect.Core.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace TheArchitect.SceneObjects
{
    public enum SmoothTalkMode
    {
        MATCH_SAME,
        RPS
    }
    public class SmoothTalk : SceneObject
    {
        public const string OUTCOME_WIN = "WIN";
        public const string OUTCOME_LOST = "LOST";
        public const string OUTCOME_GIRL_MOVE = "GIRL_MOVE";
        public const string OUTCOME_MOVE_SUCCESS = "MOVE_SUCCESS";
        public const string OUTCOME_MOVE_FAIL = "MOVE_FAIL";
        public Sprite[] SpritesGirl;
        public Sprite[] SpritesBoy;
        public Sprite[] SpritesTable;
        public RectTransform TransformWheelGirl;
        public RectTransform TransformWheelBoy;
        public Image ImageGirl1;
        public Image ImageGirl2;
        public Image ImageBoy1;
        public Image ImageBoy2;
        public Image ImageGirlGaugeLife;
        public Image ImageBoyGaugeLife;
        public Image ImageTipTable;
        public Image ImageTimerGaugeValue;
        public Text TextEnemyName;
        public Text TextHelp;
        public float InputTime = 7;
        public float SpeedGirl = 3;
        public float SpeedBoy = 3;
        public float WheelSize = 200;
        public float TableTipSpeed = 200;
        public SmoothTalkMode Mode = SmoothTalkMode.MATCH_SAME;

        private int m_CounterGirlWheel;
        private int m_CounterBoyWheel;
        private bool m_MoveOnInput;
        private float m_LifePlayer = 1;
        private float m_LifeEnemy = 1;
        private float m_Timer = 0;
        private int m_GirlStopped = 0;
        private int m_BoyStopped = 0;

        private readonly string[] choiceTable = { "daring", "charisma", "intelligence" };
        private Game game;
        private Animator m_Animator;

        private Sprite[] m_SpritesBoyRound;

        void Start()
        {
            this.m_SpritesBoyRound = SpritesBoy;
            game = Resources.Load<Game>(ResourcePaths.SO_GAME);
            this.m_Animator = GetComponent<Animator>();
            this.m_CounterGirlWheel = UnityEngine.Random.Range(0, this.SpritesGirl.Length);
            this.m_CounterBoyWheel = UnityEngine.Random.Range(0, this.SpritesBoy.Length);
            this.ImageGirl1.sprite = this.SpritesGirl[(int)Mathf.Repeat(++this.m_CounterGirlWheel, this.SpritesGirl.Length)];
            this.ImageGirl2.sprite = this.SpritesGirl[(int)Mathf.Repeat(++this.m_CounterGirlWheel, this.SpritesGirl.Length)];
            this.ImageBoy1.sprite = this.m_SpritesBoyRound[(int)Mathf.Repeat(++this.m_CounterBoyWheel, this.SpritesBoy.Length)];
            this.ImageBoy2.sprite = this.m_SpritesBoyRound[(int)Mathf.Repeat(++this.m_CounterBoyWheel, this.SpritesBoy.Length)];
            OneTimeAudioPlayer.GetAndCache("hit-miss");
            OneTimeAudioPlayer.GetAndCache("hit-strong");
            OneTimeAudioPlayer.GetAndCache("ryu-uggh");
            OneTimeAudioPlayer.GetAndCache("chun-li-scream");
            OneTimeAudioPlayer.GetAndCache("you-win");
            OneTimeAudioPlayer.GetAndCache("you-lose");
        }

        public void SetEnemyName(string name)
        {
            TextEnemyName.text = name;
        }

        public void MoveOnInput()
        {
            this.m_Timer = 0;
            this.m_MoveOnInput = true;
        }

        public void Move()
        {
            if (this.m_GirlStopped==0)
            {
                this.m_GirlStopped = 1;
                Outcome = OUTCOME_GIRL_MOVE;
            }
            else if (this.m_BoyStopped==0)
            {
                this.m_BoyStopped = this.TransformWheelBoy.anchoredPosition.y < 100 ? -1 : 1;
                this.m_CounterBoyWheel = this.m_BoyStopped == -1 ? this.m_CounterBoyWheel-1 : this.m_CounterBoyWheel;

                string enemyChoice = this.SpritesGirl[(int) Mathf.Repeat(this.m_CounterGirlWheel, this.SpritesGirl.Length)].name;
                enemyChoice = enemyChoice.Substring(enemyChoice.IndexOf("-")+1).ToLower();
                string playerChoice = this.m_SpritesBoyRound[(int) Mathf.Repeat(this.m_CounterBoyWheel, this.m_SpritesBoyRound.Length)].name;
                playerChoice = playerChoice.Substring(playerChoice.IndexOf("-")+1).ToLower();

                Debug.Log($"{playerChoice} {enemyChoice}");
                if (playerChoice == "karma-g") 
                {
                    if (game.GetFlagState("DICK_KARMA") > 0)
                    {
                        this.m_Animator.SetTrigger("heal");
                        OneTimeAudioPlayer.Play("hit-heal");
                        this.m_LifePlayer = Mathf.Clamp01(this.m_LifePlayer +0.4f);
                        Outcome = OUTCOME_MOVE_SUCCESS;
                    } else {
                        OneTimeAudioPlayer.Play("hit-miss");
                        Outcome = OUTCOME_MOVE_FAIL;
                    }
                }
                else if (playerChoice == "karma-e") 
                {
                    if (game.GetFlagState("DICK_KARMA") < 0)
                    {
                        this.m_Animator.SetTrigger("hit");
                        OneTimeAudioPlayer.Play("hit-strong");
                        this.m_LifeEnemy -= 0.4f;
                        Outcome = this.m_LifeEnemy > 0 ? OUTCOME_MOVE_SUCCESS : OUTCOME_WIN;
                    } else { 
                        OneTimeAudioPlayer.Play("hit-miss");
                        Outcome = OUTCOME_MOVE_FAIL;
                    }
                } 
                else if (Mode == SmoothTalkMode.MATCH_SAME)
                {
                    if (playerChoice==enemyChoice)
                    {
                        this.m_Animator.SetTrigger("hit");
                        OneTimeAudioPlayer.Play("hit-strong");
                        this.m_LifeEnemy -= 0.25f;
                        Outcome = this.m_LifeEnemy > 0 ? OUTCOME_MOVE_SUCCESS : OUTCOME_WIN;
                    }
                    else
                    {
                        this.m_LifePlayer -= 0.34f;
                        this.m_Animator.SetTrigger("got-hit");
                        OneTimeAudioPlayer.Play("hit-strong");
                        Outcome = this.m_LifePlayer > 0 ? OUTCOME_MOVE_FAIL : OUTCOME_LOST;
                    }
                }
                else
                {
                    int p = Array.IndexOf(choiceTable, playerChoice);
                    int e = Array.IndexOf(choiceTable, enemyChoice);
                    if (p==e)
                        OneTimeAudioPlayer.Play("hit-miss");
                    else if (choiceTable[(int) Mathf.Repeat(p+1, choiceTable.Length)] == choiceTable[e]) {
                        this.m_Animator.SetTrigger("hit");
                        OneTimeAudioPlayer.Play("hit-strong");
                        this.m_LifeEnemy -= 0.25f;
                        Outcome = this.m_LifeEnemy > 0 ? OUTCOME_MOVE_SUCCESS : OUTCOME_WIN;
                    } else if (choiceTable[(int) Mathf.Repeat(e+1, choiceTable.Length)] == choiceTable[p]) {
                        this.m_Animator.SetTrigger("got-hit");
                        OneTimeAudioPlayer.Play("hit-strong");
                        this.m_LifePlayer -= 0.25f;
                        Outcome = this.m_LifePlayer > 0 ? OUTCOME_MOVE_FAIL : OUTCOME_LOST;
                    }
                }

                this.m_CounterBoyWheel = 0;
                this.m_SpritesBoyRound = UnityEngine.Random.value < 0.75
                    ? this.SpritesBoy.Where( s => !s.name.ToLower().Contains("karma") ).ToArray()
                    : this.SpritesBoy;

                for (int i = 0; i < this.m_SpritesBoyRound.Length - 1; i++) 
                {
                    int rnd = UnityEngine.Random.Range(i, this.m_SpritesBoyRound.Length);
                    var tempGO = this.m_SpritesBoyRound[rnd];
                    this.m_SpritesBoyRound[rnd] = this.m_SpritesBoyRound[i];
                    this.m_SpritesBoyRound[i] = tempGO;
                }
            }
            else
            {
                this.m_GirlStopped = 0;
                this.m_BoyStopped = 0;
            }

            if (this.m_LifePlayer<=0)  {
                Debug.Log($"ENEMY WINS!");
                this.m_BoyStopped = 1;
                Time.timeScale = 0.25f;
                OneTimeAudioPlayer.Play("ryu-uggh");
                StartCoroutine(_EndCoroutine("you-lose"));
            } else if (this.m_LifeEnemy<=0) {
                Debug.Log($"PLAYER WINS!");
                Time.timeScale = 0.25f;
                OneTimeAudioPlayer.Play("chun-li-scream");
                StartCoroutine(_EndCoroutine("you-win"));
            }
        }

        System.Collections.IEnumerator _EndCoroutine(string sound)
        {
            yield return new WaitForSecondsRealtime(2);
            Time.timeScale = 1f;
            OneTimeAudioPlayer.Play(sound);
        }

        public void UpdateImageGirl2Sprite()
        {
            this.ImageGirl1.sprite = this.ImageGirl2.sprite;
            this.ImageGirl2.sprite = this.SpritesGirl[ (int) Mathf.Repeat(++this.m_CounterGirlWheel, this.SpritesGirl.Length) ];
        }

        public void UpdateImageBoy2Sprite()
        {
            this.ImageBoy1.sprite = this.ImageBoy2.sprite;
            this.ImageBoy2.sprite = this.m_SpritesBoyRound[ (int) Mathf.Repeat(++this.m_CounterBoyWheel, this.m_SpritesBoyRound.Length) ];
        }

        void Update()
        {
            this.m_Timer += Time.deltaTime;
            this.ImageTimerGaugeValue.fillAmount = Mathf.InverseLerp(0, InputTime, this.m_Timer);
            this.TransformWheelBoy.parent.gameObject.SetActive(m_MoveOnInput || m_BoyStopped != 0);

            RunWheel(this.TransformWheelGirl, SpeedGirl, m_GirlStopped, UpdateImageGirl2Sprite);
            RunWheel(this.TransformWheelBoy, SpeedBoy, m_BoyStopped, UpdateImageBoy2Sprite);

            this.ImageBoyGaugeLife.fillAmount = Mathf.MoveTowards(this.ImageBoyGaugeLife.fillAmount, this.m_LifePlayer, Time.deltaTime * 0.25f);
            this.ImageGirlGaugeLife.fillAmount = Mathf.MoveTowards(this.ImageGirlGaugeLife.fillAmount, this.m_LifeEnemy, Time.deltaTime * 0.25f);

            TextHelp.transform.parent.gameObject.SetActive(m_MoveOnInput);
            if (m_MoveOnInput && (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump") || this.m_Timer > this.InputTime ) )
            {
                Debug.Log("CLICK");
                m_MoveOnInput = false;
                Move();
            }
        }

        private void RunWheel(RectTransform transformWheel, float speed, int stopped, System.Action UpdateAction)
        {
            
            if (stopped!=0)
            {
                float y = Mathf.Repeat(transformWheel.anchoredPosition.y + Time.deltaTime * WheelSize * speed * stopped, this.WheelSize);
                if (stopped==1)
                    y =  y > transformWheel.anchoredPosition.y ? y : WheelSize;
                else
                    y =  y < transformWheel.anchoredPosition.y ? y : 0;
                transformWheel.anchoredPosition = 
                    new Vector2(transformWheel.anchoredPosition.x, y );
            }
            else
            {
                float y = Mathf.Repeat(transformWheel.anchoredPosition.y + Time.deltaTime * WheelSize * speed, this.WheelSize);
                if ( y < transformWheel.anchoredPosition.y)
                    UpdateAction();

                transformWheel.anchoredPosition = 
                    new Vector2( transformWheel.anchoredPosition.x, y);
            }
        }
        
    }
}