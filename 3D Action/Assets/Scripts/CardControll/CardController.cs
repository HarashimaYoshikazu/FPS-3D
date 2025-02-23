﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 



public class CardController : MonoBehaviour, IDragHandler, IPointerDownHandler, IBeginDragHandler, IPointerUpHandler
{
    /// <summary>テーブルオブジェクト（"TableTag" が付いている UI オブジェクト）</summary>
    GameObject m_table = null;

    /// <summary>このオブジェクトの Rect Transform</summary>
    RectTransform m_rectTransform = default;
    /// <summary>動かす前に所属していたデッキ</summary>
    Transform m_originDeck = default;
    GameObject m_parentPanel = null;
    IsPanelScript isPanelScript;
    GameObject _GUIPanel;
    PanelController _panelController;

    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_table = GameObject.FindGameObjectWithTag("TableTag");
        m_parentPanel = GameObject.FindGameObjectWithTag("ParentTag");
        if (m_parentPanel)
        {
            isPanelScript = m_parentPanel.gameObject.GetComponent<IsPanelScript>();
        }

        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        _panelController = gm.GetComponent<PanelController>();    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _panelController.InfoOnOf(false);
        m_rectTransform.position = eventData.position;
        //Transform.SetAsLastSibling()

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        string message = $"OnPointerDown: {this.name}: ";
        var currentDeck = GetCurrentDeck(eventData);


        if (currentDeck.gameObject.name =="PlayPanel")
        {
            //カード動かせないようにこのスクリプト切る
            this.enabled = false;
            message += $"マウスポインタは {currentDeck.name} の上にあります";
        }
        else
        {
            message += "マウスポインタはデッキの上にありません";
        }
        m_originDeck = currentDeck.transform;


        Debug.Log(message);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag: {this.name}");
        Debug.Log($"OnBeginDrag: {this.name}");
        Debug.Log($"OnBeginDrag: {this.name}");
        this.transform.SetParent(m_table.transform);

        this.gameObject.transform.SetAsLastSibling();

    }

    /// <summary>
    /// マウスカーソルが現在どのデッキの上にあるかを返す。デッキとは "DeckTag" がタグ付けされた GameObject のこと。
    /// なお、デッキは UI オブジェクトつまり Rect Transform コンポーネントがアタッチされたオブジェクトである必要がある。
    /// </summary>
    /// <param name="eventData">PointerEventData 型の引数。マウス操作の情報が入っている。</param>
    /// <returns></returns>
    GameObject GetCurrentDeck(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results); // マウスポインタの位置上に重なっている UI を全て results に取得する（※）

        // results に入っているオブジェクトのうち、DeckTag が付いているオブジェクトを一つ result に取得する
        RaycastResult result = default;



        foreach (var item in results)
        {
            if (item.gameObject.CompareTag("DeckTag"))
            {
                result = item;
                break;
            }
        }



        return result.gameObject;   // 結果の GameObject を返す


        //（※）EventSystem のインターフェイスを使った通常のプログラミングだと、オブジェクトが重なっている場合は「一番上に描画されているオブジェクト」しかマウスの動きを検出できない。
        // そのため、デッキの上にカードが重なっている場合、デッキ側でマウスの動きを検出できない。そのため EventSystem.current.RaycastAll を使う必要があった。
        // ちなみに Hierarchy 上で下にある UI オブジェクトが前面に描画される。
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        string message = $"OnPointerDown: {this.name}: ";
        var currentDeck = GetCurrentDeck(eventData);


        if (currentDeck)
        {
            //
            message += $"マウスポインタは {currentDeck.name} の上にあります";
            this.transform.SetParent(currentDeck.transform);
            PanelActive();
            //tagをPlayingに変更する
            this.gameObject.tag = "PlayingCard";

        }
        else
        {
            this.transform.SetParent(m_originDeck.transform);
        }



        Debug.Log(message);

    }
    GameObject GetCurrentCard(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results); // マウスポインタの位置上に重なっている UI を全て results に取得する（※）

        // results に入っているオブジェクトのうち、DeckTag が付いているオブジェクトを一つ result に取得する
        RaycastResult result = default;



        foreach (var item in results)
        {
            if (item.gameObject.CompareTag("CardTag"))
            {
                result = item;
                break;
            }
        }



        return result.gameObject;   // 結果の GameObject を返す


        //（※）EventSystem のインターフェイスを使った通常のプログラミングだと、オブジェクトが重なっている場合は「一番上に描画されているオブジェクト」しかマウスの動きを検出できない。
        // そのため、デッキの上にカードが重なっている場合、デッキ側でマウスの動きを検出できない。そのため EventSystem.current.RaycastAll を使う必要があった。
        // ちなみに Hierarchy 上で下にある UI オブジェクトが前面に描画される。
    }
    void PanelActive()
    {
        if (this.gameObject.transform.parent.name == "PlayPanel")
        {
            isPanelScript.PanelOn();
        }
    }


}
