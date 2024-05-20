using System;
using System.Linq;

namespace QuickPick.Utilities
{
    public interface IPercentageValueHandler
    {
        void HandleNewValue(double value);
    }
}