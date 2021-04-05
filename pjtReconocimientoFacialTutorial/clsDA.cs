using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace pjtReconocimientoFacialTutorial
{
    public class clsDA
    {
        //private static OleDbConnection cnx = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source =DBRostros.accdb;");
        private static SqlConnection cnx = new SqlConnection("Data Source=localhost;Initial Catalog=DBRostros;Persist Security Info=True;User ID=sa;Password=linKmobiL1");

        public static string[] Nombre;
        private static byte[] Rostro;
        public static List<byte[]> ListadoRostros = new List<byte[]>();
        public static int TotalRostros;



        public static bool GuardarImagen(string Nombre, byte[] Imagen)
        {
            cnx.Close();
            cnx.Open();
            //OleDbCommand cmd = new OleDbCommand("INSERT INTO Rostros (Nombre, Imagen) Values ('" + Nombre + "',?);", cnx);
            //OleDbParameter parImagen = new OleDbParameter("@Imagen", OleDbType.VarBinary, Imagen.Length);
            //parImagen.Value = Imagen;
            //cmd.Parameters.Add(parImagen);
            //int Resultado = cmd.ExecuteNonQuery();

            //SqlCommand cmd = new SqlCommand("insert into Persona values (@ccPersona,@nombrePersona,@apellidoPersona,@fechaNacimiento,@foto)", con);
            SqlCommand cmd = new SqlCommand("INSERT INTO Rostros Values (@nombre, @imagen);", cnx);
            cmd.Parameters.AddWithValue("@nombre", Nombre);
            // var image = Convert.ToBase64String(Imagen);
            cmd.Parameters.AddWithValue("@imagen", Imagen);

            var resultado = cmd.ExecuteNonQuery();
            cnx.Close();

            return Convert.ToBoolean(resultado);




        }


        public static DataTable Consultar(DataGridView DATA)
        {
            cnx.Open();
            //OleDbCommand cmd = new OleDbCommand("SELECT * FROM Rostros;", cnx);
            //OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Rostros;", cnx);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DATA.DataSource = dt;
            int Cont = dt.Rows.Count;
            Nombre = new string[Cont];
            Rostro = new byte[Cont];
            cnx.Close();
            for (int i = 0; i < Cont; i++)
            {
                Nombre[i] = dt.Rows[i]["Nombre"].ToString();
                //var result = dt.Rows[0]["Imagen"];
                Rostro = (byte[])dt.Rows[i]["Imagen"];
                //Rostro = Convert.FromBase64String(result.ToString());
                ListadoRostros.Add(Rostro);

            }


            try
            {
                DATA.Columns[0].Width = 60;
                DATA.Columns[1].Width = 160;
                DATA.Columns[2].Width = 160;

                for (int i = 0; i < Cont; i++)
                {

                    DATA.Rows[i].Height = 110;
                }
            }
            catch
            {

            }

            TotalRostros = Cont;

            return dt;

        }


        ////

        public static byte[] ConvertImgToBinary(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            MemoryStream Memoria = new MemoryStream();
            bmp.Save(Memoria, ImageFormat.Bmp);

            byte[] imagen = Memoria.ToArray();

            return imagen;/// arreglo de Binario de la imagen

        }

        public static Image ConvertBinaryToImg(int C)
        {
            Image Imagen;
            byte[] img = ListadoRostros[C];

             var image = Convert.ToBase64String(img);
            MemoryStream Memoria = new MemoryStream(img);
            Imagen = Image.FromStream(Memoria);
            Memoria.Close();
            return Imagen;
        }
    }
}
