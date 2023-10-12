using System;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ExternalCommunicationManager : MonoBehaviour
{
    [SerializeField]
    protected TextAsset _dummyData;

    [SerializeField]
    protected string _apiURL;

    [SerializeField]
    protected TableManager _tableManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest(_apiURL));
    }

    protected IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:

                    BlockData[] blocks;
                    try
                    {
                        blocks = JsonConvert.DeserializeObject<BlockData[]>(webRequest.downloadHandler.text);
                    }
                    catch
                    {
                        try
                        {
                            blocks = JsonConvert.DeserializeObject<BlockData[]>(_dummyData.text);
                        }
                        catch(Exception ex)
                        {
                            Debug.LogException(ex);
                            break;
                        }
                    }

                    _tableManager.GenerateBlockDictionary(blocks);

                    break;

                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:

                    Debug.LogError(string.Format("Something went wrong: {0}", webRequest.error));

                    break;
            }
        }
    }
}
