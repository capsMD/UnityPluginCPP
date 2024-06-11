
#include "CppBackend.h"

extern "C"
{
	DLL_EXPORT ReturnMetaData RunSLAM(Point2D* pts2DArray, int pts2DCount)
	{
		//data to be output (3D map points and camera poses)
		ReturnMetaData returnData;

		//Do some fake processing
		//generate a random number of 3D pts and cam poses
		std::random_device rdPtsCount;
		std::mt19937 gen(rdPtsCount());
		std::uniform_int_distribution<> disInt(1, 150);
		int ptsCount = disInt(gen);

		//allocate space in memory for 3D pts
		returnData.pts3DArray = new Point3D[ptsCount];

		//generate random 3D pts
		std::uniform_real_distribution<float> disFloat(-10.0f, 10.0f);
		for (int i = 0; i < ptsCount; i++)
		{
			returnData.pts3DArray[i].x = disFloat(gen);
			returnData.pts3DArray[i].y = disFloat(gen);
			returnData.pts3DArray[i].z = disFloat(gen);
		}
		returnData.pts3DCount = ptsCount;

		std::uniform_int_distribution<> dis(1, 5);
		int posesCount = disInt(gen);

		//allocate space in memory for poses
		returnData.posesArray = new Pose[posesCount];

		//generate poses: 0 rotation and random positions
		for (int j = 0; j < posesCount; j++)
		{
			returnData.posesArray[j].w = 0.0f;
			returnData.posesArray[j].x = 0.0f;
			returnData.posesArray[j].y = 0.0f;
			returnData.posesArray[j].z = 0.0f;
			returnData.posesArray[j].t0 = disFloat(gen);
			returnData.posesArray[j].t1 = disFloat(gen);
			returnData.posesArray[j].t2 = disFloat(gen);
		}
		returnData.posesCount = posesCount;


		return returnData;
	}
}

extern "C"
{
	void FreeMemory(ReturnMetaData returnData)
	{
		//deallocate memory
		delete[] returnData.pts3DArray;
		delete[] returnData.posesArray;
	}
}
