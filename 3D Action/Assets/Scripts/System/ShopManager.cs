﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField, Tooltip("売るカードを表示するPanel")]
    GameObject _sellPanel;

    private void OnEnable()
    {
        TextManager.Instance.SetMessage("いらっしゃいませ！\n 所持ゴールド：" + PlayerPalam.Instance.Gold) ;
    }
    /// <summary>お金を減らしてインベントリにカードを追加する関数</summary>
    /// <param name="value">購入に必要なお金</param>
    public void BuyRandomCard(int value)
    {
        if (0>PlayerPalam.Instance.Gold + value)
        {
            TextManager.Instance.SetMessage("ゴールドが足りません\n 所持ゴールド：" + PlayerPalam.Instance.Gold);
        }
        else
        {
            PlayerPalam.Instance.Goldfluctuation(value);
            //全てのカードからランダムなインデックスを取得
            int ran = Random.Range(0, CardManager.Instance.AllCards.Length);
            //取得したインデックスのカードを追加する
            GameObject card = CardManager.Instance.AllCards[ran];
            CardManager.Instance.AddCard(card);
            //インデックスの設定
            CardBase cardBase = card.GetComponent<CardBase>();
            //cardBase.CardIndex = CardManager.Instance.InventriCards.Count - 1;
            TextManager.Instance.SetMessage($"<color=#0073FF>{cardBase.Name}</color>を手に入れた！\n 所持ゴールド：<color=#ffff00>{PlayerPalam.Instance.Gold}</color>");
        }

    }
    
    /// <summary>
    /// インベントリリストから該当カードを削除し、ゴールドを増やす関数
    /// </summary>
    /// <param name="index">削除するカードのインデックス</param>
    /// <param name="value">売値（正の値）</param>
    public void SellCard(int index,int value,GameObject card)
    {
        //削除するカードのCardManagerコンポーネントを取得
        CardBase cardBase = CardManager.Instance.InventriCards[index].GetComponent<CardBase>();
        TextManager.Instance.SetMessage($"<color=#0073FF>{cardBase.Name}</color>を{value}で売った\n 所持ゴールド：<color=#ffff00>{PlayerPalam.Instance.Gold}</color>");
        //ゴールド追加
        PlayerPalam.Instance.Goldfluctuation(value);
        //削除
        CardManager.Instance.RemoveAtCard(index);
        //GameObject.FindGameObjectWithTag("CardInfoTag").SetActive(false);
        Destroy(card);
        
    }

    //public void SellGear(int index,)
    //{

    //}
}
