using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WorkoutBlueprint
{
    public partial class Form1 : Form
    {
        SqlCommand cmd;
        SqlConnection conn;

        //For ease of creating exercises to add to the list and subsequentially the database
        struct Movement
        {
            public string Exercise;
            public string MuscleGroup;
            public string SpecificTarget;
            public int IsCompound;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }



        private void btnGo_Click(object sender, EventArgs e)
        {
            //Prevent double clicking
            btnGo.Enabled = false;
            
            try
            {
                //Input validation
                if(listboxWorkoutType.SelectedItem == null)
                {
                    throw new ArgumentException("Select a workout type from the listbox");
                }
                if(radioHypertrophy.Checked == false && radioStrength.Checked == false)
                {
                    throw new ArgumentException("Use the radio buttons to choose between strength and hypertrophy training methods");
                }


                //Open the connection and connect to the database
                conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ryan_\source\repos\WorkoutBlueprint\WorkoutBlueprint\Blueprint.mdf;Integrated Security=True;Connect Timeout=30");
                conn.Open();

                DataTable ProgramTable = new DataTable();
                
                //Run queries based on the selections for workout type to generate the workout
                switch (listboxWorkoutType.SelectedItem)
                {
                    case ("Push"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(PushDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Pull"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(PullDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Legs"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(LegDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Chest"):
                        {
                            SqlDataAdapter Adapter = new SqlDataAdapter(ChestDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Back"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(BackDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Arms"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(ArmDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Shoulders"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(ShoulderDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Accessories"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(AccessoryDay(), conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    default:
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Exercises;", conn);
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                }

                //Temporarily just bumping it to 100% when it's done, but maybe it'll be done in instalments 
                //once the workout generation is an actual genuine thing
                GenerationProgressBar.Increment(100);
                

                //Scale up the form
                Form1.ActiveForm.Width = 816;
                Form1.ActiveForm.Height = 871;


                //Resize the ProgramDisplay based on the number of items in it
                ProgramDisplay.Height = 23 + (20 * ProgramTable.Rows.Count);
                
                //Messy spaghetti but this reconfigures the layout when there are more than 25 exercises being prescribed
                if (ProgramTable.Rows.Count > 25)
                {
                    Form1.ActiveForm.Width = (int)Math.Ceiling(Form1.ActiveForm.Width * 1.5);
                    pictureBox1.Left += 375;
                    ProgramDisplay.Left += 50;
                    ProgramDisplay.Top -= 368;

                    btnGo.Left -= 100;
                    radioHypertrophy.Left -= 100;
                    radioStrength.Left -= 100;
                    listboxWorkoutType.Left -= 100;
                    label1.Left -= 100;
                    label2.Left -= 100;
                    label3.Left -= 100;
                    PopulationProgressBar.Left -= 100;
                    GenerationProgressBar.Left -= 100;

                    Form1.ActiveForm.Height = Form1.ActiveForm.Height - 803; //Set form height just long enough to see the column headings (further adjustments imminent)
                    pictureBox1.Top = ProgramDisplay.Bottom - pictureBox1.Height;

                }
                else    //If there are 25 or less movements, the window will still be adjusted to match the output
                {
                    Form1.ActiveForm.Width = Form1.ActiveForm.Width + 25;
                    Form1.ActiveForm.Height = Form1.ActiveForm.Height - 439 - 75; //Set form height just long enough to see the column headings (further adjustments imminent)

                    ProgramDisplay.Left -= 100;
                    ProgramDisplay.Top -= 75;
                    pictureBox1.Left += 25;
                    pictureBox1.Top = ProgramDisplay.Bottom - pictureBox1.Height;
                }
                Form1.ActiveForm.Height = Form1.ActiveForm.Height + (20 * ProgramTable.Rows.Count); //Adjust form height based on number of rows
                Form1.ActiveForm.Height = Form1.ActiveForm.Height + 50; //Add a bottom margin to the form


                //Display the ProgramDisplayer on the form
                ProgramDisplay.Visible = true;

                //Just here to show how to access the table (dunno why I'd need to)
                //MessageBox.Show(ProgramTable.Rows[0].ItemArray[1].ToString());
                
                //Close the connection
                conn.Close();


                //Take a screenshot
                using (var bmp = new System.Drawing.Bitmap(Form1.ActiveForm.Width, Form1.ActiveForm.Height))
                {
                    Form1.ActiveForm.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                    if (radioStrength.Checked)
                    {
                        bmp.Save(@"Workouts/" + listboxWorkoutType.SelectedItem.ToString() + "Strength.bmp");
                    }
                    else
                    {
                        bmp.Save(@"Workouts/" + listboxWorkoutType.SelectedItem.ToString() + "Hypertrophy.bmp");
                    }
                }
            }
            catch (Exception E) //Error handling
            {
                MessageBox.Show(E.Message);
                btnGo.Enabled = true;   //In the case of input errors, the user will need to be re-enabled to click btnGo
            }
        }

        private void radioStrength_CheckedChanged(object sender, EventArgs e)
        {
            //Prevent selecting both radio buttons
            if(radioStrength.Checked)
            {
                radioHypertrophy.Checked = false;
            }
        }

        private void radioHypertrophy_CheckedChanged(object sender, EventArgs e)
        {
            //Prevent selecting both radio buttons
            if (radioHypertrophy.Checked)
            {
                radioStrength.Checked = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'blueprintDataSet.Exercises' table. You can move, or remove it, as needed.
            this.exercisesTableAdapter.Fill(this.blueprintDataSet.Exercises);
            //Create the list full of Movements
            List<Movement> MovementList = new List<Movement>();
            Movement M;

            //Read exercises in from the JSON file and add them to the MovementList
            string[] lines = System.IO.File.ReadAllLines(@"..\..\Movements.json");
            foreach(string l in lines)
            {
                M = JsonConvert.DeserializeObject<Movement>(l);
                MovementList.Add(M);
            }
           

            //Connect to the database
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ryan_\source\repos\WorkoutBlueprint\WorkoutBlueprint\Blueprint.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();

            string[] Tables = new string[] { "Abdominals", "AnteriorDeltoid", "Back", "Biceps", "Calves", "Chest", "Exercises", "Forearms",
                                                "Glutes", "Hamstrings", "LateralDeltoid", "PosteriorDeltoid", "Quadriceps", "Trapezius", "Triceps"};
            foreach(string T in Tables)
            {
                cmd = new SqlCommand("TRUNCATE TABLE " + T + ";", conn);    //Truncate clears the table AND resets the ID
                cmd.ExecuteNonQuery();
            }


            //todo:
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //I've made back and chest selections from the gui pull from those tables (cause they're straight pulls)
            //But the next stage will be to work with SQL joins etc for shoulders / arms / legs / etc
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Populating the Exercises table based on the MovementList
            foreach(Movement m in MovementList)
            {
                //Sorting exercises into their specific tables
                switch(m.MuscleGroup)
                {
                    case ("Abdominals"):
                        {
                            cmd = new SqlCommand("INSERT INTO Abdominals (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            break;
                        }

                    case ("Arms"):
                        {
                            if (m.SpecificTarget.Contains("Biceps Brachii"))
                            {
                                cmd = new SqlCommand("INSERT INTO Biceps (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            else if (m.SpecificTarget.Contains("Triceps Brachii"))
                            {
                                cmd = new SqlCommand("INSERT INTO Triceps (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            if (m.SpecificTarget == "Forearms")
                            {
                                cmd = new SqlCommand("INSERT INTO Forearms (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            break;
                        }

                    case ("Back"):
                        {
                            cmd = new SqlCommand("INSERT INTO Back (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            break;
                        }

                    case ("Chest"):
                        {
                            cmd = new SqlCommand("INSERT INTO Chest (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            break;
                        }

                    case ("Legs"):
                        {
                            if (m.SpecificTarget == "Quadriceps")
                            {
                                cmd = new SqlCommand("INSERT INTO Quadriceps (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            else if (m.SpecificTarget == "Hamstrings")
                            {
                                cmd = new SqlCommand("INSERT INTO Hamstrings (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);

                            }
                            else if (m.SpecificTarget == "Calves")
                            {
                                cmd = new SqlCommand("INSERT INTO calves (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            else if(m.SpecificTarget == "Glutes")
                            {
                                cmd = new SqlCommand("INSERT INTO Glutes (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            break;
                        }

                    case ("Shoulders"):
                        {
                            if (m.SpecificTarget == "Anterior Deltoid")
                            {
                                cmd = new SqlCommand("INSERT INTO AnteriorDeltoid (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            else if (m.SpecificTarget == "Lateral Deltoid")
                            {
                                cmd = new SqlCommand("INSERT INTO LateralDeltoid (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            else if (m.SpecificTarget == "Posterior Deltoid")
                            {
                                cmd = new SqlCommand("INSERT INTO PosteriorDeltoid (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            }
                            break;
                        }

                    case ("Trapezius"):
                        {
                            cmd = new SqlCommand("INSERT INTO Trapezius (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            break;
                        }

                    default:
                        {
                            cmd = new SqlCommand("INSERT INTO Exercises (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                            break;
                        }
                }

                cmd.Parameters.Add("@Exercise", m.Exercise);
                cmd.Parameters.Add("@MuscleGroup", m.MuscleGroup);
                cmd.Parameters.Add("@SpecificTarget", m.SpecificTarget);
                cmd.Parameters.Add("@IsCompound", m.IsCompound);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                PopulationProgressBar.Value += (100 / MovementList.Count());
                Console.WriteLine(PopulationProgressBar.Value);
            }

            //Close connection to the database
            conn.Close();


            //Once the database has been fully populated, the progress bar reflects this and the Go button becomes active
            PopulationProgressBar.Value = 100;
            btnGo.Enabled = true;
        }


        private string PullDay()
        {
            string Workout = "";

            if (radioStrength.Checked)
            {
                Workout =
                    "WITH " +
                    "TrapsCounter AS " +
                    "( " +
                        "SELECT *, ROW_NUMBER() OVER(ORDER BY ID) as RowNumber FROM Trapezius " +
                    "), " +
                    "TrapsFarmersWalkIntersect as " +
                    "( " +
                        "Select RowNumber from TrapsCounter " +
                            "Intersect " +
                        "SELECT * FROM (SELECT FLOOR(RAND() * ((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0) - 1 + 1))+1 as RandNum)A " +
                    "), " +
                    "FarmersWalkSelector as " +
                    "( " +
                        "SELECT * FROM TrapsCounter WHERE Exercise LIKE '%Farmers Walk%' " +
                    ") " +


                    "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Back " +
                        "WHERE Exercise LIKE '%Ups' " +
                        "ORDER BY NEWID() " +
                    ")A " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Back " +
                        "WHERE Exercise LIKE '%Row%' AND Exercise NOT LIKE '%Cable%'" +     // Drop cable from strength days
                        "ORDER BY NEWID() " +
                    ")B " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Back " +
                        "WHERE Exercise LIKE '%Pulldown%' " +
                    ")C " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM PosteriorDeltoid " +
                        "WHERE Exercise LIKE '%Face Pull%' " +
                    ")D " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM PosteriorDeltoid " +
                        "WHERE Exercise NOT LIKE '%Face Pull%' " +
                        "ORDER BY NEWID() " +
                    ")E " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Trapezius " +
                        "WHERE Exercise LIKE '%Upright Row%' " +
                    ")F " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Trapezius " +
                        "WHERE Exercise NOT LIKE '%Upright Row%' AND Exercise NOT LIKE '%Farmers Walk%' " +
                        "ORDER BY NEWID() " +
                    ")G " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Biceps " +
                        "WHERE Exercise LIKE '%Curl%' AND Exercise NOT LIKE '%Cable%' AND Exercise NOT LIKE '%Hammer%' AND Exercise NOT LIKE '%Reverse%' " +
                        "ORDER BY NEWID() " +
                    ")H " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Biceps " +
                        "WHERE Exercise LIKE '%Hammer Curl%' " +
                    ")I " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Biceps " +
                        "WHERE Exercise LIKE '%Reverse%' " +
                        "ORDER BY NEWID() " +
                    ")J " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM FarmersWalkSelector " +
                        "WHERE RowNumber IN (SELECT* FROM TrapsFarmersWalkIntersect) " +    // Add Farmers Walk if it has been randomly selected //
                    ")K;";
            }
            else if(radioHypertrophy.Checked)
            {
                Workout =
                    "WITH " +
                    "TrapsCounter AS " +
                    "( " +
                        "SELECT *, ROW_NUMBER() OVER(ORDER BY ID) as RowNumber FROM Trapezius " + 
                    "), " +
                	"TrapsFarmersWalkIntersect as " +
                    "( " +
                        "Select RowNumber from TrapsCounter " +
                            "INTERSECT " +         
                        "SELECT  * FROM (SELECT FLOOR(RAND() * ((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0) - 1 + 1))+1 as RandNum)A " +
                    "), " +
                    "FarmersWalkSelector as " + 
                    "( " +
                        "SELECT * FROM TrapsCounter WHERE Exercise LIKE '%Farmers Walk%' " +
                    ") " +

                    "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM " +
                    "( " + 
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back " +
                        "WHERE Exercise LIKE '%Ups' " +
                        "ORDER BY NEWID() " + 
                    ")A " + 
                    
                        "UNION ALL " +
                    
                    "SELECT * FROM " + 
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Back " +
                        "WHERE Exercise LIKE '%Row%' " +
                        "ORDER BY NEWID() " +
                    ")B " +

                        "UNION ALL " +
                    
                    "SELECT * FROM " + 
                    "( " + 
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Back " +
                        "WHERE Exercise LIKE '%Pulldown%' " +
                    ")C " +

                        "UNION ALL " + 
                    
                    "SELECT * FROM " +
                    "( " + 
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM PosteriorDeltoid " +
                        "WHERE Exercise LIKE '%Face Pull%' " +
                    ")D " + 
                    
                        "UNION ALL " +
                    
                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM PosteriorDeltoid " +
                        "WHERE Exercise NOT LIKE '%Face Pull%'" +
                        "ORDER BY NEWID() " +
                    ")E " +

                        "UNION ALL " +

                    "SELECT* FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Trapezius " +
                        "WHERE Exercise LIKE '%Upright Row%' " +
                    ")F " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Trapezius " +
                        "WHERE Exercise NOT LIKE '%Upright Row%' AND Exercise NOT LIKE '%Farmers Walk%' " +
                        "ORDER BY NEWID() " +
                    ")G " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Biceps " +
                        "WHERE Exercise LIKE '%Curl%' AND Exercise NOT LIKE '%Hammer%' AND Exercise NOT LIKE '%Reverse%' " +
                        "ORDER BY NEWID() " +
                    ")H " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Biceps " +
                        "WHERE Exercise LIKE '%Hammer Curl%' " +
                    ")I " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Biceps " +
                        "WHERE Exercise LIKE '%Reverse%' " +
                        "ORDER BY NEWID() " +
                    ")J " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM FarmersWalkSelector " +      /* Add optional Farmers Walk exercises */
                        "WHERE RowNumber IN (SELECT* FROM TrapsFarmersWalkIntersect) " +
                    ")K";
            }

            return Workout;
        }

        private string PushDay()
        {
            string Workout = "";

            if (radioStrength.Checked)
            {
                Workout =
                    "WITH " +
                        "ChestNonCompounds AS " +
                        "(" +
                            "SELECT *, ROW_NUMBER() OVER(ORDER BY ID) as RowNumber " +
                            "FROM Chest " +
                            "WHERE IsCompound = 0" +
                        "), " +
                        "ChestNonCompIntersect as " +
                        "( " +
                            "Select RowNumber FROM ChestNonCompounds " +
                                "Intersect " +
                            "Select * FROM (SELECT FLOOR(RAND() * ((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0) - 1 + 1))+1 as RandNum)A" +
                        "), " +
                        "CS as" +
                        "( " +
                            "SELECT* FROM ChestNonCompounds WHERE Exercise LIKE '%Pullover%'" +
                        ") " +
                        

                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM " +
                        "(" +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Chest " +
                            "WHERE IsCompound = 1 AND Exercise LIKE '%Flat%' " +
                            "ORDER BY NEWID() " +
                        ")A " +
                            
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Chest " +
                            "WHERE IsCompound = 1 AND Exercise LIKE '%Incline%' " +
                            "ORDER BY NEWID() " +
                        ")B " +
                        
                            "UNION ALL " +
                        
                        "SELECT * FROM" +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM AnteriorDeltoid " +
                            "WHERE IsCompound = 1 AND Exercise NOT LIKE '%Arnold%' AND Exercise NOT LIKE '%Machine%' " +    //Drop Arnold and Machine presses from Strength workouts
                            "ORDER BY NEWID() " +
                        ")C " +
                        
                            "UNION ALL " +
                        
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +     // Add fly/crossover variation 
                            "FROM Chest " +
                            "WHERE Exercise LIKE '%Crossover%' OR Exercise LIKE '%Fly%' " +
                            "ORDER BY NEWID() " +
                        ")D " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM CS " +   // Add optional Chest exercises 
                            "WHERE RowNumber IN (SELECT* FROM ChestNonCompIntersect) " +
                        ")E " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "(" +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +     // Pick a Lateral Deltoid non-compound movement 
                            "FROM LateralDeltoid " +
                            "WHERE IsCompound = 0 " +
                            "ORDER BY NEWID() " +
                        ")F " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +     // Pick a lateral deltoid non-compound exercise 
                            "FROM AnteriorDeltoid " +
                            "WHERE IsCompound = 0 " +
                            "ORDER BY NEWID() " +
                        ")G " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +     // Target the tricep's long head 
                            "FROM Triceps " +
                            "WHERE SpecificTarget LIKE '%Long Head%' " +
                            "ORDER BY NEWID() " +
                        ")H " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +     // Target the tricep's lateral head
                            "FROM Triceps " +
                            "WHERE SpecificTarget LIKE '%Lateral Head%' " +
                            "ORDER BY NEWID() " +
                        ")I " +
                        
                            "UNION ALL " +
                            
                        "SELECT* FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +     // Target the tricep's medial head 
                            "FROM Triceps " +
                            "WHERE SpecificTarget LIKE '%Medial%' " +
                            "ORDER BY NEWID() " +
                        ")J " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Triceps " +
                            "WHERE SpecificTarget LIKE '%Triceps Brachii' " +       // Not head-specific tricep exercises to finish */
                            "ORDER BY NEWID() " +
                        ")H";
            }
            else if (radioHypertrophy.Checked)
            {
                Workout =
                    "WITH " +
                    "ChestNonCompounds AS " +
                    "( " +
                        "SELECT *, ROW_NUMBER() OVER(ORDER BY ID) as RowNumber FROM Chest WHERE IsCompound = 0 " +
                    "), " +
                    "ChestNonCompIntersect as " +
                    "( " +
                        "SELECT RowNumber from ChestNonCompounds " +

                            "INTERSECT " +

                        "SELECT * FROM (SELECT FLOOR(RAND() * ((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0) - 1 + 1))+1 as RandNum)A " +
                    "), " +
                    "CS as " +
                    "( " +
                        "SELECT * FROM ChestNonCompounds WHERE Exercise LIKE '%Pullover%' " +
                    ") " +

                    "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Chest " +
                        "WHERE IsCompound = 1 AND Exercise LIKE '%Flat%' " +
                        "ORDER BY NEWID() " +
                    ")A " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Chest " +
                        "WHERE IsCompound = 1 AND Exercise LIKE '%Incline%' " +
                        "ORDER BY NEWID() " +
                    ")B " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM AnteriorDeltoid " +
                        "WHERE IsCompound = 1 " +
                        "ORDER BY NEWID() " +
                    ")C " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Chest " +
                        "WHERE Exercise LIKE '%Crossover%' OR Exercise LIKE '%Fly%' " +
                        "ORDER BY NEWID() " +
                    ")D " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM CS " +
                        "WHERE RowNumber IN (SELECT* FROM ChestNonCompIntersect) " +
                    ")E " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM LateralDeltoid " +
                        "WHERE IsCompound = 0 " +
                        "ORDER BY NEWID() " +
                    ")F " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM AnteriorDeltoid " +
                        "WHERE IsCompound = 0 " +
                        "ORDER BY NEWID() " +
                    ")G " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Triceps " +
                        "WHERE SpecificTarget LIKE '%Long Head%' " +
                        "ORDER BY NEWID() " +
                    ")H " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Triceps " +
                        "WHERE SpecificTarget LIKE '%Lateral Head%' " +
                        "ORDER BY NEWID() " +
                    ")I " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Triceps " +
                        "WHERE SpecificTarget LIKE '%Medial%' " +
                        "ORDER BY NEWID() " +
                    ")J " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Triceps " +
                        "WHERE SpecificTarget LIKE '%Triceps Brachii' " +
                        "ORDER BY NEWID() " +
                    ")H";
            }

            return Workout;
        }

        private string LegDay()
        {
            string Workout = "";

            if (radioStrength.Checked)
            {
                Workout =
                    "WITH " +
                        "HammyCounter AS " +
                        "( " +
                            "SELECT *, ROW_NUMBER() OVER(ORDER BY ID) as RowNumber FROM Hamstrings " +
                        "), " +
                        "ReverseHackSquatIntersect as " +
                        "( " +
                            "SELECT RowNumber from HammyCounter " +
                                "INTERSECT " +
                            "SELECT * FROM (SELECT FLOOR(RAND() * ((SELECT COUNT(*) FROM Hamstrings) - 1 + 1))+1 as RandNum)A " +
                        "), " +
                        "ReverseHackSquatSelector as " +
                        "( " +
                            "SELECT * FROM HammyCounter WHERE Exercise LIKE '%Reverse Hack%' " +
                        ") " +
                        
                        
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Quadriceps " +
                            "WHERE Exercise = 'Back Squat' " +
                        ")A " +
                        
                            "UNION ALL " +
                        
                        "SELECT * FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Back " +
                            "WHERE Exercise = 'Deadlift' " +
                        ")B " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Hamstrings " +
                            "WHERE Exercise LIKE '%Deadlift%' " +
                            "ORDER BY NEWID() " +
                        ")C " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM ReverseHackSquatSelector " +
                            "WHERE RowNumber IN (SELECT* FROM ReverseHackSquatIntersect) " +    // Add optional Reverse Hack Squat 
                        ")D " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Glutes " +
                            "ORDER BY NEWID() " +
                        ")E " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Quadriceps " +
                            "WHERE Exercise NOT LIKE 'Back%' AND Exercise NOT LIKE 'Sissy%' AND Exercise NOT LIKE '%Split%' AND(Exercise LIKE '%Squat' OR Exercise LIKE '%Press') " +
                            "ORDER BY NEWID() " +
                        ")F " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Hamstrings " +
                            "WHERE Exercise LIKE '%Curls%' " +
                        ")G " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Quadriceps " +
                            "WHERE Exercise LIKE '%Extension%' " +
                        ")H " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Calves " +
                            "WHERE Exercise LIKE 'Seated%' " +
                        ")I " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Calves " +
                            "WHERE Exercise NOT LIKE 'Seated%' AND Exercise NOT LIKE 'Reverse%' " +
                        ")J " +
                        
                            "UNION ALL " +
                            
                        "SELECT * FROM " +
                        "( " +
                            "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                            "FROM Calves " +
                            "WHERE Exercise LIKE 'Reverse%' " +
                        ")K;";
            }
            else if (radioHypertrophy.Checked)
            {
                Workout =
                    "WITH " +
                    "HammyCounter AS " +
                    "( " +
                        "SELECT *, ROW_NUMBER() OVER(ORDER BY ID) as RowNumber FROM Hamstrings " +
                    "), " +
                    "ReverseHackSquatIntersect as " +
                    "( " +
                        "SELECT RowNumber FROM HammyCounter " +

                            "INTERSECT " +

                        "SELECT * FROM(SELECT FLOOR(RAND() *((SELECT COUNT(*) FROM Hamstrings)-1 + 1))+1 as RandNum)A " +
                    "), " +
                    "ReverseHackSquatSelector as " +
                    "( " +
                        "SELECT * FROM HammyCounter WHERE Exercise LIKE '%Reverse Hack%' " +
                    ") " +

                    "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Quadriceps " +
                        "WHERE Exercise = 'Back Squat' " +
                    ")A " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound " +
                        "FROM Hamstrings " +
                        "WHERE Exercise LIKE '%Deadlift%' " +
                        "ORDER BY NEWID() " +
                    ")B " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM ReverseHackSquatSelector " +      /* Add optional Reverse Hack Squat */
                        "WHERE RowNumber IN (SELECT* FROM ReverseHackSquatIntersect) " +
                    ")C " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Glutes " +
                        "ORDER BY NEWID() " +
                    ")D " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Quadriceps " +
                        "WHERE Exercise NOT LIKE 'Back%' AND Exercise NOT LIKE 'Sissy%' AND Exercise NOT LIKE '%Split%' AND(Exercise LIKE '%Squat' OR Exercise LIKE '%Press') " +
                        "ORDER BY NEWID() " +
                    ")E " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Hamstrings " +
                        "WHERE Exercise LIKE '%Curls%' " +
                    ")F " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Quadriceps " +
                        "WHERE Exercise LIKE '%Extension%' " +
                    ")G " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves " +
                        "WHERE Exercise LIKE 'Seated%' " +
                    ")H " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves " +
                        "WHERE Exercise NOT LIKE 'Seated%' AND Exercise NOT LIKE 'Reverse%' " +
                    ")I " +

                        "UNION ALL " +

                    "SELECT * FROM " +
                    "( " +
                        "SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves " +
                        "WHERE Exercise LIKE 'Reverse%' " +
                    ")J";
            }

            return Workout;
        }

        private string ChestDay()
        {
            string Workout;

            if (radioStrength.Checked)
            {

            }
            else if (radioHypertrophy.Checked)
            {

            }

            Workout = "SELECT * FROM Chest;";

            return Workout;
        }

        private string BackDay()
        {
            string Workout;

            if (radioStrength.Checked)
            {

            }
            else if (radioHypertrophy.Checked)
            {

            }

            Workout = "SELECT * FROM Back;";

            return Workout;
        }

        private string ArmDay()
        {
            string Workout;

            if (radioStrength.Checked)
            {

            }
            else if (radioHypertrophy.Checked)
            {

            }

            Workout = "SELECT * FROM Biceps " +
                        "UNION " +
                        "SELECT * FROM Triceps " +
                        "UNION " +
                        "SELECT * FROM Forearms " +
                        "ORDER BY SpecificTarget;";

            return Workout;
        }

        private string ShoulderDay()
        {
            string Workout;

            if (radioStrength.Checked)
            {

            }
            else if (radioHypertrophy.Checked)
            {

            }

            Workout = "SELECT * FROM AnteriorDeltoid " +
                        "UNION " +
                        "SELECT* FROM LateralDeltoid " +
                        "UNION " +
                        "SELECT * FROM PosteriorDeltoid " +
                        "ORDER BY SpecificTarget;";

            return Workout;
        }

        private string AccessoryDay()
        {
            string Workout;

            radioStrength.Enabled = false;
            radioStrength.Checked = false;
            radioHypertrophy.Checked = true;

            Workout = "SELECT * FROM Abdominals " +
                        "UNION " +
                        "SELECT * FROM Glutes " +
                        "UNION " +
                        "SELECT * FROM Trapezius;";

            return Workout;
        }

        private string CardioDay()      //todo: This is just here incase I add cardio in later
        {
            string Workout;

            if (radioStrength.Checked)
            {

            }
            else if (radioHypertrophy.Checked)
            {

            }

            Workout = "";
            return Workout;
        }
    }
}