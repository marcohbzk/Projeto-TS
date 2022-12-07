using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

namespace Projeto
{
    public partial class Form1 : Form
    {
        private RSACryptoServiceProvider rsa;
        private AesCryptoServiceProvider aes;
        private const int PORT = 10000;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient tcpClient;
        Thread thread;

        private RSACryptoServiceProvider rsaSign;
        private RSACryptoServiceProvider rsaVerify;
        public Form1(TcpClient tcpClient, string batata, RSACryptoServiceProvider rsa)
        {
            InitializeComponent();
            this.rsa = rsa;
            this.aes = new AesCryptoServiceProvider();
            this.tcpClient = tcpClient;
            networkStream = tcpClient.GetStream();
            protocolSI = new ProtocolSI();
            aes.Key = GerarChavePrivada(batata);
            aes.IV = GerarIv(batata);
            textBoxMsg.MaxLength = 75;
            ThreadHandler();

            rsaSign = new RSACryptoServiceProvider();
            string publickey = rsaSign.ToXmlString(false);
            rsaVerify = new RSACryptoServiceProvider();
            rsaVerify.FromXmlString(publickey);
        }


        // Envia a mensagem do cliente para o servidor
        private void buttonSend_Click(object sender, EventArgs e)
        {
            string msg = textBoxMsg.Text;

            if (msg == null || msg == "")
            {
                return;
            }
            else
            {
                //Fazer assinatura
                byte[] dados = Encoding.UTF8.GetBytes(msg);
                byte[] signature;
                using (SHA1 sha1 = SHA1.Create())
                {
                    signature = rsa.SignData(dados, sha1);
                }
                //Converter a mensagem num pacote
                textBoxMsg.Clear();
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
                networkStream.Write(packet, 0, packet.Length);

                packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_4, signature);
                networkStream.Write(packet, 0, packet.Length);
            }
        }
        //Iniciar Thread
        public void ThreadHandler()
        {
            thread = new Thread(threadhandler);
            thread.Start();
        }

        delegate void ComparatorCallback(string text);

        private void ThreadComparator(string text)
        {
            /*
                InvokeRequired compara o ID do encadeamento 
             chama o thread para o ID do thread de criação.
                Se esses threads forem diferentes, ele retornará true.
            */
            try
            {
                if (this.ChatlistBox.InvokeRequired)
                {
                    ComparatorCallback d = new ComparatorCallback(ThreadComparator);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    ChatlistBox.Items.Add(text);
                }
            }
            catch (Exception)
            {

            }
        }

