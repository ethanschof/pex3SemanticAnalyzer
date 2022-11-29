using CS426.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;


namespace CS426.analysis
{
    class CodeGenerator : DepthFirstAdapter
    {
        StreamWriter _output;

        public CodeGenerator(string outputFilename)
        {
            _output = new StreamWriter(outputFilename);
        }

        private void Write(string textToWrite)
        {
            Console.Write(textToWrite);
            _output.Write(textToWrite);
        }

        private void WriteLine(string textToWrite)
        {
            Console.WriteLine(textToWrite);
            _output.WriteLine(textToWrite);
        }

        public override void InAProgram(AProgram node)
        {
            WriteLine(".assembly extern mscorlib {}");
            WriteLine(".assembly pex4program");
            WriteLine("{\n\t.ver 1:0:1:0\n}\n");
        }

        public override void OutAProgram(AProgram node)
        {
            _output.Close();

            Console.WriteLine("\n\n");
        }



    }
}
