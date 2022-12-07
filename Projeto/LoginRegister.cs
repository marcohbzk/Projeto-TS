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
using Newtonsoft.Json;

namespace Projeto
{
    public partial class LoginRegister : Form
    {
        private RSACryptoServiceProvider rsa;
        private const int PORT = 10000;
        AesCryptoServiceProvider aes;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient tcpClient;
        string batata;

        private const int  SALTSIZE = 8;

        public LoginRegister()
        {
            InitializeComponent();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, PORT);
            tcpClient = new TcpClient();
            tcpClient.Connect(endpoint);
            networkStream = tcpClient.GetStream();
            protocolSI = new ProtocolSI();
            rsa = new RSACryptoServiceProvider();
            aes = new AesCryptoServiceProvider();
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
        private void LoginRegister_Load(object sender, EventArgs e)
        {
            string publickey = rsa.ToXmlString(false);
            rsa.ToXmlString(true);
            
            byte[] packet = protocolSI.Make(ProtocolSICmdType.PUBLIC_KEY, publickey);
            networkStream.Write(packet, 0, packet.Length);

            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }

            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            batata = Convert.ToBase64String(rsa.Decrypt(protocolSI.GetData(), true));

            packet = protocolSI.Make(ProtocolSICmdType.ACK);

            networkStream.Write(packet, 0, packet.Length);

            aes.Key = GerarChavePrivada("batata");
            aes.IV = GerarIv("batata");
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            String password = textBoxPassword.Text;
            String username = textBoxUsername.Text;
            string[] user = { username, password };
            string json = JsonConvert.SerializeObject(user);

            if (password == "" || username == "")
            {
                MessageBox.Show("Credenciais Inválidas!");
            }
            else
            {

                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, json);
                networkStream.Write(packet, 0, packet.Length);
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                string boolean = protocolSI.GetStringFromData();
                bool login = bool.Parse(boolean);
                if (login)
                {
                    MessageBox.Show("Registado com sucesso!");
                }
                else
                {
                    MessageBox.Show("Credenciais Inválidas!");
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String password = textBoxPassword.Text;
            String username = textBoxUsername.Text;
            string[] user = { username, password };
            string json = JsonConvert.SerializeObject(user);
            
            if (password == "" || username == "")
            {
                MessageBox.Show("Credenciais Inválidas!");
            } else 
            {
                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, json);
                networkStream.Write(packet, 0, packet.Length);
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                string boolean = protocolSI.GetStringFromData();
                bool login = bool.Parse(boolean);

                if (login)
                {
                    Form1 formChat = new Form1(tcpClient, batata, rsa);
                    formChat.Closed += (s, args) => this.Close();
                    formChat.Show();
                    this.Hide();
                } else
                {
                    MessageBox.Show("Credenciais Inválidas!");
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

        private void LoginRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            byte[] packet = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(packet, 0, packet.Length);
            networkStream.Close();
            tcpClient.Close();
        }
    }
}
