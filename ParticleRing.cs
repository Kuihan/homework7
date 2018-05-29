using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRing : MonoBehaviour {
    public int mode = 0;
    private ParticleSystem particleSys;  // 粒子系统  
    private ParticleSystem.Particle[] particleArr;  // 粒子数组 
    public int count = 10000;       // 粒子数量  
    public float size = 0.03f;      // 粒子大小  
    // Use this for initialization
    void Start () {
        // 初始化粒子数组  
        particleArr = new ParticleSystem.Particle[count];
        //circle = new CirclePosition[count];

        // 初始化粒子系统  
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;            // 粒子位置由程序控制  
        particleSys.startSize = size;          // 设置粒子大小  
        particleSys.loop = false;
        particleSys.maxParticles = count;      // 设置最大粒子量  
        particleSys.Emit(count);               // 发射粒子  
        particleSys.GetParticles(particleArr);

        RandomlySpread();   // 初始化各粒子位置
    }
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < count; i++)
        {
            //求得粒子所对应方向的单位向量
            Vector3 v = particleArr[i].position;
            v -= this.transform.position;
            float mod = v.magnitude;
            v /= mod;
            //逆时针
            if(mode == 0)v = new Vector3(-v.y, v.x, v.z);
            //顺时针
            else if(mode == 1) v = new Vector3(v.y, -v.x, v.z);
            //随机速度
            float speed = Random.Range(1f, 5f);
            Vector3 pos = particleArr[i].position + v * speed * Time.deltaTime;
            //发散
            if (pos.magnitude > mod && mode != 2)
            {
                pos *= mod / pos.magnitude;
            }
            particleArr[i].position = pos;
        }
        particleSys.SetParticles(particleArr, particleArr.Length);
    }

    void RandomlySpread()
    {
        int length = particleArr.Length;
        for(int i = 0; i < length; i++)
        {
            //设置平均半径为10,范围在8到12之间
            float r = Random.Range(8f, 12f);
            if (i % 3 == 1) r = Random.Range(9f, 11f);
            if (i % 3 == 2) r = Random.Range(9.5f, 10.5f);
            //随机生成弧度角，范围为0到2π
            float angle = Random.Range(0, 2 * Mathf.PI);
            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;
            //初始化粒子坐标，注意要转成世界坐标
            particleArr[i].position = new Vector3(this.transform.position.x + x, this.transform.position.y + y, this.transform.position.z);
        }
        particleSys.SetParticles(particleArr, particleArr.Length);
    }
}
