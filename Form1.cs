using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace WorkoutBlueprint
{
    public partial class Form1 : Form
    {
        SqlCommand cmd;
        SqlConnection conn;
        SqlDataAdapter da;

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

                //Run queries based on the selections for workout type to generate the workout
                switch (listboxWorkoutType.SelectedItem)
                {
                    case ("Push"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Exercises " +
                                "WHERE MuscleGroup LIKE 'Triceps'" +
                                "OR MuscleGroup LIKE 'Chest'" +
                                "OR MuscleGroup LIKE 'Shoulders';", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    case ("Pull"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Exercises " +
                                "WHERE MuscleGroup LIKE 'Biceps'" +
                                "OR MuscleGroup LIKE 'Back'" +
                                "OR MuscleGroup LIKE 'Trap%';", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    case ("Legs"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Exercises " +
                                "WHERE MuscleGroup LIKE 'Legs';", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }

                    case ("Chest"):
                        {
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Chest;", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    case ("Back"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Back;", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    case ("Arms"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Exercises " +
                                "WHERE MuscleGroup LIKE 'Triceps'" +
                                "OR MuscleGroup LIKE 'Biceps'" +
                                "OR MuscleGroup LIKE 'Forearms';", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    case ("Shoulders"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Exercises " +
                                "WHERE MuscleGroup LIKE 'Shoulders';", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    case ("Accessories"):
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter(
                                "SELECT Exercise, MuscleGroup, SpecificTarget FROM Exercises " +
                                "WHERE MuscleGroup LIKE 'Ab%'" +
                                "OR MuscleGroup LIKE 'Obliques'" +
                                "OR MuscleGroup LIKE 'Trap%';", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                    default:
                        {
                            //DataAdapter for SQL Queries
                            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Exercises;", conn);
                            DataTable ProgramTable = new DataTable();
                            Adapter.Fill(ProgramTable);
                            ProgramDisplay.DataSource = ProgramTable;
                            break;
                        }
                        }
                //Display the ProgramDisplayer on the form
                ProgramDisplay.Visible = true;
                

                //Close the connection
                conn.Close();
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

            /*  Template
            M.Exercise = "";
            M.MuscleGroup = "";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);
            */

            //Shoulders
            M.Exercise = "Arnold Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Behind-The-Neck Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Military Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Clean and Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Machine Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Push Press";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior / Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Lateral Raises";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Cable Lateral Raises";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Lateral Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Overhead Dumbbell Laterals";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Lateral / Posterior Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Machine Laterals";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Lateral deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Front Dumbbell Raises";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Anterior Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bent-Over Dumbbell Laterals";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Posterior Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bent-Over Cable Laterals";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Posterior Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Face Pulls";
            M.MuscleGroup = "Shoulders";
            M.SpecificTarget = "Posterior Deltoid";
            M.IsCompound = 0;
            MovementList.Add(M);


            //Trapezius
            M.Exercise = "Upright Rows";
            M.MuscleGroup = "Trapezius";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Shrugs";
            M.MuscleGroup = "Trapezius";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Barbell Shrugs";
            M.MuscleGroup = "Trapezius";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Farmers Walk";
            M.MuscleGroup = "Trapezius";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);


            //Chest
            M.Exercise = "Barbell Flat Bench Press";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Mid Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Wide-Grip Barbell Flat Bench Press";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Mid Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Barbell Incline Bench Press";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Upper Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Flat Bench Press";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Mid Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Incline Bench Press";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Upper Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Decline Bench Press";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Lower Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Parallel Bar Dips";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Upper Pectorals";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Flys";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Mid Pectorals";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Incline Dumbbell Flys";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Upper Pectorals";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Cable Crossovers";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bench Cable Crossovers";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Machine Flys";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "Mid Pectorals";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Straight-Arm Pullovers";
            M.MuscleGroup = "Chest";
            M.SpecificTarget = "";
            M.IsCompound = 0;
            MovementList.Add(M);


            //Back
            M.Exercise = "Wide-Grip Pullups";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Upper Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Wide-Grip Behind-The-Neck Pullups";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Upper Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Chin-Ups";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Lower Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Lat Pulldowns";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Upper Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Close / Medium-Grip Pulldown";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Lower Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bent-Over Barbell Row";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Rhomboids?";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bent-Over Dumbbell Row";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Rhomboids?";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "T-Bar Row";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Rhomboids?";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Single-Arm Dumbbell Row";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Lower Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Single-Arm Cable Row";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Lower Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Seated Cable Row";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Rhomboids & Lower Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bent-Arm Barbell Pullover";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Lower Latissimus Dorsi";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Deadlift";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Good Mornings";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Erector Spinae";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Hyperextensions";
            M.MuscleGroup = "Back";
            M.SpecificTarget = "Erector Spinae";
            M.IsCompound = 0;
            MovementList.Add(M);


            //Arms
            M.Exercise = "Barbell Curl";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Curl";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Preacher Curl";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "3-Part Curls (21s)";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Incline Dumbbell Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Hammer Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Concentration Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Lying Dumbbell Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Cable Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii & Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Cable Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii & Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Preacher Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Biceps Brachii & Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);


            M.Exercise = "Cable Pushdowns";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Cable Pushdowns";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Barbell Skullcrushers";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii - Long Head";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Skullcrushers";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii - Long Head";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Barbell Tricep Extensions";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii - Long Head";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Tricep Extensions";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii - Long Head";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Kickbacks";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Close-Grip Bench Press";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dips";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bench Dips";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Triceps Brachii";
            M.IsCompound = 0;
            MovementList.Add(M);


            M.Exercise = "Barbell Wrist Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Dumbbell Wrist Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Behind-The-Back Wrist Curls";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Barbell Wrist Curl";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Dumbbell Wrist Curl";
            M.MuscleGroup = "Arms";
            M.SpecificTarget = "Forearms";
            M.IsCompound = 0;
            MovementList.Add(M);

            //Legs      
            //Quadriceps - | Rectus Femoris | Vastus Intermedius | Vastus Medialis | Vastus Lateralis |
            M.Exercise = "Back Squat";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Front Squat";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Sissy Squat";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Leg Press";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Hack Squat";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 1;
            MovementList.Add(M);

            M.Exercise = "Lunges";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bulgarian Split Squat";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Leg Extension";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Quadriceps";
            M.IsCompound = 0;
            MovementList.Add(M);

            //Hamstrings
            M.Exercise = "Hamstring Curls";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Hamstrings";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Romanian Deadlift";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Hamstrings";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Single-Stiff-Leg Dealift";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Hamstrings";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Hack Squat";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Hamstrings";
            M.IsCompound = 1;
            MovementList.Add(M);

            //Calves
            M.Exercise = "Calf Raises";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Calves";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Calf Press";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Calves";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Calf Raises";
            M.MuscleGroup = "Legs";
            M.SpecificTarget = "Tibialis Anterior?";
            M.IsCompound = 0;
            MovementList.Add(M);


            //Abdominals -      | Rectus Abdominis | External Obliques | Intercostals |
            M.Exercise = "Roman Chair";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Upper Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Crunches";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Upper Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Twisting Crunches";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Upper Rectus Abdominis & External Obliques";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Reverse Crunches";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Hanging Reverse Crunches";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Vertical Bench Crunches";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Cable Crunches";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Upper & Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Seated Twists";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "External Obliques";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Bent-Over Twists";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "External Obliques";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Leg Raises";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Vertical Bench Leg Raises";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Hanging Leg Raises";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Twisting Hanging Leg Raises";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Lower Rectus Abdominis & External Obliques";
            M.IsCompound = 0;
            MovementList.Add(M);

            M.Exercise = "Vacuums";
            M.MuscleGroup = "Abdominals";
            M.SpecificTarget = "Rectus Abdominis";
            M.IsCompound = 0;
            MovementList.Add(M);




            

            //Connect to the database
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ryan_\source\repos\WorkoutBlueprint\WorkoutBlueprint\Blueprint.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();

            //Clear the database (table) in case it is not already empty
            cmd = new SqlCommand("TRUNCATE TABLE Exercises;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();

            //TESTING NEW TABLES
            cmd = new SqlCommand("TRUNCATE TABLE Abdominals;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE AnteriorDeltoid;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Back;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Biceps;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Calves;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Chest;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Forearms;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Hamstrings;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE LateralDeltoid;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE PosteriorDeltoid;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Quadriceps;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Trapezius;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("TRUNCATE TABLE Triceps;", conn);    //Truncate clears the table AND resets the ID
            cmd.ExecuteNonQuery();


            //todo:
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Fix the Shoulders so that they work below (you have 7 that are a mix of deltoids and are defaulting to the
            //Exercises table rather than a specific deltoid table
            //I've made back and chest selections from the gui pull from those tables (cause they're straight pulls)
            //But the next stage will be to work with SQL joins etc for shoulders / arms / legs / etc
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Inserts the following exercises (parameters) into the Exercises table
            cmd = new SqlCommand("INSERT INTO Exercises (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);

            //Populating the Exercises table based on the MovementList
            foreach(Movement m in MovementList)
            {
                //Sorting exercises into their specific tables
                switch(m.MuscleGroup)
                {
                    case ("Abdominals"):
                        cmd = new SqlCommand("INSERT INTO Abdominals (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        break;

                    case ("Arms"):
                        if (m.SpecificTarget == "Biceps Brachii")
                        {
                            cmd = new SqlCommand("INSERT INTO Biceps (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        }
                        else if (m.SpecificTarget == "Triceps Brachii")
                        {
                            cmd = new SqlCommand("INSERT INTO Triceps (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        }
                        else if (m.SpecificTarget == "Forearms")
                        {
                            cmd = new SqlCommand("INSERT INTO Forearms (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        }
                        break;

                    case ("Back"):
                        cmd = new SqlCommand("INSERT INTO Back (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        break;

                    case ("Chest"):
                        cmd = new SqlCommand("INSERT INTO Chest (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        break;

                    case ("Legs"):
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
                        break;

                    case ("Shoulders"):
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

                    case ("Trapezius"):
                        cmd = new SqlCommand("INSERT INTO Trapezius (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        break;

                    default:
                        cmd = new SqlCommand("INSERT INTO Exercises (Exercise, MuscleGroup, SpecificTarget, IsCompound) VALUES (@Exercise, @MuscleGroup, @SpecificTarget, @IsCompound)", conn);
                        break;
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
    }
}