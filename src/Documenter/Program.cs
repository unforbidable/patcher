using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Documenter
{
    class Program
    {
        public const string RootNamespace = "Patcher.Rules.Compiled";
        public const string RootFolder = "reference";
        public const string TargetPath = @"..\..\..\..\doc\generated";

        static void Main(string[] args)
        {
            var assembly = Assembly.GetAssembly(typeof(Patcher.Rules.Compiled.Forms.IForm));
            var xmlFilePath = Path.GetFileNameWithoutExtension(assembly.GetName().CodeBase) + ".xml";

            XDocument doc = XDocument.Load(xmlFilePath);
            

            foreach (var type in assembly.GetTypes())
            {
                string name = type.GetLocalName();
                string ns = type.GetLocalNamespace();

                // Ignore extensions (extension methods will be added to the respective class they extend)
                if (ns.StartsWith("Extensions"))
                    continue;

                Console.WriteLine("Type {0} in {1}", name, ns);



            }




            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
