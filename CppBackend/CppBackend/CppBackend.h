#ifndef CPP_BACKEND
#define CPP_BACKEND

#include <stdint.h>
#include <stdlib.h>
#include <time.h>
#include <random>

#define DLL_EXPORT __declspec (dllexport)

struct Point2D
{
    int u;
    int v;
};

struct Point3D
{
	float x;
	float y;
	float z;
};

struct Pose
{
    float w;
    float x;
    float y;
    float z;

    float t0;
    float t1;
    float t2;
};

struct ReturnMetaData
{
    Point3D* pts3DArray{ nullptr };
    int   pts3DCount{ 0 };

    Pose* posesArray{ nullptr };
    int posesCount{ 0 };
};

extern "C"
{
	DLL_EXPORT ReturnMetaData RunSLAM(Point2D* pts2DArray, int pts2DCount);
	DLL_EXPORT void FreeMemory(ReturnMetaData returnData);
}


#endif
