using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlmNet;
using Tao.OpenGl;
using System.Drawing;
using System.IO;


namespace Graphics
{
    public partial class GraphicsForm : Form
    {
        Renderer renderer = new Renderer();
        Thread MainLoopThread;
        List<Vertex> VertexList;
        List<Model> ModelList;
        String txt;
        int vertIndex;
        int modelIndex;

        int selected_index;
        int Model_selected_index;

        List<String> MyTextures;

        List<Vertex> Drawlist = new List<Vertex>();


        public GraphicsForm()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            initialize();
            MainLoopThread = new Thread(MainLoop);
            MainLoopThread.Start();

        }
        void initialize()
        {
            VertexList = new List<Vertex>();
            ModelList = new List<Model>();
            MyTextures = new List<String>();
            vertIndex = 0;
            modelIndex = 0;
            txt = "";
            selected_index = 0;
            Model_selected_index = 0;


            renderer.Initialize(VertexList, MyTextures);

        }

        void MainLoop()
        {
            while (true)
            {
                renderer.Draw(ModelList);
                simpleOpenGlControl1.Refresh();
            }
        }
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
            MainLoopThread.Abort();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw(ModelList);
        }

        private void tabPage1_Click(object sender, System.EventArgs e)
        {

        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void GraphicsForm_Load(object sender, System.EventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            vec3 pos = new vec3(float.Parse(PosX.Text), float.Parse(PosY.Text), float.Parse(PosZ.Text));
            vec3 col = new vec3(float.Parse(red.Text), float.Parse(green.Text), float.Parse(blue.Text));
            vec3 Normal = new vec3(float.Parse(NormX.Text), float.Parse(NormY.Text), float.Parse(NormZ.Text));
            vec2 UVData = new vec2(float.Parse(U_Data.Text), float.Parse(V_Data.Text));

            Vertex MyVert = new Vertex(pos, col, Normal, UVData, vertIndex);

            listVertex.Items.Add('V' + vertIndex.ToString());

            vertIndex++;

            VertexList.Add(MyVert);
            Drawlist.Add(MyVert);

            txt = "";
            for (int i = 0; i < VertexList.Count(); i++)
            {
                if (i == 0)
                {
                    txt += VertexList.ElementAt(i).index.ToString();
                }
                else
                {
                    txt += "," + VertexList.ElementAt(i).index.ToString();
                }
            }
            idxVerts.Text = txt;

            PosX.Text = "0.0";
            PosY.Text = "0.0";
            PosZ.Text = "0.0";
            red.Text = "0.0";
            green.Text = "0.0";
            blue.Text = "0.0";
            NormX.Text = "0.0";
            NormY.Text = "0.0";
            NormZ.Text = "0.0";
            U_Data.Text = "0.0";
            V_Data.Text = "0.0";
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            int index = selected_index;

            //Vertex MyVert = VertexList.ElementAt(index);
            vec3 pos = new vec3(float.Parse(PosX.Text), float.Parse(PosY.Text), float.Parse(PosZ.Text));
            vec3 col = new vec3(float.Parse(red.Text), float.Parse(green.Text), float.Parse(blue.Text));
            vec3 Normal = new vec3(float.Parse(NormX.Text), float.Parse(NormY.Text), float.Parse(NormZ.Text));
            vec2 UVData = new vec2(float.Parse(U_Data.Text), float.Parse(V_Data.Text));

            VertexList.ElementAt(index).Update_Vertex(pos, col, Normal, UVData);
            Drawlist.ElementAt(index).Update_Vertex(pos, col, Normal, UVData);

            PosX.Text = "0.0";
            PosY.Text = "0.0";
            PosZ.Text = "0.0";
            red.Text = "0.0";
            green.Text = "0.0";
            blue.Text = "0.0";
            NormX.Text = "0.0";
            NormY.Text = "0.0";
            NormZ.Text = "0.0";
            U_Data.Text = "0.0";
            V_Data.Text = "0.0";

        }

        private void listVertex_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listVertex.Items.Count; i++)
            {
                if (listVertex.Items[i].Selected)
                {
                    selected_index = i;
                    break;
                }
            }

            Vertex MyVert = VertexList.ElementAt(selected_index);
            PosX.Text = MyVert.coor.x.ToString();
            PosY.Text = MyVert.coor.y.ToString();
            PosZ.Text = MyVert.coor.z.ToString();
            red.Text = MyVert.color.x.ToString();
            green.Text = MyVert.color.y.ToString();
            blue.Text = MyVert.color.z.ToString();
            NormX.Text = MyVert.normal.x.ToString();
            NormY.Text = MyVert.normal.y.ToString();
            NormZ.Text = MyVert.normal.z.ToString();
            U_Data.Text = MyVert.UV_data.x.ToString();
            V_Data.Text = MyVert.UV_data.y.ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {

            int index = selected_index;

            VertexList.RemoveAt(index);
            Drawlist.RemoveAt(index);
            listVertex.Items.RemoveAt(index);

            txt = "";
            int max = 0;
            for (int i = 0; i < VertexList.Count(); i++)
            {
                int idx = VertexList.ElementAt(i).index;
                if (i == 0)
                {
                    if (idx >= index)
                    {
                        idx--;
                        VertexList.ElementAt(i).index--;
                    }
                    txt += idx.ToString();
                }
                else
                {
                    if (idx >= index)
                    {
                        idx--;
                        VertexList.ElementAt(i).index--;
                    }
                    txt += "," + idx.ToString();
                }

                if (idx > max)
                {
                    max = idx;
                }
            }
            idxVerts.Text = txt;

            vertIndex = max + 1;

            PosX.Text = "0.0";
            PosY.Text = "0.0";
            PosZ.Text = "0.0";
            red.Text = "0.0";
            green.Text = "0.0";
            blue.Text = "0.0";
            NormX.Text = "0.0";
            NormY.Text = "0.0";
            NormZ.Text = "0.0";
            U_Data.Text = "0.0";
            V_Data.Text = "0.0";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String txt = idxVerts.Text;
            String[] vertices = txt.Split(',');
            Drawlist.Clear();
            for (int i = 0; i < VertexList.Count ; i++)
            {
                int x = int.Parse(vertices[i]);
                Drawlist.Add(VertexList[x]);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            String MName = ModelName.Text;
            int start_idx = int.Parse(ModelStart.Text);
            int count = int.Parse(ModelCount.Text);

            Model MyModel = new Model(MName, modelIndex, start_idx, count);

            ModelList.Add(MyModel);
            listModel.Items.Add(MName + modelIndex.ToString());

            modelIndex++;

            ModelName.Text = "Point";
            ModelStart.Text = "0";
            ModelCount.Text = "0";

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int index = Model_selected_index;
            ModelList.ElementAt(index).name = ModelName.Text;
            ModelList.ElementAt(index).V_start_idx = int.Parse(ModelStart.Text);
            ModelList.ElementAt(index).V_count = int.Parse(ModelCount.Text);

            listModel.Items[Model_selected_index].Text = ModelList.ElementAt(index).name + ModelList.ElementAt(index).index.ToString();

            ModelName.Text = "Point";
            ModelStart.Text = "0";
            ModelCount.Text = "0";
        }

        private void listModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listModel.Items.Count; i++)
            {
                if (listModel.Items[i].Selected)
                {
                    Model_selected_index = i;
                    break;
                }
            }

            ModelName.Text = ModelList.ElementAt(Model_selected_index).name;
            ModelStart.Text = ModelList.ElementAt(Model_selected_index).V_start_idx.ToString();
            ModelCount.Text = ModelList.ElementAt(Model_selected_index).V_count.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int index = Model_selected_index;

            listModel.Items.RemoveAt(index);
            ModelList.RemoveAt(index);

            ModelName.Text = "Point";
            ModelStart.Text = "0";
            ModelCount.Text = "0";
        }

        //Draw Button
        private void button8_Click(object sender, EventArgs e)
        {
            renderer.Initialize(Drawlist, MyTextures);
            renderer.Draw(ModelList);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Choose an image to set as texture";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

            }
            String Path = openFileDialog1.FileName;
            Pathtxt.Text = openFileDialog1.FileName;
            MyTextures.Add(Path);
            pictureBox1.ImageLocation = Path;
            //pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            String Path = FilePathEx.Text + "\\" + TxtName.Text;
            FileStream FS = new FileStream(Path + ".txt", FileMode.Create);
            StreamWriter SW = new StreamWriter(FS);
            for (int i = 0; i < VertexList.Count; i++)
            {
                SW.Write(VertexList[i].index.ToString());
                SW.Write("&");

                SW.Write(VertexList[i].coor.x.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].coor.y.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].coor.z.ToString());
                SW.Write("&");

                SW.Write(VertexList[i].color.x.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].color.y.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].color.z.ToString());
                SW.Write("&");

                SW.Write(VertexList[i].normal.x.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].normal.y.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].normal.z.ToString());
                SW.Write("&");

                SW.Write(VertexList[i].UV_data.x.ToString());
                SW.Write("&");
                SW.Write(VertexList[i].UV_data.y.ToString());

                SW.Write("*");
            }
            SW.Close();
            FS.Close();

            FS = new FileStream(Path + "-Shapes.txt", FileMode.Create);
            SW = new StreamWriter(FS);
            for (int i = 0; i < ModelList.Count; i++)
            {
                SW.Write(ModelList[i].index.ToString());
                SW.Write("&");
                SW.Write(ModelList[i].name);
                SW.Write("&");
                SW.Write(ModelList[i].V_start_idx.ToString());
                SW.Write("&");
                SW.Write(ModelList[i].V_count.ToString());

                SW.Write("*");
            }
            SW.Close();
            FS.Close();

            FS = new FileStream(Path + "-Textures.txt", FileMode.Create);
            SW = new StreamWriter(FS);
            for (int i = 0; i < MyTextures.Count; i++)
            {
                SW.Write(MyTextures[i]);
                SW.Write("*");
            }
            SW.Close();
            FS.Close();

            FilePathEx.Text = "";
            TxtName.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select file to export to";
            DialogResult result = folderBrowserDialog1.ShowDialog();

            String Path = folderBrowserDialog1.SelectedPath;
            FilePathEx.Text = Path;
            TxtName.Text = "Model";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select file that contains the 3 text files";
            DialogResult result = folderBrowserDialog1.ShowDialog();

            String Path = folderBrowserDialog1.SelectedPath;
            filePathIm.Text = Path;
            TxtName2.Text = "Model";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listModel.Clear(); //
            ModelList.Clear(); //
            listVertex.Clear(); //
            VertexList.Clear(); //
            MyTextures.Clear();
            idxVerts.Clear(); //
            String txt = ""; //
            vertIndex = 0; //
            modelIndex = 0; //

            String Path = filePathIm.Text + "\\" + TxtName2.Text + ".txt";
            FileStream FS = new FileStream(Path, FileMode.Open);
            StreamReader SR = new StreamReader(FS);
            while (SR.Peek() != -1)
            {
                String s1 = SR.ReadLine();
                String[] s2 = s1.Split('*');
                int x = s2.GetLength(0);
                for (int i = 0; i < x - 1; i++)
                {
                    String[] s3 = s2[i].Split('&');
                    int index = int.Parse(s3[0]);
                    vec3 coor = new vec3(float.Parse(s3[1]), float.Parse(s3[2]), float.Parse(s3[3]));
                    vec3 color = new vec3(float.Parse(s3[4]), float.Parse(s3[5]), float.Parse(s3[6]));
                    vec3 norm = new vec3(float.Parse(s3[7]), float.Parse(s3[8]), float.Parse(s3[9]));
                    vec2 UVData = new vec2(float.Parse(s3[10]), float.Parse(s3[11]));

                    Vertex MyVert = new Vertex(coor, color, norm, UVData, index);

                    VertexList.Add(MyVert);
                    Drawlist.Add(MyVert);
                    listVertex.Items.Add('V' + index.ToString());
                    vertIndex++;
                    if (i == 0)
                        txt += index.ToString();
                    else
                        txt += "," + index.ToString();
                }
                idxVerts.Text = txt;
            }
            SR.Close();
            FS.Close();

            Path = filePathIm.Text + "\\" + TxtName2.Text + "-Shapes.txt";
            FS = new FileStream(Path, FileMode.Open);
            SR = new StreamReader(FS);
            while (SR.Peek() != -1)
            {
                String s1 = SR.ReadLine();
                String[] s2 = s1.Split('*');
                int x = s2.GetLength(0);
                for (int i = 0; i < x - 1; i++)
                {
                    String[] s3 = s2[i].Split('&');
                    int index = int.Parse(s3[0]);
                    String name = s3[1];
                    int start = int.Parse(s3[2]);
                    int count = int.Parse(s3[3]);

                    Model MyModel = new Model(name, index, start, count);
                    ModelList.Add(MyModel);
                    listModel.Items.Add(name + index.ToString());
                    modelIndex++;
                }
            }
            SR.Close();
            FS.Close();

            Path = filePathIm.Text + "\\" + TxtName2.Text + "-Textures.txt";
            FS = new FileStream(Path, FileMode.Open);
            SR = new StreamReader(FS);
            while (SR.Peek() != -1)
            {
                String s1 = SR.ReadLine();
                String[] s2 = s1.Split('*');
                int x = s2.GetLength(0);
                for (int i = 0; i < x - 1; i++)
                {
                    String p = s2[i];
                    MyTextures.Add(p);
                }
            }
            SR.Close();
            FS.Close();

            filePathIm.Text = "";
            TxtName2.Text = "";
        }

        float prevX = 0, prevY = 0;
        private void simpleOpenGlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float speed = 0.04f;
                float delta = e.X - prevX;
                if (delta > 2)
                    renderer.cam.Pitch(-speed);
                else if (delta < -2)
                    renderer.cam.Pitch(speed);

                delta = e.Y - prevY;
                if (delta > 2)
                    renderer.cam.Yaw(-speed);
                else if (delta < -2)
                    renderer.cam.Yaw(speed);

                MoveCursor();
                renderer.Update();
            }
        }

        private void simpleOpenGlControl1_Load(object sender, EventArgs e)
        {

        }

        private void MoveCursor()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Point p = PointToScreen(simpleOpenGlControl1.Location);
            Cursor.Position = new Point(simpleOpenGlControl1.Size.Width / 2 + p.X, simpleOpenGlControl1.Size.Height / 2 + p.Y);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
            prevX = simpleOpenGlControl1.Location.X + simpleOpenGlControl1.Size.Width / 2;
            prevY = simpleOpenGlControl1.Location.Y + simpleOpenGlControl1.Size.Height / 2;
        }
    }
}