        //Passar a mensagem da Consola para a Chatbox
        public void threadhandler()
        {
            NetworkStream networkStream = this.tcpClient.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();
            while (true)
            {
                try
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                }
                catch (Exception)
                {
                    break; 
                }
                if (protocolSI.GetCmdType() == ProtocolSICmdType.DATA)
                {
                    string packet = protocolSI.GetStringFromData();
                    ThreadComparator(packet);
                }             
            }
        }
        private string CifrarTexto(string txt)
        {
            //VARIÁVEL PARA GUARDAR O TEXTO DECIFRADO EM BYTES
            byte[] txtDecifrado = Encoding.UTF8.GetBytes(txt);
            //VARIÁVEL PARA GUARDAR O TEXTO CIFRADO EM BYTES
            byte[] txtCifrado;
            //RESERVAR ESPAÇO NA MEMÓRIA PARA COLOCAR O TEXTO E CIFRÁ-LO
            MemoryStream ms = new MemoryStream();
            //INICIALIZAR O SISTEMA DE CIFRAGEM (WRITE)
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            //CRIFRAR OS DADOS
            cs.Write(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();
            //GUARDAR OS DADOS CRIFRADO QUE ESTÃO NA MEMÓRIA
            txtCifrado = ms.ToArray();
            //CONVERTER OS BYTES PARA BASE64 (TEXTO)
            string txtCifradoB64 = Convert.ToBase64String(txtCifrado);
            //DEVOLVER OS BYTES CRIADOS EM BASE64
            return txtCifradoB64;
        }
        //FUNÇÃO PARA DECIFRAR O TEXTO
        private string DecifrarTexto(string txtCifradoB64)
        {
            //VARIÁVEL PARA GUARDAR O TEXTO CIFRADO EM BYTES
            byte[] txtCifrado = Convert.FromBase64String(txtCifradoB64);
            //RESERVAR ESPAÇO NA MEMÓRIA PARA COLOCAR O TEXTO E CIFRÁ-LO
            MemoryStream ms = new MemoryStream(txtCifrado);
            //INICIALIZAR O SISTEMA DE CIFRAGEM (READ)
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            //VARIÁVEL PARA GUARDO O TEXTO DECIFRADO
            byte[] txtDecifrado = new byte[ms.Length];
            //VARIÁVEL PARA TER O NÚMERO DE BYTES DECIFRADOS
            int bytesLidos = 0;
            //DECIFRAR OS DADOS
            bytesLidos = cs.Read(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();
            //CONVERTER PARA TEXTO
            string textoDecifrado = Encoding.UTF8.GetString(txtDecifrado, 0, bytesLidos);
            //DEVOLVER TEXTO DECRIFRADO
            return textoDecifrado;
        }
        private byte[] GerarChavePrivada(string pass)
        {
            // O salt, explicado de seguida tem de ter no mínimo 8 bytes e não
            //é mais do que array be bytes. O array é caracterizado pelo []
            byte[] salt = new byte[] { 0, 1, 0, 8, 2, 9, 9, 7 };
            /* A Classe Rfc2898DeriveBytes é um método para criar uma chave e um vector de inicialização.
				Esta classe usa:
				pass = password usada para derivar a chave;
				salt = dados aleatório usado como entrada adicional. É usado para proteger password.
				1000 = número mínimo de iterações recomendadas
			*/
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pass, salt, 1000);
            //GERAR KEY

            return pwdGen.GetBytes(16);
        }
        private byte[] GerarIv(string pass)
        {
            byte[] salt = new byte[] { 7, 8, 7, 8, 2, 5, 9, 5 };
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pass, salt, 1000);

            return pwdGen.GetBytes(16);
        }
        //GERAR A HASH
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] dados = Encoding.UTF8.GetBytes(tbDataToSign.Text);

                //MÉTODO COMPUTEHASH CALCULA O VALOR DE HASH
                //SOBRE OS DADOS E DEVOLVE NUM VETOR DE BYTES
                // FONTE: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-5.0
                byte[] hash = sha1.ComputeHash(dados);

                // Converte o valor de um array de 8-bit de inteiros não-assinados para uma string
                // equivalente que está codificada em dígitos de base-64.
                // Fonte: https://bit.ly/35nXfzK
                tbHashData.Text = Convert.ToBase64String(hash);

            }
        }

        // MÉTODOS/BOTÕES DE CIFRAGEM //
        //DEFINIR MÉTODO QUE CIFRA A HASH DOS DADOS COM A CHAVE PRIVADA DO EMISSOR
        private void buttonSignHash_Click(object sender, EventArgs e)
        {
            byte[] hash = Convert.FromBase64String(tbHashData.Text);
            // MÉTODO MapNameToOID RECUPERA O IDENTIFICADOR DE OBJETOS (OID)
            // DO NOME DA SEQUÊNCIA DO ALGORITMO SHA1. O CRYPTOCONFIG
            // ACEDE ÀS INFORMAÇÕES DE CONFIGURAÇÃO DE CRIPTOGRAFIA
            byte[] signature = rsaSign.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            // NESTE CASO VAMOS USAR O ALGORITMO SHA1, MAS APLICANDO OUTROS ALGORITMOS
            // ERA EXPECTÁVEL QUE O PROGRAMA TAMBÉM TIVESSE SUCESSO, À SEMELHANÇA DO
            // QUE USÁMOS NA AULA PASSADA.

            tbSignature.Text = Convert.ToBase64String(signature);
        }

        // DEFINIR MÉTODO QUE CIFRA A HASH DOS DADOS COM A CHAVE PRIVADA DO EMISSOR
        private void buttonSignData_Click(object sender, EventArgs e)
        {
            byte[] dados = Encoding.UTF8.GetBytes(tbDataToSign.Text);


            using (SHA1 sha1 = SHA1.Create())
            {
                // PROCESSA O VALOR DA HASH DE UM DADO EM ESPECÍFICO E ASSINA-O.
                byte[] signature = rsaSign.SignData(dados, sha1);
                tbSignature.Text = Convert.ToBase64String(signature);
            }
        }

        // MÉTODOS/BOTÕES DE VERIFICAÇÃO //
        // DEFINIR MÉTODO PARA:
        // - VERIFICAR SE OS DADOS RECEBIDOS NÃO FORAM ALTERADOS POR TERCEIROS
        // - GARANTIR QUE OS DADOS FORAM ASSINADOS PELO EMISSOR
        private void buttonVerifyHash_Click(object sender, EventArgs e)
        {
            byte[] signature = Convert.FromBase64String(tbSignature.Text);
            byte[] hash = Convert.FromBase64String(tbHashData.Text);

            // VERIFICA QUE A ASSINATURA DIGITIAL É VÁLIDA
            bool verify = rsaVerify.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
            tbIsVerified.Text = verify.ToString();
        }

        // DEFINIR MÉTODO PARA VERIFICAR SE OS DADOS NÃO FORAM ALTERADOS E
        // CORRECTAMENTE ASSINADOS PELO EMISSOR
        private void buttonVerifyData_Click(object sender, EventArgs e)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] signature = Convert.FromBase64String(tbSignature.Text);
                byte[] dados = Encoding.UTF8.GetBytes(tbDataToSign.Text);

                // VERIFICA QUE UMA ASSINATURA DIGITIAL É VÁLIDA,
                bool verify = rsaVerify.VerifyData(dados, sha1, signature);
                tbIsVerified.Text = verify.ToString();
            }
        }
        //Função para fechar o cliente ao fechar a aplicação
        private void ClientForm_Close(object sender, FormClosingEventArgs e)
        {
            thread.Abort();
            byte[] packet = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(packet, 0, packet.Length);
            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                }
            networkStream.Close();
            tcpClient.Close();
        }
    }
}
