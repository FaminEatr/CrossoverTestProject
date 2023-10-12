using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    protected TextMeshPro _text;

    [SerializeField]
    protected GameObject _glassBlockPrefab;

    [SerializeField]
    protected GameObject _woodBlockPrefab;

    [SerializeField]
    protected GameObject _stoneBlockPrefab;

    [SerializeField]
    protected Transform _blockParent;

    protected List<GameObject> _blocks = new List<GameObject>();

    [SerializeField]
    protected float _widthPadding = 0.025f;

    [SerializeField]
    protected float _heightPadding = 0.001f;

    public void GenerateBlocks(string grade, List<BlockData> dataList, DataDisplayManager dataDisplay)
    {
        _text.text = grade;

        int levelNumber = 1;
        int levelBlockNumber = 0;
        float verticalPosition = 0;

        Quaternion rotation;
        Vector3 position = Vector3.zero;

        foreach (var block in dataList)
        {
            if (levelNumber % 2 == 0)
            {
                rotation = Quaternion.Euler(0, 0, 0);
                switch (levelBlockNumber)
                {
                    case 0:
                        position = new Vector3(0 - _woodBlockPrefab.transform.localScale.x - _widthPadding, verticalPosition + _heightPadding, 0);
                        levelBlockNumber++;
                        break;

                    case 1:
                        position = new Vector3(0, verticalPosition + _heightPadding, 0);
                        levelBlockNumber++;
                        break;

                    case 2:
                        position = new Vector3(0 + _woodBlockPrefab.transform.localScale.x + _widthPadding, verticalPosition + _heightPadding, 0);
                        levelBlockNumber = 0;
                        levelNumber++;
                        verticalPosition += _woodBlockPrefab.transform.localScale.y;
                        break;
                }
            }
            else
            {
                rotation = Quaternion.Euler(0, 90, 0);
                switch (levelBlockNumber)
                {
                    case 0:
                        position = new Vector3(0, verticalPosition + _heightPadding, 0 - _woodBlockPrefab.transform.localScale.x - _widthPadding);
                        levelBlockNumber++;
                        break;

                    case 1:
                        position = new Vector3(0, verticalPosition + _heightPadding, 0);
                        levelBlockNumber++;
                        break;

                    case 2:
                        position = new Vector3(0, verticalPosition + _heightPadding, 0 + _woodBlockPrefab.transform.localScale.x + _widthPadding);
                        levelBlockNumber = 0;
                        levelNumber++;
                        verticalPosition += _woodBlockPrefab.transform.localScale.y;
                        break;
                }
            }

            GameObject newBlock = null;
            switch (block.mastery)
            {
                case 0:
                    newBlock = Instantiate(_glassBlockPrefab, _blockParent);
                    break;

                case 1:
                    newBlock = Instantiate(_woodBlockPrefab, position, rotation, _blockParent);
                    break;

                case 2:
                    newBlock = Instantiate(_stoneBlockPrefab, position, rotation, _blockParent);
                    break;
            }
            Block nb = newBlock.GetComponent<Block>();
            nb.BlockData = block;
            UnityAction<string> setTextAction = (s) =>
            {
                dataDisplay.SetText(s);
            };
            nb._onSendToDisplayEvent.AddListener(setTextAction);
            newBlock.transform.SetLocalPositionAndRotation(position, rotation);
            newBlock.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _blocks.Add(newBlock);
        }

        EnablePhysics(true);
    }

    public void EnablePhysics(bool enabled)
    {
        foreach(var block in _blocks)
        {
            block.GetComponent<Rigidbody>().useGravity = enabled;
            block.GetComponent<Collider>().enabled = enabled;
        }
    }

    public void DestroyBlockType(int type)
    {
        List<int> indexesToDestroy = new List<int>();

        for (int i = 0; i < _blocks.Count; ++i)
        {
            GameObject block = _blocks[i];
            if (block.GetComponent<Block>().BlockData.mastery == type)
            {
                indexesToDestroy.Add(i);
            }
        }

        indexesToDestroy.Reverse();

        foreach(var index in indexesToDestroy)
        {
            Destroy(_blocks[index]);
            _blocks.RemoveAt(index);
        }
    }

    public void DestroyAll()
    {
        DestroyBlockType(0);
        DestroyBlockType(1);
        DestroyBlockType(2);
        _blocks.Clear();
    }
}
