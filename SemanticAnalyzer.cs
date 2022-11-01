using CS426.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS426.analysis
{
    class SemanticAnalyzer : DepthFirstAdapter
    {
        Dictionary<string, Definition> globalSymbolTable;
        Dictionary<string, Definition> localSymbolTable;
        Dictionary<Node, Definition> decoratedParseTree;

        void PrintWarning(Token t, String message)
        {
            Console.WriteLine("Error on line " + t.Line + ":" + t.Pos + " :: " + message);
        }

        public override void InAProgram(AProgram node)
        {
            globalSymbolTable = new Dictionary<string, Definition>();
            localSymbolTable = new Dictionary<string, Definition>();
            decoratedParseTree = new Dictionary<Node, Definition>();

            Definition intDefinition = new IntegerDefinition();
            intDefinition.name = "int";

            Definition stringDefinition = new StringDefinition();
            stringDefinition.name = "string";

            globalSymbolTable.Add("int", intDefinition);
            globalSymbolTable.Add("string", stringDefinition);
        }

        // ----------------------------------------------------------------
        // OPERAND
        // ----------------------------------------------------------------
        public override void OutAIntOperand(AIntOperand node)
        {
            Definition intDefinition = new IntegerDefinition();
            intDefinition.name = "int";

            decoratedParseTree.Add(node, intDefinition);
        }

        public override void OutAStrOperand(AStrOperand node)
        {
            Definition stringDefinition = new StringDefinition();
            stringDefinition.name = "string";

            decoratedParseTree.Add(node, stringDefinition);
        }

        public override void OutAFloOperand(AFloOperand node)
        {
            Definition floatDefinition = new FloatDefinition();
            floatDefinition.name = "float";

            decoratedParseTree.Add(node, floatDefinition);
        }

        public override void OutAVariableOperand(AVariableOperand node)
        {
            // Step 1:  Get thename of the id
            String varName = node.GetId().Text;

            // Make a Object to Store the Definition
            Definition varDefinition;

            if (!localSymbolTable.TryGetValue(varName, out varDefinition))
            {
                PrintWarning(node.GetId(), varName + " not found");
            }
            else if (!(varDefinition is VariableDefinition))
            {
                PrintWarning(node.GetId(), varName + " is not a variable");
            }
            else
            {
                VariableDefinition v = (VariableDefinition)varDefinition;

                decoratedParseTree.Add(node, v.variableType);
            }
        }

        // EVERYTHING BELOW HERE HAS NOT BEEN CHECKED WITH OUR CODES NAMING CONVENTIONS

        // ----------------------------------------------------------------
        // EXPRESSION 3
        // ----------------------------------------------------------------
        public override void OutAPassExpression3(APassExpression3 node)
        {
            Definition operandDefinition;

            if (!decoratedParseTree.TryGetValue(node.GetOperand(), out operandDefinition))
            {
                // If the tree wasn't decorated, the problem/error was printed at a lower level
            }
            else
            {
                decoratedParseTree.Add(node, operandDefinition);
            }
        }

        public override void OutANegativeExpression3(ANegativeExpression3 node)
        {
            Definition operandDefinition;

            if (!decoratedParseTree.TryGetValue(node.GetOperand(), out operandDefinition))
            {
                // Problem occured lower on the tree
            }
            else if (!(operandDefinition is NumberDefinition))
            {
                PrintWarning(node.GetSub(), "Only a number can be negated");
            }
            else
            {
                decoratedParseTree.Add(node, operandDefinition);
            }
        }

        // ----------------------------------------------------------------
        // EXPRESSION 2
        // ----------------------------------------------------------------
        public override void OutAPassExpression2(APassExpression2 node)
        {
            Definition expression3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {
                // If the tree wasn't decorated, the problem/error was printed at a lower level
            }
            else
            {
                decoratedParseTree.Add(node, expression3Def);
            }
        }

        // ----------------------------------------------------------------
        // EXPRESSION 1
        // ----------------------------------------------------------------
        public override void OutAPassExpression(APassExpression node)
        {
            Definition expression2Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {
                // If the tree wasn't decorated, the problem/error was printed at a lower level
            }
            else
            {
                decoratedParseTree.Add(node, expression2Def);
            }
        }


        // ----------------------------------------------------------------
        // Declaration
        // ----------------------------------------------------------------
        public override void OutADeclareStatement(ADeclareStatement node)
        {
            Definition typeDef;
            Definition idDef;

            if (!globalSymbolTable.TryGetValue(node.GetType().Text, out typeDef))
            {
                // Check if the type is a valid type
                PrintWarning(node.GetType(), "Type " + node.GetType().Text + " does not exist");
            }
            else if (localSymbolTable.TryGetValue(node.GetVarname().Text, out idDef))
            {
                // Check if variable name is already being used
                PrintWarning(node.GetVarname(), "ID already declared in local symbol table");
            }
            else
            {
                VariableDefinition newVariable = new VariableDefinition();
                newVariable.name = node.GetVarname().Text;
                newVariable.variableType = (TypeDefinition)typeDef;

                localSymbolTable.Add(node)
            }
        }

    }

}

