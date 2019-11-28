using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBD
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
