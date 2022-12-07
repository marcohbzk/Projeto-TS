using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EI.SI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Projeto
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // var form = new Form1(); Em caso de colocar o chattextbox public nao funcionar
            Application.Run(new LoginRegister());
        }
    }
}
