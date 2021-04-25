namespace MySkillsServer.Data.Models
{
    using MySkillsServer.Data.Common.Models;

    public class Contact : BaseModel<int>
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string LinkText { get; set; }
    }
}
