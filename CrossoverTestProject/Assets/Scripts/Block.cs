using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Block : MonoBehaviour
{
    protected BlockData _blockData;
    public BlockData BlockData { get { return _blockData; } set { _blockData = value; } }

    public UnityEvent<string> _onSendToDisplayEvent = new UnityEvent<string>();

    public void SendDataToDisplay()
    {
        string dataToSend = new StringBuilder(string.Format("{0}: {1}\n{2}\n{3}: {4}", _blockData.grade, _blockData.domain, _blockData.cluster, _blockData.standardid, _blockData.standarddescription)).ToString();

        _onSendToDisplayEvent.Invoke(dataToSend);
    }
}
