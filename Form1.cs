using DragRacerPrep;
using System;
//FileHandling
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;


namespace APPR_TheRace_xxSD_BPRJ_sj2022
{
    public partial class Form1 : Form
    {
        //objects
        csRacerBase obRacerOneJKLO = new csRacerBase();
        csRacerBase obRacerTwoJKLO = new csRacerBase();
        csRacerBase obRacerThreeJKLO = new csRacerBase();
        csRacerBase obRacerFourJKLO = new csRacerBase();

        //globals average position calculation, modify to class properties
        double racerOneTotalPositionsJKLO = 0;//store total of all races
        double racerTwoTotalPositionsJKLO = 0;
        double racerThreeTotalPositionsJKLO = 0;
        double racerFourTotalPositionsJKLO = 0;
        double thisRacerAveragerRacePositionJKLO = 0; //used for all racers once finished
        int nrOfRacesJKLO = 1;//start with the first race, this is not zero

        //globals race
        int laneLength = 0;
        int finishedPosition = 0;
        bool positionsReset = false;
        int raceTime = 0;
        int raceNumber = 0;
        int minSpeed = 0;
        int maxSpeed = 0;

        //sounds
        SoundPlayer startSoundJKLO = null;

        //globals fileHandling
        string relativeFileDirectory = "";
        string storedFileName = "";

        //graph init
        System.Drawing.Pen DrawingPen = new System.Drawing.Pen(System.Drawing.Color.Black, 0.1f);
        System.Drawing.Graphics formGraphics;

        //graph variables
        int xStartPosRacerOne;
        int yStartPosRacerOne;
        int xStartPosRacerTwo;
        int yStartPosRacerTwo;
        int xStartPosRacerThree;
        int yStartPosRacerThree;
        int xStartPosRacerFour;
        int yStartPosRacerFour;
        int xEndPos;
        int yEndPos;

        public Form1()
        {
            InitializeComponent();
        }
        //messagebox with different name and date of dev 28,1
        private void Form1_Load(object sender, EventArgs e)
        {
            InitApplicationJKLO();
            startSoundJKLO = new SoundPlayer(APPR_TheRace_xxSD_BPRJ_sj2022.Properties.Resources.horsesound); //play race sound IPC 7,3
            InitGraphJKLO();
            InitDrawingGlobals();
            MessageBox.Show("Created by Jayden Klomp, 02/02/23", "Creator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnStartJKLO.Enabled = false;
        }

        private void InitApplicationJKLO()
        {
            /// <summary>
            /// Initialising the program and the name.
            /// </summary>
            this.Text = "The Race - BPRJ - by TrickyTronix, Edited By: Jayden Klomp";

            //setup the tabcontrol that the tabs will not show, dirty trick but..
            tbcMainJKLO.Appearance = TabAppearance.FlatButtons;
            tbcMainJKLO.ItemSize = new Size(0, 1);
            tbcMainJKLO.SizeMode = TabSizeMode.Fixed;
            tbcMainJKLO.SelectedIndex = 0;
            trbMaxSpeedJKLO.Enabled = false;
            trbMinSpeedJKLO.Enabled = false;


            //first get the location of the .doc directory
            relativeFileDirectory = Application.StartupPath + "\\...\\..\\doc\\";

            ResetPositionsJKLO();
        }

        private void mspMainQuit_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("You are about the quit the appliction, unsaved work might be lost. Continue?", "Quit?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                Application.Exit();
            }
        }

        //IPC 13,1
        private void mspMainViewTheRaceJKLO_Click(object sender, EventArgs e)
        {
            tbcMainJKLO.SelectedTab = tbpTheRaceJKLO;
            this.Size = new Size(895, 479);
        }

        private void mspMainViewStatsJKLO_Click(object sender, EventArgs e)
        {
            tbcMainJKLO.SelectedTab = tbpSetupAndStatisticsJKLO;
        }

        private void mspMainViewLoggingJKLO_Click(object sender, EventArgs e)
        {
            tbcMainJKLO.SelectedTab = tbpLoggingJKLO;
            this.Size = new Size(445, 375);
        }

