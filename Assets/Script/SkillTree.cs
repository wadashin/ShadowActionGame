using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTree : MonoBehaviour
{
    //二次元配列の一個目
    /// <summary>
    /// 各配列の0番目に本人を入れて以降に本人とつながってるお隣さんを入れてね
    /// </summary>
    [SerializeField] List<Aaaa> buttonAll = new List<Aaaa>();

    //起動済みか否かの判別用のbool型二次元配列
    //public  Button[] buttons;
    bool[] donePush;
    bool[] canPush;

    bool a = false;

    List<Button> aa = new List<Button>();

    [SerializeField] TextMeshProUGUI _num;


    private void Start()
    {
        donePush = new bool[buttonAll.Count];
        canPush = new bool[buttonAll.Count];
        canPush[0] = true;
    }

    private void Update()
    {
        if (aa != null)
        {
            //aa.GetComponent<Image>().fillAmount = Mathf.MoveTowards(0, 1, 10);
            //foreach (Button x in aa)
            for (int i = 0; i < aa.Count; i++)
            {
                aa[i].GetComponent<Image>().fillAmount += Time.deltaTime;
                if (aa[i].GetComponent<Image>().fillAmount >= 1)
                {
                    aa.Remove(aa[i]);
                }
            }
        }
    }


    public void On(Button x)
    {
        for (int i = 0; i < buttonAll.Count; i++)
        {
            if (x == buttonAll[i].buttons[0])
            {
                for (int j = 1; j < buttonAll[i].buttons.Count; j++)
                {
                    for (int k = 0; k < buttonAll.Count; k++)
                    {
                        if (buttonAll[i].buttons[j] == buttonAll[k].buttons[0])
                        {
                            if (!donePush[k])
                            {
                                return;
                            }
                            break;
                        }
                    }
                }
                donePush[i] = true;
                aa.Add(buttonAll[i].buttons[0]);
                a = true;
                return;
            }
        }
        Debug.LogError("該当するボタンが配列に設定されてないか存在してねぇよバカ");
    }








    public void TextChangeZero()
    {
        if (a)
        {
            Debug.Log(1);
            _num.text = 0.ToString();
            a = false;
        }
    }
    public void TextChangeOne()
    {
        if (a)
        {
            _num.text = 1.ToString();
            a = false;
        }
    }
    public void TextChangetwo()
    {
        if (a)
        {
            _num.text = 2.ToString();
            a = false;
        }
    }
    public void TextChangeThree()
    {
        if (a)
        {
            _num.text = 3.ToString();
            a = false;
        }
    }
    public void TextChangeFour()
    {
        if (a)
        {
            _num.text = 4.ToString();
            a = false;
        }
    }
    public void TextChangeFive()
    {
        if (a)
        {
            _num.text = 5.ToString();
            a = false;
        }
    }
    public void TextChangeSix()
    {
        if (a)
        {
            _num.text = 6.ToString();
            a = false;
        }
    }
    public void TextChangeSeven()
    {
        if (a)
        {
            _num.text = 7.ToString();
            a = false;
        }
    }
    public void TextChangeEight()
    {
        if (a)
        {
            _num.text = 8.ToString();
            a = false;
        }
    }
}
[System.Serializable]
public class Aaaa
{
    public List<Button> buttons = new List<Button>();
}