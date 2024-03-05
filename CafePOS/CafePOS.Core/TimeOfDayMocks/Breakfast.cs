using CafePOS.Core.Interfaces.Application;

namespace CafePOS.Application.Mocks
{
    public class Breakfast : ITimeOfDaySetting
    {
        public int GetTimeOfDaySetting()
        {
            return 1;
        }
    }
}
