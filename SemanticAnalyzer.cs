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
        List<VariableDefinition> parameters;
        List<TypeDefinition> args;

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
            // The thing being bitted is not a boolean
            else if (!(expressionMath4 is BooleanDefinition))
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
        // GreaterEQ DONE
        // ----------------------------------------------------------------
        public override void OutAGreateqExpression4(AGreateqExpression4 node)
        {
            Definition expressionMath1Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1Def))
            {
                // Problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {
                // Problem occured lower on the tree
            }
            else if ((expressionMath1Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetGreatEqThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else if (!(expressionMath1Def is IntegerDefinition) || !(expressionMath1Def is FloatDefinition))
            {
                PrintWarning(node.GetGreatEqThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else
            {
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        // ----------------------------------------------------------------
        // LesserEQ ADDED BY JD, CHECK
        // ----------------------------------------------------------------
        public override void OutALesseqExpression4(ALesseqExpression4 node)
        {
            Definition expressionMath1Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1Def))
            {
                // Problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {
                // Problem occured lower on the tree
            }
            else if ((expressionMath1Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetLessEqThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else if (!(expressionMath1Def is IntegerDefinition) || !(expressionMath1Def is FloatDefinition))
            {
                PrintWarning(node.GetLessEqThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else
            {
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        // ----------------------------------------------------------------
        // GreatThan ADDED BY JD, CHECK
        // ----------------------------------------------------------------
        public override void OutAGreatthanExpression4(AGreatthanExpression4 node)
        {
            Definition expressionMath1Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1Def))
            {
                // Problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {
                // Problem occured lower on the tree
            }
            else if ((expressionMath1Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetGreaterThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else if (!(expressionMath1Def is IntegerDefinition) || !(expressionMath1Def is FloatDefinition))
            {
                PrintWarning(node.GetGreaterThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else
            {
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        // ----------------------------------------------------------------
        // LessThan ADDED BY JD, CHECK
        // ----------------------------------------------------------------
        public override void OutALessthanExpression4(ALessthanExpression4 node)
        {
            Definition expressionMath1Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpressionMath1(), out expressionMath1Def))
            {
                // Problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {
                // Problem occured lower on the tree
            }
            else if ((expressionMath1Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetLessThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else if (!(expressionMath1Def is IntegerDefinition) || !(expressionMath1Def is FloatDefinition))
            {
                PrintWarning(node.GetLessThan(), "Cannot compare " + expressionMath1Def.name + " with " + expression4Def.name);
            }
            else
            {
                decoratedParseTree.Add(node, new BooleanDefinition());
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

        public override void OutAEquivalentExpression3(AEquivalentExpression3 node)
        {
            Definition expression3Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {
                // problem occured lower on the tree
            } 
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {
                // problem lower on tree
            }
            else if ((expression3Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetEquivalence(), "Cannot compare " + expression3Def.name + " with " + expression4Def.name);
            } 
            else if (!(expression3Def is IntegerDefinition) || !(expression3Def is FloatDefinition))
            {
                PrintWarning(node.GetEquivalence(), "Can only compare integers and floats of same type");
            } 
            else
            {
                decoratedParseTree.Add(node, new BooleanDefinition());
            }

        }

        public override void OutANotequivalentExpression3(ANotequivalentExpression3 node)
        {
            Definition expression3Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {
                // problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {
                // problem lower on tree
            }
            else if ((expression3Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetNotEquivalent(), "Cannot compare " + expression3Def.name + " with " + expression4Def.name);
            }
            else if (!(expression3Def is IntegerDefinition) || !(expression3Def is FloatDefinition))
            {
                PrintWarning(node.GetNotEquivalent(), "Can only compare integers and floats of same type");
            }
            else
            {
                decoratedParseTree.Add(node, new BooleanDefinition());
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

        public override void OutABitandExpression2(ABitandExpression2 node)
        {
            Definition expression2Def;
            Definition expression3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {
                // problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {
                // problem lower on tree
            }
            else if ((expression2Def.GetType() != expression3Def.GetType()))
            {
                PrintWarning(node.GetBitAnd(), "Cannot bit operate " + expression2Def.name + " with " + expression3Def.name);
            }
            else if (!(expression2Def is BooleanDefinition))
            {
                PrintWarning(node.GetBitAnd(), "Can only do bit operations on Boolean types");
            }
            else
            {
                decoratedParseTree.Add(node, expression2Def);
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

        public override void OutABitorExpression(ABitorExpression node)
        {
            Definition expressionDef;
            Definition expression2Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // problem occured lower on the tree
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {
                // problem lower on tree
            }
            else if ((expressionDef.GetType() != expression2Def.GetType()))
            {
                PrintWarning(node.GetBitOr(), "Cannot bit operate " + expressionDef.name + " with " + expression2Def.name);
            }
            else if (!(expressionDef is BooleanDefinition))
            {
                PrintWarning(node.GetBitOr(), "Can only do bit operations on Boolean types");
            }
            else
            {
                decoratedParseTree.Add(node, expressionDef);
            }
        }

        // ----------------------------------------------------------------
        // while_statement DONE
        // ----------------------------------------------------------------

        public override void OutAWhileWhileStatement(AWhileWhileStatement node)
        {
            Definition whileConditionalDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out whileConditionalDef))
            {
                // error occured lower in the tree
            }
            else if (!(whileConditionalDef is BooleanDefinition))
            {
                PrintWarning(node.GetWhile(), "Conditional is not in boolean form");
            }
        }

        // ----------------------------------------------------------------
        // if_statement DONE
        // ----------------------------------------------------------------

        public override void OutAJustifIfStatement(AJustifIfStatement node)
        {
            Definition ifConditionalDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out ifConditionalDef))
            {
                // Error occured lower in the tree
            }
            else if (!(ifConditionalDef is BooleanDefinition))
            {
                PrintWarning(node.GetIf(), "Conditional is not in boolean form");
            }
        }

        public override void OutAIfelseIfStatement(AIfelseIfStatement node)
        {
            Definition ifConditionalDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out ifConditionalDef))
            {
                // Error occured lower in the tree
            }
            else if (!(ifConditionalDef is BooleanDefinition))
            {
                PrintWarning(node.GetIf(), "Conditional is not in boolean form");
            }
        }


        // ----------------------------------------------------------------
        // actual_param DONE
        // ----------------------------------------------------------------
        public override void OutAActualParamActualParam(AActualParamActualParam node)
        {
            Definition expressionDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // Error was already printed lower in the parse tree if this error occured
            } 
            else
            {
                // place the function call arguement into args global
                args.Add((TypeDefinition)expressionDef);
            }
        }


        // ----------------------------------------------------------------
        // function_call_statement DONE
        // ----------------------------------------------------------------
        public override void InAFunctionCallStatement(AFunctionCallStatement node)
        {
            List<TypeDefinition> args = new List<TypeDefinition>();
        }

        public override void OutAFunctionCallStatement(AFunctionCallStatement node)
        {
            Definition idDef;

            // function not in global table
            if (!globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " was not registered in the global symbol table");
            } 
            // variable is registered as a function
            else if (!(idDef is FunctionDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is not a function definition");
            } 
            else
            {
                FunctionDefinition func = (FunctionDefinition)idDef;
                // Make sure number of arguements and parameters are the same
                if (!(args.Count == func.parameters.Count) || !((func.parameters.Count == 0) && (parameters.Count == args.Count)) )
                {
                    PrintWarning(node.GetId(), "Incorrect number of arguments for function call");
                } 
                else 
                {
                    // Loop through to check each type of arguement and variable
                    for (var i = 0; i < args.Count; i++)
                    {
                        if(args[i].GetType() != func.parameters[i].GetType())
                        {
                            PrintWarning(node.GetId(), "Types do not match between declaration and call");
                        }
                    }
                }

            }
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
        // param DONE
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
                parameters.Add((VariableDefinition)typeDef);
            }
        }

        // ----------------------------------------------------------------
        // function DONE
        // ----------------------------------------------------------------
        public override void InAFunction(AFunction node)
        {
            // Refresh the parameters list
            List<VariableDefinition> parameters = new List<VariableDefinition>();

            Definition idDef;

            if (globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
            } 
            else
            {
                localSymbolTable = new Dictionary<string, Definition>();

                // Register the new function definition in teh global table
                FunctionDefinition newFunctionDefinition = new FunctionDefinition();
                newFunctionDefinition.name = node.GetId().Text;

                newFunctionDefinition.parameters = new List<VariableDefinition>();

                globalSymbolTable.Add(node.GetId().Text, newFunctionDefinition);
            }
        }

        public override void OutAFunction(AFunction node)
        {
            globalSymbolTable.Remove(node.GetId().Text);

            FunctionDefinition newFunctionDefinition = new FunctionDefinition();
            newFunctionDefinition.name = node.GetId().Text;

            newFunctionDefinition.parameters = parameters;
            globalSymbolTable.Add(node.GetId().Text, newFunctionDefinition);

            // Wipe global paramaters variable
            parameters = new List<VariableDefinition>();
        }


        // ----------------------------------------------------------------
        // constant DONE
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


    }

}

