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

        // SECTIONS DONE 1 2 4 5 6 7 8 9 10 11 12


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

        public override void InAMainState(AMainState node)
        {
            WriteLine(".method static void main() cil managed");
            WriteLine("{");
            WriteLine("\t.maxstack 128");
            WriteLine("\t.entrypoint\n");

        }

        public override void OutAMainState(AMainState node)
        {
            WriteLine("\n\tret");
            WriteLine("}");
        }

        public override void OutADeclareStatement(ADeclareStatement node)
        {
            WriteLine("\t// Declaring Variable " + node.GetVarname().ToString());
            Write("\t.locals init (");
            if (node.GetType().Text == "int")
            {
                Write("int32 ");
            } 
            else if (node.GetType().Text == "float")
            {
                Write("float32 ");
            }
            else if (node.GetType().Text == "string")
            {
                Write("string ");
            }

            WriteLine(node.GetVarname().Text + ")\n");
        }

        public override void OutAIntOperand(AIntOperand node)
        {
            WriteLine("\tldc.i4 " + node.GetInteger().Text);
        }

        public override void OutAFloOperand(AFloOperand node)
        {
            WriteLine("\tldc.r8 " + node.GetFloat().Text);
        }

        public override void OutAStrOperand(AStrOperand node)
        {
            WriteLine("\tldstr " + node.GetString().Text);
        }

        public override void OutAVariableOperand(AVariableOperand node)
        {
            WriteLine("\tldloc " + node.GetId().Text);
        }

        public override void OutAAssignStatement(AAssignStatement node)
        {
            WriteLine("\tstloc " + node.GetId().Text + "\n");

        }

        public override void OutAAddExpressionMath1(AAddExpressionMath1 node)
        {
            WriteLine("\tadd");
        }

        public override void OutASubtractExpressionMath1(ASubtractExpressionMath1 node)
        {
            WriteLine("\tsub");
        }

        public override void OutADivideExpressionMath2(ADivideExpressionMath2 node)
        {
            WriteLine("\tdiv");
        }

        public override void OutAMultiplyExpressionMath2(AMultiplyExpressionMath2 node)
        {
            WriteLine("\tmul");
        }

        public override void OutANegativeExpressionMath3(ANegativeExpressionMath3 node)
        {
            WriteLine("\tneg");
        }

        public override void OutAFunctionCallStatement(AFunctionCallStatement node)
        {
            if (node.GetId().Text == "printInt")
            {
                WriteLine("\tcall void [mscorlib]System.Console::Write(int32)");
            }
            else if (node.GetId().Text == "printString")
            {
                WriteLine("\tcall void [mscorlib]System.Console::Write(string)");
            } 
            else if (node.GetId().Text == "printLine")
            {
                WriteLine("\tldstr \"\\n\"");
                WriteLine("\tcall void [mscorlib]System.Console::Write(string)");
            } 
            else if (node.GetId().Text == "printFloat")
            {
                WriteLine("\tcall void [mscorlib]System.Console::Write(float32)");
            }
            else
            {
                WriteLine("\t call void " + node.GetId().Text);
            }
        }




    }
}
