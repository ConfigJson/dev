using System.Collections.Generic;

namespace ConfigJsonNET.Test
{
    public class SampleConfigClass1
    {
        public SampleConfigClass1()
        {
            SampleClassProperty = new SomeClass();
            SampleListOfClassProperty = new List<SomeClass>();
        }
        public string SampleStringProperty { set; get; }
        public SomeClass SampleClassProperty { set; get; }
        public List<SomeClass> SampleListOfClassProperty { set; get; }
    }

    public class SampleConfigClass2 
    {
        public SampleConfigClass2()
        {
            SampleClassProperty = new SomeClass();
            SampleListOfClassProperty = new List<SomeClass>();
        }
        public string SampleStringProperty { set; get; }
        public SomeClass SampleClassProperty { set; get; }
        public List<SomeClass> SampleListOfClassProperty { set; get; }
    }

    public class SampleConfigClass3 
    {
        public SampleConfigClass3()
        {
            SampleClassProperty = new SomeClass();
            SampleListOfClassProperty = new List<SomeClass>();
        }
        public string SampleStringProperty { set; get; }
        public SomeClass SampleClassProperty { set; get; }
        public List<SomeClass> SampleListOfClassProperty { set; get; }
    }
}