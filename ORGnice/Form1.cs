namespace ORGnicee
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cbDept.Items.AddRange(new string[]
            {
                "Singing Group",
                "Dance Group",
                "Band Group"
            });


        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            AddMember addMember = new AddMember();
            addMember.ShowDialog();
        }

        private void btnAddTarGoal_Click(object sender, EventArgs e)
        {
            AddTargetGoal addTargetGoal = new AddTargetGoal();
            addTargetGoal.ShowDialog();
        }

        private void btnEditTarGoal_Click(object sender, EventArgs e)
        {
            AddTargetGoal addTargetGoal = new AddTargetGoal();
            addTargetGoal.ShowDialog();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