        private void btnSetUpRacers_Click(object sender, EventArgs e)
        {

            btnStartJKLO.Enabled = true;

            laneLength = pnlStartPositionsJKLO.Left + pnlStartPositionsJKLO.Width + pnlFinishLineJKLO.Left + pnlFinishLineJKLO.Width;
            try
            {
                tmrMain.Interval = Convert.ToInt32(txbTrackSpeed.Text);

            }
            catch
            {
                MessageBox.Show("Please fill in the track speed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (cbxUseTrackbarJKLO.Checked == true)
            {
                try
                {
                    minSpeed = trbMinSpeedJKLO.Value;
                    maxSpeed = trbMaxSpeedJKLO.Value;
                }
                catch
                {
                    MessageBox.Show("Please fill in the min and max values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (cbxUseTrackbarJKLO.Checked == false)
            {
                try
                {
                    minSpeed = Convert.ToInt32(txbMinSpeed.Text);
                    maxSpeed = Convert.ToInt32(txbMaxSpeed.Text);
                }
                catch
                {
                    MessageBox.Show("Please fill in the min and max values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (minSpeed > maxSpeed)
            {
                MessageBox.Show("Min cannot be higher than the max", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                SetUpRacersJKLO();
                btnStartJKLO.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Please fill in all the correct values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// juycvbioon
        /// </summary>
        private void SetUpRacersJKLO()
        {
            /// <summary>
            ///	sets up all the racers and calculates all the values
            /// </summary>

            obRacerOneJKLO.setSpeed(minSpeed, maxSpeed);
            obRacerTwoJKLO.setSpeed(minSpeed, maxSpeed);
            obRacerThreeJKLO.setSpeed(minSpeed, maxSpeed);
            obRacerFourJKLO.setSpeed(minSpeed, maxSpeed);

            obRacerOneJKLO.calculateStepSize(laneLength);
            obRacerTwoJKLO.calculateStepSize(laneLength);
            obRacerThreeJKLO.calculateStepSize(laneLength);
            obRacerFourJKLO.calculateStepSize(laneLength);

            lblRacerOneSpeed.Text = obRacerOneJKLO.GetTunedRacerSpeed.ToString();
            lblRacerTwoSpeed.Text = obRacerTwoJKLO.GetTunedRacerSpeed.ToString();//ERROR EXAMPLE, set to racer one
            lblRacerThreeSpeed.Text = obRacerThreeJKLO.GetTunedRacerSpeed.ToString();
            lblRacerFourSpeed.Text = obRacerFourJKLO.GetTunedRacerSpeed.ToString();

            lblRacerOneStep.Text = obRacerOneJKLO.GetStepSizePerTick.ToString();
            lblRacerTwoStep.Text = obRacerTwoJKLO.GetStepSizePerTick.ToString();
            lblRacerThreeStep.Text = obRacerThreeJKLO.GetStepSizePerTick.ToString();
            lblRacerFourStep.Text = obRacerFourJKLO.GetStepSizePerTick.ToString();

            obRacerOneJKLO.racerName = txbRacerOneName.Text;
            obRacerTwoJKLO.racerName = txbRacerTwoName.Text;
            obRacerThreeJKLO.racerName = txbRacerThreeName.Text;
            obRacerFourJKLO.racerName = txbRacerFourName.Text;

            UpdateSettingsJKLO();
        }

        private void mspMainViewSetupJKLO_Click(object sender, EventArgs e)
        {
            tbcMainJKLO.SelectedTab = tbpSetupAndStatisticsJKLO;

            this.Height = Convert.ToInt32(455 * 0.75) + 21;
            this.Width = Convert.ToInt32(1191 * 0.75) + 10;
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            //IPC 2,2
            pbxTrail1JKLO.Width = pbxRacerOneJKLO.Left;
            pbxTrail2JKLO.Width = pbxRacerTwoJKLO.Left;
            pbxTrail3JKLO.Width = pbxRacerThreeJKLO.Left;
            pbxTrail4JKLO.Width = pbxRacerFourJKLO.Left;

            raceTime++;

            lblRaceTime.Text = raceTime.ToString();


            UpdatePositionsJKLO();

            ModifySpeedDynamically();

            DrawGraphHorizontally();
        }

        private void UpdatePositionsJKLO()
        {
            UpdatePosAndFinishRacerOneJKLO();

            UpdatePosAndFinishRacerTwoJKLO();

            UpdatePosAndFinishRacerThreeJKLO();

            UpdatePosAndFinishRacerFourJKLO();

            FinalizeRace();
        }

        private void FinalizeRace()
        {
            /// <summary>
            ///	checks if all the racers are finished logs the final results
            /// </summary>
            if (obRacerOneJKLO.racerFinished == true &&
                obRacerTwoJKLO.racerFinished == true &&
                obRacerThreeJKLO.racerFinished == true &&
                obRacerFourJKLO.racerFinished == true)
            {
                tmrMain.Enabled = false; //ERROR EXAMPLE, set to true
                positionsReset = false;
                rtbRaceResults.AppendText("\n");
                btnResetPositions.Enabled = true;
                startSoundJKLO.Stop(); //IPC 7,2

                nrOfRacesJKLO++;//ERROR example, remove; keep the slashes
                lblNrOfRaces.Text = nrOfRacesJKLO.ToString();
            }
        }

        /// <summary>
        /// updates the racer status IPC 24,1
        /// </summary>
        private void UpdatePosAndFinishRacerFourJKLO()
        {
            double m_prevRacerAveragePosition = 0;

            if (pbxRacerFourJKLO.Right > pnlFinishLineJKLO.Right)
            {

                pbxTrail4JKLO.Width = 0;

                if (obRacerFourJKLO.racerFinished == false)
                {
                    //handle finishing, 'checkered flag' when finished, update rank en do not come back here
                    obRacerFourJKLO.racerFinished = true;
                    finishedPosition++;
                    lblRacerFourFinishedPosition.Text = finishedPosition.ToString();

                    //average ranking and positions in up/down listing
                    racerFourTotalPositionsJKLO = racerFourTotalPositionsJKLO + finishedPosition;
                    m_prevRacerAveragePosition = thisRacerAveragerRacePositionJKLO; //save prev result and compare later
                    thisRacerAveragerRacePositionJKLO = Math.Round(racerFourTotalPositionsJKLO / nrOfRacesJKLO, 1);
                    lblRacerFourAverage.Text = thisRacerAveragerRacePositionJKLO.ToString();

                    //set up/down based on results
                    if (thisRacerAveragerRacePositionJKLO < m_prevRacerAveragePosition)
                    {
                        lblRacerFourRankUpDown.Text = "up";
                        lblRacerFourRankUpDown.ForeColor = Color.Lime;
                    }
                    else if (thisRacerAveragerRacePositionJKLO > m_prevRacerAveragePosition)
                    {
                        lblRacerFourRankUpDown.Text = "down";
                        lblRacerFourRankUpDown.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblRacerFourRankUpDown.Text = "same";
                        lblRacerFourRankUpDown.ForeColor = Color.Blue;
                    }

                    //add average to log
                    rtbRaceResults.AppendText("Race: " + raceNumber.ToString() + " > position: " + finishedPosition.ToString() + " > racer 4 " + " > race time: " + raceTime.ToString() + "\n");
                }
            }
            else
            {
                //execute until finished
                obRacerFourJKLO.calculateCurrentPosition();
                pbxRacerFourJKLO.Left = obRacerFourJKLO.GetActualPositionInLaneRounded;

                lblRacerFourStepActual.Text = obRacerFourJKLO.GetTunedRacerSpeed.ToString();
            }
        }

        /// <summary>
        /// check out comment in handling of racer four!
        /// </summary>
        private void UpdatePosAndFinishRacerThreeJKLO()
        {
            double m_prevRacerAveragePosition = 0;

            //checks if the racer is finished
            if (pbxRacerThreeJKLO.Right > pnlFinishLineJKLO.Right)
            {
                pbxTrail3JKLO.Width = 0;

                //lest the racer finish
                if (obRacerThreeJKLO.racerFinished == false)
                {
                    obRacerThreeJKLO.racerFinished = true;
                    finishedPosition++;
                    lblRacerThreeFinishedPosition.Text = finishedPosition.ToString();

                    racerThreeTotalPositionsJKLO = racerThreeTotalPositionsJKLO + finishedPosition;
                    m_prevRacerAveragePosition = thisRacerAveragerRacePositionJKLO;
                    thisRacerAveragerRacePositionJKLO = Math.Round(racerThreeTotalPositionsJKLO / nrOfRacesJKLO, 1);
                    lblRacerThreeAverage.Text = thisRacerAveragerRacePositionJKLO.ToString();

                    //ranks the average position with the up down and same ranking
                    if (thisRacerAveragerRacePositionJKLO > m_prevRacerAveragePosition)
                    {
                        lblRacerThreeRankUpDown.Text = "up";
                        lblRacerThreeRankUpDown.ForeColor = Color.Lime;
                    }
                    else if (thisRacerAveragerRacePositionJKLO < m_prevRacerAveragePosition)
                    {
                        lblRacerThreeRankUpDown.Text = "down";
                        lblRacerThreeRankUpDown.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblRacerThreeRankUpDown.Text = "same";
                        lblRacerThreeRankUpDown.ForeColor = Color.Blue;
                    }

                    rtbRaceResults.AppendText("Race: " + raceNumber.ToString() + " > position: " + finishedPosition.ToString() + " > racer 3 " + " > race time: " + raceTime.ToString() + "\n");
                }
            }
            else
            {
                obRacerThreeJKLO.calculateCurrentPosition();
                pbxRacerThreeJKLO.Left = obRacerThreeJKLO.GetActualPositionInLaneRounded;

                lblRacerThreeStepActual.Text = obRacerThreeJKLO.GetTunedRacerSpeed.ToString();
            }
        }

        /// <summary>
        /// makes the racers go up and down in rank, IPC 29,1
        /// </summary>
        private void UpdatePosAndFinishRacerTwoJKLO()
        {
            double m_prevRacerAveragePosition = 0;

            if (pbxRacerTwoJKLO.Right > pnlFinishLineJKLO.Right)
            {
                pbxTrail2JKLO.Width = 0;

                if (obRacerTwoJKLO.racerFinished == false)
                {
                    obRacerTwoJKLO.racerFinished = true;
                    finishedPosition++;
                    lblRacerTwoFinishedPosition.Text = finishedPosition.ToString();

                    racerTwoTotalPositionsJKLO = racerTwoTotalPositionsJKLO + finishedPosition;
                    m_prevRacerAveragePosition = thisRacerAveragerRacePositionJKLO;
                    thisRacerAveragerRacePositionJKLO = Math.Round(racerTwoTotalPositionsJKLO / nrOfRacesJKLO, 1);
                    lblRacerTwoAverage.Text = thisRacerAveragerRacePositionJKLO.ToString();

                    if (thisRacerAveragerRacePositionJKLO > m_prevRacerAveragePosition)
                    {
                        lblRacerTwoRankUpDown.Text = "up";
                        lblRacerTwoRankUpDown.ForeColor = Color.Lime;
                    }
                    else if (thisRacerAveragerRacePositionJKLO < m_prevRacerAveragePosition)
                    {
                        lblRacerTwoRankUpDown.Text = "down";
                        lblRacerTwoRankUpDown.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblRacerTwoRankUpDown.Text = "same";
                        lblRacerTwoRankUpDown.ForeColor = Color.Blue;
                    }


                    rtbRaceResults.AppendText("Race: " + raceNumber.ToString() + " > position: " + finishedPosition.ToString() + " > racer 2 " + " > race time: " + raceTime.ToString() + "\n");
                }
            }
            else
            {
                obRacerTwoJKLO.calculateCurrentPosition();
                pbxRacerTwoJKLO.Left = obRacerTwoJKLO.GetActualPositionInLaneRounded;

                lblRacerTwoStepActual.Text = obRacerTwoJKLO.GetTunedRacerSpeed.ToString();
            }
        }

        /// <summary>
        /// call the comments you need are in the fourth and third racer
        /// </summary>
        private void UpdatePosAndFinishRacerOneJKLO()
        {
            double m_prevRacerAveragePosition = 0;

            if (pbxRacerOneJKLO.Right > pnlFinishLineJKLO.Right)
            {
                // IPC 4,1
                pbxTrail1JKLO.Width = 0;

                if (obRacerOneJKLO.racerFinished == false)
                {
                    obRacerOneJKLO.racerFinished = true;
                    finishedPosition++;
                    lblRacerOneFinishedPosition.Text = finishedPosition.ToString();

                    racerOneTotalPositionsJKLO = racerOneTotalPositionsJKLO + finishedPosition;
                    m_prevRacerAveragePosition = thisRacerAveragerRacePositionJKLO;
                    thisRacerAveragerRacePositionJKLO = Math.Round(racerOneTotalPositionsJKLO / nrOfRacesJKLO, 1);
                    lblRacerOneAverage.Text = thisRacerAveragerRacePositionJKLO.ToString();

                    if (thisRacerAveragerRacePositionJKLO > m_prevRacerAveragePosition)
                    {
                        lblRacerOneRankUpDown.Text = "up";
                        lblRacerOneRankUpDown.ForeColor = Color.Lime;
                    }
                    else if (thisRacerAveragerRacePositionJKLO < m_prevRacerAveragePosition)
                    {
                        lblRacerOneRankUpDown.Text = "down";
                        lblRacerOneRankUpDown.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblRacerOneRankUpDown.Text = "same";
                        lblRacerOneRankUpDown.ForeColor = Color.Blue;
                    }

                    rtbRaceResults.AppendText("Race: " + raceNumber.ToString() + " > position: " + finishedPosition.ToString() + " > racer 1 " + " > race time: " + raceTime.ToString() + "\n");
                }
            }
            else
            {
                obRacerOneJKLO.calculateCurrentPosition();
                pbxRacerOneJKLO.Left = obRacerOneJKLO.GetActualPositionInLaneRounded;

                lblRacerOneStepActual.Text = obRacerOneJKLO.GetTunedRacerSpeed.ToString();
            }
        }

        /// <summary>
        /// activates the dynamic speed modifier
        /// </summary>
        private void ModifySpeedDynamically()
        {
            //when selected, the dynamic speed mode is active
            if (cbxDynamicSpeed.Checked == true)
            {
                //change the speed on the checked racetime IPC 15,2
                if (raceTime == 222)
                {
                    SetUpRacersJKLO();
                }
                if (raceTime == 422)
                {
                    SetUpRacersJKLO();
                }
                if (raceTime == 622)
                {
                    SetUpRacersJKLO();
                }
            }
        }

        private void btnStartJKLO_Click(object sender, EventArgs e)
        {
            btnResetPositions.Enabled = true;
            btnStartJKLO.Enabled = false;
            //when the racers finished the timer was disabled,
            //the finish position was on the last finished ranking
            //the next race is started so this is the Xth race
            //a new drawing will appear so the drawing must be reset
            if (positionsReset == true)
            {
                tmrMain.Enabled = true;

                startSoundJKLO.Play(); //IPC 7,2

                finishedPosition = 0;

                raceNumber++;

                InitDrawingGlobals();//lesson week 16
            }
        }

        private void btnResetPositions_Click(object sender, EventArgs e)
        {
            //not only the picturbox must be on the start line
            //also the racer object values must be reset
            tmrMain.Stop();
            btnStartJKLO.Enabled = false;
            pbxTrail1JKLO.Width = 0;
            pbxTrail2JKLO.Width = 0;
            pbxTrail3JKLO.Width = 0;
            pbxTrail4JKLO.Width = 0;
            tmrBackwardsJKLO.Start();
            SetUpRacersJKLO();
            raceNumber = 0;
            lblRaceTime.Text = "-";
            ResetPositionsJKLO();
            btnResetPositions.Enabled = false;


            //this will force the screen to redraw everything,
            //if only is available in the screen buffer it will be cleared
            Refresh();
        }

        /// <summary>
        ///	resets all the positions of all the racers
        /// </summary>
        private void ResetPositionsJKLO()
        {
            //when finished the racer bools are true and must be reset
            obRacerOneJKLO.racerFinished = false;
            obRacerTwoJKLO.racerFinished = false;
            obRacerThreeJKLO.racerFinished = false;
            obRacerFourJKLO.racerFinished = false;

            //no ranking value shall appear in the ranking labels
            lblRacerOneFinishedPosition.Text = "---";
            lblRacerTwoFinishedPosition.Text = "---";
            lblRacerThreeFinishedPosition.Text = "---";
            lblRacerFourFinishedPosition.Text = "---";


            //if reset is executed it must prohibited to reset again
            positionsReset = true;
            raceTime = 0;
        }

        private void btnClearRaceResults_Click(object sender, EventArgs e)
        {
            rtbRaceResults.Clear();
        }

        private void btnDefaultSetup_Click(object sender, EventArgs e)
        {
            txbTrackSpeed.Text = "1";
            txbMinSpeed.Text = "500";
            txbMaxSpeed.Text = "800";
            cbxDynamicSpeed.Checked = true;
        }

        private void mspFileSaveAsJKLO_Click(object sender, EventArgs e)//workshop week 15
        {
            diaFileSave.FileName = "TheRace - Racervalues.txt"; //default file name in the dialogbox
            diaFileSave.Filter = "Text file|*.txt"; //show the file format preferences
            diaFileSave.ShowDialog();
            relativeFileDirectory = diaFileSave.FileName; //save for later use
            rtbLoggingJKLO.AppendText("Location and filename: " + relativeFileDirectory + "\n" + "\n");

            storedFileName = Path.GetFileName(relativeFileDirectory); //only file name
            rtbLoggingJKLO.AppendText("Filename only: " + storedFileName + "\n");

            //append adds text to the existing file contents
            WriteTheRaceSettingsFileJKLO();
        }

        private void WriteTheRaceSettingsFileJKLO()//workshop week 15
        {
            StreamWriter SettingsRecorder = new StreamWriter(relativeFileDirectory, false);

            //write the lines, only the number of written lines can be read
            SettingsRecorder.WriteLine(obRacerOneJKLO.racerName);
            SettingsRecorder.WriteLine(obRacerTwoJKLO.racerName);
            SettingsRecorder.WriteLine(obRacerThreeJKLO.racerName);
            SettingsRecorder.WriteLine(obRacerFourJKLO.racerName);

            SettingsRecorder.Close();
        }

        //IPC 12,1
        private void mspFileOpenJKLO_Click(object sender, EventArgs e)//workshop week 15
        {
            string m_filenameAndLocation;
            try
            {
                diaFileOpen.Filter = "Text file|*.txt|All files(*.*)|*.*";
                diaFileOpen.ShowDialog();
                diaFileOpen.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                m_filenameAndLocation = diaFileOpen.FileName;

                ReadTheRaceSettingsFileJKLO(m_filenameAndLocation);

                //send read values to the GUI
                UpdateSettingsJKLO();
            }
            catch
            {
                MessageBox.Show("Something went wrong, please try again");
            }
        }

        private void ReadTheRaceSettingsFileJKLO(string m_filenameAndLocation)
        {
            StreamReader SettingsLoader = new StreamReader(m_filenameAndLocation);

            //read only the lines which were written
            obRacerOneJKLO.racerName = SettingsLoader.ReadLine();
            obRacerTwoJKLO.racerName = SettingsLoader.ReadLine();
            obRacerThreeJKLO.racerName = SettingsLoader.ReadLine();
            obRacerFourJKLO.racerName = SettingsLoader.ReadLine();

            SettingsLoader.Close();
        }

        private void UpdateSettingsJKLO()//workshop week 15 IPC 17,1
        {
            txbRacerOneName.Text = obRacerOneJKLO.racerName;
            txbRacerTwoName.Text = obRacerTwoJKLO.racerName;
            txbRacerThreeName.Text = obRacerThreeJKLO.racerName;
            txbRacerFourName.Text = obRacerFourJKLO.racerName;

            lblRacerOneName.Text = obRacerOneJKLO.racerName;
            lblRacerTwoName.Text = obRacerTwoJKLO.racerName;
            lblRacerThreeName.Text = obRacerThreeJKLO.racerName;
            lblRacerFourName.Text = obRacerFourJKLO.racerName;
        }

        //IPC 6,1 - 6,2
        private void mspHelpUserManualJKLO_Click(object sender, EventArgs e)//workshop week 15
        {
            string filePathJKLO = Application.StartupPath + @"\Files\TheRaceHelp.pdf";
            
            try
            {
                System.Diagnostics.Process.Start(filePathJKLO);
            }
            catch
            {
                MessageBox.Show("Could not find file");
            }

        }

        private void btnUpdateNames_Click(object sender, EventArgs e)//workshop week 15
        {
            obRacerOneJKLO.racerName = txbRacerOneName.Text;
            obRacerTwoJKLO.racerName = txbRacerTwoName.Text;
            obRacerThreeJKLO.racerName = txbRacerThreeName.Text;
            obRacerFourJKLO.racerName = txbRacerFourName.Text;
        }

        private void InitGraphJKLO()//lesson week 16
        {
            formGraphics = pnlGraphJKLO.CreateGraphics();
            DrawingPen.Width = 1;
        }

        /// <summary>
        /// initialise all the global variables 
        /// </summary>
        private void InitDrawingGlobals()//workshop week 16
        {
            //xStartPos = 0;//pbxRacerOneJKLO.Left - pbxRacerOneJKLO.Width;// 0;// pnlGraphJKLO.Left; // + pnlGraphJKLO.Width;
            yStartPosRacerOne = pnlGraphJKLO.Height;
            yStartPosRacerTwo = pnlGraphJKLO.Height;
            yStartPosRacerThree = pnlGraphJKLO.Height;
            yStartPosRacerFour = pnlGraphJKLO.Height;
            xStartPosRacerOne = 0;
            xStartPosRacerTwo = 0;
            xStartPosRacerThree = 0;
            xStartPosRacerFour = 0;

            xEndPos = 0;// pnlGraphJKLO.Left;//pbxRacerOneJKLO.Left - pbxRacerOneJKLO.Width;//0;// pnlGraphJKLO.Left;//pnlGraphJKLO.Left + 
            yEndPos = pnlGraphJKLO.Top + pnlGraphJKLO.Height;
        }
        private void DrawGraphHorizontally()
        {
            DrawRacerOneSpeedSettingsJKLO();
            DrawRacerTwoSpeedSettingsJKLO();
            DrawRacerThreeSpeedSettingsJKLO();
            DrawRacerFourSpeedSettingsJKLO();
        }

        /// <summary>
        /// draws the racers lines 25,1
        /// </summary>
        private void DrawRacerOneSpeedSettingsJKLO()//workshop week 16
        {
            int m_offset;

            DrawingPen.Color = System.Drawing.Color.Red;

            m_offset = pnlGraphJKLO.Height + (pnlGraphJKLO.Height / 2) - (obRacerOneJKLO.GetTunedRacerSpeed / 5); //since the result is an integer, it is automatically rounded

            xEndPos = pbxRacerOneJKLO.Left;
            yEndPos = Convert.ToInt32(m_offset);

            formGraphics.DrawLine(DrawingPen, xStartPosRacerOne, yStartPosRacerOne, xEndPos, yEndPos);

            xStartPosRacerOne = pbxRacerOneJKLO.Left;
            yStartPosRacerOne = Convert.ToInt32(m_offset);
        }
        private void DrawRacerTwoSpeedSettingsJKLO()//workshop week 16
        {
            int m_offset;

            DrawingPen.Color = System.Drawing.Color.Orange;

            m_offset = pnlGraphJKLO.Height + (pnlGraphJKLO.Height / 2) - (obRacerTwoJKLO.GetTunedRacerSpeed / 5); //since the result is an integer, it is automatically rounded

            xEndPos = pbxRacerTwoJKLO.Left;
            yEndPos = Convert.ToInt32(m_offset);

            formGraphics.DrawLine(DrawingPen, xStartPosRacerTwo, yStartPosRacerTwo, xEndPos, yEndPos);

            xStartPosRacerTwo = pbxRacerTwoJKLO.Left;
            yStartPosRacerTwo = Convert.ToInt32(m_offset);
        }
        private void DrawRacerThreeSpeedSettingsJKLO()//workshop week 16
        {
            int m_offset;

            DrawingPen.Color = System.Drawing.Color.Green;

            m_offset = pnlGraphJKLO.Height + (pnlGraphJKLO.Height / 2) - (obRacerThreeJKLO.GetTunedRacerSpeed / 5); //since the result is an integer, it is automatically rounded

            xEndPos = pbxRacerThreeJKLO.Left;
            yEndPos = Convert.ToInt32(m_offset);

            formGraphics.DrawLine(DrawingPen, xStartPosRacerThree, yStartPosRacerThree, xEndPos, yEndPos);

            xStartPosRacerThree = pbxRacerThreeJKLO.Left;
            yStartPosRacerThree = Convert.ToInt32(m_offset);
        }
        private void DrawRacerFourSpeedSettingsJKLO()//workshop week 16
        {
            int m_offset;

            DrawingPen.Color = System.Drawing.Color.Blue;

            m_offset = pnlGraphJKLO.Height + (pnlGraphJKLO.Height / 2) - (obRacerFourJKLO.GetTunedRacerSpeed / 5); //since the result is an integer, it is automatically rounded

            xEndPos = pbxRacerFourJKLO.Left;
            yEndPos = Convert.ToInt32(m_offset);

            formGraphics.DrawLine(DrawingPen, xStartPosRacerFour, yStartPosRacerFour, xEndPos, yEndPos);

            xStartPosRacerFour = pbxRacerFourJKLO.Left;
            yStartPosRacerFour = Convert.ToInt32(m_offset);
        }

        //IPC 10,1 / IPC 20,1
        private void mspMainAboutJKLO_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks to: Dick van kalsbeek;" + "\n" + "Modified by: Jayden Klomp", "About");
        }

        private void btnMainStartJKLO_Click(object sender, EventArgs e)
        {
            tbcMainJKLO.SelectedTab = tbpSetupAndStatisticsJKLO;
            this.Size = new Size(905, 385);
        }

        /// <summary>
        ///	the backwards timer which makes the racers go back again IPC 8,1
        /// </summary>
        private void tmrBackwardsJKLO_Tick(object sender, EventArgs e)
        {
            if (pbxRacerOneJKLO.Left > 5)
            {
                obRacerOneJKLO.calculateBackwardsPositionJKLO();
                pbxRacerOneJKLO.Left = obRacerOneJKLO.GetActualPositionInLaneRounded;
            }
            else
            {
                obRacerOneJKLO.resetPositionJKLO(5);
            }
            if (pbxRacerTwoJKLO.Left > 5)
            {
                obRacerTwoJKLO.calculateBackwardsPositionJKLO();
                pbxRacerTwoJKLO.Left = obRacerTwoJKLO.GetActualPositionInLaneRounded;
            }
            else
            {
                obRacerTwoJKLO.resetPositionJKLO(5);
            }
            if (pbxRacerThreeJKLO.Left > 5)
            {
                obRacerThreeJKLO.calculateBackwardsPositionJKLO();
                pbxRacerThreeJKLO.Left = obRacerThreeJKLO.GetActualPositionInLaneRounded;
            }
            else
            {
                obRacerThreeJKLO.resetPositionJKLO(5);
            }
            if (pbxRacerFourJKLO.Left > 5)
            {
                obRacerFourJKLO.calculateBackwardsPositionJKLO();
                pbxRacerFourJKLO.Left = obRacerFourJKLO.GetActualPositionInLaneRounded;
            }
            else
            {
                obRacerOneJKLO.resetPositionJKLO(5);
            }
            if (pbxRacerOneJKLO.Left < 8 && pbxRacerTwoJKLO.Left < 8 && pbxRacerThreeJKLO.Left < 8 && pbxRacerFourJKLO.Left < 8)
            {
                tmrBackwardsJKLO.Stop();
                btnStartJKLO.Enabled = true;
            }
        }

        /// <summary>
        /// checks what type of settings applier it uses, trackbar or a textbox IPC 1,2
        /// </summary>
        private void cbxUseTrackbarJKLO_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxUseTrackbarJKLO.Checked)
            {
                txbMaxSpeed.Enabled = false;
                txbMinSpeed.Enabled = false;
                trbMaxSpeedJKLO.Enabled = true;
                trbMinSpeedJKLO.Enabled = true;
            }
            else
            {
                txbMaxSpeed.Enabled = true;
                txbMinSpeed.Enabled = true;
                trbMaxSpeedJKLO.Enabled = false;
                trbMinSpeedJKLO.Enabled = false;
            }
        }

        private void trbMinSpeedJKLO_Scroll(object sender, EventArgs e)
        {
            txbMinSpeed.Text = trbMinSpeedJKLO.Value.ToString();
        }

        private void trbMaxSpeedJKLO_Scroll(object sender, EventArgs e)
        {
            txbMaxSpeed.Text = trbMaxSpeedJKLO.Value.ToString();
        }
    }
}