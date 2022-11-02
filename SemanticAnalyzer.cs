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

            Definition floatDefintion = new FloatDefinition();
            floatDefintion.name = "float";

            // Register these definitions in the decorated parse tree
            globalSymbolTable.Add("int", intDefinition);
            globalSymbolTable.Add("string", stringDefinition);
            globalSymbolTable.Add("float", floatDefintion);
        }

        // ----------------------------------------------------------------
        // operand
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

            // See if the variable is in the local symbol table
            if (!localSymbolTable.TryGetValue(varName, out varDefinition))
            {
                // If it's not in the local check the global
                // THIS IF STATEMENT MIGHT HAVE TO BE MOVED INTO THE TOP ONE WITH AN OR RATHER THAN NESTED
                if (!globalSymbolTable.TryGetValue(varName, out varDefinition))
                {
                    // It's not in either table so print an error
                    PrintWarning(node.GetId(), varName + " not found");
                }
            }
            // Check to see if varDefinition is a variable and not a function or constant
            else if (!(varDefinition is VariableDefinition))
            {
                PrintWarning(node.GetId(), varName + " is not a variable");
            }
            // Then it's in a table and is a variable so we can decorate the tree with its type
            else
            {
                VariableDefinition v = (VariableDefinition)varDefinition;

                decoratedParseTree.Add(node, v.variableType);
            }
        }

        // ----------------------------------------------------------------
        // expression_math4
        // ----------------------------------------------------------------
        public override void OutAPassExpressionMath4(APassExpressionMath4 node)
        {
            Definition operandDefinition;

            if (!decoratedParseTree.TryGetValue(node.GetOperand(), out operandDefinition))
            {
                // If the tree isn't decorated then something happened at the operand node
            } 
            else
            {
                decoratedParseTree.Add(node, operandDefinition);
            }
        }

        public override void OutAParenthesisExpressionMath4(AParenthesisExpressionMath4 node)
        {
            
        }

        // ----------------------------------------------------------------
        // expression_math3
        // ----------------------------------------------------------------

        public override void OutAPassExpressionMath3(APassExpressionMath3 node)
        {
            Definition expressionMath4;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath4(), out expressionMath4))
            {
                // Problem occured lower on the tree
            } 
            else
            {
                decoratedParseTree.Add(node, expressionMath4);
            }
        }

        public override void OutANegativeExpressionMath3(ANegativeExpressionMath3 node)
        {
            Definition expressionMath4;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath4(), out expressionMath4))
            {
                // Problem occured lower on the tree
            }
            // The thing being negatived is not a float or an integer
            else if (!(expressionMath4 is IntegerDefinition) && !(expressionMath4 is FloatDefinition))
            {
                PrintWarning(node.GetMinus(), "Only floats and integers can be negated");
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath4);
            }
        }

        // ----------------------------------------------------------------
        // expression_math2
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // expression_math1
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // expression4
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // expression3
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // expression2
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // expression
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // while_statement
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // if_statement
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // actual_param
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // actual_parameters
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // function_call_statement
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // assign_statement
        // ----------------------------------------------------------------

        public override void OutAAssignStatement(AAssignStatement node)
        {
            Definition idDef;
            Definition expressionDef;

            if (!localSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " does not exist");
            }
            else if (!(idDef is VariableDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is not a variable");
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // Error was already printed lower in the parse tree if this error occured
            }
            else if (((VariableDefinition)idDef).variableType.name != expressionDef.name)
            {
                PrintWarning(node.GetId(), "Types don't match");
            }
            else
            {
                // Nothing is required
            }
        }


        // ----------------------------------------------------------------
        // declare_statement
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
            // Make sure the type is a type and not something else in the symbol table
            else if (!(typeDef is TypeDefinition))
            {
                PrintWarning(node.GetType(), "Type " + node.GetType().Text + " is not a recognized data type");
            }
            else if (localSymbolTable.TryGetValue(node.GetVarname().Text, out idDef))
            {
                // Check if variable name is already being used
                PrintWarning(node.GetVarname(), "ID already declared in local symbol table");
            }
            else if (globalSymbolTable.TryGetValue(node.GetVarname().Text, out idDef))
            {
                // variable name was already used in global, maybe a function, maybe tried to name something int
                PrintWarning(node.GetVarname(), "ID already declared in global symbol table");
            }
            else
            {
                VariableDefinition newVariable = new VariableDefinition();
                newVariable.name = node.GetVarname().Text;
                newVariable.variableType = (TypeDefinition)typeDef;

                localSymbolTable.Add(node.GetVarname().Text, newVariable);
            }
        }

        // ----------------------------------------------------------------
        // statement
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // statements
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // param
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // params
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // function
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // functions
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // constant
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // constants
        // ----------------------------------------------------------------



    }

}

