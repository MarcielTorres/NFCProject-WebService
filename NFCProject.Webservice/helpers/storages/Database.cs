using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;

//-------------------------------------------------------------------------------------
// Database
// Descrição: Classe do objeto Database para conexão com banco de dados
// Autor: Marciel Torres
// Data: 07/07/2014
// Propriedades:
// -> SqlConnection(private): Conexão com banco de dados MSSQLSERVER
// -> connectionString(private): string de conexão com banco de dados
// -> cmd(get / private set): Objeto DbCommand.
// -> SqlStat(get / private set): String com comando SQL para execução
// -> DataReader(get / private set): Objeto DbDataReader
// -> parameters(get / private set): Dicionário com os parâmetros para comando SQL
// -> serverName(get / private set): Servidor que está executando a aplicação (Localhost, etc)
// -> erro(get / private set): Exception lançada pela aplicação
// -> Obs(get / private set): Informações sobre as operações que estão sendo executadas.(geralmente erros amigáveis)
// Métodos:	
// -> Open: Abre conexão com banco de dados.
// -> Close: Fecha conexão com banco de dados.
// -> CreateParameter: Cria parâmetros para consulta SQL.
// -> ClearParameters: Limpa parâmetros para consulta SQL.
// -> ExecuteDataReader: Executa determinada consulta e atribui valores para o dataReader da classe.
// -> ExecuteNonQuery: Executa determinada consulta.
// -> GenerateID: Gera ID aleatório com relação a tabela já existente.
// Observações
// -> Classe otimizada exclusivamente para utilização da base MSSQLServer.
//--------------------------------------------------------------------------------------
namespace NFCProject.Webservice.helpers.storages
{
    public class Database : IDisposable
    {
        //***************
        //Construtores
        //***************

        //Construtor sem parâmetros
        public Database()
        {
            //Define os dados básicos para a classe de conexão com banco de dados
            this.DefineDataDefault();
        }

        //Construtor com partâmetro de string de conexão
        public Database(string strConn)
        {
            //Define os dados básicos para a classe de conexão com banco de dados
            this.connectionString = strConn;
            this.DefineDataDefault();
        }

        //************************************************
        //Declaração das Propriedades (get/set)
        //************************************************

        //Propriedades Privadas
        private SqlConnection sqlConn { get; set; }
        private String connectionString { get; set; }

        //Propriedades Públicas (set privado)
        public DbCommand cmd { get; private set; }
        public StringBuilder SqlStat { get; set; }
        public DbDataReader DataReader { get; private set; }
        public List<SqlParameter> parameters { get; private set; }
        public Exception erro { get; private set; }
        public String Obs { get; private set; }

        //*****************
        //MÉTODOS PÚBLICOS
        //*****************

        //Abre conexão com banco de dados
        public void Open()
        {
            if (this.sqlConn.State == System.Data.ConnectionState.Closed)
            {
                this.sqlConn.Open();
            }
        }

        //Fecha conexão com banco de dados
        public void Close()
        {
            if (this.cmd != null) { this.cmd.Dispose(); }
            if (this.sqlConn != null) { this.sqlConn.Close(); }
            if (this.DataReader != null) { this.DataReader.Dispose(); }
        }

        //Cria parâmetros para consulta
        public void CreateParameter(string param, object valor)
        {
            this.parameters.Add(new SqlParameter(param, valor == null ? DBNull.Value : valor));
        }
        //remove todos parâmetros
        public void ClearParameters()
        {
            this.parameters.Clear();
        }

        //Executa determinada consulta e atribui valores para o dataReader da classe
        public Boolean ExecuteDataReader()
        {
            bool resp = false;
            this.DataReader = null;

            try
            {
                this.Open();
                this.cmd.Parameters.Clear();
                this.cmd.CommandText = this.SqlStat.ToString();
                if (this.parameters.Count > 0)
                {
                    this.cmd.Parameters.AddRange(this.parameters.ToArray<SqlParameter>());
                }
                this.DataReader = this.cmd.ExecuteReader();

                resp = true;
            }
            catch (Exception ex)
            {
                this.erro = ex;
                this.Obs = "Erro ao executar consulta com leitura de dados";
                resp = false;
                this.Close();
            }
            return resp;
        }

