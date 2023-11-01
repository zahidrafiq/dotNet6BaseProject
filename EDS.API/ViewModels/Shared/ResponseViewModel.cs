using System;
using Newtonsoft.Json;

namespace HMIS.IM.API.ViewModels.Shared
{
    public class ResponseViewModel<ViewModel>
    {
        public Status status;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Navigation navigation;
        
        public ViewModel data;

        public ResponseViewModel(ViewModel viewModel)
        {
            this.data = viewModel;
            this.navigation = new Navigation();
            this.status = new Status((int)System.Net.HttpStatusCode.OK, "Success","");
            
        }

        public ResponseViewModel()
        {
            this.navigation = new Navigation();
            this.status = new Status((int)System.Net.HttpStatusCode.OK, "Success", "");
        }

        public void AddNavigation(string prevLink, string nextLink, int totalPages, int totalCount)
        {
            this.navigation = new Navigation(prevLink, nextLink, totalPages, totalCount);
        }

        public void AddStatus(int code, string message, string description)
        {
            this.status = new Status(code, message, description);
        }
       
        public class Status
        {
            public Status()
            {
            }

            public Status(int code, string message, string description)
            {
                this.code = code;
                this.message = message;
                this.description = description;
            }

            public int code { get; set; }

            public string message { get; set; }

            public string description { get; set; }
        }

        public class Navigation
        {
            public Navigation() { }
            public Navigation(string prevLink, string nextLink, int totalPages, int totalCount)
            {
                this.prevLink = prevLink;
                this.nextLink = nextLink;
                this.totalPages = totalPages;
                this.totalCount = totalCount;
            }

            public string prevLink { get; set; }

            public string nextLink { get; set; }

            public int totalPages { get; set; }

            public int totalCount { get; set; }
        }
    }
}