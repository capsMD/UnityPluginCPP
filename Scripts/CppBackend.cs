using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


//Attribute to specify layout in memory
//Ensures fields are arranged in same order as defined (C++ -> C#)
[StructLayout(LayoutKind.Sequential)]
public struct Point2D
{
    public Point2D(int u, int v) 
    { this.u = u; this.v = v; }
    public int u;
    public int v;
}

[StructLayout(LayoutKind.Sequential)]
public struct Point3D
{
    public float x; 
    public float y; 
    public float z;
}

[StructLayout(LayoutKind.Sequential)]
public struct Pose
{
    //quaternion (orientation)
    public     float w;
    public     float x;
    public     float y;
    public     float z;
   
    //vec3 (position)
    public     float t0;
    public     float t1;
    public     float t2;
}

[StructLayout(LayoutKind.Sequential)]
public struct ReturnMetaData
{
    public IntPtr points3DArray;
    public int points3DCount;

    public IntPtr posesArray;
    public int posesCount;
}

public class CppBackend : MonoBehaviour
{
    [DllImport("Backend")]
    private static extern ReturnMetaData RunSLAM([In] Point2D[] pts, int pointsCount);

    [DllImport("Backend")]
    private static extern void FreeMemory(ReturnMetaData returnData);

    private void Awake()
    {
        //make a list of 2D vectors
        List<Point2D> imagePoints = new List<Point2D>();


        int maxPointsCount = 150;
        System.Random random = new System.Random();
        int randomInt = random.Next(0, maxPointsCount);

        int minValue = 0;
        int maxValue = 1024;

        Debug.Log("Total image points:  " + randomInt);

        for (int i = 0; i < randomInt; i++)
        {
            int u = random.Next(minValue, maxValue);
            int v = random.Next(minValue, maxValue);
            imagePoints.Add(new Point2D(u,v));

            Debug.Log("point " + i + ": (" + imagePoints[i].u + "), (" + imagePoints[i].v + ")");

        }

        // Convert List<Point2D> to Point2D[]
        Point2D[] imagePointsArray = imagePoints.ToArray();

        //now test it with plugin:
        ReturnMetaData data = new ReturnMetaData();
        data = RunSLAM(imagePointsArray, randomInt);



        //verify:
        // After calling RunSLAM function

        // Marshal the pointers to managed arrays
        Point3D[] points3DArray = new Point3D[data.points3DCount];
        for (int i = 0; i < data.points3DCount; i++)
        {
            IntPtr ptr = new IntPtr(data.points3DArray.ToInt64() + i * Marshal.SizeOf<Point3D>());
            points3DArray[i] = Marshal.PtrToStructure<Point3D>(ptr);
        }

        Pose[] posesArray = new Pose[data.posesCount];
        for (int i = 0; i < data.posesCount; i++)
        {
            IntPtr ptr = new IntPtr(data.posesArray.ToInt64() + i * Marshal.SizeOf<Pose>());
            posesArray[i] = Marshal.PtrToStructure<Pose>(ptr);
        }

        // Access and validate the data
        Debug.Log("Points3D:");
        foreach (var point in points3DArray)
        {
            Debug.Log("(" + point.x + ", " + point.y + ", " + point.z + ")");
        }

        Debug.Log("Poses:");
        foreach (var pose in posesArray)
        {
            Debug.Log("Quaternion: (" + pose.w + ", " + pose.x + ", " + pose.y + ", " + pose.z + ")");
            Debug.Log("Position: (" + pose.t0 + ", " + pose.t1 + ", " + pose.t2 + ")");
        }



    }

}
