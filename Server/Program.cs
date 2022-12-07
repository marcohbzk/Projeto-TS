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
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Server
{
    static class Vars
    {
       public static List<ClientHandler> Clientes = new List<ClientHandler>();
    }
    internal class Program
    {
        private const int PORT = 10000;
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
            TcpListener tcplistener = new TcpListener(endPoint);
            tcplistener.Start();

            int clientCounter = 0;

            Console.WriteLine("Server Ready");


            while (true)
            {

                TcpClient client = tcplistener.AcceptTcpClient();
                clientCounter++;
                Console.WriteLine("\nClient {0} connected" + "\n", clientCounter);
                string user = "user";

                ClientHandler clientHandler = new ClientHandler(client, clientCounter, user);
                clientHandler.Handle();

            }
        }
    }

    

    // Criar Clientes

    class ClientHandler
    {
        private TcpClient client;
        private int clientID;
        private string clientUser;
        private RSACryptoServiceProvider rsa;
        AesCryptoServiceProvider aes;


        private const int SALTSIZE = 8;
        private const int NUMBER_OF_ITERATIONS = 50000;

        public ClientHandler(TcpClient client, int clientID, string username)
        {
            this.client = client;
            this.clientID = clientID;
            this.clientUser = username;
            Vars.Clientes.Add(this);
            aes = new AesCryptoServiceProvider();
        }

        public void Handle()
        {
            Thread thread = new Thread(threadhandler);
            thread.Start();
        }
        public void msgSender(string msg)
        {
            ProtocolSI protocolSI = new ProtocolSI();
            foreach (ClientHandler client in Vars.Clientes)
            {
                NetworkStream net = client.client.GetStream();
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);

                net.Write(packet, 0, packet.Length);
            }
        }
        private string hashConvert(string text)
        {
            //Cria um SHA256
            SHA256 sha256 = SHA256.Create();
            //Retorna uma array de bytes a partir da password
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

            //Converte a array numa string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
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
        public void threadhandler()
        {
            NetworkStream networkStream = this.client.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();
            string[] user;
            byte[] packet;
            bool respo;
            rsa = new RSACryptoServiceProvider();

            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            string teste = protocolSI.GetStringFromData();
            rsa.FromXmlString(protocolSI.GetStringFromData());

            packet = protocolSI.Make(ProtocolSICmdType.ACK);

            networkStream.Write(packet, 0, packet.Length);

            string batata = "batata";

            aes.Key = GerarChavePrivada(batata);
            aes.IV = GerarIv(batata);

            byte[] dadosEnc = rsa.Encrypt(Encoding.ASCII.GetBytes(batata), true);

            packet = protocolSI.Make(ProtocolSICmdType.SECRET_KEY, dadosEnc);

            networkStream.Write(packet, 0, packet.Length);

            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }

            while (protocolSI.GetCmdType() != ProtocolSICmdType.EOT)
            {
                int bytesRead = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                switch (protocolSI.GetCmdType())
                {
                    case ProtocolSICmdType.DATA:
                        string getString = "Client " + clientID + ": " + protocolSI.GetStringFromData();
                        Console.WriteLine(getString);
                        msgSender(getString);
                        string textAdd = getString;
                        string texto = " Conversação entre clientes ";
                        // LOCALIZAÇÃO DO TXT
                        string path = @"log.txt";
                        // LIMPA O TXT E ESCREVE A STRING COM UMA NOVA LINHA NO FIM, COM CODIFICAÇÃO UTF8
                        File.WriteAllText(path, texto + textAdd + Environment.NewLine, Encoding.UTF8);
                        // ADICIONA AO TXT A STRING COM UMA NOVA LINHA NO FIM, COM CODIFICAÇÃO UTF8
                        File.AppendAllText(path, texto + textAdd + Environment.NewLine, Encoding.UTF8);
                        break;

                    case ProtocolSICmdType.USER_OPTION_1:
                        string getLogin = "Client " + clientID + " Tentou iniciar a sessão...";
                        string reqData = protocolSI.GetStringFromData();
                        user = JsonConvert.DeserializeObject<string[]>(reqData);
                        respo = VerifyLogin(user[0], user[1]);

                        packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, respo.ToString());
                        networkStream.Write(packet, 0, packet.Length);

                        Console.WriteLine(getLogin, respo);
                        break;

                    case ProtocolSICmdType.USER_OPTION_2:
                        string getRegister = "Client " + clientID + " Tentou iniciar o registo...";
                        reqData = protocolSI.GetStringFromData();
                        user = JsonConvert.DeserializeObject<string[]>(reqData);
                        
                        byte[] salt = GenerateSalt(SALTSIZE);

                        byte[] hash = GenerateSaltedHash(user[1], salt);
                        
                        respo = Register(user[0], hash, salt);

                        packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, respo.ToString());
                        networkStream.Write(packet, 0, packet.Length);

                        Console.WriteLine(getRegister, respo);
                        break;
                }
            }

            packet = protocolSI.Make(ProtocolSICmdType.ACK);
            networkStream.Write(packet, 0, packet.Length);
            this.clientID -= 1;
            networkStream.Close();
            client.Close();
        }
        private bool Register(string username, byte[] saltedPasswordHash, byte[] salt)
        {
            SqlConnection conn = null;
            try
            {

                // Configurar ligação à Base de Dados
                conn = new SqlConnection();

                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\USERS\MARKM\DESKTOP\ESCOLA\SEMESTRE2\TOPICOS SEGURANCA\PROJETO_TS\SERVER\TSDATABASE.MDF';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração dos parâmetros do comando SQL
                SqlParameter paramUsername = new SqlParameter("@username", username);
                SqlParameter paramPassHash = new SqlParameter("@saltedPasswordHash", saltedPasswordHash);
                SqlParameter paramSalt = new SqlParameter("@salt", salt);

                // Declaração do comando SQL
                String sql = "INSERT INTO Users (Username, SaltedPasswordHash, Salt) VALUES (@username,@saltedPasswordHash,@salt)";

                // Prepara comando SQL para ser executado na Base de Dados
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Introduzir valores aos parâmentros registados no comando SQL
                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassHash);
                cmd.Parameters.Add(paramSalt);

                // Executar comando SQL
                int lines = cmd.ExecuteNonQuery();

                // Fechar ligação
                conn.Close();
                if (lines == 0)
                {
                    // Se forem devolvidas 0 linhas alteradas então o não foi executado com sucesso
                    return false;
                }
                Console.WriteLine("Registado com sucesso!");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private bool VerifyLogin(string username, string password)
        {
            SqlConnection conn = null;

            try
            {
                // Configurar ligação à Base de Dados
                conn = new SqlConnection();

                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\USERS\MARKM\DESKTOP\ESCOLA\SEMESTRE2\TOPICOS SEGURANCA\PROJETO_TS\SERVER\TSDATABASE.MDF';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração do comando SQL
                String sql = "SELECT * FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;

                // Declaração dos parâmetros do comando SQL
                SqlParameter param = new SqlParameter("@username", username);

                // Introduzir valor ao parâmentro registado no comando SQL
                cmd.Parameters.Add(param);

                // Associar ligação à Base de Dados ao comando a ser executado
                cmd.Connection = conn;

                // Executar comando SQL
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    return false;
                }

                // Ler resultado da pesquisa
                reader.Read();

                // Obter Hash (password + salt)
                byte[] saltedPasswordHashStored = (byte[])reader["SaltedPasswordHash"];

                // Obter salt
                byte[] saltStored = (byte[])reader["Salt"];

                conn.Close();

                byte[] hash = GenerateSaltedHash(password, saltStored);

                return saltedPasswordHashStored.SequenceEqual(hash);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return false;
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
        private string DecifrarTexto(string txtCifradoB64, AesCryptoServiceProvider aes)
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
        private static byte[] GenerateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
        }

        private static byte[] GenerateSaltedHash(string plainText, byte[] salt)
        {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(plainText, salt, NUMBER_OF_ITERATIONS);
            return rfc2898.GetBytes(32);
        }
    }
}
