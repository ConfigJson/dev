using System.Collections.Generic;

namespace ConfigJsonNET.Test
{
    public class SampleConfigClass
    {
        public SampleConfigClass()
        {
            SampleClassProperty = new SomeClass();
            SampleListOfClassProperty = new List<SomeClass>();
        }

        public string SampleStringProperty { set; get; }

        public SomeClass SampleClassProperty { set; get; }

        public List<SomeClass> SampleListOfClassProperty { set; get; }
    }
}