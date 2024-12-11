using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Variables globales para el máximo y mínimo saldo
        decimal maxSaldo = decimal.MinValue;
        decimal minSaldo = decimal.MaxValue;
        string maxId = null;
        string minId = null;
        string maxName = null;
        string minName = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ruta al archivo TXT
            string filePath = Path.Combine(Application.StartupPath, "data.txt");

            // Verificar que el archivo exista
            if (!File.Exists(filePath))
            {
                MessageBox.Show("El archivo no existe. Por favor, verifica la ruta.");
                return;
            }

            // Limpiar el ListBox
            listBox1.Items.Clear();

            // Variables de corte de control
            string idActual = null; // ID actual que estamos procesando
            decimal saldoAcumulado = 0; // Saldo acumulado para el ID actual

            // Leer archivo línea por línea
            foreach (string linea in File.ReadAllLines(filePath))
            {
                // Dividir los campos de la línea
                string[] campos = linea.Split(';');
                if (campos.Length != 4) continue;

                string id = campos[0];
                string nombre = campos[1];
                string operacion = campos[2].ToLower();
                decimal monto;

                // Validar que el monto sea numérico
                if (!decimal.TryParse(campos[3], out monto)) continue;

                // Corte de control: si el ID cambia, mostramos el saldo y reiniciamos acumulador
                if (idActual != null && id != idActual)
                {
                    listBox1.Items.Add($"ID: {idActual}, {nombre}, Saldo: {saldoAcumulado}");
                    
                    // Actualizar el máximo y mínimo
                    if (saldoAcumulado > maxSaldo)
                    {
                        maxSaldo = saldoAcumulado;
                        maxName = nombre;
                        maxId = idActual;
                    }
                    if (saldoAcumulado < minSaldo)
                    {
                        minSaldo = saldoAcumulado;
                        minName = nombre;
                        minId = idActual;
                    }

                    saldoAcumulado = 0; // Reiniciamos acumulador
                }

                // Actualizamos ID actual
                idActual = id;

                // Sumar o restar el monto al saldo acumulado
                if (operacion == "suma")
                    saldoAcumulado += monto;
                else if (operacion == "resta")
                    saldoAcumulado -= monto;
            }

            // Mostrar el último saldo acumulado (para el último ID del archivo)
            if (idActual != null)
            {
                listBox1.Items.Add($"ID: {idActual}, Saldo: {saldoAcumulado}");

                // Actualizar el máximo y mínimo
                if (saldoAcumulado > maxSaldo)
                {
                    maxSaldo = saldoAcumulado;
                    maxId = idActual;
                }
                if (saldoAcumulado < minSaldo)
                {
                    minSaldo = saldoAcumulado;
                    minId = idActual;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (maxId != null)
            {
                MessageBox.Show($"El usuario con el máximo saldo es ID: {maxId}, Nombre: {maxName}, Saldo: {maxSaldo}");
            }
            else
            {
                MessageBox.Show("No se ha procesado ningún saldo.");
            }
        }

        private void buttonMin_Click(object sender, EventArgs e)
        {
            if (minId != null)
            {
                MessageBox.Show($"El usuario con el mínimo saldo es ID: {minId}, Nombre: {minName}, Saldo: {minSaldo}");
            }
            else
            {
                MessageBox.Show("No se ha procesado ningún saldo.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}