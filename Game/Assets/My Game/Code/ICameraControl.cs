using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CornTheory
{
    public interface ICameraControl
    {
        void GotoHomePosition();
        void Up(float degrees = 5.0f);
        void Down(float degrees = 5.0f);
        void Left(float degrees = 5.0f);
        void Right(float degrees = 5.0f);
    }
}
