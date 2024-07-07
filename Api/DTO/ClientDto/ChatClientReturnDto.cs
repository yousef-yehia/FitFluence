namespace Api.DTO.ClientDto
{
    public class ChatClientReturnDto
    {
        public string Id { get; set; }
        public string Role {  get; set; }
        public string Image { get; set; }
        public string About { get; set; }
        public string User_name { get; set; }
        public string Created_At { get; set; }
        public bool Is_online { get; set; }
        public string Last_Active { get; set; }
        public string Email { get; set; }
        public string MainGoal { get; set; }
        public string Activity_level { get; set; }
        public string Push_token { get; set; }
    }
}
