using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace esp8266_derby_app
{
    public partial class Main : Form
    {
        public Derby derby { get; set; } = new Derby();
                                
        BindingSource bsDenList = new BindingSource();
        BindingSource bsCarList = new BindingSource();
        BindingSource bsCarDen = new BindingSource();
        BindingSource bsCompletedRaces = new BindingSource();
        BindingSource bsParticipants = new BindingSource();        

        public Main()
        {
            InitializeComponent();            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (derby.saved == true)
            {
                Util.SaveDerby(derby.savedFileName, derby);
            }
            else
            {
                saveAsToolStripMenuItem.PerformClick();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Save derby file";
                    sfd.AddExtension = true;
                    sfd.OverwritePrompt = true;
                    sfd.Filter = "Let's Race Files|*.derby";

                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Util.SaveDerby(sfd.FileName, derby);
                        derby.saved = true;
                        derby.savedFileName = sfd.FileName;                        
                    }
                }
            //} catch
            //{
                ///todo
            //}
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (derby.saved == false)
            {
                DialogResult dr = MessageBox.Show("Save current derby before opening new one?",
                      "Save?", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem.PerformClick();
                }
            }

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Open derby file";
                    ofd.CheckFileExists = true;
                    ofd.Multiselect = false;
                    ofd.Filter = "Let's Race Files|*.derby";

                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {                        
                        derby = Util.LoadDerby(ofd.FileName);
                        LoadDerbyControls();
                    }
                }
            }
            catch
            {
                ///todo
            }
        }

        private void LoadDerbyControls()
        {                        
            lblRaceStatus.Text = "Race Status:Ready";
            lblRemainingRaces.Text = "Remaining Races: " + derby.laneSchedule[0].Count();
            txtPackName.Text = derby.pack.name;
            txtRaceMaster.Text = derby.pack.raceMaster;
            numHeatsPerCar.Text = " ";  // workaround for value not updating screen control
            numHeatsPerCar.Value = Convert.ToDecimal(derby.heatsPerCar);
            numTrackLanes.Text = " ";
            numTrackLanes.Value = Convert.ToDecimal(derby.trackLanes);
            chkUseTimer.Checked = derby.useTimer;
            txtTimerIPAddr.Text = derby.timerIP;
            btnNewRace.Enabled = false;
            btnNewCar.Enabled = false;
            derby.participants.Clear();

            bsDenList.DataSource = derby.dens;
            lstDenList.DisplayMember = "DisplayMember";
            lstDenList.ValueMember = "ID";
            lstDenList.DataSource = bsDenList;

            bsCarDen.DataSource = derby.dens;
            cmbCarDen.DisplayMember = "DisplayMember";
            cmbCarDen.ValueMember = "ID";
            cmbCarDen.DataSource = bsCarDen;

            bsCarList.DataSource = derby.cars;
            lstCarList.DisplayMember = "DisplayMember";
            lstCarList.ValueMember = "ID";
            lstCarList.DataSource = bsCarList;

            bsParticipants.DataSource = derby.participants;
            lstParticipants.DisplayMember = "DisplayMember";
            lstParticipants.ValueMember = "ID";
            lstParticipants.DataSource = bsParticipants;

            bsCompletedRaces.DataSource = derby.races;
            lstCompletedRaces.DisplayMember = "DisplayMember";
            lstCompletedRaces.ValueMember = "ID";
            lstCompletedRaces.DataSource = bsCompletedRaces;

            cmbDenRank.SelectedIndex = 0;

            bsCarList.ResetBindings(false);
            bsDenList.ResetBindings(false);
            bsCarDen.ResetBindings(false);
            bsParticipants.ResetBindings(false);
            bsCompletedRaces.ResetBindings(false);

            if (derby.dens.Count > 0)
            {
                btnNewCar.Enabled = true;
                btnEditDen.Enabled = true;
                btnDeleteDen.Enabled = true;
            }

            if (derby.cars.Count > 0)
            {
                if (derby.races.Count == 0 && derby.laneSchedule[0].Count == 0)                
                    btnNewRace.Enabled = true;
                if (derby.laneSchedule.Count > 0)
                    btnNewRace.Enabled = true;

                btnEditCar.Enabled = true;
                btnDeleteCar.Enabled = true;                
            }

            if (derby.races.Count > 0)
            {
                btnDeleteRace.Enabled = true;
                btnRedoRace.Enabled = true;
                lstCompletedRaces.SetSelected(0, true);
            }

            dgvRace.ColumnCount = 4;
            dgvRace.Columns[0].Name = "Lane";            
            dgvRace.Columns[1].Name = "Name";            
            dgvRace.Columns[2].Name = "Number";            
            dgvRace.Columns[3].Name = "Time";

            dgvLeaderBoard.ColumnCount = 3;
            dgvLeaderBoard.Columns[0].Name = "Name";
            dgvLeaderBoard.Columns[1].Name = "Number";
            dgvLeaderBoard.Columns[2].Name = "AvgT";

            RefreshSummary();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            DialogResult dr = MessageBox.Show("Save current derby before creating new one?",
                    "Save?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                saveAsToolStripMenuItem.PerformClick();
            }

            Util.ClearControl(this);            
            derby = new Derby();
            for (int i = 0; i < derby.trackLanes; i++)
                derby.laneSchedule.Add(new List<Guid>());
            LoadDerbyControls();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < derby.trackLanes; i++)
                derby.laneSchedule.Add(new List<Guid>());
            LoadDerbyControls();        
        }                

        private void txtPackName_TextChanged(object sender, EventArgs e)
        {
            derby.pack.name = txtPackName.Text;
        }

        private void txtRaceMaster_TextChanged(object sender, EventArgs e)
        {
            derby.pack.raceMaster = txtRaceMaster.Text;
        }

        private void txtTimerIPAddr_TextChanged(object sender, EventArgs e)
        {
            derby.timerIP = txtTimerIPAddr.Text;
        }

        private void chkUseTimer_CheckedChanged(object sender, EventArgs e)
        {
            derby.useTimer = chkUseTimer.Checked;            
        }

        private void btnNewDen_Click(object sender, EventArgs e)
        {            
            txtDenNickname.Text = "";
            txtDenNickname.Enabled = true;
            cmbDenRank.Enabled = true;
        }

        private void txtDenNickname_TextChanged(object sender, EventArgs e)
        {
            if (txtDenNickname.Text == "")            
                btnSaveDen.Enabled = false;
            else            
                btnSaveDen.Enabled = true;            
        }

        private void btnSaveDen_Click(object sender, EventArgs e)
        {
            derby.AddDen(txtDenNickname.Text, cmbDenRank.Text);
                      
            bsDenList.ResetBindings(false);
            bsCarDen.ResetBindings(false);
            txtDenNickname.Text = "";
            txtDenNickname.Enabled = false;
            cmbDenRank.Enabled = false;
            btnDeleteDen.Enabled = true;
            btnEditDen.Enabled = true;
            btnNewCar.Enabled = true;
        }

        private void btnEditDen_Click(object sender, EventArgs e)
        {
            Den newDen = derby.GetDen((Guid)lstDenList.SelectedValue);
            txtDenNickname.Text = newDen.name;
            txtDenNickname.Enabled = true;            
            cmbDenRank.SelectedValue = newDen.ID;
            cmbDenRank.Enabled = true;
        }

        private void btnDeleteDen_Click(object sender, EventArgs e)
        {            
            DialogResult dr = MessageBox.Show("This will delete all cars in this Den, continue!?",
                      "Delete?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                // delete cars that belong to the den
                derby.DeleteDen((Guid)lstDenList.SelectedValue);

                bsCarList.ResetBindings(false);
                bsDenList.ResetBindings(false);
                bsCarDen.ResetBindings(false);
               
                if (bsDenList.Count == 0)
                {
                    btnDeleteDen.Enabled = false;
                    btnEditDen.Enabled = false;
                    btnNewCar.Enabled = false;
                }
            }            
        }

        private void btnNewCar_Click(object sender, EventArgs e)
        {            
            txtCarName.Text = "";
            txtCarName.Enabled = true;
            cmbCarDen.SelectedIndex = 0;
            numCarWeight.Value = 0;            
            cmbCarDen.Enabled = true;
            numCarWeight.Enabled = true;
            numCarNumber.Enabled = true;
        }

        private void btnSaveCar_Click(object sender, EventArgs e)
        {            
            derby.AddCar(txtCarName.Text, Convert.ToDouble(numCarWeight.Value), (Guid)cmbCarDen.SelectedValue, Convert.ToInt32(numCarNumber.Value));
            
            bsCarList.ResetBindings(false);
            txtCarName.Enabled = false;
            cmbCarDen.Enabled = false;
            numCarWeight.Enabled = false;
            numCarNumber.Enabled = false;
            btnEditCar.Enabled = true;
            btnDeleteCar.Enabled = true;
            btnSaveCar.Enabled = false;
            btnNewRace.Enabled = true;
        }

        private void btnNewRace_Click(object sender, EventArgs e)
        {
            btnNewRace.Enabled = false;

            derby.NewRace();

            lblRemainingRaces.Text = "Remaining Races: " + derby.laneSchedule[0].Count();
            bsParticipants.ResetBindings(false);

            if (derby.useTimer == true)            
                btnStartTimer.Enabled = true;                
            else
                btnFinishRace.Enabled = true;

            lblRaceStatus.Text = "Race Status:Ready";

        }

        private void btnStartTimer_Click(object sender, EventArgs e)
        {            
            if (Timer.NewRace(derby.timerIP))
            {
                btnStartTimer.Enabled = false;
                btnFinishRace.Enabled = true;
                lblRaceStatus.Text = "Race Status: Running";
            }
            else
            {
                lblRaceStatus.Text = "Race Status: Timer Start Error";

                // below lines are just for testing without timer
                btnStartTimer.Enabled = false;
                btnFinishRace.Enabled = true;
            }
        }

        private void btnFinishRace_Click(object sender, EventArgs e)
        {                        
            if (derby.FinishRace())
            {
                btnFinishRace.Enabled = false;
                btnRedoRace.Enabled = true;
                btnDeleteRace.Enabled = true;

                // if there are any races left enable the next race button
                if (derby.laneSchedule[0].Count > 0)
                    btnNewRace.Enabled = true;

                lblRaceStatus.Text = "Finished";                
                bsCompletedRaces.ResetBindings(false);                
                bsParticipants.ResetBindings(false);
                lstCompletedRaces.SetSelected(lstCompletedRaces.Items.Count - 1, true);
            }
            else
            {
                lblRaceStatus.Text = "Race Status: Timer Results Error";
                
                btnFinishRace.Enabled = false;                
                btnRedoRace.Enabled = true;
                if (derby.laneSchedule[0].Count > 0)
                    btnNewRace.Enabled = true;
            }
        }

        private void btnTestTimer_Click(object sender, EventArgs e)
        {
            if (Timer.Test(derby.timerIP))
            {
                lblTestTimer.Text = "Success!";
            } else
            {
                lblTestTimer.Text = "Error!";
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete this car!?",
                      "Delete?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                derby.DeleteCar((Guid)lstCarList.SelectedValue);

                bsCarList.ResetBindings(false);

                if (bsCarList.Count == 0)
                {
                    btnDeleteCar.Enabled = false;
                    btnEditCar.Enabled = false;
                    btnNewRace.Enabled = false;
                }
            }
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            Car newCar = derby.GetCar((Guid)lstCarList.SelectedValue);

            txtCarName.Text = newCar.name;
            txtCarName.Enabled = true;
            cmbCarDen.SelectedValue = newCar.denID;
            cmbCarDen.Enabled = true;
            numCarWeight.Value = Convert.ToDecimal(newCar.weight);
            numCarWeight.Enabled = true;
            numCarNumber.Value = Convert.ToDecimal(newCar.number);
            numCarNumber.Enabled = true;
            btnSaveCar.Enabled = true;            
        }

        private void txtCarName_TextChanged(object sender, EventArgs e)
        {
            /*
            if (numCarWeight.Value != 0 && txtCarName.Text != "")
                btnSaveCar.Enabled = true;
            else
                btnSaveCar.Enabled = false;
            */
            if (txtCarName.Text != "")
                btnSaveCar.Enabled = true;
            else
                btnSaveCar.Enabled = false;
        }

        private void numCarWeight_ValueChanged(object sender, EventArgs e)
        {
            /*
            if (numCarWeight.Value != 0 && txtCarName.Text != "")
                btnSaveCar.Enabled = true;
            else
                btnSaveCar.Enabled = false;
            */                
        }

        private void numTrackLanes_ValueChanged(object sender, EventArgs e)
        {
            derby.trackLanes = Convert.ToInt32(numTrackLanes.Value);
            derby.laneSchedule = new List<List<Guid>>();
        }

        private void numHeatsPerCar_ValueChanged(object sender, EventArgs e)
        {
            derby.heatsPerCar = Convert.ToInt32(numHeatsPerCar.Value);
        }

        private void btnRedoRace_Click(object sender, EventArgs e)
        {            
            derby.RedoRace((Guid)lstCompletedRaces.SelectedValue);
            
            bsCompletedRaces.ResetBindings(false);
            bsParticipants.ResetBindings(false);
            btnStartTimer.Enabled = true;
            btnFinishRace.Enabled = false;
            btnRedoRace.Enabled = false;
        }

        private void lstCompletedRaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRace.Rows.Clear();
            List<FinishTime> finishTimes = derby.finishTimes.Where(finishTime => finishTime.raceID == (Guid)lstCompletedRaces.SelectedValue).ToList();
            
            foreach (FinishTime finishTime in finishTimes)
            {
                double time = finishTime.time;
                int lane = finishTime.lane;
                Car car = derby.cars.Where(thisCar => thisCar.ID == finishTime.carID).First();
                string name = car.name;
                int number = car.number;

                dgvRace.Rows.Add(lane, name, number, time);                
            }
            dgvRace.Sort(dgvRace.Columns["Time"], ListSortDirection.Ascending);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void btnDeleteRace_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete this race!?",
                      "Delete?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                derby.DeleteRace((Guid)lstCompletedRaces.SelectedValue);

                bsCompletedRaces.ResetBindings(false);
                if (derby.races.Count == 0)
                    btnDeleteRace.Enabled = false;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Ready to exit? Unsaved changes will be lost!", "Exit?",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            e.Cancel = (result == DialogResult.No);
        }
        

        private void btnSaveSummary_Click(object sender, EventArgs e)
        {

        }

        private void btnRefreshSummary_Click(object sender, EventArgs e)
        {
            RefreshSummary();
        }

        private void RefreshSummary()
        {
            dgvLeaderBoard.Rows.Clear();

            foreach (Car car in derby.cars)
            {
                List<FinishTime> finishTimes = derby.finishTimes.Where(i => i.carID == car.ID).ToList();

                double timeAvg = 0.0;
                if (finishTimes.Count > 0)
                {
                    timeAvg = finishTimes.Average(i => i.time);
                }
                dgvLeaderBoard.Rows.Add(car.name, car.number, timeAvg);
            }
            dgvLeaderBoard.Sort(dgvLeaderBoard.Columns["AvgT"], ListSortDirection.Ascending);

            string summary = "";                       

            summary += "Car Results" + Environment.NewLine;
            summary += "-----------" + Environment.NewLine + Environment.NewLine;

            summary += String.Format("{0,-30}{1,-3}{2,-4}", "Name", "#", "AvgT") + Environment.NewLine;

            foreach (DataGridViewRow row in dgvLeaderBoard.Rows)
            {
                summary += String.Format("{0,-30}{1,-3}{2,-4}", row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value) + Environment.NewLine;
            }                        

            summary += Environment.NewLine + Environment.NewLine;

            summary += "Individual Race Results" + Environment.NewLine;
            summary += "-----------------------" + Environment.NewLine + Environment.NewLine;

            foreach (Race race in derby.races)
            {
                summary += "Race Number: " + race.number + Environment.NewLine;
                summary += "Race Date: " + race.dateTime.ToString() + Environment.NewLine + Environment.NewLine;
                summary += String.Format("{0,-6}{1,-30}{2,-3}{3,-4}", "Lane", "Name", "#", "Time") + Environment.NewLine ;

                foreach (Guid finishID in race.finishIDs)
                {
                    try
                    {
                        FinishTime finishTime = derby.finishTimes.Where(i => i.ID == finishID).First();
                        Car car = derby.cars.Where(i => i.ID == finishTime.carID).First();
                        summary += String.Format("{0,-6}{1,-30}{2,-3}{3,-4}", finishTime.lane, car.name, car.number, finishTime.time) + Environment.NewLine;
                    }
                    catch
                    {
                        summary += "Error fetching results for finish ID " + finishID.ToString() + Environment.NewLine;
                    }
                }

                summary += Environment.NewLine + Environment.NewLine;
            }
            
            txtRaceSummary.Text = summary;
        }
    }
}
