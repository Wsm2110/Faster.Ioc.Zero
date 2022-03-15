using Faster.Ioc.Zero.SourceGenerator.CodeGeneration.Enums;

namespace Faster.Ioc.Zero.SourceGenerator.CodeGeneration
{
    public class Field : BaseElement
    {
        public Field() { }

        public Field(BuiltInDataType builtInDataType, string name) : base(builtInDataType, name) { }

        public Lifetime Lifetime { get; set; }

        public Field(string customDataType, string name) : base(customDataType, name) { }

        public override AccessModifier AccessModifier { get; set; } = AccessModifier.Protected;

        public virtual string Body { get; }

        public virtual string DefaultValue { get; set; }
        protected string DefaultValueFormated => DefaultValue != null && Lifetime == Lifetime.Singleton ? " = " + DefaultValue : " => " + DefaultValue;

        public override bool HasAttributes => false;

        protected virtual string Ending { get; } = ";";

        public override string ToString() => base.ToString() + Body + DefaultValueFormated + Ending;
    }
}
