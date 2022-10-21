using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Titrator
{
    // Log
    public enum LogSection
    {
        Debug,
        Error,
        Main,
        Com_Board,
    }

    public enum MainState
    { 
        Unknown,
        Idle,
        Run,
        Warning,
        Error
    }

    public enum CommonStatus
    { 
        NA,
        OK,
        Error
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
