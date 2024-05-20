using System;
using System.Linq;

namespace QuickPick.Utilities
{
    public interface IValueHandler
    {
        void HandleNewValue(double value);
    }
}