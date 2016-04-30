using System.ComponentModel.DataAnnotations;

namespace PhotoProspector.Validations
{
    public class EqualValueAttribute : ValidationAttribute
    {
        public object value;

        public EqualValueAttribute(object value)
        {
            this.value = value;
        }

        public override bool IsValid(object value)
        {
            return this.value.Equals(value);
        }
    }
}
