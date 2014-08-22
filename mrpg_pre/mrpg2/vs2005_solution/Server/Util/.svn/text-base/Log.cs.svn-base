using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Server
{
    class Log
    {
        // Call this log routine from within static methods to print a message 
        // identifying the current point of execution.  Use as follows:
        //
        //          Log.Write();
        //
        public static void Write()
        {
            StackTrace callStack = new StackTrace();
            StackFrame callingFrame = callStack.GetFrame(1);
            MethodBase callingMethod = callingFrame.GetMethod();
            Debug.WriteLine(
                callingMethod.DeclaringType.Namespace + 
                "." + 
                callingMethod.DeclaringType.Name + 
                "." + 
                callingMethod.Name + 
                "()");
        }

        // Call this log routine from within instance methods to print a message 
        // identifying the current point of execution.  Use as follows:
        //
        //          Log.Write(this);
        //
        public static void Write(object obj)
        {
            StackTrace callStack = new StackTrace();
            StackFrame callingFrame = callStack.GetFrame(1);
            MethodBase callingMethod = callingFrame.GetMethod();
            Debug.WriteLine(
                obj.GetType() + 
                "." + 
                callingMethod.Name + 
                "()");
        }

        // This function is similar to Write(), except that a user-defined 
        // message is also printed. Use as follows:
        //
        //          Log.Write("x = " + x);
        //
        public static void Write(string message)
        {
            StackTrace callStack = new StackTrace();
            StackFrame callingFrame = callStack.GetFrame(1);
            MethodBase callingMethod = callingFrame.GetMethod();
            Debug.WriteLine(callingMethod.DeclaringType.Namespace + "." + callingMethod.DeclaringType.Name + "." + callingMethod.Name + "()" + " : " + message);
        }

        // This function is similar to Write(object), except that a user-defined 
        // message is also printed. Use as follows:
        //
        //          Log.Write(this, "x = " + x);
        //
        public static void Write(object obj, string message)
        {
            StackTrace callStack = new StackTrace();
            StackFrame callingFrame = callStack.GetFrame(1);
            MethodBase callingMethod = callingFrame.GetMethod();
            Debug.WriteLine(obj.GetType() + "." + callingMethod.Name + "()" + " : " + message);
        }
    }
}
