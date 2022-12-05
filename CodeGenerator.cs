using CS426.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

namespace CS426.analysis
{
    class CodeGenerator : DepthFirstAdapter
    {
        StreamWriter _output;

        // SECTIONS DONE 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15
        // Sections TODO 16 17 18
        string branchLabel = "mylabel";


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


        public override void OutABitnotExpressionMath3(ABitnotExpressionMath3 node)
        {
            WriteLine("\tldc.i4 0");
            WriteLine("\tceq");
        }

        public override void OutABitandExpression2(ABitandExpression2 node)
        {
            WriteLine("and");
        }

        public override void OutABitorExpression(ABitorExpression node)
        {
            WriteLine("or");
        }

        public override void InAFunction(AFunction node)
        {
            WriteLine(".method static void <function>() cil managed");
            WriteLine("{");
            WriteLine("\t.maxstack 128");
        }

        public override void OutAFunction(AFunction node)
        {
            WriteLine("\tret");
            WriteLine("}");
        }


        public override void CaseAJustifIfStatement(AJustifIfStatement node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;

            string label3 = branchLabel;
            branchLabel += 1;

            InAJustifIfStatement(node);
            if (node.GetIf() != null)
            {
                node.GetIf().Apply(this);
            }
            if (node.GetOpenParenthesis() != null)
            {
                node.GetOpenParenthesis().Apply(this);
            }
            if (node.GetExpression() != null)
            {
                node.GetExpression().Apply(this);
            }
            if (node.GetCloseParenthesis() != null)
            {
                node.GetCloseParenthesis().Apply(this);
            }

            WriteLine("brtrue " + label1);
            WriteLine("br " + label2);
            WriteLine(label1 + ":");

            if (node.GetOpenBracket() != null)
            {
                node.GetOpenBracket().Apply(this);
            }
            if (node.GetStatements() != null)
            {
                node.GetStatements().Apply(this);
            }
            if (node.GetCloseBracket() != null)
            {
                node.GetCloseBracket().Apply(this);
            }


            WriteLine("br " + label3);
            WriteLine(label2 + ":");


            WriteLine(label3 + ":");

            OutAJustifIfStatement(node);
        }

        public override void CaseAIfelseIfStatement(AIfelseIfStatement node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;

            string label3 = branchLabel;
            branchLabel += 1;


            InAIfelseIfStatement(node);
            if (node.GetIf() != null)
            {
                node.GetIf().Apply(this);
            }
            if (node.GetOpenParenthesis() != null)
            {
                node.GetOpenParenthesis().Apply(this);
            }
            if (node.GetExpression() != null)
            {
                node.GetExpression().Apply(this);
            }
            if (node.GetCloseParenthesis() != null)
            {
                node.GetCloseParenthesis().Apply(this);
            }

            WriteLine("brtrue " + label1);
            WriteLine("br " + label2);
            WriteLine(label1 + ":");

            if (node.GetIfOpenBracket() != null)
            {
                node.GetIfOpenBracket().Apply(this);
            }
            if (node.GetIfStatements() != null)
            {
                node.GetIfStatements().Apply(this);
            }
            if (node.GetIfCloseBracket() != null)
            {
                node.GetIfCloseBracket().Apply(this);
            }

            WriteLine("br " + label3);
            WriteLine(label2 + ":");

            if (node.GetElse() != null)
            {
                node.GetElse().Apply(this);
            }
            if (node.GetElseOpenBracket() != null)
            {
                node.GetElseOpenBracket().Apply(this);
            }
            if (node.GetElseStatements() != null)
            {
                node.GetElseStatements().Apply(this);
            }
            if (node.GetElseCloseBracket() != null)
            {
                node.GetElseCloseBracket().Apply(this);
            }

            WriteLine(label3 + ":");

            OutAIfelseIfStatement(node);
        }

        public override void OutAEquivalentExpression3(AEquivalentExpression3 node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;


            Write("beq ");
            WriteLine(label1);

            WriteLine("\tldc.i4 0");
            WriteLine("\tbr " + label2);

            WriteLine(label1 + ":");
            WriteLine("\tldc.i4 1");

            WriteLine(label2 + ":");

        }

        public override void OutANotequivalentExpression3(ANotequivalentExpression3 node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;


            Write("bne.un ");
            WriteLine(label1);

            WriteLine("\tldc.i4 0");
            WriteLine("\tbr " + label2);

            WriteLine(label1 + ":");
            WriteLine("\tldc.i4 1");

            WriteLine(label2 + ":");
        }

        public override void OutAGreateqExpression4(AGreateqExpression4 node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;


            Write("bge ");
            WriteLine(label1);

            WriteLine("\tldc.i4 0");
            WriteLine("\tbr " + label2);

            WriteLine(label1 + ":");
            WriteLine("\tldc.i4 1");

            WriteLine(label2 + ":");
        }

        public override void OutALesseqExpression4(ALesseqExpression4 node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;


            Write("ble ");
            WriteLine(label1);

            WriteLine("\tldc.i4 0");
            WriteLine("\tbr " + label2);

            WriteLine(label1 + ":");
            WriteLine("\tldc.i4 1");

            WriteLine(label2 + ":");
        }

        public override void OutALessthanExpression4(ALessthanExpression4 node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;


            Write("blt ");
            WriteLine(label1);

            WriteLine("\tldc.i4 0");
            WriteLine("\tbr " + label2);

            WriteLine(label1 + ":");
            WriteLine("\tldc.i4 1");

            WriteLine(label2 + ":");
        }

        public override void OutAGreatthanExpression4(AGreatthanExpression4 node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;


            Write("bgt ");
            WriteLine(label1);

            WriteLine("\tldc.i4 0");
            WriteLine("\tbr " + label2);

            WriteLine(label1 + ":");
            WriteLine("\tldc.i4 1");

            WriteLine(label2 + ":");
        }

        public override void CaseAWhileWhileStatement(AWhileWhileStatement node)
        {
            string label1 = branchLabel;
            branchLabel += 1;

            string label2 = branchLabel;
            branchLabel += 1;

            InAWhileWhileStatement(node);
            if (node.GetWhile() != null)
            {
                node.GetWhile().Apply(this);
            }

            WriteLine(label1 + ":");

            if (node.GetOpenParenthesis() != null)
            {
                node.GetOpenParenthesis().Apply(this);
            }
            if (node.GetExpression() != null)
            {
                node.GetExpression().Apply(this);
            }
            if (node.GetCloseParenthesis() != null)
            {
                node.GetCloseParenthesis().Apply(this);
            }

            WriteLine("brzero " + label2);

            if (node.GetOpenBracket() != null)
            {
                node.GetOpenBracket().Apply(this);
            }
            if (node.GetStatements() != null)
            {
                node.GetStatements().Apply(this);
            }
            if (node.GetCloseBracket() != null)
            {
                node.GetCloseBracket().Apply(this);
            }

            WriteLine("br " + label1);
            WriteLine(label2 + ":");

            OutAWhileWhileStatement(node);
        }


    }
}
