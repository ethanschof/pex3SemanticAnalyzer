using CS426.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        // operand DONE
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
        // expression_math4 DONE
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
            Definition expressionMath1;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath1);
            }
        }

        // ----------------------------------------------------------------
        // expression_math3 DONE
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

        public override void OutABitnotExpressionMath3(ABitnotExpressionMath3 node)
        {
            Definition expressionMath4;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath4(), out expressionMath4))
            {
                // Problem occured lower on the tree
            }
            // The thing being bitted is not a float or an integer
            else if (!(expressionMath4 is IntegerDefinition) && !(expressionMath4 is FloatDefinition))
            {
                PrintWarning(node.GetBitNot(), "Only floats and integers can be negated");
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath4);
            }
        }

        // ----------------------------------------------------------------
        // expression_math2 DONE
        // ----------------------------------------------------------------

        public override void OutAMultiplyExpressionMath2(AMultiplyExpressionMath2 node)
        {
            Definition expressionMath2Def;
            Definition expressionMath3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath2(), out expressionMath2Def))
            {
                // error but occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpressionMath3(), out expressionMath3Def))
            {
                // error occured lower on the tree
            }
            else if (expressionMath2Def.GetType() != expressionMath3Def.GetType())
            {
                PrintWarning(node.GetMult(), "Cannot multiply " + expressionMath2Def.name + " by " + expressionMath3Def.name);
            } 
            else if (!(expressionMath2Def is IntegerDefinition) || !(expressionMath2Def is FloatDefinition))
            {
                PrintWarning(node.GetMult(), "You can only multiply integers and floats");
            }
            else if (!(expressionMath3Def is IntegerDefinition) || !(expressionMath3Def is FloatDefinition))
            {
                PrintWarning(node.GetMult(), "You can only multiply integers and floats");
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath2Def);
            }
        }

        public override void OutADivideExpressionMath2(ADivideExpressionMath2 node)
        {
            Definition expressionMath2Def;
            Definition expressionMath3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath2(), out expressionMath2Def))
            {
                // error but occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpressionMath3(), out expressionMath3Def))
            {
                // error occured lower on the tree
            }
            else if (expressionMath2Def.GetType() != expressionMath3Def.GetType())
            {
                PrintWarning(node.GetDivide(), "Cannot divide " + expressionMath2Def.name + " by " + expressionMath3Def.name);
            }
            else if (!(expressionMath2Def is IntegerDefinition) || !(expressionMath2Def is FloatDefinition))
            {
                PrintWarning(node.GetDivide(), "You can only divide integers and floats");
            }
            else if (!(expressionMath3Def is IntegerDefinition) || !(expressionMath3Def is FloatDefinition))
            {
                PrintWarning(node.GetDivide(), "You can only divide integers and floats");
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath2Def);
            }
        }

        public override void OutAPassExpressionMath2(APassExpressionMath2 node)
        {
            Definition expressionMath3;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath3(), out expressionMath3))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath3);
            }
        }

        // ----------------------------------------------------------------
        // expression_math1 DONE
        // ----------------------------------------------------------------
        public override void OutAPassExpressionMath1(APassExpressionMath1 node)
        {
            Definition expressionMath2;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath2(), out expressionMath2))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath2);
            }
        }

        public override void OutAAddExpressionMath1(AAddExpressionMath1 node)
        {
            Definition expressionMath1Def;
            Definition expressionMath2Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1Def))
            {
                // error but occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpressionMath2(), out expressionMath2Def))
            {
                // error occured lower on the tree
            }
            else if (expressionMath1Def.GetType() != expressionMath2Def.GetType())
            {
                PrintWarning(node.GetPlus(), "Cannot add " + expressionMath1Def.name + " to " + expressionMath2Def.name);
            }
            else if (!(expressionMath1Def is IntegerDefinition) || !(expressionMath1Def is FloatDefinition))
            {
                PrintWarning(node.GetPlus(), "You can only add integers and floats");
            }
            else if (!(expressionMath2Def is IntegerDefinition) || !(expressionMath2Def is FloatDefinition))
            {
                PrintWarning(node.GetPlus(), "You can only add integers and floats");
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath1Def);
            }
        }

        public override void OutASubtractExpressionMath1(ASubtractExpressionMath1 node)
        {
            Definition expressionMath1Def;
            Definition expressionMath2Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1Def))
            {
                // error but occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpressionMath2(), out expressionMath2Def))
            {
                // error occured lower on the tree
            }
            else if (expressionMath1Def.GetType() != expressionMath2Def.GetType())
            {
                PrintWarning(node.GetMinus(), "Cannot subtract " + expressionMath1Def.name + " by " + expressionMath2Def.name);
            }
            else if (!(expressionMath1Def is IntegerDefinition) || !(expressionMath1Def is FloatDefinition))
            {
                PrintWarning(node.GetMinus(), "You can only subtract integers and floats");
            }
            else if (!(expressionMath2Def is IntegerDefinition) || !(expressionMath2Def is FloatDefinition))
            {
                PrintWarning(node.GetMinus(), "You can only subtract integers and floats");
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath1Def);
            }
        }

        // ----------------------------------------------------------------
        // expression4
        // ----------------------------------------------------------------
        public override void OutAPassExpression4(APassExpression4 node)
        {
            Definition expressionMath1;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expressionMath1);
            }
        }

        // ----------------------------------------------------------------
        // expression3
        // ----------------------------------------------------------------
        public override void OutAPassExpression3(APassExpression3 node)
        {
            Definition expression4;

            if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expression4);
            }
        }

        // ----------------------------------------------------------------
        // expression2
        // ----------------------------------------------------------------
        public override void OutAPassExpression2(APassExpression2 node)
        {
            Definition expression3;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expression3);
            }
        }

        // ----------------------------------------------------------------
        // expression
        // ----------------------------------------------------------------
        public override void OutAPassExpression(APassExpression node)
        {
            Definition expression2;

            if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2))
            {
                // Problem occured lower on the tree
            }
            else
            {
                decoratedParseTree.Add(node, expression2);
            }
        }

        // ----------------------------------------------------------------
        // while_statement
        // ----------------------------------------------------------------


        // ----------------------------------------------------------------
        // if_statement
        // ----------------------------------------------------------------

        // ----------------------------------------------------------------
        // actual_param DONE BY JD, PLS CHECK
        // ----------------------------------------------------------------
        public override void OutAActualParamActualParam(AActualParamActualParam node)
        {
            Definition expressionDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // Error was already printed lower in the parse tree if this error occured
            }
        }

        // ----------------------------------------------------------------
        // actual_parameters DONE BY JD, CHECK PLS
        // ----------------------------------------------------------------
        public override void OutAManyActualParameters(AManyActualParameters node)
        {
            Definition actualParamDef;
            Definition actualParametersDef;

            if (!decoratedParseTree.TryGetValue(node.GetActualParam(), out actualParamDef))
            {
                // Error was already printed lower in the parse tree if this error occured
            }
            else if (!decoratedParseTree.TryGetValue(node.GetActualParameters(), out actualParametersDef))
            {
                // Error was already printed lower in the parse tree if this error occured
            }
            else
            {
                // No errors found
            }

        }
        public override void OutASingleActualParameters(ASingleActualParameters node)
        {
            Definition actualParamDef;

            if (!decoratedParseTree.TryGetValue(node.GetActualParam(), out actualParamDef))
            {
                // Error was already printed lower in the parse tree if this error occured
            }
        }

        // ----------------------------------------------------------------
        // function_call_statement
        // ----------------------------------------------------------------
        public override void OutAFunctionCallStatement(AFunctionCallStatement node)
        {
            Definition idDef;

            if (!globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " was not registered in the global symbol table");
            } 
            else if (!(idDef is FunctionDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is not a function definition");
            }
            // HOW DO I CHECK THE ARGUEMENTS TO THE PARAMETERS IN THE DEFINITION)
            
        }

        // ----------------------------------------------------------------
        // assign_statement DONE
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
        // declare_statement DONE
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
        // param
        // ----------------------------------------------------------------

        public override void OutAOneParamParam(AOneParamParam node)
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
            else if (!globalSymbolTable.TryGetValue(node.GetVarname().Text, out idDef))
            {
                // variable name does not exist in global, maybe a function, maybe tried to name something int
                PrintWarning(node.GetVarname(), "ID doesn't exist in global symbol table");
            }
            else
            {
                // Everything is correct
            }
        }

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
        // constant POSSIBLY DONE, CHECK PLS
        // ----------------------------------------------------------------

        public override void OutAConstantDeclareConstant(AConstantDeclareConstant node)
        {
            // Create the definition
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
                VariableDefinition newConstant = new VariableDefinition();
                newConstant.name = node.GetVarname().Text;
                newConstant.variableType = (TypeDefinition)typeDef;

                globalSymbolTable.Add(node.GetVarname().Text, newConstant);
            }

        }

        // ----------------------------------------------------------------
        // constants
        // ----------------------------------------------------------------



    }

}