        //Executa determinada consulta
        public Boolean ExecuteNonQuery()
        {
            bool resp = false;
            this.DataReader = null;

            try
            {
                this.Open(); //Abre conexão
                this.cmd.Parameters.Clear();
                this.cmd.CommandText = this.SqlStat.ToString();
                if (this.parameters.Count > 0)
                {
                    this.cmd.Parameters.AddRange(this.parameters.ToArray<SqlParameter>());
                }
                this.cmd.ExecuteNonQuery();

                resp = true;
            }
            catch (Exception ex)
            {
                this.erro = ex;
                this.Obs = "Erro ao executar consulta";
                resp = false;
            }
            finally
            {
                this.Close();
            }
            return resp;
        }

        //Executa determinada consulta
        public object ExecuteScalar()
        {
            object resp = false;
            this.DataReader = null;

            try
            {
                this.Open(); //Abre conexão
                this.cmd.Parameters.Clear();
                this.cmd.CommandText = this.SqlStat.ToString();
                if (this.parameters.Count > 0)
                {
                    this.cmd.Parameters.AddRange(this.parameters.ToArray<SqlParameter>());
                }
                resp = this.cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                this.erro = ex;
                this.Obs = "Erro ao executar consulta";
                resp = null;
            }
            finally
            {
                this.Close();
            }
            return resp;
        }

        //Função para retornar um ID aleatório com relação a tabela já existente
        public string GenerateID(string table, string prefix)
        {
            int i = 0;
            bool codOk = false;
            string resp = string.Empty;
            DbCommand _cmd = this.sqlConn.CreateCommand();
            _cmd.CommandText = string.Format("SELECT COUNT(1) FROM {0} WHERE ID = @ID", table);

            //Prepara dados
            if (String.IsNullOrEmpty(prefix)) { prefix = ""; }

            while (!codOk && i < 5)
            {
                i++;
                resp = prefix + this.GenerateCode();
                _cmd.Parameters.Clear();
                _cmd.Parameters.Add(new SqlParameter("@ID", resp));
                try
                {
                    this.Open();
                    if (Convert.ToInt32(_cmd.ExecuteScalar()) == 0)
                        codOk = true;
                }
                catch (Exception e)
                {
                    codOk = false;

                    this.erro = e;
                    this.Obs = "Erro ao definir código aleatório para o registro";
                }
            }

            //libera variáveis
            _cmd.Parameters.Clear();
            _cmd.Dispose();
            this.sqlConn.Close();

            return resp;
        }

        //libera recursos
        public void Dispose()
        {
            //Liberar recursos
            this.Close();
            GC.SuppressFinalize(this);
        }

        //*****************************
        //Funções privadas da classe
        //*****************************

        //Função para definir os dados padrões para classe
        private void DefineDataDefault()
        {

            //Cria String para consulta e também os parâmetros
            this.SqlStat = new StringBuilder();

            try
            {
                this.DefineConnectionString();

                //cria conexão
                this.sqlConn = new SqlConnection(this.connectionString);

                //Cria Command
                this.cmd = this.sqlConn.CreateCommand();

                //Cria objeto para lista de parâmetros
                this.parameters = new List<SqlParameter>();
            }
            catch (Exception ex)
            {
                this.Obs = "Erro ao definir string de conexão com banco de dados";
                this.erro = ex;
            }
        }

        //função para definir a string de conexão com base na variável de sessão criada no global.asax
        private void DefineConnectionString()
        {
            string _strconn = this.connectionString;
            if (String.IsNullOrEmpty(_strconn)) { _strconn = "strConMain"; }

            //define string de conexão
            try
            {
                this.connectionString = ConfigurationManager.ConnectionStrings[_strconn].ConnectionString;
            }
            catch (Exception ex)
            {
                this.erro = ex;
                this.Obs = "Erro ao definir string de conexão com banco de dados";
            }
        }

        //gera string aleatória de 9 caracteres
        private string GenerateCode()
        {
            string resp = string.Empty;
            Random rnd = new Random();
            resp = "000000000" + rnd.Next(999999999).ToString();
            resp = resp.Substring(resp.Length - 9);
            return resp;
        }
    }
}
