using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SignusMobile20.Droid;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly:Dependency(typeof(SQLite_Doid))]
namespace SignusMobile20.Droid
{
    public class SQLite_Doid : ISQLite
    {
        //Este código foi gerado com base na fonte:(1)
        //SQLiteConnection conn;//Comentar este código caso algo dê errado
        private SQLiteConnection conn;
        public SQLiteConnection GetConnection()
        {
            var dbName = "dbDadosCampo.sqlite";
            if (conn == null)
            {
                string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                var path = System.IO.Path.Combine(dbPath, dbName);//de acordo com a fonte (1), ...Combine(dbName, dbPath); 
                conn = new SQLiteConnection(path);
            }

            return conn;
        }

/*
        public bool SavePreparoSoloOP(PrepSoloOP prepsoloop)
        {
            bool res = false;
            try
            {
                //conn.Insert(prepsoloop);//Comentar este código caso algo dê errado
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;

        }

        public bool SavePreparoSoloCQ(PrepSoloCQ prepsolocq)
        {
            bool res = false;
            try
            {
                //conn.Insert(prepsolocq);//Comentar este código caso algo dê errado
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;

        }

        public bool UpdatePreparoSoloOP(PrepSoloOP prepsoloop)
        {
            bool res = false;
            try
            {
                //Comentar esta parte do código caso dê errado
                //string sql = $"UPDATE PrepSoloOP SET IdentifOperacao = '{prepsoloop.IdentifOperacao}', ProcedOperac = '{prepsoloop.ProcedOperac}'," +
                //    $"Equipamento = '{prepsoloop.Equipamento}' WHERE PKPrepSoloOP = {prepsoloop.PKPrepSoloOP}";
                //conn.Execute(sql);
                //res = true;
            }
            catch
            {

            }
            return res;
        }

        public bool UpdatePreparoSoloCQ(PrepSoloCQ prepsolocq)
        {
            bool res = false;
            try
            {
                //Comentar esta parte do código caso dê errado
                //string sql = $"UPDATE PrepSoloOP SET IdentifOperacao = '{prepsoloop.IdentifOperacao}', ProcedOperac = '{prepsoloop.ProcedOperac}'," +
                //    $"Equipamento = '{prepsoloop.Equipamento}' WHERE PKPrepSoloCQ = {prepsoloop.PKPrepSoloCQ}";
                //conn.Execute(sql);
                //res = true;
            }
            catch
            {

            }
            return res;
        }
        public void DeletePreparoSoloOP(int Id)
        {
            //Comentar este código caso algo dê errado
            string sql = $"DELETE FROM PrepSoloOP WHERE PKPrepSoloOP={Id}";
            //conn.Execute(sql);
        }

        public void DeletePreparoSoloCQ(int Id)
        {
            //Comentar este código caso algo dê errado
            string sql = $"DELETE FROM PrepSoloCQ WHERE PKPrepSoloCQ={Id}";
            //conn.Execute(sql);
        }
*/

    }
}