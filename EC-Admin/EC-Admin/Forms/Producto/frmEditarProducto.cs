﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EC_Admin.Forms
{
    public partial class frmEditarProducto : Form
    {
        Producto p;
        List<int> idPro = new List<int>();
        List<int> idAlm = new List<int>();
        List<int> idCat = new List<int>();
        Unidades u;

        public frmEditarProducto(int id)
        {
            InitializeComponent();
            p = new Producto(id);
        }

        private void CargarProveedores()
        {
            try
            {
                string sql = "SELECT id, nombre FROM proveedor";
                DataTable dt = ConexionBD.EjecutarConsultaSelect(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    idPro.Add((int)dr["id"]);
                    cboProveedor.Items.Add(dr["nombre"]);
                }
            }
            catch (MySqlException ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al cargar los proveedores. No se ha podido conectar con la base de datos. La ventana se cerrará.", "EC-Admin", ex);
                this.Close();
            }
            catch (Exception ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al cargar los proveedores. La ventana se cerrará.", "EC-Admin", ex);
                this.Close();
            }
        }

        private void CargarAlmacenes()
        {
            try
            {
                string sql = "SELECT id, num_alm FROM almacen";
                DataTable dt = ConexionBD.EjecutarConsultaSelect(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    idAlm.Add((int)dr["id"]);
                    cboAlmacen.Items.Add(dr["num_alm"]);
                }
            }
            catch (MySqlException ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al cargar los almacenes. No se ha podido conectar con la base de datos. La ventana se cerrará.", "EC-Admin", ex);
                this.Close();
            }
            catch (Exception ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al cargar los almacenes. La ventana se cerrará.", "EC-Admin", ex);
                this.Close();
            }
        }

        private void CargarCategorias()
        {
            try
            {
                string sql = "SELECT id, nombre FROM categoria";
                DataTable dt = ConexionBD.EjecutarConsultaSelect(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    idCat.Add((int)dr["id"]);
                    cboCategoria.Items.Add(dr["nombre"]);
                }
            }
            catch (MySqlException ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al cargar las categorías. No se ha podido conectar con la base de datos. La ventana se cerrará.", "EC-Admin", ex);
                this.Close();
            }
            catch (Exception ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al cargar las categorías. La ventana se cerrará.", "EC-Admin", ex);
                this.Close();
            }
        }

        private int AsignarComboBox(List<int> l, int id)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private void CargarDatos()
        {
            try
            {
                p.ObtenerDatos();
                cboProveedor.SelectedIndex = AsignarComboBox(idPro, p.IDProveedor);
                cboAlmacen.SelectedIndex = AsignarComboBox(idAlm, p.IDAlmacen);
                cboCategoria.SelectedIndex = AsignarComboBox(idCat, p.IDCategoria);
                txtNombre.Text = p.Nombre;
                txtMarca.Text = p.Marca;
                txtCodigo.Text = p.Codigo;
                txtDescripcion01.Text = p.Descripcion01;
                txtDescripcion02.Text = p.Descripcion02;
                txtCosto.Text = p.Costo.ToString();
                txtPrecio.Text = p.Precio.ToString();
                txtCant.Text = p.Cantidad.ToString();
                txtPrecioMedioMayoreo.Text = p.PrecioMedioMayoreo.ToString();
                txtPrecioMayoreo.Text = p.PrecioMayoreo.ToString();
                txtCantMedioMayoreo.Text = p.CantidadMedioMayoreo.ToString();
                txtCantMayoreo.Text = p.CantidadMayoreo.ToString();
                pcbImagen.Image = p.Imagen;
                switch (p.Unidad)
                {
                    case Unidades.Gramo:
                        cboUnidad.SelectedIndex = 0;
                        break;
                    case Unidades.Kilogramo:
                        cboUnidad.SelectedIndex = 1;
                        break;
                    case Unidades.Mililitro:
                        cboUnidad.SelectedIndex = 2;
                        break;
                    case Unidades.Litro:
                        cboUnidad.SelectedIndex = 3;
                        break;
                    case Unidades.Pieza:
                        cboUnidad.SelectedIndex = 4;
                        break;
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Editar()
        {
            try
            {
                decimal costo, precio, cant, precioMedioMayoreo, precioMayoreo, cantMedioMayoreo, cantMayoreo;
                decimal.TryParse(txtCosto.Text, out costo);
                decimal.TryParse(txtPrecio.Text, out precio);
                decimal.TryParse(txtCant.Text, out cant);
                decimal.TryParse(txtPrecioMedioMayoreo.Text, out precioMedioMayoreo);
                decimal.TryParse(txtPrecioMayoreo.Text, out precioMayoreo);
                decimal.TryParse(txtCantMedioMayoreo.Text, out cantMedioMayoreo);
                decimal.TryParse(txtCantMayoreo.Text, out cantMayoreo);
                p.IDProveedor = idPro[cboProveedor.SelectedIndex];
                p.IDAlmacen = idAlm[cboAlmacen.SelectedIndex];
                p.IDCategoria = idCat[cboCategoria.SelectedIndex];
                p.Nombre = txtNombre.Text;
                p.Marca = txtMarca.Text;
                p.Codigo = txtCodigo.Text;
                p.Descripcion01 = txtDescripcion01.Text;
                p.Descripcion02 = txtDescripcion02.Text;
                p.Costo = costo;
                p.Precio = precio;
                p.Cantidad = cant;
                p.PrecioMedioMayoreo = precioMedioMayoreo;
                p.PrecioMayoreo = precioMayoreo;
                p.CantidadMedioMayoreo = cantMedioMayoreo;
                p.CantidadMayoreo = cantMayoreo;
                p.Unidad = u;
                p.Imagen = pcbImagen.Image;
                p.Editar();
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool VerificarDatos()
        {
            if (cboProveedor.SelectedIndex < 0)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo proveedor es obligatorio", "EC-Admin");
                return false;
            }
            if (cboAlmacen.SelectedIndex < 0)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo almacen es obligatorio", "EC-Admin");
                return false;
            }
            if (cboCategoria.SelectedIndex < 0)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo categoría es obligatorio", "EC-Admin");
                return false;
            }
            if (txtNombre.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo nombre es obligatorio", "EC-Admin");
                return false;
            }
            if (txtMarca.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo marca es obligatorio", "EC-Admin");
                return false;
            }
            if (txtCodigo.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo código es obligatorio", "EC-Admin");
                return false;
            }
            if (txtDescripcion01.Text.Trim() == "" && txtDescripcion02.Text.Trim() != "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El primer campo de descripción se debe ingresar antes que el segundo", "EC-Admin");
                return false;
            }
            if (txtCosto.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo costo es obligatorio", "EC-Admin");
                return false;
            }
            if (txtPrecio.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo precio es obligatorio", "EC-Admin");
                return false;
            }
            if (txtCant.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo cantidad es obligatorio", "EC-Admin");
                return false;
            }
            if (cboUnidad.SelectedIndex < 0)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo unidad es obligatorio", "EC-Admin");
                return false;
            }
            return true;
        }

        private void txtNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            FuncionesGenerales.VerificarEsNumero(ref sender, ref e, false);
        }

        private void pcbImagen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Archivos de imagen (*.jpg, *.jpeg) | *.jpg; *.jpeg";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                ofd.Multiselect = false;
                ofd.Title = "Seleccione la imagen del producto";
                DialogResult r = ofd.ShowDialog(this);
                if (r == System.Windows.Forms.DialogResult.OK)
                {
                    pcbImagen.Image = Bitmap.FromFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al obtener la imagen.", "EC-Admin", ex);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            pcbImagen.Image = null;
        }

        private void frmEditarProducto_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            CargarAlmacenes();
            CargarCategorias();
            CargarDatos();
        }

        private void cboUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboUnidad.SelectedIndex)
            {
                case 0:
                    u = Unidades.Gramo;
                    break;
                case 1:
                    u = Unidades.Kilogramo;
                    break;
                case 2:
                    u = Unidades.Mililitro;
                    break;
                case 3:
                    u = Unidades.Litro;
                    break;
                case 4:
                    u = Unidades.Pieza;
                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (VerificarDatos())
            {
                try
                {
                    Editar();
                    FuncionesGenerales.Mensaje(this, Mensajes.Exito, "¡Se ha modificado el producto correctamente!", "EC-Admin");
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al modificar el producto. No se ha podido conectar con la base de datos.", "EC-Admin", ex);
                }
                catch (Exception ex)
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al modificar el producto.", "EC-Admin", ex);
                }
            }
        }

    }
}