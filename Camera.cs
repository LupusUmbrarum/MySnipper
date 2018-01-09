using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testCamera
{
    class Camera
    {
        public int camPosX, camPosY, cameraLensWidth, cameraLensHeight;

        public Camera(int x = 0, int y = 0, int width = 0, int height = 0)
        {
            camPosX = x;
            camPosY = y;
            cameraLensWidth = width;
            cameraLensHeight = height;
        }
    }
}
