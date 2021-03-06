﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SQLite;


namespace notepad
{
    public partial class Form1 : Form
    {
        public sql note = new sql("bazadannih.db");
        public static bool refresh = false;

        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int size = note.getCountNote();
            notebooks[] answer = new notebooks[size];
            phones[] phoneAnswer = new phones[note.getCountMultiPhone()];


            answer = note.getMultiNote(size);
            phoneAnswer = note.getMultiPhone();

            createFormObject(size, answer, phoneAnswer);
        }

        private void add_Click(object sender, EventArgs e)
        {
            formEdit formAdd = new formEdit();
            formAdd.id = 0;
            formAdd.Show();
        }

        private void view_Click(object sender, EventArgs e)
        {
            formView formView = new formView();
            string[] splits = ((Button)sender).Name.Split('_');
            int id = Convert.ToInt32(splits[1]);
            formView.id = id;
            formView.Show();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            formEdit formEdit = new formEdit();
            string[] splits = ((Button)sender).Name.Split('_');
            int id = Convert.ToInt32(splits[1]);
            formEdit.id = id;
            formEdit.Show();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            string[] splits = ((Button)sender).Name.Split('_');
            int id = Convert.ToInt32(splits[1]);

            string message = "Вы уверены что хотите удалить анкету?";
            string caption = "Удаление анкеты";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                note.deletePhone(id);
                note.deleteAddress(id);
                note.deleteNote(id);

                refresh = true;
                refreshForm();

            }
        }



        private void createFormObject(int size, notebooks[] answer, phones[] phoneAnswer)
        {
            for (int i = 0; i < size; i++)
            {
                notes.Controls.Add(new GroupBox() { Name = "note" + i, Location = new Point(10, i * 160 + 10), Size = new Size(540, 160) });

                Control[] temp = notes.Controls.Find("note" + i, true);
                temp[0].Controls.Add(new GroupBox() { Name = "fio" + i, Text = "Инициалы", Location = new Point(10, 10), Size = new Size(200, 100) });
                temp[0].Controls.Add(new GroupBox() { Name = "date" + i, Text = "Дата рождения", Location = new Point(10, 110), Size = new Size(200, 40) });
                temp[0].Controls.Add(new GroupBox() { Name = "phone" + i, Text = "Номера телефонов", Location = new Point(220, 10), Size = new Size(200, 140) });
                temp[0].Controls.Add(new Panel() { Name = "button" + i, Location = new Point(430, 10), Size = new Size(100, 140) });

                Control[] temp_l = temp[0].Controls.Find("fio" + i, true);
                temp_l[0].Controls.Add(new Label() { Name = "lastname_l" + i, Text = "Фамилия", Location = new Point(5, 20), Size = new Size(60, 20) });
                temp_l[0].Controls.Add(new Label() { Name = "firstname_l" + i, Text = "Имя", Location = new Point(5, 40), Size = new Size(60, 20) });
                temp_l[0].Controls.Add(new Label() { Name = "secondname_l" + i, Text = "Отчество", Location = new Point(5, 60), Size = new Size(60, 20) });

                temp_l[0].Controls.Add(new Label() { Name = "lastname" + i, Text = answer[i].lastname, Location = new Point(70, 20), Size = new Size(120, 20) });
                temp_l[0].Controls.Add(new Label() { Name = "firstname" + i, Text = answer[i].firstname, Location = new Point(70, 40), Size = new Size(120, 20) });
                temp_l[0].Controls.Add(new Label() { Name = "secondname" + i, Text = answer[i].secondname, Location = new Point(70, 60), Size = new Size(120, 20) });

                temp_l = temp[0].Controls.Find("date" + i, true);
                temp_l[0].Controls.Add(new Label() { Name = "date_l" + i, Text = answer[i].date.ToString("dd.MM.yyyy"), Location = new Point(5, 20), Size = new Size(190, 15) });

                temp_l = temp[0].Controls.Find("phone" + i, true);
                temp_l[0].Controls.Add(new Label() { Name = "phone_l" + i, Text = "Номер", Location = new Point(5, 20), Size = new Size(90, 20) });
                temp_l[0].Controls.Add(new Label() { Name = "note_l" + i, Text = "Примечания", Location = new Point(100, 20), Size = new Size(90, 20) });

                for (int j = 1; j < 5; j++)
                {
                    temp_l[0].Controls.Add(new Label() { Name = "phone" + i + "-" + j, Text = "", Location = new Point(5, 30 + j * 20), Size = new Size(90, 20) });
                    temp_l[0].Controls.Add(new Label() { Name = "note" + i + "-" + j, Text = "", Location = new Point(100, 30 + j * 20), Size = new Size(90, 20) });
                }

                temp_l = temp[0].Controls.Find("button" + i, true);
                temp_l[0].Controls.Add(new Button() { Name = "viewButton_" + answer[i].id, Text = "Просмотр", Location = new Point(5, 65), Size = new Size(90, 20) });
                temp_l[0].Controls.Add(new Button() { Name = "editButton_" + answer[i].id, Text = "Изменить", Location = new Point(5, 90), Size = new Size(90, 20) });
                temp_l[0].Controls.Add(new Button() { Name = "delButton_" + answer[i].id, Text = "Удалить", Location = new Point(5, 115), Size = new Size(90, 20) });

                temp_l = temp[0].Controls.Find("viewButton_" + answer[i].id, true);
                temp_l[0].Click += new EventHandler(view_Click);

                temp_l = temp[0].Controls.Find("editButton_" + answer[i].id, true);
                temp_l[0].Click += new EventHandler(edit_Click);

                temp_l = temp[0].Controls.Find("delButton_" + answer[i].id, true);
                temp_l[0].Click += new EventHandler(delete_Click);

                int y = 1;
                int x = 0;
                while(x < phoneAnswer.Length)
                {
                    if (phoneAnswer[x].id_notebook > answer[i].id) break;
                    else if (phoneAnswer[x].id_notebook == answer[i].id)
                    {
                        temp_l = temp[0].Controls.Find("phone" + i + "-" + y, true);
                        temp_l[0].Text = phoneAnswer[x].phone;
                        temp_l = temp[0].Controls.Find("note" + i + "-" + y, true);
                        temp_l[0].Text = phoneAnswer[x].note;
                        y++;
                    }
                    x++;
                }
            }
            
        }

        private void notes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grats_Click(object sender, EventArgs e)
        {
            shablon shablon = new shablon();
            shablon.Show();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void refreshForm()
        {
            if (refresh == true)
            {
                notes.Controls.Clear();
                int size = note.getCountNote();
                notebooks[] answer = new notebooks[size];
                phones[] phoneAnswer = new phones[note.getCountMultiPhone()];


                answer = note.getMultiNote(size);
                phoneAnswer = note.getMultiPhone();

                createFormObject(size, answer, phoneAnswer);

                refresh = false;
            }
        }
    }

}