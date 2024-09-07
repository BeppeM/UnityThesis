using UnityEngine;

public class RunJacamo : MonoBehaviour
{
    public GameObject[] avatars;
    public GameObject[] environmentArtifacts;

    async void Start()
    {
        // Define agents into the multi agent system
        UnityJacamoIntegrationUtil.configureJcmFile(avatars);

        print(".jcm file configuration done successfully.");

        // Run jacamo application in async
        await UnityJacamoIntegrationUtil.RunJaCaMoApp();

        print("JaCaMo application started successfully.");

        // Start to connect each avatar
        await UnityJacamoIntegrationUtil.StartWebSocketConnections(avatars, environmentArtifacts);

        // await checkConnections();

        print("ALL AVATARS AND ENVIRONMENT OBJECTS HAVE BEEN CONNECTED TO JACAMO!!!");
    }
}