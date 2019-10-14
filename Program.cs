using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KattisSearch {
    public class Program {
        public static void Main (string[] args) {
            if (args.Length != 1)
                throw new Exception ("Must provide one flag. Valid flags would be -s --search and -g --generate");
            string flag = args[0];
            Stopwatch watch = new Stopwatch ();
            watch.Start ();
            if (Regex.IsMatch (flag, "-s|--search")) {
                Kattis.LoadJson ();
                Kattis.Search ();
            } else if (Regex.IsMatch (flag, "-g|--generate")) {
                if (!Kattis.FileExists ()) {
                    Kattis.Generate ();
                    Kattis.CreateJson ();
                }
            } else {
                throw new Exception ("Invalid flag. Valid flags would be -s --search and -g --generate");
            }
            watch.Stop ();
            Console.WriteLine ("Time: " + watch.Elapsed);
        }
    }
}