using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Histria.Sys
{
    ///<summary>
    /// Stack of PropertyChanged
    /// Entry format: uid.Property[.uid.Property]
    ///</summary>
    public class BaseStack
    {
        static int capacity = 100;
        Stack<string> _stack = new Stack<string>(capacity);

        //create a regex expression to search for match in the stack
        private Regex BuildRegex(string fixedPart, string variable)
        {
            string[] segments = variable.Split('.');
            if (segments[segments.Length-1] == "*")
            {
                segments[segments.Length-1] = "(.*)";
            }
            string regex = string.Format(@"^{0}\.{1}$", fixedPart, string.Join("\\.(.*)\\.", segments));
            return new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        private bool MatchOne(Regex rg)
        {
            foreach (string value in _stack)
            {
                if (rg.IsMatch(value))
                    return true;
            }
            return false;
        }

        //Search for match in the stack
        protected bool Contains(string fixedPart, string variable)
        {
            if (_stack.Count < 2) return false;

            string last = _stack.Pop();
            try
            {
                if (string.IsNullOrEmpty(variable))
                {
                    return _stack.Contains(fixedPart);
                }
                return MatchOne(BuildRegex(fixedPart, variable));
            }
            finally
            {
                _stack.Push(last);
            }
        }

        protected void DoPush(string value)
        {
            _stack.Push(value);
        }

        protected void DoClear()
        {
            _stack.Clear();
        }

        protected string DoPop()
        {
            return _stack.Pop();
        }

    }
}
