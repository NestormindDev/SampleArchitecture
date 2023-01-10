using Architecture.Domain.SeedWork;


namespace Architecture.Domain.AggregatesModel
{
    public class Country : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

        public Country(string name, string code)
        {
            this.Name = name;
            this.Code = code; 
        }
    }
}
