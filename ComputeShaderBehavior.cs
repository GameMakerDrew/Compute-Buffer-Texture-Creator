using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComputeShaderBehavior : MonoBehaviour
{

    public struct Agent
    {
        public Vector3 position;
        public float angle;
    };

    public Agent agent;

    public Vector2 resolution;
    public int numAgents;
    public Agent[] agents;

    public int posSize = sizeof(float) * 3;
    public int floatSize = sizeof(float);
    public int totalSize;

    public ComputeShader computeShader;
    public RenderTexture renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        totalSize = posSize + floatSize;


        renderTexture = new RenderTexture((int)resolution.x, (int)resolution.y, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        agents = new Agent[numAgents];
        addAgents();
        ComputeBuffer computeBuffer = new ComputeBuffer(agents.Length, totalSize);
        computeBuffer.SetData(agents);

        computeShader.SetFloat("Time", Time.time);
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.SetInt("width", (int)resolution.x);
        computeShader.SetInt("numAgents", numAgents);
        computeShader.SetTexture(0, "Map", renderTexture);
        computeShader.SetBuffer(0, "agents", computeBuffer);
        computeShader.Dispatch(0, 1, 1, 1);
        computeBuffer.Release();  
    }

    // Update is called once per frame
    void Update()
    {
        agents = new Agent[numAgents];
        addAgents();
        ComputeBuffer computeBuffer = new ComputeBuffer(agents.Length, totalSize);
        computeBuffer.SetData(agents);

        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.SetFloat("Time", Time.time);
        computeShader.SetInt("width", (int)resolution.x);
        computeShader.SetInt("numAgents", numAgents);
        computeShader.SetTexture(0, "Map", renderTexture);
        computeShader.SetBuffer(0, "agents", computeBuffer);
        computeShader.Dispatch(0, 1, 1, 1);
        computeBuffer.Release();

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(renderTexture, destination);
    }

    void addAgents()
    {
        for (int i = 0; i < numAgents; i++)
        {
            Agent agent;
            agent.position = new Vector3(resolution.x/2,resolution.y/2);
            agent.angle = 0;
            agents[i] = agent;
        }
    }

    private void OnGUI()
    {
        GUI.Button(new Rect(resolution.x/2, resolution.y/2, 10, 10), "HI");
    }

}
