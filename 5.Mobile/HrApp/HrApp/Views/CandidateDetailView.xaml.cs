using HrApp.API.Beans;
using HrApp.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
//using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandidateDetailView : ContentPage
    {
        private CandidatesResponse candidate = new CandidatesResponse();

        public CandidateDetailView(CandidatesResponse candidate)
        {
            InitializeComponent();
            this.candidate = candidate;
            Name.Text = candidate.Name+" "+ candidate.LastName;
            DNI.Text = candidate.DNI.ToString();
            PhoneNumber.Text = candidate.PhoneNumber;            
            EnglishLevel.Text =  candidate.EnglishLevel.ToString();
            ContactDay.Text =candidate.ContactDay.ToString() ;
            Status.Text = candidate.Status.ToString();
            Skills.Text = string.Join(",", candidate.CandidateSkills.Select(x => x.Skill.Type.Name));

        }

        private void OnButtonEmailClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:" + candidate.EmailAddress));
        }

        private void OnButtonLinkedinClicked(object sender, EventArgs e)
        {
                var button = ((Button)sender);
            button.BackgroundColor = button.BackgroundColor.Equals(Color.Transparent)
                ? Color.FromHex("#22ac38")
                : Color.Transparent;
              if (candidate.LinkedInProfile!=null && candidate.LinkedInProfile!="")
                Device.OpenUri(new Uri(candidate.LinkedInProfile));
              else
                DependencyService.Get<IToastMessage>().LongAlert("There is no linkedin profile");
        }

        private void OnButtonPhoneClicked(object sender, EventArgs e)
        {
            if (candidate.PhoneNumber != null && candidate.PhoneNumber != "")
                Device.OpenUri(new Uri(String.Format("tel:{0}", candidate.PhoneNumber)));
            else
                DependencyService.Get<IToastMessage>().LongAlert("There is no phonen number");
          
        }
    }
}
