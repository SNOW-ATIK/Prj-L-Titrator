using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Titrator_Alpha
{
    // Log
    public enum LogSection
    {
        Debug,
        Error,
        Main,
        Com_Board,
    }

    public enum IOType
    { 
        Input,
        Output,
    }

    public enum ValveState
    {
        Unknown,
        Close,
        Open
    }

    public enum NO_ValveState
    {
        Open,
        Close
    }

    public enum NC_ValveState
    {
        Close,
        Open
    }

    public enum TemperatureSensor
    {
        RTD,
        TC
    }
}
