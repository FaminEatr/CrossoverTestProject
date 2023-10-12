using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TableManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject _towerRootPrefab;

    [SerializeField]
    protected float _rootPadding = 0.3f;

    [SerializeField]
    protected Transform _towerRootParent;

    [SerializeField]
    protected DataDisplayManager _dataDisplayTMP;

    [SerializeField]
    protected UnityEvent<Transform> _onTowerSelectedEvent = new UnityEvent<Transform>();

    protected List<GameObject> _towerList = new List<GameObject>();

    protected Dictionary<string, List<BlockData>> _gradeBlocks = new Dictionary<string, List<BlockData>>();

    protected int _selectedTowerIndex;

    public void SetSelectedTowerIndex(int selectedTowerIndex)
    {
        _selectedTowerIndex = selectedTowerIndex;
        if (_selectedTowerIndex >= 0)
        {
            _onTowerSelectedEvent.Invoke(_towerList[_selectedTowerIndex].transform);
        }
    }

    public void GenerateBlockDictionary(BlockData[] blocks)
    {
        foreach (var block in blocks)
        {
            if (block.grade.ToLower().Contains("grade"))
            {
                if (!_gradeBlocks.ContainsKey(block.grade))
                {
                    _gradeBlocks.Add(block.grade, new List<BlockData>());
                }

                bool matchingStandardID = false;
                int matchingIndex = -1;
                for (int i = 0; i < _gradeBlocks[block.grade].Count; ++i)
                {
                    BlockData data = _gradeBlocks[block.grade][i];
                    if (data.standardid.Equals(block.standardid))
                    {
                        matchingStandardID = true;
                        matchingIndex = i;
                        break;
                    }
                }

                if (matchingStandardID)
                {
                    if (_gradeBlocks[block.grade][matchingIndex].mastery < block.mastery)
                    {
                        _gradeBlocks[block.grade].RemoveAt(matchingIndex);
                        _gradeBlocks[block.grade].Insert(matchingIndex, block);
                    }
                }
                else
                {
                    _gradeBlocks[block.grade].Add(block);
                }
            }
        }

        SortBlockData();

        GenerateTowers();
        EnablePhysics(true);
    }

    public void ResetTowers()
    {
        foreach (var tower in _towerList)
        {
            tower.GetComponent<TowerManager>().DestroyAll();
        }

        for (int i = _towerList.Count - 1; i >= 0; --i)
        {
            Destroy(_towerList[i]);
            _towerList.RemoveAt(i);
        }

        _towerList.Clear();

        GenerateTowers();
        EnablePhysics(true);
    }

    public void EnablePhysics(bool enable)
    {
        foreach (var tower in _towerList)
        {
            tower.GetComponent<TowerManager>().EnablePhysics(enable);
        }
    }

    public void DestroyTowerBlocksByType(int type)
    {
        if (_selectedTowerIndex >= 0)
        {
            DestroyBlockType(_selectedTowerIndex, type);
        }
        else
        {
            _dataDisplayTMP.SetText("Please Select a Tower");
        }
    }

    private void DestroyBlockType(int towerIndex, int type)
    {
        _towerList[towerIndex].GetComponent<TowerManager>().DestroyBlockType(type);
    }

    private void SortBlockData()
    {
        foreach (var grade in _gradeBlocks)
        {
            grade.Value.Sort
                (
                    delegate (BlockData d1, BlockData d2)
                    {
                        return d1.domain.CompareTo(d2.domain);
                    }
                );

            grade.Value.Sort
                (
                    delegate (BlockData d1, BlockData d2)
                    {
                        return d1.cluster.CompareTo(d2.cluster);
                    }
                );

            grade.Value.Sort
                (
                    delegate (BlockData d1, BlockData d2)
                    {
                        return d1.standardid.CompareTo(d2.standardid);
                    }
                );
        }
    }

    private void GenerateTowers()
    {
        float offset = _gradeBlocks.Count > 1 ? -(Mathf.Floor(_gradeBlocks.Count / 2) * _rootPadding) : 0;
        foreach (var gradeBlock in _gradeBlocks)
        {
            Vector3 spawnPos = new Vector3(offset, 0, 0);

            GameObject newTower = Instantiate(_towerRootPrefab, _towerRootParent.transform);
            newTower.transform.SetLocalPositionAndRotation(spawnPos, Quaternion.Euler(0, 0, 0));
            _towerList.Add(newTower);

            offset += _rootPadding;
        }

        int i = 0;
        foreach (var gradeBlock in _gradeBlocks)
        {
            _towerList[i].GetComponent<TowerManager>().GenerateBlocks(gradeBlock.Key, gradeBlock.Value, _dataDisplayTMP);
            ++i;
        }

        _selectedTowerIndex = (int)Mathf.Floor(_gradeBlocks.Count / 2);
        _onTowerSelectedEvent.Invoke(_towerList[_selectedTowerIndex].transform);
    }
}
