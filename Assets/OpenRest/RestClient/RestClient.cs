using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestClient
{
    public enum RequestMethod { GET, POST, PUT, DELETE }

    private string _url;
    private RequestMethod _method;
    private Dictionary<string, string> _headers;
    private string _jsonData;
    private Action<string> _onSuccess;
    private Action<string> _onError;
    private Action<float> _onProgress;


    private RestClient(RequestMethod method)
    {
        _method = method;
    }

    public static RestClient Get()
    {
        return CreateRequest(RequestMethod.GET);
    }

    public static RestClient Post()
    {
        return CreateRequest(RequestMethod.POST);
    }

    public static RestClient Put()
    {
        return CreateRequest(RequestMethod.PUT);
    }

    public static RestClient Delete()
    {
        return CreateRequest(RequestMethod.DELETE);
    }

    private static RestClient CreateRequest(RequestMethod method)
    {
        return new RestClient(method);
    }

    public RestClient Url(string url)
    {
        _url = url;
        return this;
    }

    public RestClient Headers(Dictionary<string, string> headers)
    {
        _headers = headers;
        return this;
    }

    public RestClient JsonData(string jsonData)
    {
        _jsonData = jsonData;
        return this;
    }

    public RestClient OnSuccess(Action<string> onSuccess)
    {
        _onSuccess = onSuccess;
        return this;
    }

    public RestClient OnError(Action<string> onError)
    {
        _onError = onError;
        return this;
    }

    public RestClient OnProgress(Action<float> onProgress)
    {
        _onProgress = onProgress;
        return this;
    }

    public void Send()
    {
        if (Application.isPlaying)
        {
            CoroutineRunner.Instance.StartCoroutine(PerformRequest(_url, _method, _onSuccess, _onError, _headers, _jsonData, _onProgress));
        }
        else
        {
#if UNITY_EDITOR
            Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutineOwnerless(PerformRequest(_url, _method, _onSuccess, _onError, _headers, _jsonData, _onProgress));
#endif
        }
    }

    private IEnumerator PerformRequest(string url, RequestMethod method, Action<string> onSuccess, Action<string> onError, Dictionary<string, string> headers, string jsonData, Action<float> onProgress)
    {
        using (var request = new UnityWebRequest(url, method.ToString()))
        {
            if (jsonData != null)
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set custom headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            // Start the web request
            request.SendWebRequest();

            // Monitor progress while the request is not done
            while (!request.isDone)
            {
                onProgress?.Invoke(request.downloadProgress);
                yield return null;
            }

            // Handle the result after the request is complete
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
        }
    }
}
