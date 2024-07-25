using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Threading;
using System.Text;
using System;
using System.IO;


public class UnityServer : MonoBehaviour
{

    private HttpListener httpListener;
    private Thread listenerThread;
    private bool isRunning;
    public GameObject avatar;

    // Start is called before the first frame update
    void Start()
    {
        StartServer();
    }

    // Update is called once per frame
    void Update()
    {
        //StopServer();
    }

    public void StartServer()
    {
        listenerThread = new Thread(new ThreadStart(RunServer));
        listenerThread.Start();
    }

    public void StopServer()
    {
        isRunning = false;
        httpListener.Stop();
        listenerThread.Abort();
    }

    private void RunServer()
    {
        isRunning = true;
        httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:8080/");
        httpListener.Prefixes.Add("http://127.0.0.1:8080/");
        httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        httpListener.Start();

        Debug.Log("HTTP Server started on http://localhost:8080/");

        while (isRunning)
        {
            try
            {
                HttpListenerContext context = httpListener.GetContext();
                ThreadPool.QueueUserWorkItem(o => HandleRequest(context));
            }
            catch (Exception ex)
            {
                Debug.LogError("HTTP Server error: " + ex.Message);
            }
        }
    }

    private void HandleRequest(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        try
        {

            // Read the request body
            string requestBody = "";
            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }

            Debug.Log("Received request body: " + requestBody);

            // Set the response status code and content
            response.StatusCode = (int)HttpStatusCode.OK; // Set status code to 200 OK
            response.ContentType = "text/plain";
            string responseString = "Received data: " + requestBody;

            if (requestBody == "\"move\"")
            {
                MoveCapsule();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error handling request: " + e.Message);
            response.StatusCode = (int)HttpStatusCode.InternalServerError; // Set status code to 500 Internal Server Error
            response.ContentType = "text/plain";
            byte[] buffer = Encoding.UTF8.GetBytes("Error occurred: " + e.Message);
            response.ContentLength64 = buffer.Length;
            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
        finally
        {
            response.Close();
        }

    }

    private void MoveCapsule()
    {
        Debug.Log("Moving...");
        // Enqueue the action to move the capsule
        MainThreadDispatcher.Enqueue(() =>
        {
            if (avatar != null)
            {
                Debug.Log("Entered");
                // Perform the movement or any other operation here
                Transform avatarTransform = avatar.GetComponent<Transform>();
                avatarTransform.localPosition += new Vector3(0.5f, 1, 5);
            }
        });
    }

}
