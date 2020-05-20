using System;
using System.Collections.Generic;
using Action = AI.Goap.Action;

namespace Utils
{
    static class DebugPrinter
    {
        public static string prettyPrint(HashSet<KeyValuePair<string,object>> state) {
            var s = "";
            foreach (var kvp in state) {
                s += kvp.Key + ":" + kvp.Value.ToString();
                s += ", ";
            }
            return s;
        }

        public static string prettyPrint(Queue<Action> actions) {
            String s = "";
            foreach (Action a in actions) {
                s += a.GetType().Name;
                s += "-> ";
            }
            s += "GOAL";
            return s;
        }

        public static string prettyPrint(Action[] actions) {
            String s = "";
            foreach (Action a in actions) {
                s += a.GetType().Name;
                s += ", ";
            }
            return s;
        }

        public static string prettyPrint(Action action) {
            String s = ""+action.GetType().Name;
            return s;
        }
    }
}