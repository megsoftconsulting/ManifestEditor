using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Manifest
{
    public enum ExitCode
    {
        Success = 0,
        EmptyOrNullVersionCode = 1,
        InvalidFilename = 2,
        NotEnoughParameters = 3
    }

    public static class ManifestConsole
    {
        private static string Header()
        {
            var builder = new StringBuilder();
            builder.AppendLine("=====================================");
            builder.AppendLine("Android Manifest Editor v.0.1");
            builder.AppendLine("=====================================");

            return builder.ToString();
        }

        private static string Help()
        {
            var builder = new StringBuilder();
            builder.AppendLine("=====================================");
            builder.AppendLine("Android Manifest Editor  v.0.1");
            builder.AppendLine("=====================================");
            builder.AppendLine("Usage: setversioncode filename versionCode packageid");
            builder.AppendLine("Example:setversioncode manifest.xml 150 com.acme.dev");

            return builder.ToString();
        }

        private static ExitCode ValidateParameters(string[] args)
        {
            // At least two parameters
            return args.Length < 2 ? ExitCode.NotEnoughParameters : ExitCode.Success;
       
        }

        public static int Main(string[] args)
        {
            Console.WriteLine(Header());
           
            //validations
            var isValid = ValidateParameters(args);

            if (isValid != ExitCode.Success)
            {
                Console.Write(Help());

                return (int) isValid;
            }

            var versionCode = args[1];
            var manifestFilePath = args[0];
            var packageId = string.Empty;

            if (args.Length >=3)  packageId = args[2];
            try
            {
                //var file = System.IO.File.ReadAllText(manifestFilePath);
                var element = XElement.Load(manifestFilePath);
                var attributes = element.Attributes().ToList();

                // Update Version Code
                var versionAttribute = attributes
                    .FirstOrDefault(a => a.Name.LocalName.Equals("versionCode"));

                if (versionAttribute != null) versionAttribute.Value = versionCode;

                // Update Package ID
                var packageAttribute = attributes
                    .FirstOrDefault(a => a.Name.LocalName.Equals("package"));

                if (packageAttribute != null && !string.IsNullOrEmpty(packageId)) packageAttribute.Value = packageId;


                element.Save(manifestFilePath );
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Manifest File not found", manifestFilePath);
                return (int) ExitCode.InvalidFilename;
            }

            Console.WriteLine("Your manifest file has been changed successfully");
           // Console.WriteLine("Press ENTER to continue....");
           Console.ReadLine();
            return (int) ExitCode.Success;
        }
    }
}