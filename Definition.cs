using System.Collections.Generic;

namespace CS426.analysis
{
    public abstract class Definition
    {
        public string name;

        public string toString()
        {
            return name;
        }
    }

    public abstract class TypeDefinition : Definition { }

    public class IntegerDefinition : TypeDefinition { }

    public class FloatDefinition : TypeDefinition { }

    public class StringDefinition : TypeDefinition { }

    public class BooleanDefinition : Definition { }

    public class VariableDefinition : Definition {
        public TypeDefinition variableType;
    }

    public class FunctionDefinition : Definition
    {
        public List<VariableDefinition> parameters;
    }


}

