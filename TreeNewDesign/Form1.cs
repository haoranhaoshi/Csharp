using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {  
            ImageList imageList = new ImageList();
            try
            {
                if (Image.FromFile(@"..\..\Resources\tree_NodeCollaps.png") != null)
                {
                    imageList.Images.Add(Image.FromFile(@"..\..\Resources\tree_NodeCollaps.png"));
                }
            }
            catch (Exception efdsfdsf)
            {
            } 
			//imageList.Images.Add(Image.FromFile(@"..\..\Resources\tree_NodeExpend.png")); 
			//imageList.Images.Add(Image.FromFile(@"..\..\Resources\tree_NodeExpend.png"));
			//imageList.Images.Add(Image.FromFile(@"..\..\Resources\A.ico"));
            //imageList.Images.Add(Image.FromFile(@"..\..\Resources\B.ico"));
            //imageList.Images.Add(Image.FromFile(@"..\..\Resources\C.ico"));
            treeView1.IsToggleIcon = true;
            treeView1.ImageList = imageList;

        }
    }
}
