using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Conect4Manager : MonoBehaviour
{

    const int FIELD_SIZE_X = 7;
    const int FIELD_SIZE_Y = 6;

    [SerializeField] GameObject _spawnPrefab = null;
    [SerializeField] GameObject _boardPrefab = null;
    [SerializeField] GameObject _lineHPrefab = null;
    GameObject spawnerObj;
    private int[,] squares = new int[FIELD_SIZE_X, FIELD_SIZE_Y];

    //EMPTY = 0,RED = 1,BLUE = -1 で定義
    private const int EMPTY = 0;
    private const int RED = 1;
    private const int BLUE = -1;

    //現在のプレイヤー
    private int _currentPlayer = RED;

    //落下位置
    public float _moveFinishZ = -2.5f;

    //prefabs
    [SerializeField] GameObject redStone = null;
    [SerializeField] GameObject blueStone = null;

    // ボードの実体
    private GameObject _boardObject = null;

    //SPACE押せるかどうか
    public bool isAct =true;

    //終了
    private bool isEnd = false;

    //ターン表示テキスト
    public Text turn;

    //勝利時テキすと
    public GameObject winner;
    public Text winnerText;

    //ターン表示の定型文
    private const string redTurn = "1Pのターン";
    private const string blueTurn = "2Pのターン";

    private const string redWin = "1Pの勝利";
    private const string blueWin = "2Pの勝利";



    // Start is called before the first frame update
    void Start()
    {
        winner.SetActive(false);
        turn.text = redTurn;

        IntializeArray();
        //DebugArray();

        //ボードの生成
        _boardObject = GameObject.Instantiate<GameObject>(_boardPrefab);
        _boardObject.transform.localScale = new Vector3(FIELD_SIZE_X, 1, FIELD_SIZE_Y);

        //ラインの生成
        for (int j = 0; j <= FIELD_SIZE_X; j++)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(_lineHPrefab);
            temp.transform.localPosition = new Vector3(-(FIELD_SIZE_X) * 0.5f + j, 0.0f, 0.0f);
            temp.transform.localScale = new Vector3(0.05f, 0.05f, FIELD_SIZE_Y);
        }
        //スポナーの生成
        spawnerObj = GameObject.Instantiate<GameObject>(_spawnPrefab);
        spawnerObj.transform.localPosition = new Vector3(0, 0, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //スポナーの移動
        if (Input.GetKeyUp(KeyCode.RightArrow) && spawnerObj.transform.localPosition.x <= 2)
        {
            float nowX = spawnerObj.transform.localPosition.x;
            nowX += 1;
            spawnerObj.transform.localPosition = new Vector3(nowX, 0, 3.5f);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && spawnerObj.transform.localPosition.x >= -2)
        {
            float nowX = spawnerObj.transform.localPosition.x;
            nowX -= 1;
            spawnerObj.transform.localPosition = new Vector3(nowX, 0, 3.5f);
        }

        TurnMove();
    }

    //配列の初期化
    private void IntializeArray()
    {
        for (int i = 0; i < FIELD_SIZE_X; i++)
        {
            for (int j = 0; j < FIELD_SIZE_Y; j++)
            {
                squares[i, j] = EMPTY;
            }
        }
    }
    //1ターンの動作
    private void TurnMove()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isAct &&(isEnd != true ))
        {
            int x = (int)spawnerObj.transform.position.x + 3;
            if(_currentPlayer == RED)
            {
                if (squares[x, 5] == EMPTY)
                {
                    isAct = false;
                    for (int i = FIELD_SIZE_Y - 1; i >= 0; i--)
                    {
                        if (squares[x, i] == EMPTY)
                        {
                            _moveFinishZ = i - 2.5f;

                        }
                    }
                    GameObject stone = Instantiate(redStone);
                    stone.transform.position = new Vector3(x - 3, 0, 2.5f);
                    float z = _moveFinishZ + 2.5f;
                    squares[x, (int)z] = RED;
                    _currentPlayer = BLUE;
                    turn.text = blueTurn;

                }
            }
            else if (_currentPlayer == BLUE)
            {
                if (squares[x, 5] == EMPTY)
                {
                    isAct = false;
                    for (int i = FIELD_SIZE_Y - 1; i >= 0; i--)
                    {
                        if (squares[x, i] == EMPTY)
                        {
                            _moveFinishZ = i - 2.5f;

                        }
                    }
                    GameObject stone = Instantiate(blueStone);
                    stone.transform.position = new Vector3(x - 3, 0, 2.5f);
                    float z = _moveFinishZ + 2.5f;
                    squares[x, (int)z] = BLUE;
                    _currentPlayer = RED;
                    turn.text = redTurn;
                }
            }
            if ((CheckStone(RED) || CheckStone(BLUE)))
            {
                isEnd = true;
                return;
            }
        }
    }
    private bool CheckStone(int color)
    {
        //碁石の数をカウントする
        int count = 0;
        //横向き
        for (int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for (int j = 0; j < FIELD_SIZE_X; j++)
            {              
                if (squares[j, i] == EMPTY || squares[j, i] != color)
                {
                    count = 0;
                }
                else
                {
                    count++;
                }
                if (count == 4)
                {
                    if (color == RED)
                    {
                        winnerText.text = redWin;
                        winner.SetActive(true);
                    }
                    else
                    {
                        winnerText.text = blueWin;
                        winner.SetActive(true);

                    }
                    return true;
                }
            }
        }

        //countの値を初期化
        count = 0;

        //斜め（右上がり）
        for (int i = 0; i < FIELD_SIZE_X; i++)
        {
            //上移動用
            int up = 0;

            for (int j = i; j < FIELD_SIZE_Y; j++)
            {
                if (squares[j, up] == EMPTY || squares[j, up] != color)
                {
                    count = 0;
                }
                else
                {
                    count++;
                }

                if (count == 4)
                {
                    if (color == RED)
                    {
                        winnerText.text = redWin;
                        winner.SetActive(true);

                    }
                    else
                    {
                        winnerText.text = blueWin;
                        winner.SetActive(true);

                    }
                    return true;
                }

                up++;

            }
        }
        count = 0;
        //斜め上（その他
        for (int i = 2; i < 5; i++)
        {
            //上移動用
            int up_x = 0;
            int up_y = 0;

            for (int j = i; j < FIELD_SIZE_Y; j++)
            {
                if(i + up_y < FIELD_SIZE_Y)
                {
                    if (squares[up_x,i + up_y] == EMPTY || squares[up_x, i + up_y] != color)
                    {
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }

                    if (count == 4)
                    {
                        if (color == RED)
                        {
                            winnerText.text = redWin;
                            winner.SetActive(true);

                        }
                        else
                        {
                            winnerText.text = blueWin;
                            winner.SetActive(true);;
                        }
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }

                up_x++;
                up_y++;

            }
        }

        //countの値を初期化
        count = 0;

        //斜め（右下がり）
        for (int i = 0 ; i < FIELD_SIZE_X; i++)
        {
            //下移動用
            int down = 5;

            for (int j = i; j < FIELD_SIZE_X; j++)
            {
                if(down >= 0)
                {
                    if (squares[j, down] == EMPTY || squares[j, down] != color)
                    {
                        count = 0;
                    }
                    else
                    {
                        count++;

                    }

                    //countの値が4になったとき
                    if (count == 4)
                    {
                        if (color == RED)
                        {
                            winnerText.text = redWin;
                            winner.SetActive(true);

                        }
                        else
                        {
                            winnerText.text = blueWin;
                            winner.SetActive(true);
                        }
                        return true;
                    }
                    down--;
                }

            }
        }

        //countの値を初期化
        count = 0;
        //斜め下（その他）
        for (int i = 4; i >2; i--)
        {
            //下移動用
            int up_x = 0;
            int up_y = 0;

            for (int j  =0; j < FIELD_SIZE_Y; j++)
            {
                if (i - up_y >= 0)
                {
                    if (squares[up_x, i - up_y] == EMPTY || squares[0 + up_x, i - up_y] != color)
                    {
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }


                    if (count == 4)
                    {
                        if (color == RED)
                        {
                            winnerText.text = redWin;
                            winner.SetActive(true);

                        }
                        else
                        {
                            winnerText.text = blueWin;
                            winner.SetActive(true);
                        }
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }

                up_x++;
                up_y++;

            }
        }

        count = 0;
        //縦向き
        for (int i = 0; i < FIELD_SIZE_X; i++)
        {
            for (int j = 0; j < FIELD_SIZE_Y; j++)
            {
                if (squares[i, j] == EMPTY || squares[i, j] != color)
                {
                    count = 0;
                }
                else
                {
                    count++;
                }
                if (count == 4)
                {
                    if (color == RED)
                    {
                        winnerText.text = redWin;
                        winner.SetActive(true);

                    }
                    else
                    {
                        winnerText.text = blueWin;
                        winner.SetActive(true);
                    }
                    return true;
                }
            }
        }
        count = 0;
        return false;
    }

}
