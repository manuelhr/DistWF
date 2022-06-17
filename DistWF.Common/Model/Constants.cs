namespace DistWF.Common.Model
{
    public class CalculationServiceNames
    {
        public const string Sum = "sum";
        public const string Substract = "substract";
        public const string Multiply = "multiply";
        public const string Divide = "divide";
    }
    public class CalculationAPIMethods
    {
        public static string Calculate = "Calculate";
    }
    public class DistWFAssemblyNames
    {
        public const string BackEnd = "DistWF.Backend.dll";
        public const string Engine = "DistWF.Engine.dll";
        public const string Adapter = "DistWF.Adapter.dll";
    }

    public class Messages
    {
        public const string Welcome = "Bienvenido";
        public const string InvalidRequest = "Solicitud no válida";
        public const string DivisorCannotBeZero = "El divisor no puede ser cero";
        public const string ServiceNotFound = "Servicio no reconocido";
        public const string ErrorInvokingService = "Error al invocar al servicio";
        public const string EnterFirstOperand = "Por favor, ingrese el primer operando";
        public const string EnterOperator = "Por favor, ingrese la operación ( +, - , * , / )";
        public const string EnterSecondOperand = "Por favor, ingrese el segundo operando";
        public const string AssemblyDirectoryNotFound = "Directorio de ensamblados compartidos no encontrado";
        public const string AssemblyDirectoryDoesNotContainAssemblies = "No se encontró ensamblados en el directorio compartido";
        public const string AssemblyNotFound= "Ensamblado no encontrado";
    }
}
