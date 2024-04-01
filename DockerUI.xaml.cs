using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using corel = Corel.Interop.VGCore;
using System.Reflection;

namespace Latex4CorelDraw
{

    public partial class DockerUI : UserControl
    {
        private LatexDialog m_dialog;
        private corel.Application corelApp;
        public corel::Application CorelApp
        {
            get { return corelApp; }
        }
        static private DockerUI m_current;
        static public DockerUI Current
        {
            get { return m_current; }
        }
        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            m_current = this;
            InitializeComponent();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AddinUtilities.initFonts();
            AddinUtilities.copyLatexTemplate("LatexTemplate.txt", Properties.Resources.LatexTemplate);

            m_dialog = new LatexDialog();
            SettingsManager mgr = SettingsManager.getCurrent();
        }

        private void btn_addEquation_Click(object sender, RoutedEventArgs e)
        {
            string template = "\\begin{equation*}\r\n\t<Enter latex code>\r\n\\end{equation*}\r\n";
            createLatexObject(template, "Create latex equation");
        }

        public void createLatexObject(string template, string title)
        {
            List<string> args = new List<string>();
            bool useTemplate = template != null;

            m_dialog.init(template, useTemplate, title);
            m_dialog.ShowDialog();
        }

        public bool editLatexObject(Corel.Interop.VGCore.Shape s)
        {
            LatexEquation eq = ShapeTags.getLatexEquation(s);
            if (eq != null)
            {
                m_dialog.init(eq, "Edit latex object");

                m_dialog.ShowDialog();
                if (m_dialog.Result == System.Windows.Forms.DialogResult.OK)
                {
                    Corel.Interop.VGCore.Shape latexObj = m_dialog.LatexEquation.m_shape;
                    if (latexObj != null)
                    {
                        latexObj.TransformationMatrix = s.TransformationMatrix;
                        s.Delete();
                    }
                }
                return true;
            }
            return false;
        }

        private void btn_editLatex_Click(object sender, RoutedEventArgs e)
        {
            Corel.Interop.VGCore.ShapeRange sel = corelApp.ActiveDocument.SelectionRange;
            bool found = false;
            foreach (Corel.Interop.VGCore.Shape s in sel)
            {
                if (editLatexObject(s))
                    found = true;
            }
            if (!found)
                createLatexObject(null, "Create latex object");
        }

        private void btn_addLatex_Click(object sender, RoutedEventArgs e)
        {
            createLatexObject(null, "Create latex object");
        }

        private void btn_addEqnarray_Click(object sender, RoutedEventArgs e)
        {
            string template = "\\begin{eqnarray*}\r\n\t<Enter latex code>\r\n\\end{eqnarray*}\r\n";
            createLatexObject(template, "Create latex equation array");
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.RequestingAssembly != null)
                return args.RequestingAssembly;
            Assembly asm = null;
            string name = args.Name;
            if (name.Contains(".resources"))
                name = name.Replace(".resources", "");
            asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(r => string.Equals(r.FullName.Split(',')[0], name.Split(',')[0]));
            if (args.Name.Contains(".resources"))
                asm = LoadResourceAssembly(asm);
            if (asm == null)
                asm = Assembly.LoadFrom(Name);
            return asm;
        }
        private Assembly LoadResourceAssembly(Assembly executingAsm)
        {
            if (executingAsm == null)
                return executingAsm;
            string[] resourcesName = executingAsm.GetManifestResourceNames();
            string resourceName = string.Empty;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                if (!resourcesName[i].Contains(".g."))
                {
                    resourceName = resourcesName[i];
                    break;
                }
            }
            if (string.IsNullOrEmpty(resourceName))
                return executingAsm;
            using (var stream = executingAsm.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            }
            return executingAsm;
        }
    }
}
