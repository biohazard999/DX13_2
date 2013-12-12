using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp.Utils;
using Microsoft.CSharp;
using Para.Modules.Win.TaskbarIntegration.Helpers;
using Vestris.ResourceLib;

namespace Para.Modules.Win.TaskbarIntegration.ResourceManagers
{
    public class RuntimeImageResourceManager
    {
        private readonly Dictionary<string, int> ImageIndexes = new Dictionary<string, int>();

        public string WriteImageResouces(IEnumerable<string> imageNames)
        {
            var location = Path.GetDirectoryName(typeof(TaskbarJumpListWindowController).Assembly.Location);
            var assemblyFileName = "AutoGen.dll";

            var assemblyPath = Path.Combine(location, assemblyFileName);

            var codeProvider = new CSharpCodeProvider();
            var icc = codeProvider.CreateCompiler();
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = assemblyFileName;

            assemblyPath = Path.Combine(location, parameters.OutputAssembly);

            CompilerResults results = icc.CompileAssemblyFromDom(parameters, new CodeCompileUnit());

            if (results.Errors.HasErrors)
                return null;

            foreach (var imageName in imageNames)
            {
                var info = ImageLoader.Instance.GetImageInfo(imageName);
                var infoLarge = ImageLoader.Instance.GetImageInfo(imageName);

                if (info == null || info.Image == null || infoLarge == null || infoLarge.Image == null)
                    continue;

                var writer = new IconFileWriter();

                writer.Images.Add(info.Image);
                writer.Images.Add(infoLarge.Image);

                writer.Save(imageName + ".ico");
            }

            using (var scope = new FilesScope(imageNames.Select(m => m + ".ico")))
            {
                uint c = 100;
                uint id = 1;

                foreach (var file in scope.FilesNames)
                {
                    var rc = new IconDirectoryResource();

                    rc.Name = new ResourceId(c);
                    rc.Language = GetCurrentLangId();

                    var iconFile = new IconFile(file);

                    foreach (var icon in iconFile.Icons)
                    {
                        rc.Icons.Add(new IconResource(icon, new ResourceId(id), rc.Language));
                    }

                    rc.SaveTo(assemblyFileName);

                    ImageIndexes[Path.GetFileNameWithoutExtension(file)] = Convert.ToInt32(id) - 1;

                    c++;
                    id++;
                }
            }

            Assembly.Load(results.CompiledAssembly.FullName);

            return assemblyPath;
        }

        public int GetImageIndex(string imageName)
        {
            int keyValue = -1;
            
            ImageIndexes.TryGetValue(imageName, out keyValue);

            return keyValue;
        }

        private ushort GetCurrentLangId()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            int pid = PRIMARYLANGID(currentCulture.LCID);
            int sid = SUBLANGID(currentCulture.LCID);
            return (ushort)MAKELANGID(pid, sid);
        }

        public static int MAKELANGID(int primary, int sub)
        {
            return (((ushort)sub) << 10) | ((ushort)primary);
        }

        public static int PRIMARYLANGID(int lcid)
        {
            return ((ushort)lcid) & 0x3ff;
        }

        public static int SUBLANGID(int lcid)
        {
            return ((ushort)lcid) >> 10;
        }
 
    }
}